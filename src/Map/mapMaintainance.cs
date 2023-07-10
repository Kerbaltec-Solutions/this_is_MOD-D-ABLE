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
        foreach(mapPixel pixel in mapArray){
            pixel.print();
        }
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
    public void updateMap(TIM.main game){
        for(int x=0; x<sizeX; x++){
            for(int y=0; y<sizeY; y++){
                mapEntities[x,y]=new List<entityProperties>();
            }
        }
        foreach(KeyValuePair<string, functionProperties> entry in game.entities){
            functionProperties entity = entry.Value;
            var boolVar= entity.fType.GetProperty("draw");
            if(boolVar!=null){
                var boolVal= boolVar.GetValue(entity.fObject, null);
                bool hasPos = (bool)boolVal;
                if(hasPos==true){
                    var posVar= entity.fType.GetProperty("position");
                    var posVal= posVar.GetValue(entity.fObject, null);
                    Position position = (Position)posVal;

                    var charVar= entity.fType.GetProperty("mapChar");
                    var charVal= charVar.GetValue(entity.fObject, null);
                    char mapChar = (char)charVal;

                    var colorVar= entity.fType.GetProperty("mapColor");
                    var colorVal= colorVar.GetValue(entity.fObject, null);
                    int mapColor = (int)colorVal;
                    
                    mapEntities[position.X,position.Y].Add(new entityProperties(mapChar,mapColor,entity,position,entry.Key));
                }
            }
        }
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
        string output=color.ToString();
        Console.WriteLine(output);
    }
}

