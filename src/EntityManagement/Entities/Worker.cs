using RoutePlanning;
public class Worker :Creature{
    public bool isTarget{get;} = true; // creature is target for enemies
    public override  bool createByPlayer{get;} = false;
    public override bool controlledByPlayer{get;} = true;
    public override char mapChar{get;} = 'W';
    public override int mapColor{get;} = 0;
    protected override float speed{get;} = 7; // in fields per second

    protected override  int maxHealthPoints {get;} = 10;
    protected override int healthPoints{get;set;} = 10;

    private bool entityCanDestroyStone = true; // worker is able to create mines

    private int mineSpeed{get;} = 1; // in hits per second
    private float timeSinceLastHit = 0;

    public override void step(TIM.main game)
    {
        base.step(game);
        move();
        checkAttack(game);
    }

    // override: to change stone or snow if worker destroys them
    protected override void move(){
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
    protected override void setRoute(mapPixel[,] maparr){
        if(this.target is not null){
            this.route = new Route(maparr, this.position, target, entityCanDestroyStone);
            target = this.route.targetPos;
        }     
    }

    // check whether the target can be hit
    private void checkAttack(TIM.main game){
        if(target is not null && trackEntity){
            if(getDistanceToTarget() <= 1){
                if(this.timeSinceLastHit* mineSpeed >= 1){
                    float steps = timeSinceLastHit*mineSpeed;
                    this.timeSinceLastHit = 0;
                    for(int i = 0; i< steps;i++){
                        mineOre(game);
                    }                
                }   
            }
            this.timeSinceLastHit += TIM.main.STEPTIME;  
        }        
    }

    // hits an Ore and reduces its durability
    private void mineOre(TIM.main game){
        if(tgtEntity != null){
            var oreP = tgtEntity.fType.GetProperty("isOre");
            if(oreP != null){
                bool ore = (bool)oreP.GetValue(tgtEntity.fObject, null)!;
                if(ore){
                    try{
                        tgtEntity.fType.GetMethod("getHit")!.Invoke(tgtEntity.fObject, new object[]{1});
                        var tgtHPP = tgtEntity.fType.GetProperty("durability");
                        if(tgtHPP != null){
                            int tgtHP = (int)tgtHPP.GetValue(tgtEntity.fObject, null)!;
                            if(tgtHP<= 0){
                                tgtEntity = null;
                            }
                        }
                    }catch(NullReferenceException){}    
                }
            }    
        }        
    }

    // calculates distance to target trough Pythagoras
    private double getDistanceToTarget(){
        if(tgtEntity != null){
            var posP = tgtEntity.fType.GetProperty("position");
            if(posP != null){
                Position tgtposition= (Position) posP.GetValue(tgtEntity.fObject, null)!;
                return Math.Sqrt(Math.Pow(position.X-tgtposition.X,2)+Math.Pow(position.Y-tgtposition.Y,2));
            }
        }
        return 0;
    }

    // target nearest Ore 
    public void mineNearestOre(TIM.main game){
        entityProperties? tgtCandidate = game.gameMap.findNearestP(this.position.X,this.position.Y,100,"isOre");
        if(tgtCandidate!=null){
            tgtEntity = tgtCandidate.entity;
            trackEntity = true;
            Console.WriteLine("Targeting {0} at {1}|{2}",tgtCandidate.name,tgtCandidate.position.X,tgtCandidate.position.Y);
        }else{
            Console.WriteLine("No Ore in range.");
        }
    }
    public void mine(TIM.main game){
        mineNearestOre(game);
    }

    private int checkMaterials(string entityClass){
        switch(entityClass){
            case "House":{
                try{
                    int[] mat ={0,-4};
                    methods.callMethod("mat_std","IncMaterialsSave",mat,game);
                }catch(System.Reflection.TargetInvocationException){
                    return 0;
                }
                return 1;
            }
            default: return -1;
        }
    }

    public void spawnEntity(string input,TIM.main game){
        string[] inp = input.Split(",");
        try{
            new functionProperties(inp[0]);
        }catch(NotImplementedException){
            return;
        }
        string entityClass;
        string name;
        try{
            entityClass = inp[0];
            name = inp[1];
        }catch(IndexOutOfRangeException){
            Console.WriteLine("Wrong Syntax: spawnEntity(entityClass,name)");
            return;
        }
        
        functionProperties entity=new functionProperties(entityClass);
        switch(checkMaterials(entityClass)){
            case 0:{
                return;
            }case -1:{
                Console.WriteLine("{0} can not be created here.", entityClass);
                return;
            }
            default:{
                try{
                    if(entity.fType.GetMethod("autoSetup") is not null){
                        entity.fType.GetMethod("autoSetup")!.Invoke(entity.fObject, new object[]{this.position,game});
                        game.entities.Add(name,entity);
                        game.sys.displayMap(game);
                    }
                }catch (ArgumentException){
                    Console.WriteLine("INF: Entity '{0}' already exists.",name);
                }
                return;
            }
        }  
    }
    public void nE(string input,TIM.main g){
        spawnEntity(input,game);
    }
}

