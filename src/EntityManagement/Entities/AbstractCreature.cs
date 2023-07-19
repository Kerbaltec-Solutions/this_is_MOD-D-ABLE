using RoutePlanning;
public abstract class Creature{

    protected TIM.main game = null!; // reference go game
    public bool draw{get;} = true; //if the entity should be drawn on the map
    public bool iterate{get;} = true;  //if the entity has actions to perform every frame
    public abstract  bool createByPlayer{get;}    //can the entity be created by the player directly
    
    public abstract bool controlledByPlayer{get;} // can the player controll the entity
    public abstract char mapChar{get;}     //character which will represent the entity on the map
    public abstract int mapColor{get;}  //color in which the entity will be drawn on the map
    protected abstract float speed{get;} //movement speed of the entity in fields per second

    protected bool isDead{get;set;} = false; 

    protected abstract  int maxHealthPoints {get;}
    protected abstract int healthPoints{get;set;}


    public Position position{get;set;} = null!;  //current position of the entity
    protected Position? target = null;    //target position 
    protected Route? route = null;   // currently assigned route
    protected functionProperties? tgtEntity = null; // target entity
    protected bool trackEntity = false; // is currently following entity
    protected float timeSinceLastMove = TIM.main.STEPTIME; // in seconds
    

    //setup function, set position ect.
    public virtual void setup(string input, TIM.main game){
        this.game = game;
        this.position= new Position(Math.Min(int.Parse(input.Split(",")[0]),game.mapsize.X),Math.Min(int.Parse(input.Split(",")[1]),game.mapsize.Y));
        this.route = null;
    }

    // setup for spawning entities at start of game
    public virtual void autoSetup(Position pos, TIM.main game){
        this.game = game;
        this.position = pos;
    }

    //frame actions
    public virtual void step(TIM.main game){
        if(isDead){
            // exception is handeled in MAIN (where entity is removed)
            throw new NotSupportedException("Entity is dead");
        }
        move(); 
    }

    // move a certain distance
    protected virtual void move(){
        if(trackEntity){
            updateMovement(this.game.gameMap.mapArray);
        }
        if(this.route != null && this.route.Path != null){ // if path available
            float steps = timeSinceLastMove*getModifiedSpeed(); // number of fields the entity can move
            if(steps >= 1){ // if entity can move again                
                // speed is based on resource of field at beginning of movement 
                this.timeSinceLastMove = 0; // reset time
                for(int i = 0; i< steps;i++){ // move along path
                    if(this.route != null){
                        try{
                            this.position = this.route.Path[0]; 
                            this.route.Path.RemoveAt(0); // deletes current position from route
                        }catch(ArgumentOutOfRangeException){
                            route = null;
                            break;
                        }                        
                    }                    
                }                
            }
            this.timeSinceLastMove += TIM.main.STEPTIME;
        }
        else{
            if( target is not null &&this.position != target){
                setRoute(this.game.gameMap.mapArray); // if no route available but target is set: create new route
            }
        } 
    }
    // create new route
    protected virtual void setRoute(mapPixel[,] maparr){
        if(this.target is not null){
            this.route = new Route(maparr, this.position, target);
            target = this.route.targetPos;
        }
    }

    // update path to target entity
    protected virtual void updateMovement(mapPixel[,] maparr){
        if(tgtEntity != null){
            // tgtEntity always has position (tested in GoToEntity)
            var posVar= tgtEntity.fType.GetProperty("position");
            var posVal= posVar!.GetValue(tgtEntity.fObject, null)!;
            target = (Position)posVal;
            setRoute(maparr);
        }
    }

    // divide speed by speedDivider of resource on current position
    protected float getModifiedSpeed(){
        return speed/this.game.gameMap.mapArray[position.X,position.Y].resource.SpeedDevider;
    }

    // takes damage
    public void getHit(int damage){
        healthPoints -= damage;
        if(healthPoints <= 0){
            isDead = true;
        }
    }

    // functions for player

     // prints coordinates of position
    public void printPosition(TIM.main game){
        Console.WriteLine("{0}",position);
    }

    // prints current route
    public void printRoute(TIM.main game){
        if(route != null){
            Console.WriteLine("{0}",route);
            return;
        }
        else Console.WriteLine("no current route");        
    }

    // print health points and maximal health points
    public void printHP(TIM.main game){
        Console.WriteLine("HP: {0} | maxHp: {1}",healthPoints, maxHealthPoints);
    }

    //set a target
    public void GoTo(string input, TIM.main game){
        if(controlledByPlayer){
            // set target to position thats on the map
            this.target=new Position(Math.Min(int.Parse(input.Split(",")[0]),game.mapsize.X),Math.Min(int.Parse(input.Split(",")[1]),game.mapsize.Y));
            setRoute(game.gameMap.mapArray);
            trackEntity=false;
        }else{
            Console.WriteLine("You can't control this entity");
        }
    }

    // set new targetEntity
    public void GoToEntity(string input, TIM.main game){
        // try to find entity
        try{
             if(controlledByPlayer){ 
                functionProperties entity = game.entities[input];
                var boolVar= entity.fType.GetProperty("draw");
                if(boolVar!=null){
                    var boolVal= boolVar.GetValue(entity.fObject, null);
                    bool hasPos = (bool)boolVal!;
                    if(hasPos==true){
                        tgtEntity=entity;
                    }
                }
                trackEntity=true;
            }else{
                Console.WriteLine("You can't control this entity");
            }
        }catch(KeyNotFoundException){
            Console.WriteLine("ERR: Entity '{0}' not found!",input);
            return;
        }       
    }
}


 