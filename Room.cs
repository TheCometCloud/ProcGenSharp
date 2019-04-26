// Houston Tyler Webb
//
// A base class for mananing a set of Points in a ProcGen Map.
using System;
using System.Collections.Generic;

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
        public void TraverseWith(Action<int, int> action)
        {
            foreach(Tile point in Points)
            {
                action(point.x, point.y);
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
            TraverseWith( (x, y) =>
            {
                for (int i = y - 1; i < y + 2; i++)
                {
                    for(int j = x - 1; j < x + 2; j++)
                    {
                        if (!Points.Contains(new Tile(j, i, ParentMap.Grid[i, j])))
                        {
                            perimeterPoints.Add(new Tile(x, y, ParentMap.Grid[y, x]));
                            break;
                        }
                    }
                }
            });

            return perimeterPoints;
        }

        // Draws the room to the parent grid.
        public void DrawToParentGrid()
        {
            // Can't draw to parent map if it doesn't exist
            if (ParentMap == null)
                return;

            List<Tile> perimeter = GetPerimeter();
            List<Tile> drawList = new List<Tile>();

            // Assign characters and add them to the drawlist.
            TraverseWith( (x, y) =>
            {
                Tile temp = new Tile(x, y);

                if (perimeter.Contains(temp))
                {
                    temp.value = Wall;
                }
                else
                {
                    temp.value = Floor;
                }

                drawList.Add(temp);
            });

            // Make respective changes to the grid
            foreach (Tile tile in drawList)
            {
                ParentMap.Grid[tile.y, tile.x] = tile.value;
            }
        }
    }
}
