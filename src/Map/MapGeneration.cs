using System;
using Accord.Math;

// procedual map generation using perlin noise for terrain -> usage of Accord.Math
public class MapGenerator{

    // constant Parameters for NoiseFunction:
    private const int octaves = 3; 
    private const double persistance = 0.8; 
    private const double initAmplitude = 1;
    private const double initFrequency = 0.03; 
    private const double forestFrequency = 1.5;

    private const int seedRange = 10000; // maximum of random seed 
    private const int forestOffset = 100; 

    /* assigns a resource type based on a noise value
    ......(-0.85).....(-0.5)......(-0.35)......(0.5).....(0.8)......
    DeepWater |  Water  |   Sand     |   Grass   |  Stone  |   Snow

    */
    private Resource.ResourceType assignResource(double noise){
        switch(noise)
        {
            case < -0.85:{
                return Resource.ResourceType.DeepWater;
            }
            case < -0.5:{
                return Resource.ResourceType.Water;
            }
            case < -0.35:{
                return Resource.ResourceType.Sand;
            }
            case < 0.5:{
                return Resource.ResourceType.Grass;
            }
            case < 0.8:{
                return Resource.ResourceType.Stone;
            }
            case >= 0.8:{
                return Resource.ResourceType.Snow;
            }
            default: return Resource.ResourceType.None;
        }
    }

    // extra overlay for forrest -> forrest is indepentant from rest of the height map
    private Resource.ResourceType plantForest(double noise){
        switch(noise){
            case < -0.25:{
                return Resource.ResourceType.Forest;
            }
            default: {
                return Resource.ResourceType.None;}
        }
    }

    private PerlinNoise generateNoise(){
        return new PerlinNoise(octaves,persistance,initFrequency, initAmplitude);
    }
    private PerlinNoise generateNoise(double frequency){
        return new PerlinNoise(octaves,persistance,frequency, initAmplitude);
    }

    // generates new terrain for a given map
    public void generateTerrain(mapPixel[,] map){
        int sizeX = map.GetUpperBound(1)+1;
        int sizeY = map.GetUpperBound(0)+1;

        Random random = new Random(Environment.TickCount);
        PerlinNoise mapNoise = generateNoise();

        // different frequency for forrest (forrest is not part of the height map)
        PerlinNoise forestNoise = generateNoise(initFrequency*forestFrequency);

        // random seeds to add to the coordinates to choose different parts of the noise map
        int seedX = random.Next(0,seedRange);  
        int seedY = random.Next(0,seedRange); 
        
        for(int y = 0; y< sizeY; y++){
            for(int x = 0; x< sizeX; x++){
                Resource.ResourceType type = assignResource(mapNoise.Function2D( x + seedX , y +seedY));
                Resource.ResourceType forest = plantForest(forestNoise.Function2D( x + seedX + forestOffset , y +seedY + forestOffset));

                // forrest can only grow on grass and sand 
                if(forest == Resource.ResourceType.Forest && (type == Resource.ResourceType.Grass || type == Resource.ResourceType.Sand)){
                    type = Resource.ResourceType.Forest;
                }
                map[x,y].resource = new Resource(type);
                map[x,y].color = map[x,y].resource.getColor();
            }
        }
    }
    
}