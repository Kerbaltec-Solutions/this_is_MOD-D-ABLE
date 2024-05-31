using System;
using System.Collections;

// Types of Resources a Pixel can have
public class Resource{
    public ResourceType Type{get; private set;}

    // marks wether a normal creature should be able to walk on this resource:
    public bool IsWalkable{get; private set;} 

    // value that divides the speed of an entity on this resource (how much the resource slows it down)
    public int SpeedDevider{get; private set;}

    // resourcen type: constant value equals color of that resource
    public enum ResourceType{
        None = 0, //black
        Stone = 1, //dark grey
        Water = 9, //blue
        DeepWater = 8, //dark blue
        Grass = 7, //green
        Sand = 10, //dark yellow
        Forest = 6, // dark green
        Snow = 3, // white
        Mine = 2, // light gray
    }

    // define Properties of the resources:
    private void setProperties(){
        switch(Type){
            case ResourceType.Stone:{
                this.IsWalkable = false;
                this.SpeedDevider = 8;
                break;
            }
            case ResourceType.Water:{
                this.IsWalkable = true;
                this.SpeedDevider = 5;
                break;
            }
            case ResourceType.DeepWater:{
                this.IsWalkable = true;
                this.SpeedDevider = 6;
                break;
            }
            case ResourceType.Forest:{
                this.IsWalkable = true;
                this.SpeedDevider = 3;
                break;
            }
            case ResourceType.Sand: case ResourceType.Grass:{
                this.IsWalkable = true;
                this.SpeedDevider = 1;
                break;
            }
            case ResourceType.Snow:{
                this.IsWalkable = false;
                this.SpeedDevider = 8;
                break;
            }
            case ResourceType.Mine:{
                this.IsWalkable = true;
                this.SpeedDevider = 3;
                break;
            }
            default:{
                break;
            }
        }
    }

    // create new resource:
    public Resource(){
        Type = ResourceType.None;
        setProperties();
    }
    public Resource(ResourceType type){
        Type= type;
        setProperties();
    }

    // get the color of this resource
    public int getColor(){
        return (int)this.Type;
    }

    // ToString returns type of resource
    public override string ToString()
    {
        return String.Format("{0}", Type);
    }


}

