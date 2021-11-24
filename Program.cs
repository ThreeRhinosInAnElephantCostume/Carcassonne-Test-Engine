using System;
using Carcassonne;

namespace Carcassonne_Test_Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            var e = GameEngine.CreateBaseGame(null, 0, 0, "");
            Console.WriteLine("Hello World!");
        }
    }
}
