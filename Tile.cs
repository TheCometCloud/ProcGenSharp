// Houston Tyler Webb
//
// A Tile type for storing point information on grids.
using System;

namespace ProcGenSharp
{
    public struct Tile
    {
        // Data fields
        public int x { get; private set; }
        public int y { get; private set; }
        public Map Map;
        
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

        // Equivalence Methods
        public override bool Equals(object o)
        {
            if (o is Tile)
            {
                Tile t = (Tile) o;
                return this == t;
            }
            
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
