public class Bear: Wildlife{
    public override char mapChar{get;} = 'E';
    public override int mapColor{get;} = 4;

    public override  int maxHealthPoints {get;} = 5;
    public override int healthPoints{get;set;} = 5;
    public override float speed{get;} = 7; // in fields per second
    public override int territoryRange{get;} = 10;

    public override int food{get;} = 4;

    public int maxDistanceToSpawn {get{return 3* territoryRange;}} // maximal distance the entity can have to spawn

    public  int damage{get;} = 1; // damage the fighter deals per hit
    public  int hitRange{get;} = 1; // maximal distance to target in which fighter can deal damage

    public  float hitSpeed{get;} = (float)0.5; // in hit per second
    private float timeSinceLastHit = 0; // in seconds

    public override void step(TIM.main game)
    {
        if(!trackEntity){
            checkTerritory(game);
        }

        base.step(game);
        checkAttack();
    }

    public override void updateMovement(mapPixel[,] maparr)
    {
        if(getDistanceToSpawn()> 3*territoryRange){ // if position is too far away from spawn, return to territory
            trackEntity = false;
            tgtEntity = null;
            initIdleMovement(game);
            return;
        }
        base.updateMovement(maparr);
    }

    // check territory for player-controlled entities to attack
    private void checkTerritory(TIM.main game){
        entityProperties? tgtCandidate = game.gameMap.findNearestP(this.position.X,this.position.Y,territoryRange + 1,"controlledByPlayer");
        if(tgtCandidate!=null){
            tgtEntity = tgtCandidate.entity;
            trackEntity = true;}
        return;
    }

    // check whether the target can be hit
    private void checkAttack(){
        if(target is not null && trackEntity){
            // check if target in range and fighter able to hit again
            if(getDistanceToTarget() < hitRange){ 
                if(this.timeSinceLastHit* hitSpeed >= 1){
                        float steps = timeSinceLastHit*hitSpeed;
                        this.timeSinceLastHit = 0;
                        for(int i = 0; i< steps;i++){
                            hitTarget(game);
                        }                
                    }   
            }
            this.timeSinceLastHit += TIM.main.STEPTIME;  
        }        
    }

    // deal damage to target
    private void hitTarget(TIM.main game){
        if(tgtEntity is not null){
            try{
                tgtEntity.fType.GetMethod("getHit")!.Invoke(tgtEntity.fObject, new object[]{damage});
                var tgtHPP = tgtEntity.fType.GetProperty("healthPoints");
                if(tgtHPP != null){
                    int tgtHP = (int)tgtHPP.GetValue(tgtEntity.fObject, null)!;
                    if(tgtHP <= 0){
                        tgtEntity = null;
                    }
                }
            }catch(NullReferenceException){}
            
        }        
    }

    // returns current distance to target, if no target assigned (or target has no Position) returns 0
    private double getDistanceToTarget(){
        if(tgtEntity is not null){
            var val = tgtEntity.fType.GetProperty("position");
            if(val is not null){
                Position tgtposition= (Position) val.GetValue(tgtEntity.fObject, null)!;
                // calculate distance via 2 positions via Pythagoras
                return Math.Sqrt(Math.Pow(position.X-tgtposition.X,2)+Math.Pow(position.Y-tgtposition.Y,2));
            }          
        }
        return 0;
    }

    private double getDistanceToSpawn(){
        return Math.Sqrt(Math.Pow(position.X-Spawnpoint.X,2)+Math.Pow(position.Y-Spawnpoint.Y,2));
    }
}

    
