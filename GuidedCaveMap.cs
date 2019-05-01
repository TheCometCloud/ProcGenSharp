// Houston Tyler Webb
//
// This is a child class of CaveMap that creates a more elongated
// cave structure in the Grid.
using System.Collections.Generic;
using System;

namespace ProcGenSharp
{
    public class GuidedCaveMap : CaveMap
    {
        public GuidedCaveMap(int height, int width) : base(height, width) {}

        public override void Excavate()
        {   
            // Declare room corners
            BoxRoom topLeft = new BoxRoom(this, Height / 3, Width / 2, new Tile(1, 1, this));
            BoxRoom bottomRight = new BoxRoom(this, Height / 3, Width / 2, new Tile(Width / 2, Height * 2 / 3, this));

            BoxRoom topRight = new BoxRoom(this, Height / 3, Width / 2, new Tile(Width / 2, 1, this));
            BoxRoom bottomLeft = new BoxRoom(this, Height / 3, Width / 2, new Tile(1, Height / 3 * 2, this));

            // Pair the room corners
            Tuple<BoxRoom, BoxRoom>[] assortments = new Tuple<BoxRoom, BoxRoom>[]
            {
                new Tuple<BoxRoom, BoxRoom>(topLeft, bottomRight),
                new Tuple<BoxRoom, BoxRoom>(topRight, bottomLeft),
            };
            
            // Randomly choose one the viable assortments
            var selection = assortments[Rng.Next() % 2];

            // Fill the rooms with walls
            selection.Item1.TraverseWith((tile) => 
            {
                DevelopGrid[tile.y, tile.x] = Wall;
                tile.character = Wall;
            });
            
            selection.Item2.TraverseWith((tile) => 
            {
                DevelopGrid[tile.y, tile.x] = Wall;
                tile.character = Wall;
            });
            
            base.Excavate();
        }
    }
}
