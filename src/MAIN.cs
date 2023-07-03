using System;
using System.Threading;
using System.Reflection;

public class TIM{
    public static string version {get;} = "0.0.2-dev"; //public variable for the versioning info
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
        public bool input_interrupt{get; set;}
        public Thread InpH{get; set;}

        public const float STEPTIME = (float)1; //time intervals between steps in seconds
        
        public main(){
            //parameter initialisation
            gameMap=new map(mapsize.X,mapsize.Y);
            exit = false;
            entities = new Dictionary<string,functionProperties>();
            zoom=1;
            center_pos=new Position(mapsize.X/2,mapsize.Y/2);
            sys=new system();
            input_interrupt=false;
            InpH=new Thread(new ThreadStart(InputHandler));
        }
        public void Start(main g){
            game=g;
            gameMap.createMap();    //create Map
            entities.Add("sys",new functionProperties());   //add system entity, the basic control entity of the game
            InpH.Start();   //start the input handler
            while(!exit){   //play the game until it is stopped
                if(input_interrupt){    //if the input mode is open
                    string? input=Console.ReadLine();   //read the input
                    if(input==" "){  //if the input is a space
                        input_interrupt=false;  //leave the input node
                        Console.WriteLine("Input mode closed:");
                        InpH=new Thread(new ThreadStart(InputHandler));
                        InpH.Start();   //restart the input handler
                    }else if(input!=null){
                        methods.callMethod(input,game); //try to call methods accordingly
                    }
                }else{
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

        //input handler
        public void InputHandler(){
            ConsoleKey cki;
            do{
                cki = Console.ReadKey(true).Key;
                Thread.Sleep(0);
                switch(cki){    //special quickkeys for panning and zooming the map
                    case ConsoleKey.Add:
                        sys.incZoom(-1,game);
                        break;
                    case ConsoleKey.Subtract:
                        sys.incZoom(1,game);
                        break;
                    case ConsoleKey.UpArrow:
                        sys.incPosition(0,-zoom,game);
                        break;
                    case ConsoleKey.DownArrow:
                        sys.incPosition(0,zoom,game);
                        break;
                    case ConsoleKey.LeftArrow:
                        sys.incPosition(-zoom,0,game);
                        break;
                    case ConsoleKey.RightArrow:
                        sys.incPosition(zoom,0,game);
                        break;
                    default:
                        break;
                }
            }while(cki!=ConsoleKey.Spacebar);   //if the spacebar is pressed
            input_interrupt=true;   //open the input mode
            Console.WriteLine("Input mode open:");
            return; //quit the input handler, input from now on is handeled differently
        }
    }
}
