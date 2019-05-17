using ProcGenSharp;
using System;
using System.Collections.Generic;

// Data structure class for pathfinding in a perfect maze.
public class MazeTree
{
    // Fields
    public Node root;
    List<Tile> checkedTiles;
    MazeMap map;

    // Parameterized Constructor
    public MazeTree(MazeMap map, Tile start)
    {
        // Set the fieldss
        this.map = map;
        this.checkedTiles = new List<Tile>();
        this.root = new Node(start);

        // Start the queue at the start tile
        Queue<Node> populateQueue = new Queue<Node>();
        populateQueue.Enqueue(this.root);

        // Construct the tree by traversing neighboring, empty tiles
        while (populateQueue.Count > 0)
        {
            Node node = populateQueue.Dequeue();
            List<Tile> neighbors = node.value.GetNeighbors(false);
            
            // Loop through and strip the neighbors
            for (int i = 0; i < neighbors.Count; i++)
            {
                if (neighbors[i].IsOutOfBounds() || neighbors[i].character != map.Empty || (!object.ReferenceEquals(node.parent, null) && node.parent.value == neighbors[i]))
                {    
                    neighbors.Remove(neighbors[i]);
                    i--;
                }
            }

            // Queue up the remaining children and set their parents
            foreach(Tile neighbor in neighbors)
            {
                var child = new Node(neighbor);
                child.parent = node;
                node.children.Add(child);
                populateQueue.Enqueue(child);
            }
        }
    }

    // Node class to make up the tree
    public class Node : IEquatable<Node>
    {
        public Tile value;
        public List<Node> children;
        public Node parent;

        public Node(Tile value)
        {
            this.value = value;
            this.children = new List<Node>();
        }

        public bool Equals(Node n)
        {
            return this == n;
        }

        public override bool Equals(object obj)
        {
            if (obj is Node)
                return (Node) obj == this;
            else
                return false;
        }

        public static bool operator ==(Node a, Node b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }
            else if (object.ReferenceEquals(b, null))
                return false;
            else
            {
                if (a.value == b.value)
                    return true;
                else
                    return false;
            }
        }

        public static bool operator !=(Node a, Node b)
        {
            return !(a == b);
        }
    }

    //Returns the order of traveled tiles
    public List<Tile> FindPathTo(Tile tile)
    {
        // Find the tile
        Node end = FindAt(tile);
        List<Tile> path = new List<Tile>();

        if (end == null)
        {
            return null;
        }
        while(end != root)
        {
            path.Add(end.value);
            end = end.parent;
        }

        return path;
    }

    // Checks if a the tree contains the tile
    public bool Contains (Tile tile)
    {
        return FindAt(tile) != null;
    }

    // Kickstarts the recursion to find the tile value.
    private Node FindAt(Tile tile)
    {
        Queue<Node> searchQueue = new Queue<Node>();
        searchQueue.Enqueue(root);
        while(searchQueue.Count > 0)
        {
            Node node = searchQueue.Dequeue();

            if(node.value == tile)
                return node;

            foreach(Node child in node.children)
            {
                if (child != null)
                    searchQueue.Enqueue(child);
            }

            if (searchQueue.Count > map.Height * map.Width)
            {
                break;
            }
        }

        return null;
    }

    // Recursively locates the node with the tile value.
    private Node FindAt(Tile tile, Node check)
    {
        Node temp = null;
        if (check == null || check.value == tile)
            temp = check;
        else
        {
            foreach(Node child in check.children)
            {
                if (child.value == tile)
                {
                    temp = child;
                    break;
                }
                else
                {
                    temp = FindAt(tile, child);
                }

                if(temp != null)
                    return temp;
            }
        }

        return temp;
    }

    // Sets the root at the particular tile
    public void SetRoot(Tile tile)
    {
        var temp = FindAt(tile);
        if (temp != null)
            SetRoot(temp);
    }

    // Private method of root setting that handles Nodes
    private void SetRoot(Node node)
    {
        root = node;
        Waterfall(node, null);
    }

    // Recursive method of parent-child waterfalling
    private void Waterfall(Node node, Node parent)
    {
        if (node != null)
        {
            node.children.Add(node.parent);
            Waterfall(node.parent, node);
            node.parent = parent;
            node.children.Remove(parent);
        }
    }

    // Centers the tree based on shortest maximum chain length
    public void CenterTree()
    {
        int lastGreatestLength = int.MaxValue;
        int greatest = 0;
        Node greatestChild = root;

        do 
        {
            if (greatest > 0)
                lastGreatestLength = greatest;

            greatest = 0;
            SetRoot(greatestChild);
            foreach(Node child in root.children)
            {
                int count = CountGreatestChain(child);
                if (count >= greatest)
                {
                    greatest = count;
                    greatestChild = child;
                }
            }
        } while (greatest < lastGreatestLength);
    }

    // Counts the number of children a node has
    private int CountChildren(Node parent)
    {
        int count = 0;
        if (parent.children.Count == 0)
        {
            return 1;
        }

        foreach (Node child in parent.children)
        {
            if (child != null)
                count += CountChildren(child) + 1;
        }

        return count;
    }
    
    // Counts the length of the greatest chain a node has
    private int CountGreatestChain(Node parent)
    {
        int count = 0;

        if(parent.children.Count == 0)
        {
            return 1;
        }

        foreach (Node child in parent.children)
        {
            if (child != null)
            {
                int childCount = CountGreatestChain(child) + 1;
                if (count < childCount)
                    count = childCount;
            }
        }

        return count;
    }

    // Counts the average distance of child nodes from the parent
    private double AverageChain(Node parent)
    {
        if (parent.children.Count == 0)
        {
            return 1;
        }

        List<int> list = DistanceList(parent);
        int sum = 0;
        
        foreach (int item in list)
        {
            sum += item;
        }

        return (double) sum / list.Count;
    }

    // Returns a list of distances of children from the parent node
    private List<int> DistanceList(Node parent)
    {
        if (parent.children.Count == 0)
        {
            return new List<int>{0};
        }

        List<int> distanceList = new List<int>();

        foreach (Node child in parent.children)
        {
            if (child != null)
            {
                distanceList.AddRange(DistanceList(child));
            }
        }

        for (int i = 0; i < distanceList.Count; i++)
            distanceList[i]++;

        return distanceList;
    }
}