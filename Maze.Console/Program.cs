using Maze.Drawing;
using MazeGenerator;
using System.Collections.Generic;

namespace Maze.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var cases = new List<Case> {
                new Case { Width = 10, Height = 10, Repeat = 10000 },
                new Case { Width = 100, Height = 50, Repeat = 2000 },
                new Case { Width = 50, Height = 100, Repeat = 2000 },
                new Case { Width = 100, Height = 100, Repeat = 1000 },
                new Case { Width = 1000, Height = 1000, Repeat = 10 },
            };

            foreach (var item in cases)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                for (int i = 0; i < item.Repeat; i++)
                {
                    var test = new Generator();
                    var maze = test.Generate(item.Width, item.Height, GeneratorType.Random);
                }

                watch.Stop();
                item.Result = watch.ElapsedMilliseconds;
                System.Console.WriteLine(item);
            }
        }
    }

    class Case
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Repeat { get; set; }
        public long Result { get; set; }
        public override string ToString()
        {
            return $"Test: {Width}x{Height} - {Repeat} time => {Result}";
        }
    }
}
