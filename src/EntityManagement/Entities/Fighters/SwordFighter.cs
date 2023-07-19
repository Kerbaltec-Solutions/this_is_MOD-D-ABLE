public class SwordFighter : Fighter{
    public override  bool createByPlayer{get;} = false;
    public override bool controlledByPlayer{get;} = true;
    public override char mapChar{get;} = 'S';
    public override int mapColor{get;} = 0;

    protected override  int maxHealthPoints {get;} = 15;
    protected override int healthPoints{get;set;} = 15;
    protected override float speed{get;} = 10; // in fields per second

    protected override int damage{get;} = 2;
    protected override int hitRange{get;} = 2;

    protected override float hitSpeed{get;} = (float)0.5; // in hit per seconds

}
