public class map
{
    public mapPixel[,] mapArray {get; private set;}
    private int sizeX = 0;
    private int sizeY = 0;

    public List<entityProperties>[,] mapEntities {get; private set;}

    // set resource value of a pixel
    public void set(Position coord, Resource resource) => 
        mapArray[coord.X,coord.Y] = new mapPixel(resource);

    // set all values of a pixel using the mapPixel class
    public void set(Position coord, mapPixel values) => 
        mapArray[coord.X,coord.Y] = values;

    // set the color of a pixel
    public void set(Position coord, int color) => 
        mapArray[coord.X,coord.Y].color = color;

    // get all values of a pixel using the mapPixel class
    public mapPixel this[int x, int y]
    {
        get { return mapArray[x, y]; }
        set { mapArray[x, y] = value; }
    }

    public mapPixel this[Position coord] 
    {
        get { return this[coord.X, coord.Y]; }
        set { this[coord.X, coord.Y] = value; }
    }

    // initialize the map
    public map(int sX, int sY)
    {
        sizeX = sX;
        sizeY = sY;

        mapArray = new mapPixel[sizeX, sizeY];
        mapEntities = new List<entityProperties>[sizeX, sizeY];        
        for (int y = 0; y < sizeY; y++)
            for (int x = 0; x < sizeX; x++)
            {
                mapArray[x, y] = new mapPixel();
                mapEntities[x, y] = new List<entityProperties> ();
            }
    }

    // fill the map
    public void createTerrain()
    {
        MapGenerator generator = new MapGenerator();
        generator.generateTerrain(this.mapArray);
    }

    // print all values of all pixels to the console
    public void print()
    {
        Console.WriteLine("{0},{1}",sizeX,sizeY);
        foreach(mapPixel pixel in mapArray)
            pixel.print();
    }
    
    // save map to file
    public void save(string filename)
    {
        try 
        {
            using var bw = new BinaryWriter(new FileStream(filename, FileMode.Create));
            bw.Write((byte)sizeX);
            bw.Write((byte)sizeY);
            foreach (mapPixel pixel in mapArray)
                bw.Write((byte)pixel.color);
        }
        catch (IOException e) 
        {
            Console.WriteLine(e.Message + "\n Failed to save to file.");
            return;
        }

        Console.WriteLine("Map saved to "+filename);
    }

    // load map from file
    public void load(string filename)
    {
        try
        {
            using var br = new BinaryReader(new FileStream(filename, FileMode.Open));
            sizeX = br.ReadByte();
            sizeY = br.ReadByte();
            foreach (mapPixel pixel in mapArray)
            {
                pixel.resource = new Resource((Resource.ResourceType)br.ReadByte());
                pixel.color = pixel.resource.getColor();
            }
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message + "\n Failed to read from file.");
            return;
        }

        Console.WriteLine("Map loaded from " + filename);
    }

    // display the map (and entities in the map)
    public void display(int locMinX, int locMinY, int locSizeX, int locSizeY, int res, TIM.main game)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
        updateMap(game);
        
        // iterate through the rows and colums of the map we can see
        for (int y = locMinY; y < locMinY + locSizeY; y += res)     
        {     
            for (int x = locMinX; x < locMinX + locSizeX; x += res)
            {
                // catch the camera showing areas outside of the map
                if (x < 0 || y < 0 || x >= sizeX || y >= sizeY)             
                {   
                    Write(0, 0, ' ');
                    continue;
                }

                // create a new array to store the entities in this pixel, that will be shown
                entityProperties[] pixelEntities = new entityProperties[2]{new entityProperties(), new entityProperties()};  
                int entityCnt = 0;
                
                // iterate through all map rows and colums, that fall into one pixel due to zoom
                for (int iy = y; iy < y + game.zoom && iy < sizeY; iy++)        
                    for (int ix = x; ix < x + game.zoom && ix < sizeX; ix++)
                        foreach (entityProperties e in mapEntities[ix, iy])
                        {
                            if (entityCnt >= 2) break;      // only 2 Entities can be shown per pixel    

                            // save their relevant properties to the array
                            pixelEntities[entityCnt].m_char  = e.m_char;
                            pixelEntities[entityCnt].m_color = e.m_color;   
                            entityCnt++;
                        }

                // show entities - if not all can be shown, the console color is yellow on black
                Write(entityCnt < 3 ? pixelEntities[0].m_color : 11, entityCnt < 3 ? mapArray[x, y].color : 0, pixelEntities[0].m_char, true);
                Write(entityCnt < 3 ? pixelEntities[1].m_color : 11, entityCnt < 3 ? mapArray[x, y].color : 0, pixelEntities[1].m_char, true);
            }
            Write(-1, -1, ' ');
            Console.BackgroundColor = colors.consoleColor(0);
            Console.WriteLine("");  // start next line of the map
        }
        Console.WriteLine("");
    }

    // Function to det data of all entities on the map
    public void updateMap(TIM.main game)
    {
        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
                mapEntities[x, y] = new List<entityProperties>();  // clear the entity list

        foreach(KeyValuePair<string, functionProperties> entry in game.entities)
        {
            functionProperties entity = entry.Value;

            var boolVar = entity.fType.GetProperty("draw");  
            if (boolVar == null) continue;      // entity is not drawable

            bool? hasPos = (bool?)boolVar.GetValue(entity.fObject, null);
            if (!(bool)hasPos || hasPos == null) continue;      // entity is not drawable

            // get the position on the map
            var posVar = entity.fType.GetProperty("position");   
            if (posVar == null)
                throw new NotImplementedException("FATAL: a entity does not contain a position neccesary for drawing on the map!");
            
            var posVal = posVar.GetValue(entity.fObject, null);
            if (posVal == null)
                throw new NotImplementedException("FATAL: a entity does not contain a position neccesary for drawing on the map!");

            Position position = (Position)posVal;

            // get the map character
            var charVar = entity.fType.GetProperty("mapChar");   
            if (charVar == null)
                throw new NotImplementedException("FATAL: a entity does not contain a character neccesary for drawing on the map!");

            var charVal = charVar.GetValue(entity.fObject, null);
            if (charVal == null)
                throw new NotImplementedException("FATAL: a entity does not contain a character neccesary for drawing on the map!");

            char mapChar = (char)charVal;

            // get the map color
            var colorVar = entity.fType.GetProperty("mapColor"); 
            if (colorVar == null)
                throw new NotImplementedException("FATAL: a entity does not contain a color neccesary for drawing on the map!");

            var colorVal = colorVar.GetValue(entity.fObject, null);
            if (colorVal == null)
                throw new NotImplementedException("FATAL: a entity does not contain a color neccesary for drawing on the map!");

            int mapColor = (int)colorVal;
            
            // add the information to the list
            mapEntities[position.X,position.Y].Add(new entityProperties(mapChar, mapColor, entity, position, entry.Key));   
        }
    }
  
    public entityProperties[] listRadius(int x, int y, int r)
    {
        List<entityProperties> eList = new List<entityProperties>();
        for (int i = 0; i <= r; i++)
        {
            if (i == 0)
                eList.AddRange(mapEntities[x, y]);

            for (int dx = -i; dx < i; dx++)
                eList.AddRange(mapEntities[x + dx, y + i]);
            
            for(int dx = -i + 1; dx <= i; dx++)
                eList.AddRange(mapEntities[x + dx, y - i]);

            for(int dy = -i; dy < i; dy++)
                eList.AddRange(mapEntities[x + i, y + dy]);
                
            for(int dy = -i + 1; dy <= i; dy++)
                eList.AddRange(mapEntities[x - i, y + dy]);
        }
        return eList.ToArray();
    }

    // return true if the entity has the property and its value is true, else return false
    private bool unpackBool(entityProperties e, string P)
    {
        if (e.entity is null) return false;

        functionProperties entity = e.entity;
        var boolVar = entity.fType.GetProperty(P);
        if (boolVar is null) return false;

        return (bool)boolVar.GetValue(entity.fObject, null);
    }

    // find nearest entity to a position in a given radius
    public entityProperties? findNearestP(int x, int y, int r, string P)
    {
        for (int i = 0; i <= r; i++)
            for (int d = -i; d <= i; d++)
            {
                foreach (entityProperties e in mapEntities[x + d, y + i])
                    if (unpackBool(e,P)) return e;

                foreach (entityProperties e in mapEntities[x + d, y - i])
                    if (unpackBool(e,P)) return e;

                foreach (entityProperties e in mapEntities[x + i, y + d])
                    if (unpackBool(e,P)) return e;

                foreach (entityProperties e in mapEntities[x - i, y + d])
                    if (unpackBool(e,P)) return e;
            }

        return null;
    }

    static int last_fgColor = -1;
    static int last_bgColor = -1;
    static string textBuffer = "";
    static void Write(int fgColor, int bgColor, char c, bool contrast = false)
    {
        // if the background color would be the same as the foreground color, slightly alter the foreground color
        if (contrast && fgColor == bgColor)
            fgColor += fgColor % 2 * 2 - 1;

        if (fgColor != last_fgColor || bgColor != last_bgColor)
        {
            if (last_fgColor != -1 && last_bgColor != -1)
            {
                Console.ForegroundColor = colors.consoleColor(last_fgColor);
                Console.BackgroundColor = colors.consoleColor(last_bgColor);
                Console.Write(textBuffer);
                textBuffer = string.Empty;
            }
            last_fgColor = fgColor;
            last_bgColor = bgColor;
        }
        textBuffer += c;
    }
}


// mapPixel class
public class mapPixel
{
    public int color {get; set;}             // color of the Pixel
    public Resource resource {get; set;}     // resource of Pixel

    // initialize the Pixel
    public mapPixel(Resource res)
    {
        color = res.getColor();
        resource = res;
    }

    public mapPixel()
    {
        resource = new Resource();
        color = resource.getColor();
    }

    // print the values of the Pixel to the console
    public void print()
    {
        string output = color.ToString();
        Console.WriteLine(output);
    }
}
