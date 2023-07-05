public class map{
    public mapPixel[,] mapArray{get; private set;}
    private int sizeX=0;
    private int sizeY=0;

    //set all values of a pixel
    public void set(Position coord, int color){
        mapArray[coord.X,coord.Y] = new mapPixel();
    }

    //set all values of a pixel using the mapPixel class
    public void set(Position coord, mapPixel values){
        mapArray[coord.X,coord.Y] = values;
    }

    //set the color of a pixel
    public void set_color(Position coord, int color){
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
        Console.Clear();
        for(int y=locMinY; y<locMinY+locSizeY; y+=res){
            for(int x=locMinX; x<locMinX+locSizeX; x+=res){
                if(x<0||y<0||x>=sizeX||y>=sizeY){ //catch the camera showing areas outside the map
                    Console.BackgroundColor = colors.consoleColor(0);
                    Console.Write("  ");
                }else{
                    Console.BackgroundColor = colors.consoleColor(mapArray[x,y].color);
                    Console.Write("  ");
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("");
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

