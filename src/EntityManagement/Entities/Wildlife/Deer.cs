public class Deer: Wildlife{
    public override char mapChar{get;} = 'F';
    public override int mapColor{get;} = 8;

    protected override  int maxHealthPoints {get;} = 10;
    protected override int healthPoints{get;set;} = 10;
    protected override float speed{get;} = 7; // in fields per second
    protected override int territoryRange{get;} = 10;

    public override int food{get;} = 2;

}