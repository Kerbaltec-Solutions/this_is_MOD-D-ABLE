using System;
using System.Threading;
using System.Reflection;

public class TIM{
    public static string version {get;} = "1.0.7-dev"; //public variable for the versioning info
    public static void Main(){
        ui.printIntro();
        Console.ReadKey();
        main game = new main(); //initialize the game
        game.Start(game);       //start the game
    }
    public class main{
        //parameter declaraion
        public map gameMap{get; set;}   //game Map
        public Boolean exit{get; set;}  //boolean when true, the game exits
        public IDictionary<string,functionProperties> entities {get; set;}  //dictionary of all existing entities
        public system sys{get;} //new main system class
        public int zoom {get; set;} //zoom level
        public Position center_pos {get; set;}  //center position of the camera
        public Position mapsize {get;} = new Position(100,100); //size of the game map

        public main? game{get; private set;}    //the game itself
        public bool input_interrupt{get; set;}  //boolean, if true, the imput mode is opened
        public Thread InpH{get; set;}   //Input handler

        public const float STEPTIME = (float)1; //time intervals between steps in seconds

        public int respawn_timer {get; set;} = 0;  //timer for respawning entities
        
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
            gameMap.createTerrain();    //create Map
            SpawnEntities spawner = new SpawnEntities();
            spawner.initialspawn(game);
            InpH.Start();   //start the input handler
            while(!exit){   //play the game until it is stopped
                if(input_interrupt){    //if the input mode is open
                    string? input=Console.ReadLine();   //read the input
                    if(input==""){  //if the input is empty
                        input_interrupt=false;  //leave the input node
                        Console.WriteLine("Input mode closed:");
                        InpH=new Thread(new ThreadStart(InputHandler));
                        InpH.Start();   //restart the input handler
                    }else if(input!=null){
                        methods.callMethod(input,game); //try to call methods accordingly
                    }
                }else{
                    if(respawn_timer==0){   //when timer runs out respawn entities and set new timer
                        spawner.spawnentities("Deer",game);
                        spawner.spawnentities("Bear",game);
                        spawner.spawnOre(game);
                        System.Random rand = new System.Random();
                        respawn_timer = rand.Next(50,201);
                    }else{
                        respawn_timer--;
                    }
                    foreach(KeyValuePair<string, functionProperties> entry in entities){    //iterate through all game entities
                        functionProperties entity = entry.Value;
                        var itVar= entity.fType.GetProperty("iterate");
                        if(itVar!=null){    //if entity could have been made to be executed every frame
                            var itVal= itVar.GetValue(entity.fObject, null);
                            bool? iterate = (bool?)itVal;
                            if(iterate==true){  //if entity was made to be executed every frame
                                try{
                                    MethodInfo? stepMethod = entity.fType.GetMethod("step");
                                    if(stepMethod == null){
                                        throw new NotImplementedException("FATAL: a entity was set to iterable but contains no step function!");
                                    }
                                    stepMethod.Invoke(entity.fObject, new object[]{game}); //try to execute the step method
                                }catch(TargetInvocationException ex){                        
                                    // ex now stores the original exception
                                    if(ex.InnerException is NotImplementedException){
                                        throw new NotImplementedException("FATAL: a entity was set to iterable but contains no step function!");
                                    }
                                    if(ex.InnerException is NotSupportedException){
                                        // the entity doesnt exist anymore and can therefore be removed
                                        entities.Remove(entry.Key);
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
                if(game==null){
                    throw new Exception("FATAL: Something tried to call the Input handler before a game was initialized properly.");
                }
                switch(cki){    
                    //special quickkeys for panning and zooming the map
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
                    //special quickkeys for moving the cursor
                    case ConsoleKey.A:
                        methods.callMethod("c","iP","-1,0",game);
                        break;
                    case ConsoleKey.D:
                        methods.callMethod("c","iP","1,0",game);
                        break;
                    case ConsoleKey.W:
                        methods.callMethod("c","iP","0,-1",game);
                        break;
                    case ConsoleKey.S:
                        methods.callMethod("c","iP","0,1",game);
                        break;
                    default:
                        break;
                }
            }while(cki!=ConsoleKey.Enter);   //if the Enter key is pressed
            input_interrupt=true;   //open the input mode
            Console.WriteLine("Input mode open:");
            return; //quit the input handler, input from now on is handeled differently
        }
    }
}
