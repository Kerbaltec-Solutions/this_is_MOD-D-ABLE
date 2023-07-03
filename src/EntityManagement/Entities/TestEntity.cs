using System;

public class TestEntity{
    public  bool draw{get;} = true; //if the entity should be drawn on the map
    public  bool iterate{get;} = true;  //if the entity has actions to perform every frame
    public bool createByPlayer{get;} = true;    //if the entity can be created by the player directly
    public char mapChar{get;} = '#';    //character which will represent the entity on the map
    public int mapColor{get;} = 0;  //color in which the entity will be drawn on the map
    public float speed{get;}=4; //movement speed of the entity

    public Position position{get;set;}  //current position of the entity
    private Position? target = null;    //target position 

    //setup function, set position ect.
    public void setup(string input, TIM.main game){
        this.position=new Position(Math.Min(int.Parse(input.Split(",")[0]),game.mapsize.X),Math.Min(int.Parse(input.Split(",")[1]),game.mapsize.Y));
    }

    //set a target
    public void GoTo(string input, TIM.main game){
        this.target=new Position(Math.Min(int.Parse(input.Split(",")[0]),game.mapsize.X),Math.Min(int.Parse(input.Split(",")[1]),game.mapsize.Y));
    }

    //frame actions
    public void step(TIM.main game){
        //this is all placeholder and will be replaced
        if(target!=null){   //if there is a target
            double dX=this.position.X-this.target.X;
            double dY=this.position.Y-this.target.Y;
            double dist=Math.Sqrt(dX*dX+dY*dY);
            if(dist>this.speed){//and the target can not be reached in this frame
                int X = Convert.ToInt32(dX*this.speed/dist);
                int Y = Convert.ToInt32(dY*this.speed/dist);
                this.position=new Position(X,Y);//move towards the target with roughly the right speed
            }else{
                this.position=this.target; //reach the target
                this.target=null;   //target is not longer needed
            }
        }
    }
}