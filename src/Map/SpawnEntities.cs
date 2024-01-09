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

    public void initialspawn(TIM.main game){
        game.entities = new Dictionary<string,functionProperties>();
        game.entities.Add("sys",new functionProperties());
        game.entities.Add("c",new functionProperties("cursor")); //add a cursor entity as a helping tool for the player
        methods.callMethod("c","fP",game);  //let the cursor find the center position of the camera
        game.entities.Add("mat_std",new functionProperties("PlayerMaterials"));
        int[] res={4,4};
        methods.callMethod("mat_std","SetMaterials",res,game);
        Console.Write("Name your first Worker: ");
        string? firstWorker = null;
        while(firstWorker==null){
            firstWorker = Console.ReadLine();
        }
        game.entities.Add(firstWorker,new functionProperties("Worker"));
        methods.callMethod(firstWorker, "setup", game.center_pos.X.ToString()+","+game.center_pos.Y.ToString(), game);
        game.respawn_timer = 0;
    }
}