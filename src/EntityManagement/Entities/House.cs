// entity that can be build by player, that is able to spawn entities that are controlled by the player
using System;
using System.Reflection;
public class House:Entity{
    
    public override bool createByPlayer{get;} = false;
    public bool isHouse{get;} = true;
    public  bool draw{get;} = true;
    public override char mapChar{get;}  = 'H';
    public override int mapColor{get;} = 4;
    public override bool controlledByPlayer{get;} = true; // can the player controll the entity

    public override void setup(string input, TIM.main g){
        base.setup(input, g);
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
                try{
                    int[] mat ={-2,0};
                    methods.callMethod("mat_std","IncMaterialsSave",mat,game);
                }catch(System.Reflection.TargetInvocationException){
                    return 0;
                }
                return 1;
            }
            case "SwordFighter":{
                try{
                    int[] mat ={-2,-2};
                    methods.callMethod("mat_std","IncMaterialsSave",mat,game);
                }catch(System.Reflection.TargetInvocationException){
                    return 0;
                }
                return 1;
            }
            case "BowFighter":{
                try{
                    int[] mat ={-2,-4};
                    methods.callMethod("mat_std","IncMaterialsSave",mat,game);
                }catch(System.Reflection.TargetInvocationException){
                    return 0;
                }
                return 1;
            }
            default: return -1;
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
                Console.WriteLine("Could not create entity.");
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