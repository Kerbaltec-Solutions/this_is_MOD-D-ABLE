public abstract class Wildlife: Creature{

    public bool isWildlife{get;} = true;
    public override bool createByPlayer{get;} = false;
    public override bool controlledByPlayer{get;} = false;
    public abstract Position Spawnpoint{get;set;}
    public abstract int territoryRange{get;} // radius of territory (around spawnpoint)

    public override void setup(string input, TIM.main game){
        this.game = game;
        position = Spawnpoint;
    }
    public override void step(TIM.main game)
    {
        if(!trackEntity && route == null){
            initIdleMovement(game);
        }
        base.step(game);
    }

    // initializes new route to a point in the territory
    public void initIdleMovement(TIM.main game){
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