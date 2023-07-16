// spawns entities at start of game
public class SpawnEntities{

    // returns chance of Entitity spawning on a field in promille
    private int getdensity(string entityClass){ 
        switch(entityClass){
            case "Deer": return 5;

            case "Bear": return 5;

            case "Ore": return 100;

            default: return 0;
        }
    }

    public void spawnOre(TIM.main game){
        string entityClass = "Ore";
        System.Random rand = new System.Random();
        for(int x = 0; x < game.mapsize.X; x++){
            for(int y = 0; y< game.mapsize.Y; y++){

                // chance of spawn = getdensity() in promille
                if(rand.Next(10000) <= getdensity(entityClass)){
                    functionProperties entity=new functionProperties(entityClass);
                    var spawnPositionP= entity.fType.GetProperty("position");
                    Position pos = new Position(x,y);
                    Resource  res = game.gameMap.mapArray[pos.X,pos.Y].resource;

                    // only spawns Ore on notWalkable fields
                    if(!res.IsWalkable&& spawnPositionP != null){
                        spawnPositionP.SetValue(entity.fObject,pos);
                        string name =String.Format(entityClass + "_" + game.entities.Count);
                        game.entities.Add(name,entity);
                    }
                }
            }
        }
    }

    public void spawnentities(string Class, TIM.main game){
        System.Random rand = new System.Random();
        for(int x = 0; x < game.mapsize.X; x++){
            for(int y = 0; y< game.mapsize.Y; y++){

                // chance of spawn = getdensity() in promille
                if(rand.Next(10000) <= getdensity(Class)){
                    functionProperties entity=new functionProperties(Class);
                    Position pos = new Position(x,y);
                    Resource res = game.gameMap.mapArray[pos.X,pos.Y].resource;
                    
                    // only spawns entities on walkable fields
                    if(res.IsWalkable){
                        string name = String.Format(Class + "_" + game.entities.Count);
                        try{
                            entity.fType.GetMethod("autoSetup")!.Invoke(entity.fObject, new object[]{pos,game});
                            game.entities.Add(name,entity);
                        }catch(NullReferenceException){}
                    }
                }
            }
        }
    }
}