// Houston Tyler Webb
//
// A Tile type for storing cell information on grids.
using System;
using System.Collections.Generic;

namespace ProcGenSharp
{
    public class Tile : IEquatable<Tile>
    {
        // Data fields
        public int x { get; private set; }
        public int y { get; private set; }
        public Map Map;

        // Neighboring directions implemented in an enumerated field
        enum Direction
        {
            Top,
            Bottom,
            Left,
            Right,
            Topright,
            Topleft,
            Bottomright,
            Bottomleft
        }
        
        // Retrieves and updates from the parental Map object.
        public char? character 
        {
            get { return Map.Grid[y, x]; } 
            set { Map.Grid[y, x] = value; }
        }

        // Parameterized Constructor
        public Tile(int x, int y, Map map)
        {
            this.x = x;
            this.y = y;
            this.Map = map;
        }

        // Out of bounds verifier
        public bool IsOutOfBounds()
        {
            return (x < 0 || y < 0 || x >= Map.Width || y >= Map.Height);
        }
        
        // Neighbor acquirer
        // Tiles are always returned in topright to bottomleft order
        public List<Tile> GetNeighbors(bool diagonals, int range = 1)
        {
            List<Tile> tiles = new List<Tile>();
            for (int i = -range; i <= range; i++)
            {
                for (int j = -range; j <= range; j++)
                {
                    if (j != 0 && i != 0)
                    {
                        if (diagonals)
                            tiles.Add(new Tile(x + j, y + i, Map));
                    }
                    else if (!(j == 0 && i == 0))
                        tiles.Add(new Tile(x + j, y + i, Map));
                }
            }

            return tiles;
        }

        // Equivalence Methods
        public bool Equals(Tile t)
        {
            return this == t;
        }

        public override bool Equals(object obj)
        {
            if (obj is Tile)
                return this == obj as Tile;
            
            return false;
        }

        // Equivalence operators
        public override int GetHashCode() 
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ character.GetHashCode() ^ Map.GetHashCode();
        }

        public static bool operator ==(Tile a, Tile b)
        {
            return (a.x == b.x && a.y == b.y && a.Map == b.Map);
        }

        public static bool operator !=(Tile a, Tile b)
        {
            return (a.x != b.x || a.y != b.y || a.Map != b.Map);
        }

        public override string ToString()
        {
            return $"Map: {Map.GetHashCode()}, X: {x}, Y: {y}";
        }
    }
}
