public abstract class Entity{
    protected TIM.main game = null!;

    public abstract  bool createByPlayer{get;}    //can the entity be created by the player directly
    
    public abstract bool controlledByPlayer{get;} // can the player controll the entity
    public abstract char mapChar{get;}     //character which will represent the entity on the map
    public abstract int mapColor{get;}  //color in which the entity will be drawn on the map

    public Position position{get;set;} = null!;

    //setup function, set position ect.
    public virtual void setup(string input, TIM.main game){
        this.game = game;
        this.position= new Position(Math.Min(int.Parse(input.Split(",")[0]),game.mapsize.X),Math.Min(int.Parse(input.Split(",")[1]),game.mapsize.Y));
    }

    // setup for spawning entities at start of game
    public virtual void autoSetup(Position pos, TIM.main game){
        this.game = game;
        this.position = pos;
    }

    // prints coordinates of position
    public void printPosition(TIM.main game){
        Console.WriteLine("{0}",position);
    }
}