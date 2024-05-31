public abstract class Wildlife: Creature{

    public bool isWildlife{get;} = true;
    public override bool createByPlayer{get;} = false;
    public override bool controlledByPlayer{get;} = false;
    protected  Position Spawnpoint = null!; 
    protected abstract int territoryRange{get;} // radius of territory (around spawnpoint)

    public abstract int[] mat {get;}

    public override void autoSetup(Position pos, TIM.main game){
        this.game = game;
        position = pos;
        Spawnpoint = pos;
    }
    public override void step(TIM.main game)
    {
        if(!trackEntity && route == null){
            initIdleMovement(game);
        }
        if(isDead){
            methods.callMethod("mat_std","IncMaterials",mat,game);
        }
        base.step(game);
    }

    // initializes new route to a point in the territory
   protected void initIdleMovement(TIM.main game){
        System.Random rand = new System.Random();
        int targetX = Spawnpoint.X + rand.Next(-territoryRange-1,territoryRange+1);
        int targetY = Spawnpoint.Y + rand.Next(-territoryRange-1,territoryRange+1);
        if(targetX >= game.mapsize.X){
            targetX = game.mapsize.X -1;
        }
        if(targetY >= game.mapsize.Y){
            targetY = game.mapsize.Y -1;
        }
        target = new Position(targetX,targetY);
        setRoute(game.gameMap.mapArray);
    }
}