public class map{
    public mapPixel[,] mapArray{get; private set;}
    private int sizeX=0;
    private int sizeY=0;

    public List<entityProperties>[,] mapEntities{get; private set;}

    //set resource value of a pixel
    public void set(Position coord, Resource resource){
        mapArray[coord.X,coord.Y] = new mapPixel(resource);
    }

    //set all values of a pixel using the mapPixel class
    public void set(Position coord, mapPixel values){
        mapArray[coord.X,coord.Y] = values;
    }

    //set the color of a pixel
    public void set(Position coord, int color){
        mapArray[coord.X,coord.Y].color = color;
    }

    //get the color of a pixel
    public int get_color(Position coord){
        return mapArray[coord.X,coord.Y].color;
    }

    //get all values of a pixel using the mapPixel class
    public mapPixel get(Position coord){
        return mapArray[coord.X,coord.Y];
    }

    //initialize the map
    public map(int sX, int sY){
        sizeX=sX;
        sizeY=sY;
        mapArray = new mapPixel [sizeX,sizeY];
        for(int x=0;x<sizeX;x++){
            for(int y=0;y<sizeY;y++){
                mapArray[x,y]=new mapPixel();
            }
        }
        mapEntities = new List<entityProperties>[sizeX,sizeY];
        for(int y=0; y<sizeY; y++){
            for(int x=0; x<sizeX; x++){
                mapEntities[x,y] = new List<entityProperties> ();
            }
        }
    }

    //fill the map
    public void createTerrain(){
        MapGenerator generator = new MapGenerator();
        generator.generateTerrain(this.mapArray);
        
    }

    //print all values of all pixels to the console
    public void print(){
        Console.WriteLine("{0},{1}",sizeX,sizeY);
        foreach(mapPixel pixel in mapArray){
            pixel.print();
        }
    }

    public void save(string filename){
        BinaryWriter bw;
         
        //create the file
        try {
            bw = new BinaryWriter(new FileStream(filename, FileMode.Create));
        } catch (IOException e) {
            Console.WriteLine(e.Message + "\n Cannot create file.");
            return;
        }
        
        //writing into the file
        try {
            bw.Write((byte)sizeX);
            bw.Write((byte)sizeY);
            foreach(mapPixel pixel in mapArray){
                bw.Write((byte)pixel.color);
            }
        } catch (IOException e) {
            Console.WriteLine(e.Message + "\n Cannot write to file.");
            return;
        }
        bw.Close();
        Console.WriteLine("Map saved to "+filename);
    }

    public void load(string filename){
        BinaryReader br;
         
        try {
            br = new BinaryReader(new FileStream(filename, FileMode.Open));
        } catch (IOException e) {
            Console.WriteLine(e.Message + "\n Cannot open file.");
            return;
        }
         
        try {
            sizeX = br.ReadByte();
            sizeY = br.ReadByte();
            foreach(mapPixel pixel in mapArray){
                
                pixel.resource = new Resource((Resource.ResourceType)br.ReadByte());
                pixel.color = pixel.resource.getColor();
            }
        } catch (IOException e) {
            Console.WriteLine(e.Message + "\n Cannot read from file.");
            return;
        }
        br.Close();
        Console.WriteLine("Map loaded from "+filename);
    }

    //display the map (and entities in the map)
    public void display(int locMinX, int locMinY, int locSizeX, int locSizeY, int res, TIM.main game){
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
        updateMap(game);
        for(int y=locMinY; y<locMinY+locSizeY; y+=res){     //iterate through the rows and colums of the map we can see
            for(int x=locMinX; x<locMinX+locSizeX; x+=res){
                
                if(x<0||y<0||x>=sizeX||y>=sizeY){   //catch the camera showing areas outside of the map
                    Console.BackgroundColor = colors.consoleColor(0);
                    Console.Write("  ");
                }else{
                    entityProperties[] pixelEntities= new entityProperties[2]{new entityProperties(), new entityProperties()};  //create a new array to store the entities in this pixel, that will be shown
                    int entityCnt=0;
                    Console.BackgroundColor = colors.consoleColor(mapArray[x,y].color);
                    for(int iy=y; iy<y+game.zoom&&iy<sizeY; iy++){      //iterate through all map rows and colums, that fall into one pixel due to zoom
                        for(int ix=x; ix<x+game.zoom&&ix<sizeX; ix++){
                            foreach(entityProperties e in mapEntities[ix,iy]){
                                if(entityCnt<2){    //2 Entities can be shown per pixel
                                    pixelEntities[entityCnt].m_char=e.m_char;
                                    pixelEntities[entityCnt].m_color=e.m_color; //save their important properties to the array
                                    entityCnt++;
                                }else{
                                    entityCnt++;    //else just count how many entities there are
                                }
                            }
                        }
                    }

                    if(entityCnt<3){    //if all entities can be shown
                        //show the first entity
                        if(mapArray[x,y].color==pixelEntities[0].m_color){  //if the background color would be the same as the foreground color
                            if(pixelEntities[0].m_color%2==0){
                                Console.ForegroundColor = colors.consoleColor(pixelEntities[0].m_color+1);  //slightly alter the color of the entitiy
                            }else{
                                Console.ForegroundColor = colors.consoleColor(pixelEntities[0].m_color-1);
                            }
                        }else{
                            Console.ForegroundColor = colors.consoleColor(pixelEntities[0].m_color);    //else set the foreground color to the color of the entity
                        }
                        Console.Write(pixelEntities[0].m_char);
                        
                        //show the secondv entity
                        if(mapArray[x,y].color==pixelEntities[1].m_color){
                            if(pixelEntities[0].m_color%2==0){
                                Console.ForegroundColor = colors.consoleColor(pixelEntities[1].m_color+1);
                            }else{
                                Console.ForegroundColor = colors.consoleColor(pixelEntities[1].m_color-1);
                            }
                        }else{
                            Console.ForegroundColor = colors.consoleColor(pixelEntities[1].m_color);
                        }
                        Console.Write(pixelEntities[1].m_char);
                        
                    }else{  //if not all entities can be shown, the console color is yellow on black to show this
                        Console.ForegroundColor = colors.consoleColor(11);
                        Console.BackgroundColor = colors.consoleColor(0);
                        Console.Write(pixelEntities[0].m_char);     //draw the entities
                        Console.Write(pixelEntities[1].m_char);
                    }
                }
            }
            Console.BackgroundColor=colors.consoleColor(0);
            Console.WriteLine("");  //start next line of the map
        }
        Console.WriteLine("");
    }

    // Function to det data of all entities on the map
    public void updateMap(TIM.main game){
        for(int x=0; x<sizeX; x++){
            for(int y=0; y<sizeY; y++){
                mapEntities[x,y]=new List<entityProperties>();  //clear the entity list
            }
        }
        foreach(KeyValuePair<string, functionProperties> entry in game.entities){
            functionProperties entity = entry.Value;
            var boolVar= entity.fType.GetProperty("draw");  
            if(boolVar!=null){  //if the entity could be drawable
                var boolVal= boolVar.GetValue(entity.fObject, null);
                bool? hasPos = (bool?)boolVal;
                if(hasPos==true){   //if the entity is drawable
                    var posVar= entity.fType.GetProperty("position");   //get the position on the map
                    if(posVar==null){
                        throw new NotImplementedException("FATAL: a entity does not contain a position neccesary for drawing on the map!");
                    }
                    var posVal= posVar.GetValue(entity.fObject, null);
                    if(posVal==null){
                        throw new NotImplementedException("FATAL: a entity does not contain a position neccesary for drawing on the map!");
                    }
                    Position position = (Position)posVal;

                    var charVar= entity.fType.GetProperty("mapChar");   //get the map character
                    if(charVar==null){
                        throw new NotImplementedException("FATAL: a entity does not contain a character neccesary for drawing on the map!");
                    }
                    var charVal= charVar.GetValue(entity.fObject, null);
                    if(charVal==null){
                        throw new NotImplementedException("FATAL: a entity does not contain a character neccesary for drawing on the map!");
                    }
                    char mapChar = (char)charVal;

                    var colorVar= entity.fType.GetProperty("mapColor"); //get the map color
                    if(colorVar==null){
                        throw new NotImplementedException("FATAL: a entity does not contain a color neccesary for drawing on the map!");
                    }
                    var colorVal= colorVar.GetValue(entity.fObject, null);
                    if(colorVal==null){
                        throw new NotImplementedException("FATAL: a entity does not contain a color neccesary for drawing on the map!");
                    }
                    int mapColor = (int)colorVal;
                    
                    mapEntities[position.X,position.Y].Add(new entityProperties(mapChar,mapColor,entity,position,entry.Key));   //add the information to the list
                }
            }
        }
    }
  
    public entityProperties[] listRadius(int x, int y, int r){
        List<entityProperties> eList=new List<entityProperties>();
        for(int i=0; i<=r; i++){
            if(i==0){
                try{
                    foreach(entityProperties e in mapEntities[x,y]){
                        eList.Add(e);
                    }
                }catch(System.IndexOutOfRangeException){}
            }
            for(int dx=-i;dx<i;dx++){
                try{
                    foreach(entityProperties e in mapEntities[x+dx,y+i]){
                        eList.Add(e);
                    }
                }catch(System.IndexOutOfRangeException){}
            }
            for(int dx=-i+1;dx<=i;dx++){
                try{
                    foreach(entityProperties e in mapEntities[x+dx,y-i]){
                        eList.Add(e);
                    }
                }catch(System.IndexOutOfRangeException){}
            }
            for(int dy=-i;dy<i;dy++){
                try{
                    foreach(entityProperties e in mapEntities[x+i,y+dy]){
                        eList.Add(e);
                    }
                }catch(System.IndexOutOfRangeException){}
            }
            for(int dy=-i+1;dy<=i;dy++){
                try{
                    foreach(entityProperties e in mapEntities[x-i,y+dy]){
                        eList.Add(e);
                    }
                }catch(System.IndexOutOfRangeException){}
            }
        }
        return eList.ToArray();
    }

    // return true if the entity has the property and its value is true, else return false
    private bool unpackBool(entityProperties e, string P){
        if(e.entity is null){return false;}
        functionProperties entity = e.entity;
        var boolVar= entity.fType.GetProperty(P);
        if(boolVar!=null){
            var boolVal= boolVar.GetValue(entity.fObject, null)!;
            bool b = (bool)boolVal;
            if(b){
                return true;
            }
        }
        return false;
    }

    // find nearest entity to a position in a given radius
    public entityProperties? findNearestP(int x, int y, int r, string P){
        for(int i=0; i<=r; i++){
            for(int dx=-i;dx<=i;dx++){
                try{
                    foreach(entityProperties e in mapEntities[x+dx,y+i]){
                        if(unpackBool(e,P)){
                            return(e);
                        }
                    }
                }catch(System.IndexOutOfRangeException){}
            }
            for(int dx=-i;dx<=i;dx++){
                try{
                    foreach(entityProperties e in mapEntities[x+dx,y-i]){
                        if(unpackBool(e,P)){
                            return(e);
                        }
                    }
                }catch(System.IndexOutOfRangeException){}
            }
            for(int dy=-i;dy<=i;dy++){
                try{
                    foreach(entityProperties e in mapEntities[x+i,y+dy]){
                        if(unpackBool(e,P)){
                            return(e);
                        }
                    }
                }catch(System.IndexOutOfRangeException){}
            }
            for(int dy=-i;dy<=i;dy++){
                try{
                    foreach(entityProperties e in mapEntities[x-i,y+dy]){
                        if(unpackBool(e,P)){
                            return(e);
                        }
                    }
                }catch(System.IndexOutOfRangeException){}
            }
        }
        return null;
    }
}

//mapPixel class
public class mapPixel{
    public int color{get; set;} //color of the Pixel
    public Resource resource{get; set;} //resource of Pixel

    //initialize the Pixel
    public mapPixel(Resource res){
        color= res.getColor();
        resource=res;
    }
    public mapPixel(){
        resource=new Resource();
        color= resource.getColor();
    }

    //print the values of the Pixel to the console
    public void print(){
        string output=resource.ToString();
        Console.WriteLine(output);
    }
}

