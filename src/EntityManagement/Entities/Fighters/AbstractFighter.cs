public abstract class Fighter :Creature{
    public bool isTarget{get;} = true; // creature is target for enemies
    protected abstract int damage{get;} // damage the fighter deals per hit
    protected abstract int hitRange{get;} // maximal distance to target in which fighter can deal damage

    protected abstract float hitSpeed{get;} // in hit per second
    private float timeSinceLastHit = 0; // in seconds

    public override void step(TIM.main game)
    {
        base.step(game);
        if(tgtEntity is not null){
            checkAttack();
        }
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
                // calculate distance via Pythagoras
                return Math.Sqrt(Math.Pow(position.X-tgtposition.X,2)+Math.Pow(position.Y-tgtposition.Y,2));
            }          
        }
        return 0;
    }

    // sets nearest wildlife as targetEntity (in a range of 100)
    public void targetNearestWildlife(TIM.main game){
        entityProperties? tgtCandidate = game.gameMap.findNearestP(this.position.X,this.position.Y,100,"isWildlife");
        if(tgtCandidate!=null){
            tgtEntity = tgtCandidate.entity;
            trackEntity = true;
            Console.WriteLine("Targeting {0} at {1}|{2}",tgtCandidate.name,tgtCandidate.position.X,tgtCandidate.position.Y);
        }else{
            Console.WriteLine("No Wildlife in range.");
        }
    }
    public void hunt(TIM.main game){
        targetNearestWildlife(game);
    }
}
