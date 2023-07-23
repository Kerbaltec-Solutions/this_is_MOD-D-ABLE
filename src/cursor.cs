public class cursor{
    public bool createByPlayer{get;} = false;
    public bool isCursor{get;} = true;
    public bool draw{get;} = true;
    public char mapChar{get;}  = '█';
    public int mapColor{get;} = 3;
    public Position position{get;set;}

    //keep cursor position inside map
    private void normPos(TIM.main game){
        if(position.X<0){position.X=0;}else if(position.X>game.mapsize.X-1){position.X=game.mapsize.X-1;}
        if(position.Y<0){position.Y=0;}else if(position.Y>game.mapsize.Y-1){position.Y=game.mapsize.Y-1;}
    }
    //increment the cursor position by the values provided
    public void iP(string inp, TIM.main game){
        int x=int.Parse(inp.Split(',')[0]);
        int y=int.Parse(inp.Split(',')[1]);
        position.X+=x;
        position.Y+=y;
        normPos(game);
    }
    //set the cursor position to the position provided
    public void sP(string inp, TIM.main game){
        int x=int.Parse(inp.Split(',')[0]);
        int y=int.Parse(inp.Split(',')[1]);
        position.X=x;
        position.Y=y;
        normPos(game);
        game.sys.displayMap(game);
    }
    //print the position of the cursor
    public void gP(TIM.main game){
        Console.WriteLine("{0}|{1}",position.X,position.Y);
    }
    //set the cursor position to the center position of the camera
    public void fP(TIM.main game){
        position.X=game.center_pos.X;
        position.Y=game.center_pos.Y;
        game.sys.displayMap(game);
        gP(game);   //also print the new cursor position
    }
    //list all entities on the map from the cursor outwards
    public void lE(TIM.main game){
        int r=Math.Max(game.mapsize.X,game.mapsize.Y);
        entityProperties[] eArray=game.gameMap.listRadius(position.X,position.Y,r);
        foreach(entityProperties e in eArray){
            if(e.entity!=null){
                Console.WriteLine("{0}: '{1}' of type '{2}' at {3}|{4}",e.m_char,e.name,e.entity.fType,e.position.X,e.position.Y);
            }
        }
    }
    public cursor(){
        position=new Position();
    }
}