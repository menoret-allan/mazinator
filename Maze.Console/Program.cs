﻿using Maze.Drawing;
using MazeGenerator;
using System;
using System.Collections.Generic;

namespace Maze.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            SpeedTest();
            //SplitMazeGeneratorTest();
        }

        static void SpeedTest()
        {
            var cases = new List<Case> {
                new Case { Width = 10, Height = 10, Repeat = 10000, GeneratorType = GeneratorType.RecursiveSplit },
                new Case { Width = 100, Height = 50, Repeat = 2000, GeneratorType = GeneratorType.RecursiveSplit },
                new Case { Width = 50, Height = 100, Repeat = 2000, GeneratorType = GeneratorType.RecursiveSplit },
                new Case { Width = 100, Height = 100, Repeat = 1000, GeneratorType = GeneratorType.RecursiveSplit },
                new Case { Width = 1000, Height = 1000, Repeat = 10, GeneratorType = GeneratorType.RecursiveSplit },
                new Case { Width = 10, Height = 10, Repeat = 10000, GeneratorType = GeneratorType.Random },
                new Case { Width = 100, Height = 50, Repeat = 2000, GeneratorType = GeneratorType.Random},
                new Case { Width = 50, Height = 100, Repeat = 2000, GeneratorType = GeneratorType.Random},
                new Case { Width = 100, Height = 100, Repeat = 1000, GeneratorType = GeneratorType.Random},
                // new Case { Width = 1000, Height = 1000, Repeat = 1, GeneratorType = GeneratorType.Random}, // too long
            };
            var rand = new Rand(new Random());
            var test = new Generator(rand);

            System.Console.WriteLine("Starting the speedtest:");

            foreach (var item in cases)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                for (int i = 0; i < item.Repeat; i++)
                {
                    var maze = test.Generate(item.Width, item.Height, item.GeneratorType);
                }

                watch.Stop();
                item.Result = watch.ElapsedMilliseconds;
                System.Console.WriteLine(item);
            }
        }

        static void SplitMazeGeneratorTest() {
            var rand = new Rand(new Random(922384482));
            var test = new Generator(rand);
            var width = rand.Next() % 50 + 50;
            var height = rand.Next() % 50 + 50;
            var maze = test.Generate(width, height, GeneratorType.RecursiveSplit);

            var result = MazeToImage.Convert(maze, 8);
            result.Save("test.jpg");
        }
    }

    class Case
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Repeat { get; set; }
        public long Result { get; set; }
        public GeneratorType GeneratorType { get; set; }
        public override string ToString()
        {
            return $"Test ({GeneratorType.ToString()}): {Width}x{Height} - {Repeat} time => {Result}";
        }
    }
}
