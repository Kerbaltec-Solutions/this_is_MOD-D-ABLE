public class Ore{
    public bool isOre{get;} = true;
    public  bool draw{get;} = true;
    public  bool iterate{get;} = true;
    public  char mapChar{get;}  = '#';
    public  int mapColor{get;} = 15;
    public bool isDestroyed = false;
    public Position position{get;set;} = null!;
    public int money{get;} = 2;

    public int durability{get;set;} = 3; // number of hits until destroyed

    public void getHit(int damage){
        durability -= damage;
        if(durability<=0){
           isDestroyed = true;          
        }
    }

    public void step(TIM.main game){
        if(isDestroyed){
            PlayerMaterials.Money += money; 
            throw new NotSupportedException("Ore was destroyed");
        }
    }

}
