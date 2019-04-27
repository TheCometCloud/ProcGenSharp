// Houston Tyler Webb
//
// A base class for mananing a set of Points in a ProcGen Map.
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcGenSharp
{
    public class Room
    {
        // List of Points this room occupies.
        public List<Tile> Points;
        public Map ParentMap;

        public char Wall = '#';
        public char Floor = '.';

        // No-Arg Constructor
        public Room()
        {
            this.Points = null;
            this.ParentMap = null;
        }

        // Parameterized Constructor
        public Room(List<Tile> Points, Map ParentMap)
        {
            this.Points = Points;
            this.ParentMap = ParentMap;
        }

        // Move through every point in the room, performing this action.
        public void TraverseWith(Action<Tile> action)
        {
            foreach(Tile tile in Points)
            {
                action(tile);
            }
        }

        // Checks if two rooms intersect at any point.
        public bool CollidesWith(Room other)
        {
            foreach(Tile point in Points)
            {
                if (other.Points.Contains(point))
                    return true;
            }

            return false;
        }

        // Returns a list of Points that border the room.
        public virtual List<Tile> GetPerimeter()
        {
            // Output storage
            List<Tile> perimeterPoints = new List<Tile>();

            // Loop through all neighbors of the point
            TraverseWith( (tile) =>
            {
                for (int i = tile.y - 1; i < tile.y + 2; i++)
                {
                    for(int j = tile.x - 1; j < tile.x + 2; j++)
                    {
                        if (!Points.Contains(new Tile(j, i, ParentMap)))
                        {
                            perimeterPoints.Add(tile);
                            break;
                        }
                    }
                }
            });

            return perimeterPoints;
        }

        public void Fill(char c)
        {
            Points = Points.Select(tile => {tile.character = c; return tile;}).ToList();
        }

        // Draws the room to the parent grid.
        public void DrawToParentGrid()
        {
            List<Tile> perimeter = GetPerimeter();
            List<Tile> drawList = new List<Tile>();

            // Assign characters
            TraverseWith( (tile) =>
            {
                if (perimeter.Contains(tile))
                    tile.character = '#';
                else
                    tile.character = '.';
            });
        }
    }
}
