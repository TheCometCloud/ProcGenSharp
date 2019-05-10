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
        List<Tile> Doors;
        public char Door = 'D';

        int MinDim;
        int MaxDim;

        int MaxDeadEnds;

        // Parameterized Constructor
        public BoxPipeMap(int height, int width, int MinDim = 3, int MaxDim = 10, float BranchRate = 0, int MaxDeadEnds = 0) : base(height, width, BranchRate)
        {
            this.MinDim = MinDim;
            this.MaxDim = MaxDim;

            this.MaxDeadEnds = MaxDeadEnds;

            Rooms = new List<Room>();
            Doors = new List<Tile>();
            FillGrid(Unknown);

            TraverseWith( (tile) =>
            {
                // 2% chance to generate a room at a location.
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
            TraverseWith( tile =>
            {
                if (tile.character == Unknown)
                    InitializeMaze(tile);
            });
            MakeDoorways();
            CleanDeadEnds();
        }

        // Retrieves a list a possible openings
        public List<Tile> GetPotentialDoorways(Room room)
        {
            List<Tile> tiles = room.Points;
            List<Tile> doorways = new List<Tile>();

            foreach(Tile tile in tiles)
            {
                for(int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        Tile neighbor = new Tile(tile.x + j, tile.y + i, tile.Map);

                        if (neighbor.IsOutOfBounds())
                            continue;

                        if (i == 0 && j == 0)
                            continue;

                        if (i != 0 && j != 0)
                            continue;

                        if (tile.character == Empty && neighbor.character == Wall)
                        {
                            Tile checkTile = new Tile(tile.x + j * 2, tile.y + i * 2, tile.Map);
                            if (!checkTile.IsOutOfBounds() && checkTile.character == Empty && !room.Points.Contains(checkTile))
                                doorways.Add(neighbor);
                        }
                    }
                }
            }

            return doorways;
        }

        // Make at least on door to each room.
        public void MakeDoorways()
        {
            List<Room> rooms = GetFloodRooms();
         
            while(rooms.Count > 1)
            {
                Room workingRoom = rooms[Rng.Next(rooms.Count)];
                var tiles = GetPotentialDoorways(workingRoom);
                if (tiles.Count < 1)
                    break;
                else
                {
                    tiles[Rng.Next(tiles.Count)].character = Empty;
                    if (Rng.Next(100) < 50)
                        tiles[Rng.Next(tiles.Count)].character = Empty;
                }
                rooms = GetFloodRooms();
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
            } while (deadEndsFound > MaxDeadEnds); // Repeat until we've got them all.
        }
    }   
}

