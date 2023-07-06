using System;
using System.Reflection;

//class to store informations needed for calling functions of game entities
public class functionProperties{
    public object fObject {get; private set;} = null!;
    public Type fType {get; private set;} = null!;
    public functionProperties(){
        setProperties("system");    //if the class isn't given, default to system class
    }
    public functionProperties(string Class){
        setProperties(Class);
    }
    private void setProperties(string Class){ //instanciate a new instance of the class
        Type? TypeN = Type.GetType(Class);
        if(TypeN==null){
            Console.WriteLine("ERR: Classname '{0}' not found!",Class);
            throw new NotImplementedException();
        }
        fType=TypeN;
        ConstructorInfo? functionConstructor = fType.GetConstructor(Type.EmptyTypes);
        if(functionConstructor==null){
            Console.WriteLine("ERR: No constructor for Class '{0}' found", Class);
            throw new NotImplementedException();
        }
        fObject=functionConstructor.Invoke(new object[]{});
    }
}

//basically dark magic. It works, don't touch it. Just kidding, touch it as much as you like, you'll take the curse of of me.
//methods for calling methods using reflection with different constellations of arguments
public class methods{
    public static void callMethod(string obj, string func, string args, TIM.main game){ //call method with arguments with object, function and arguments seperated.
        Type? functionType;
        object? functionObject;
        try{
            try{    //try getting entity properties from the Dictionary
                functionType = game.entities[obj].fType;
                functionObject = game.entities[obj].fObject;
            }catch(KeyNotFoundException){
                Console.WriteLine("ERR: Entity '{0}' not found!", obj);
                return;
            }

            MethodInfo? functionMethod = functionType.GetMethod(func);  //try finding the function we want to call
            if(functionMethod==null){
                Console.WriteLine("ERR: Command '{0}' not found!",func);
                return;
            }
            try{
                functionMethod.Invoke(functionObject, new object[]{args,game}); //call the funktion with parameters
            }catch(System.FormatException){
                Console.WriteLine("ERR: Command syntax incorrect!");
            }
        }catch(System.IndexOutOfRangeException){
            Console.WriteLine("ERR: Command syntax incorrect!");
        }
    }
    public static void callMethod(string obj, string func, TIM.main game){  //call method without arguments, with seperated entity name and function
        Type? functionType;
        object? functionObject;
        try{
            try{    //try getting entity Properties from the Dictionary
                functionType = game.entities[obj].fType;
                functionObject = game.entities[obj].fObject;
            }catch(KeyNotFoundException){
                Console.WriteLine("ERR: Entity '{0}' not found!", obj);
                return;
            }

            MethodInfo? functionMethod = functionType.GetMethod(func);  //try finding the method we want to call
            if(functionMethod==null){
                Console.WriteLine("ERR: Command '{0}' not found!",func);
                return;
            }
            try{
                functionMethod.Invoke(functionObject, new object[]{game});  //call the method without parameters
            }catch(System.FormatException){
                Console.WriteLine("ERR: Command syntax incorrect!");
            }
        }catch(System.IndexOutOfRangeException){
            Console.WriteLine("ERR: Command syntax incorrect!");
        }
    }
    public static void callMethod(string call, TIM.main game){  //call Method from just a input string formated as <entity name>:<function>(<parameters>)
        
        string[] input = call.Split(new char[]{':','(',')'});
        Type? functionType;
        object? functionObject;
        try{
            try{    //try getting entity Properties from the Dictionary
                functionType = game.entities[input[0]].fType;
                functionObject = game.entities[input[0]].fObject;
            }catch(KeyNotFoundException){
                Console.WriteLine("ERR: Entity '{0}' not found!", input[0]);
                return;
            }

            MethodInfo? functionMethod = functionType.GetMethod(input[1]);  //try finding the method we want to call
            if(functionMethod==null){
                Console.WriteLine("ERR: Command '{0}' not found!",input[1]);
                return;
            }
            try{
                if(input[2]==""){   //if no parameters are given
                    functionMethod.Invoke(functionObject, new object[]{game});  //invoke method without parameters
                }else{
                    functionMethod.Invoke(functionObject, new object[]{input[2],game}); //invoke method with parameters
                }
            }catch(TargetInvocationException ex){                        
                if(ex.InnerException is FormatException){
                    Console.WriteLine("ERR: Command syntax incorrect!");
                }else{
                    throw;
                }
            }catch(System.Reflection.TargetParameterCountException){
                Console.WriteLine("ERR: Command syntax incorrect!");
            }
        }catch(System.IndexOutOfRangeException){
            Console.WriteLine("ERR: Command syntax incorrect!");
        }
    }
}
