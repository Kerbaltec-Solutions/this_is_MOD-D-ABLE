using RoutePlanning;
public class Worker :Creature{
    public override  bool createByPlayer{get;} = true;
    public override bool controlledByPlayer{get;} = true;
    public override char mapChar{get;} = 'W';
    public override int mapColor{get;} = 0;
    public override float speed{get;} = 7; // in fields per second

    public bool entityCanDestroyStone = true; // worker is able to create mines

    public int mineSpeed{get;} = 1; // in hits per second

    // override: to change stone or snow if worker destroys them
    public override void move(){
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
                            // if worker destroyes stone or snow: a mine field is created
                            mapPixel field = game.gameMap.mapArray[position.X,position.Y];
                            if(field.resource.Type == Resource.ResourceType.Stone || field.resource.Type == Resource.ResourceType.Snow){
                                game.gameMap.set(this.position,new Resource(Resource.ResourceType.Mine));} 
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
                setRoute(this.game.gameMap.mapArray); // if no route available but target is set: create new route
            }
        } 
               
    }

    // override: worker is able to destroy stone
    public override void setRoute(mapPixel[,] maparr){
        if(this.target is not null){
            this.route = new Route(maparr, this.position, target, entityCanDestroyStone);
            target = this.route.targetNode.Pos;
        }     
    }
}

