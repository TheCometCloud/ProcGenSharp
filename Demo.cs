using System;
using ProcGenSharp;

class Demo
{
    static void Main(string[] args)
    {   
        Console.WriteLine("A Box-Pipe Map:");
        Console.WriteLine(new BoxPipeMap(50, 50));

        Console.WriteLine("A Cave Map:");
        Console.WriteLine(new CaveMap(50, 50));
    }
}