public class Ore{
    public bool isOre{get;} = true;
    public  bool draw{get;} = true;
    public  bool iterate{get;} = true;
    public  char mapChar{get;}  = '#';
    public  int mapColor{get;} = 15;
    public bool isDestroyed = false;
    public Position position{get;set;} = null!;
    public int[] mat ={0,2};

    public int durability{get;set;} = 3; // number of hits until destroyed

    public void getHit(int damage){
        durability -= damage;
        if(durability<=0){
           isDestroyed = true;
        }
    }

    public void step(TIM.main game){
        if(isDestroyed){
            methods.callMethod("mat_std","IncMaterials",mat,game);
            throw new NotSupportedException("Ore was destroyed");
        }
    }

}
