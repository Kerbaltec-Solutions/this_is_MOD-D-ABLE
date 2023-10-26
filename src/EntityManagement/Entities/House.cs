// entity that can be build by player, that is able to spawn entities that are controlled by the player
public class House{
    private TIM.main game = null!;
    public bool createByPlayer{get;} = true;
    public bool isHouse{get;} = true;
    public  bool draw{get;} = true;
    public  char mapChar{get;}  = 'H';
    public  int mapColor{get;} = 4;
    public Position position{get;set;} = null!;

    public void setup(string input, TIM.main g){
        game=g;
        this.position= new Position(Math.Min(int.Parse(input.Split(",")[0]),game.mapsize.X),Math.Min(int.Parse(input.Split(",")[1]),game.mapsize.Y));
        if(!game.gameMap.mapArray[position.X,position.Y].resource.IsWalkable){
            throw new NotSupportedException("Position not valid");
        }
    }

    // checks wether the player has enough materials to spawn an entity, if he does: returns 1 
    // if player is not able to afford entity: returns 0
    // if house is not able to spawn an entityClass: returns -1
    private int checkMaterials(string entityClass){
        switch(entityClass){
            case "Worker":{
                if(game.materials.Food >= 2){
                    return 1;
                }else{return 0;}
            }
            case "SwordFighter":{
                if(game.materials.Food >= 2 && game.materials.Money >= 2){
                    return 1;
                }else{return 0;}
            }
            case "BowFighter":{
                if(game.materials.Food >= 2 && game.materials.Money >= 4){
                    return 1;
                }else{return 0;}
            }
            default: return -1;
        }
    }

    private void subtractMaterials(string entityClass){
        switch(entityClass){
            case "Worker":{            
                game.materials.Food-= 2;
                break;
            }
            case "SwordFighter":{
                game.materials.Food -= 2;
                game.materials.Money -= 2;
                break;
            }
            case "BowFighter":{
                game.materials.Food-= 2;
                game.materials.Money -= 4;
                break;
            }
            default: return;
        }
    }


    // function that spawns an entity, can be called by player
    public void spawnEntity(string input,TIM.main game){
        string[] inp = input.Split(",");
        try{
            new functionProperties(inp[0]);
        }catch(NotImplementedException){
            return;
        }
        string entityClass;
        string name;
        try{
            entityClass = inp[0];
            name = inp[1];
        }catch(IndexOutOfRangeException){
            Console.WriteLine("Wrong Syntax: spawnEntity(entityClass,name)");
            return;
        }
        
        functionProperties entity=new functionProperties(entityClass);
        switch(checkMaterials(entityClass)){
            case 0:{
                Console.WriteLine("Not enough resources");
                return;
            }case -1:{
                Console.WriteLine("{0} can not be created here.", entityClass);
                return;
            }
            default:{
                try{
                    if(entity.fType.GetMethod("autoSetup") is not null){
                        entity.fType.GetMethod("autoSetup")!.Invoke(entity.fObject, new object[]{this.position,game});
                        game.entities.Add(name,entity);
                        subtractMaterials(entityClass);
                        game.sys.displayMap(game);
                    }
                }catch (ArgumentException){
                    Console.WriteLine("INF: Entity '{0}' already exists.",name);
                }
                return;
            }
        }  
    }
    public void nE(string input,TIM.main g){
        spawnEntity(input,game);
    }
}