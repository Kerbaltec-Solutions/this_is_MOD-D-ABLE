// keeps track of the materials a player has gathered 
//(food through killing wildlife and money through mining Ore)
public class PlayerMaterials{
    public  int Food{get; set;} = 4;
    public  int Money{get; set;} = 4;

    public PlayerMaterials(int food, int money){
        this.Food = food;
        this.Money = money;
    }

}