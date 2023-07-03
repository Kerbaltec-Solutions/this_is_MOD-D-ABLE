using System;
using System.Threading;
using System.Reflection;

public class TIM{
    public static string version {get;} = "0.0.1-dev"; //public variable for the versioning info
    public static void Main(){
        ui.printIntro();
        Thread.Sleep(2000);
        main game = new main(); //initialize the game
        game.Start(game);       //start the game
    }
    public class main{
        //parameter declaraion
        public map gameMap{get; set;}   
        public Boolean exit{get; set;}
        public IDictionary<string,functionProperties> entities {get; set;}
        public system sys{get;}
        public int zoom {get; set;}
        public Position center_pos {get; set;}
        public Position mapsize {get;} = new Position(100,100);
        public main? game{get; private set;}

        public const float STEPTIME = (float)1; //time intervals between steps in seconds
        
        public main(){
            //parameter initialisation
            gameMap=new map(mapsize.X,mapsize.Y);
            exit = false;
            entities = new Dictionary<string,functionProperties>();
            zoom=1;
            center_pos=new Position(mapsize.X/2,mapsize.Y/2);
            sys=new system();
        }
        public void Start(main g){
            game=g;
            gameMap.createMap();    //create Map
            entities.Add("sys",new functionProperties());   //add system entity, the basic control entity of the game
            while(!exit){   //play the game until it is stopped
                foreach(KeyValuePair<string, functionProperties> entry in entities){    //iterate through all game entities
                    functionProperties entity = entry.Value;
                    var itVar= entity.fType.GetProperty("iterate");
                    if(itVar!=null){    //if entity could have been made to be executed every frame
                        var itVal= itVar.GetValue(entity.fObject, null);
                        bool iterate = (bool)itVal;
                        if(iterate==true){  //if entity was made to be executed every frame
                            try{
                                entity.fType.GetMethod("step").Invoke(entity.fObject, new object[]{game}); //try to execute the step method
                            }catch(TargetInvocationException ex){                        
                                // ex now stores the original exception
                                if(ex.InnerException is NotSupportedException){
                                    Console.WriteLine("Stepfunction of {0} not found!",entry.Key);
                                }
                            }
                        }
                    }
                }
                Thread.Sleep((int)(STEPTIME * 1000));   //wait for a bit, so we can see the map properly
            }
        }
    }
}
