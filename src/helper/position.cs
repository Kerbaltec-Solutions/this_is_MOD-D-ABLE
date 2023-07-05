// class to store and compare positions
public class Position{
    public int X{get; set;}
    public int Y{get;set;}

    public Position(int x, int y){
        X = x;
        Y = y;
    }
    public Position(){
        X = 0;
        Y = 0;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Position);
    }

    public bool Equals(Position other)
    {
        return !(other is null) &&
               X == other.X &&
               Y == other.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static bool operator ==(Position p1, Position p2){
        if(p1 is null & p2 is null) return true;
        if(p1 is null ^ p2 is null) return false;
        if(p1.X == p2.X && p1.Y == p2.Y) return true;
        else return false;
    }
     public static bool operator !=(Position p1, Position p2){
        if(p1 is null ^ p2 is null) return true; 
        if(p1 is null & p2 is null) return false;
        else return !p1.Equals(p2);
    }
    
    public override string ToString()
    {
        return String.Format("({0}|{1})",X,Y);
    }
}