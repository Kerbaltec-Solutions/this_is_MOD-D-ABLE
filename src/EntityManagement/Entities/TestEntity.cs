using System;

using RoutePlanning;

public class TestEntity{
    private TIM.main game = null!; // reference to game
    public bool draw{get;} = true; //if the entity should be drawn on the map
    public bool iterate{get;} = true;  //if the entity has actions to perform every frame
    public bool createByPlayer{get;} = true;    //if the entity can be created by the player directly
    public char mapChar{get;} = '#';    //character which will represent the entity on the map
    public int mapColor{get;} = 0;  //color in which the entity will be drawn on the map
    public float speed{get;}=4; //movement speed of the entity




    public Position position{get;set;} = null!;  //current position of the entity
    private Position? target = null;    //target position 
    public Route? route = null;    
    public float timeSinceLastMove = TIM.main.STEPTIME; // in seconds

    //frame actions
    public void step(TIM.main game){
        move(); // move a certain distance
    }

    //setup function, set position ect.
    public void setup(string input, TIM.main game){
        this.position=new Position(Math.Min(int.Parse(input.Split(",")[0]),game.mapsize.X),Math.Min(int.Parse(input.Split(",")[1]),game.mapsize.Y));
        this.game = game; 
    }

    public void move(){
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
        else{this.route = null;
            if( target is not null &&this.position != target){
                setRoute(false,game.gameMap.mapArray); // if no route available but target is set: create new route
            }
        } 
    }

    // create new route
    public void setRoute(bool fastFind, mapPixel[,] maparr){
        this.route = new Route(maparr, this.position, target!);
        target = this.route.targetNode.Pos;
    }

    // divide speed by speedDivider of resource on current position
    public float getModifiedSpeed(){
        return speed/game.gameMap.mapArray[position.X,position.Y].resource.SpeedDevider;
    }

    //functions for player:

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

    //set a target
    public void GoTo(string input, TIM.main game){
        this.target=new Position(Math.Min(int.Parse(input.Split(",")[0]),game.mapsize.X),Math.Min(int.Parse(input.Split(",")[1]),game.mapsize.Y));
    }
}