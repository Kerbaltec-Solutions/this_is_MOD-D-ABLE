using System;
using System.Collections.Generic;

namespace RoutePlanning;

// Path finding algorithm for entities using simple version of A*

// class for Nodes of weighted graph:
internal class Node{
    internal bool IsWalkable{get; set;} //should node be ignored
    internal Position Pos{get;private set;} 
    internal Node? ParentNode{get; set;} 
    internal NodeState State{get; set;}

    internal int Cost{get; private set;} // equals from speedDevider of Resource type (on that position)

    internal float G { get; private set; } // cost until now

    // heuristic minimal cost to target 
    //-> minimal cost per field  =  1 -> H = distance to tagret (minimizes possible cost)
    internal float H { get; private set; } 
    internal float F { get { return this.G + this.H; } } // sum of G and H 

    internal float getDistance(Position target){
        // using pythagoras
        return (float)Math.Sqrt(Math.Pow(this.Pos.X - target.X,2)+ Math.Pow(this.Pos.Y - target.Y,2));
    }

    internal Node(Position pos, Node? parentNode, bool isWalkable, int cost){
        Pos = pos;
        ParentNode = parentNode;
        IsWalkable = isWalkable;
        Cost = cost;
        State = NodeState.Untested;
        G = 0;
        H = 0;
    }

    // returns positions of possible neighbouring nodes (entities can only walk up/down/left/right)
    private Position[] GetAdjacentPositions(){
        Position[] adjacentPositions = new Position[4];

        adjacentPositions[0] = new Position(Pos.X+1, Pos.Y);
        adjacentPositions[1] = new Position(Pos.X-1, Pos.Y);
        adjacentPositions[2] = new Position(Pos.X, Pos.Y+1);
        adjacentPositions[3] = new Position(Pos.X, Pos.Y-1);
        return  adjacentPositions;
    }

    // returns all neighbour nodes
    internal List<Node> GetAdjacentWalkableNodes(Dictionary<Position,Node>nodes, int sx, int sy, Node target, mapPixel[,] maparr, bool ignoreNotWalkable = true)
    {
        List<Node> walkableNodes = new List<Node>();
        Position[] nextPositions= GetAdjacentPositions();
    
        foreach (var position in nextPositions)
        {
            int x = position.X;
            int y = position.Y;

            //PositionKey position = new PositionKey(position.X,position.Y);

            // stay in map
            if (x < 0 || x >= sx || y < 0 || y >= sy)
                continue;

            try{
                // ignore notWalkable nodes if ignoreNotWalkeble 
                if (ignoreNotWalkable && !nodes[position].IsWalkable)
                    continue;

                // Ignore nodes that are already closed
                if (nodes[position].State == NodeState.Closed){
                    continue;
                }
                // if node is open: only add node if its G value would decrease by having <this> as new parent
                if (nodes[position].State == NodeState.Open)
                {
                    float gTemp = this.G + this.Cost;
                    if (gTemp < nodes[position].G)
                    {
                        nodes[position].ParentNode = this;
                        nodes[position].G = gTemp;
                        walkableNodes.Add(nodes[position]);
                    }
                    continue;
                }
            } 
            catch(KeyNotFoundException){ // if node is not yet created
                // create new node with <this> as parent -> add to nodes directionary
                nodes.Add(position,new Node(position,this,maparr[position.X,position.Y].resource.IsWalkable,maparr[position.X,position.Y].resource.SpeedDevider)); 
                // Ignore notWalkable nodes if ignoreNotWakable 
                if (ignoreNotWalkable && !nodes[position].IsWalkable)
                    continue;  
                nodes[position].State = NodeState.Open;    
                nodes[position].G = nodes[position].ParentNode!.G + nodes[position].ParentNode!.Cost;
                nodes[position].H = nodes[position].getDistance(target.Pos);
                walkableNodes.Add(nodes[position]); 
                continue;
            }
        }    
        return walkableNodes;
    }
}

// three different states for nodes
internal enum NodeState { Untested, Open, Closed }

// class for route
public class Route{
    private Node targetNode;
    private Node startNode;
    public List<Position>? Path{get; private set;}
    public Position targetPos{get;private set;}

    private mapPixel[,] maparr; // reference to map
    private bool ignoreNotWalkable = true; // if true: excludes notWalkable nodes from path 
    private bool entityCanDestroyStone = false; 

    private int maxIterations = 10000;

    private Node? Search(Node startNode, int sx, int sy)
        {
            // save discovered nodes in directionary -> key is their position
            Dictionary<Position,Node> discoveredNodes = new Dictionary<Position,Node>();

            // initializes nextNodes with adjacent walkable nodes from startNode
            List<Node> nextNodes = startNode.GetAdjacentWalkableNodes(discoveredNodes, sx, sy, this.targetNode, maparr, this.ignoreNotWalkable);
            startNode.State = NodeState.Closed;
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));

            // loop through nextNodes with limited iterations to prevent function from taking up too much time
            for(int i = 0;i < maxIterations; i++)
            {
                Node nextNode;
                if(nextNodes.Count>= 1){
                    nextNode = nextNodes[0];
                }else if(ignoreNotWalkable){
                    // if no path could be found: search again while considering notWalkable nodes
                    ignoreNotWalkable = false;
                    return Search(startNode,sx,sy);
                }else{return null;}
                discoveredNodes[nextNode.Pos].State = NodeState.Closed;
                
                if (nextNode.Pos == this.targetNode.Pos)
                {
                    return nextNode;
                }
                else
                {
                    nextNodes.AddRange(nextNode.GetAdjacentWalkableNodes(discoveredNodes, sx, sy, this.targetNode, maparr, this.ignoreNotWalkable));
                    nextNodes.Remove(nextNodes[0]);
                    // sort nextNodes (ascending order) to always treat estimated best node (node with lowest F value)
                    nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));                    
                }
            }
            return nextNodes[0];
        }

    private void findRoute(int sX, int sY){
        // if targetNode not walkable: consider notWalkable nodes to get "as near as possible" to target
        if(!targetNode.IsWalkable){ignoreNotWalkable = false;}

        // if entitycanDestroy stone: notWalkable nodes are now walkable
        if(entityCanDestroyStone){ignoreNotWalkable = false;}

        List<Node> pathNodes = new List<Node>();       
        Node? endNode = Search(this.startNode, sX, sY);

        if (endNode!= null)
        {
            // walk path back from end to start
            Node node = endNode;
            while (node.ParentNode != null)
            {
                pathNodes.Add(node);
                node = node.ParentNode;
            }
            pathNodes.Reverse(); // put startNode at beginning of list

            // if there are nodes in the path that aren't walkable for the entity:
            // remove all nodes after first occurence of a not Walkable node
            if(!ignoreNotWalkable && !entityCanDestroyStone){ 
                for(int i = 0; i < pathNodes.Count; i++){
                    if(!pathNodes[i].IsWalkable){
                        if(i == 0){
                            targetNode = startNode; 
                            Path = null;
                            return;
                        } 
                        pathNodes.RemoveRange(i,pathNodes.Count-i);
                        targetNode = pathNodes[i-1]; // set target node to last possible walkable node of path
                        break;
                    }
                } 
            } 
            Path = new List<Position>();
            for(int i = 0; i< pathNodes.Count; i++){
                Path.Add(pathNodes[i].Pos);
            }           
        }  
        else {Path = null; return;}; 
    }      

    // constructor of Route -> also creates path 
    public Route(mapPixel[,] map, Position start, Position target, bool entityCanDestroyStone = false){
        this.maparr = map;
        int sx =maparr.GetUpperBound(1)+1;
        int sy = maparr.GetUpperBound(0)+1;
        startNode = new Node(start,null,maparr[start.X,start.Y].resource.IsWalkable, maparr[start.X,start.Y].resource.SpeedDevider);
        targetNode = new Node(target,null,maparr[target.X,target.Y].resource.IsWalkable, maparr[target.X,target.Y].resource.SpeedDevider);

        this.entityCanDestroyStone = entityCanDestroyStone;
        ignoreNotWalkable = true;
        findRoute(sx,sy);  
        targetPos = targetNode.Pos;
    }

    public override string ToString()
    {
        return String.Format("start: {0}| target: {1}", startNode.Pos, targetNode.Pos);
    }

}
