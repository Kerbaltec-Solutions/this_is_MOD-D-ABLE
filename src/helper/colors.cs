using System;

public class colors{
    //convert int to console color
    public static ConsoleColor consoleColor(int cNum){
        switch (cNum)
        {
            case 0:
                return ConsoleColor.Black;
            case 1:
                return ConsoleColor.DarkGray;
            case 2:
                return ConsoleColor.Gray;
            case 3:
                return ConsoleColor.White;
            case 4:
                return ConsoleColor.DarkRed;
            case 5:
                return ConsoleColor.Red;
            case 6:
                return ConsoleColor.DarkGreen;
            case 7:
                return ConsoleColor.Green;
            case 8:
                return ConsoleColor.DarkBlue;
            case 9:
                return ConsoleColor.Blue;
            case 10:
                return ConsoleColor.DarkYellow;
            case 11:
                return ConsoleColor.Yellow;
            case 12:
                return ConsoleColor.DarkCyan;
            case 13:
                return ConsoleColor.Cyan;
            case 14:
                return ConsoleColor.DarkMagenta;
            case 15:
                return ConsoleColor.Magenta;

            default:
                return ConsoleColor.Black;
        }
    }

    //try to convert RGB values to console colors
    public static int RGBtoCON(int[] color){
        if(color[0]<85){
            if(color[1]<85){
                if(color[2]<85){
                    return 0;
                }else if(color[2]<170){
                    return 8;
                }else{
                    return 9;
                }
            }else if(color[1]<170){
                if(color[2]<85){
                    return 6;
                }else if(color[2]<170){
                    return 12;
                }else{
                    return 13;
                }
            }else{
                if(color[2]<85){
                    return 7;
                }else{
                    return 13;
                }
            }
        }else if(color[0]<170){
            if(color[1]<85){
                if(color[2]<85){
                    return 4;
                }else if(color[2]<170){
                    return 14;
                }else{
                    return 15;
                }
            }else if(color[1]<170){
                if(color[2]<85){
                    return 10;
                }else{
                    return 1;
                }
            }else{
                if(color[2]<85){
                    return 10;
                }else if(color[2]<170){
                    return 1;
                }else{
                    return 2;
                }
            }
        }else{
            if(color[1]<85){
                if(color[2]<85){
                    return 5;
                }else{
                    return 15;
                }
            }else if(color[1]<170){
                if(color[2]<85){
                    return 10;
                }else if(color[2]<170){
                    return 1;
                }else{
                    return 2;
                }
            }else{
                if(color[2]<85){
                    return 11;
                }else if(color[2]<170){
                    return 2;
                }else{
                    return 3;
                }
            }
        }
    }

    //try to convert HSV values to console colors
    public static int HSVtoCON(int[] color){
        if(color[1]<127){
            if(color[2]<64){
                return 0;
            }else if(color[2]<127){
                return 1;
            }else if(color[2]<191){
                return 2;
            }else {
                return 3;
            }
        }else{
            if(color[1]<127){
                if(color[0]<42){
                    return 4;
                }else if(color[0]<85){
                    return 10;
                }else if(color[0]<127){
                    return 6;
                }else if(color[0]<170){
                    return 12;
                }else if(color[0]<212){
                    return 8;
                }else{
                    return 14;
                }
            }else{
                if(color[0]<42){
                    return 5;
                }else if(color[0]<85){
                    return 11;
                }else if(color[0]<127){
                    return 7;
                }else if(color[0]<170){
                    return 13;
                }else if(color[0]<212){
                    return 9;
                }else{
                    return 15;
                }
            }
        }
    }
}