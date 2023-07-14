public class SwordFighter : Fighter{
    public override  bool createByPlayer{get;} = false;
    public override bool controlledByPlayer{get;} = true;
    public override char mapChar{get;} = 'S';
    public override int mapColor{get;} = 0;

    public override  int maxHealthPoints {get;} = 15;
    public override int healthPoints{get;set;} = 15;
    public override float speed{get;} = 10; // in fields per second

    public override int damage{get;} = 2;
    public override int hitRange{get;} = 2;

    public override float hitSpeed{get;} = (float)0.5; // in hit per seconds

}
