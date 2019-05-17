using System;
using ProcGenSharp;

class Demo
{
    static void Main(string[] args)
    {   
        Random rng = new Random();

        CaveMap cave = new CaveMap(25, 70);
        Console.WriteLine("A Cave Map:");
        Console.WriteLine(cave);

        var rooms = cave.GetFloodRooms();
        Console.WriteLine($"There are {rooms.Count} rooms.");

        GuidedCaveMap guidedCaveMap = new GuidedCaveMap(25, 70);
        Console.WriteLine("A Guided Cave Map:");
        Console.WriteLine(guidedCaveMap);

        var guidedRooms = guidedCaveMap.GetFloodRooms();
        Console.WriteLine($"There are {guidedRooms.Count} rooms.");

        BoxPipeMap boxPipe = new BoxPipeMap(25, 70, 5, 10, -100, 0);
        Console.WriteLine("A Box-Pipe Map:");
        Console.WriteLine(boxPipe);

        var pipeRooms = boxPipe.GetFloodRooms();
        Console.WriteLine($"There are {pipeRooms.Count} rooms.");

        MazeMap mazeMap = new MazeMap(25, 70, 5);
        Console.WriteLine("A Maze Map:");
        Console.WriteLine(mazeMap);

        // Randomly select the starting tile
        Tile start = null;
        while(start == null)
        {
            Tile test = new Tile(rng.Next(mazeMap.Width), rng.Next(mazeMap.Height), mazeMap);
            if (!test.IsOutOfBounds() && test.character == mazeMap.Empty)
                start = test;
        }

        // Randomly select the ending tile
        Tile end = null;
        while(end == null)
        {
            Tile test = new Tile(rng.Next(mazeMap.Width), rng.Next(mazeMap.Height), mazeMap);
            if (!test.IsOutOfBounds() && test.character == mazeMap.Empty && test != start)
                end = test; 
        }

        // Create and restructure the maze tree
        MazeTree tree = new MazeTree(mazeMap, new Tile(1, 1, mazeMap));
        tree.CenterTree();
        tree.root.value.character = 'X';
        
        // Draw the maze tree
        foreach(Tile tile in tree.FindPathTo(end))
        {
            if (tile.character != 'X')
                tile.character = '.';
        }
        
        Console.WriteLine(mazeMap);
    }
}