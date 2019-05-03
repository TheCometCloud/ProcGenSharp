// Houston Tyler Webb
//
// This is an algorithm for producing a series of rooms connected by
// Long pipes across the map.
using System;
using System.Collections.Generic;

namespace ProcGenSharp
{
    public class BoxPipeMap : MazeMap
    {
        List<Room> Rooms;
        public char Door = 'D';

        int MinDim;
        int MaxDim;

        // Parameterized Constructor
        public BoxPipeMap(int height, int width, int MinDim = 3, int MaxDim = 10, float BranchRate = 0) : base(height, width, BranchRate)
        {
            this.MinDim = MinDim;
            this.MaxDim = MaxDim;

            Rooms = new List<Room>();
            FillGrid(Unknown);

            TraverseWith( (tile) =>
            {
                // 1% chance to generate a room at a location.
                if (Rng.Next(50) == 0)
                {
                    // Try to create a room and throw it away if its out of bounds or intersects another room.
                    Room newRoom;
                    try
                    {
                        newRoom = new BoxRoom(this, Rng.Next(this.MinDim, this.MaxDim + 1), Rng.Next(this.MinDim, this.MaxDim + 1), tile);
                    }
                    catch(BoxRoom.FailedRoomInitException ex)
                    {
                        return;
                    }

                    foreach (Room room in Rooms)
                    {
                        if (newRoom.CollidesWith(room, true))
                            return;
                    }

                    // Add the room if it survives the trials
                    Rooms.Add(newRoom);
                }
            });

            // Draw each room.
            foreach (Room room in Rooms)
            {
                room.DrawToParentGrid();
            }

            WallGrid();
            InitializeMaze();
            MakeDoorways();
            CleanDeadEnds();
        }

        // Make at least on door to each room.
        public void MakeDoorways()
        {
            foreach (Room room in Rooms)
            {
                var walls = room.GetPerimeter();
                var candidates = new List<Tile>();

                foreach(Tile wall in walls)
                {
                    bool floorFound = false;
                    bool emptyFound = false;

                    foreach(Tile neighbor in wall.GetNeighbors(false))
                    {
                        if (!neighbor.IsOutOfBounds() && neighbor.character == room.Floor)
                        {
                            if (floorFound)
                                emptyFound = true;

                            floorFound = true;
                        }
                        else if (!neighbor.IsOutOfBounds() && neighbor.character == Empty)
                            emptyFound = true;
                    }

                    if (floorFound && emptyFound)
                        candidates.Add(wall);
                }

                if (candidates.Count == 0)
                    continue;
                // Add a door.
                var candidate = candidates[Rng.Next(candidates.Count)];
                candidate.character = Door;
                candidates.Remove(candidate);
                
                // Small chance to add another door
                if (Rng.Next(10) == 0)
                {
                    bool found = false;
                    do
                    {
                        var newCandidate = candidates[Rng.Next(candidates.Count)];
                        if (!newCandidate.GetNeighbors(false).Contains(candidate))
                        {
                            found = true;
                            candidates.Remove(newCandidate);
                            newCandidate.character = Door;
                        }
                    } while(!found);
                }

            }
        }
        
        // Remove all dead ends.
        public void CleanDeadEnds()
        {
            int deadEndsFound;
            do 
            {
                deadEndsFound = 0;
                TraverseWith( tile => 
                {
                    // We only care about empty tiles.
                    if (!(tile.character == Empty))
                        return;
                    
                    // Track the number of walls found
                    var tiles = tile.GetNeighbors(false);
                    int walls = 0;
                    
                    // Count the number of neighboring walls
                    foreach(Tile neighbor in tiles)
                    {
                        if(neighbor.character == Wall)
                            walls++;
                    }

                    // If an empty tile has 3 walls, its a dead end.
                    // Count it, then deal with it.
                    if (walls > 2)
                    {
                        deadEndsFound++;
                        tile.character = Wall;
                    }
                });
            } while (deadEndsFound > 0); // Repeat until we've got them all.
        }
    }   
}

