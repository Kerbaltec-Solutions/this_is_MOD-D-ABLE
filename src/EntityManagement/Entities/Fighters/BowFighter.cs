public class BowFighter : Fighter{
    public override  bool createByPlayer{get;} = false;
    public override bool controlledByPlayer{get;} = true;
    public override char mapChar{get;} = 'B';
    public override int mapColor{get;} = 0;

    protected override  int maxHealthPoints {get;} = 9;
    protected override int healthPoints{get;set;} = 9;
    protected override float speed{get;} = 10; // in fields per second

    protected override int damage{get;} = 1;
    protected override int hitRange{get;} = 10; // range of bow in fields

    private float optimalDistanceToTarget = 8; // distance to target that bow aims to have

    protected override float hitSpeed{get;} = (float)0.5; // in hit per seconds

    protected override void updateMovement(mapPixel[,] maparr)
    {
        if(tgtEntity != null){
            var posP= tgtEntity.fType.GetProperty("position");
            if(posP != null){
                var posVal= posP.GetValue(tgtEntity.fObject, null);
                // find target position based on targetEntity
                target = getTargetPos((Position)posVal!);
                setRoute(maparr);
            }
        }
    }

    // BowFighter needs to keep its distance to target entity -> target point is always 
    // a certain distance away from target (Point on line between BowFighter and target Entity)
    private Position getTargetPos(Position targetE){
        float vectorX = position.X - targetE.X ;
        float vectorY = position.Y - targetE.Y ;

        // if target and fighter stand on the same field fighter will always flee right
        if(vectorX == 0 && vectorY == 0){
            vectorX = 1;
        }

        // calculate norm and multiply by optimal distance to get vector to optimal position
        // tgtEntity------>-----------x------------this.position
        //            norm Vector   ideal position
        vectorX =vectorX*(float)(1/Math.Sqrt(Math.Pow(vectorX,2)+ Math.Pow(vectorY,2)))*optimalDistanceToTarget;
        vectorY =vectorY*(float)(1/Math.Sqrt(Math.Pow(vectorX,2)+ Math.Pow(vectorY,2)))*optimalDistanceToTarget;

        Position tgt = new Position((int)(targetE.X+vectorX),(int)(targetE.Y+vectorY));
        return tgt;
    }

}
