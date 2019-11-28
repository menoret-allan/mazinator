using MazeGenerator;
using System;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Maze.Drawing;

namespace Maze.Tests
{
    public class MazeNeverHasSquarePathPropertyTests
    {
        [Theory]
        //[InlineData(GeneratorType.Random)]
        //[InlineData(GeneratorType.Split)]
        [InlineData(GeneratorType.RecursiveSplit)]
        public void MazeGeneratorShouldGenerateMazeWithout4PathCasesInSquare(GeneratorType generatorType)
        {
            var rand = new Random();
            var generator = new Generator();

            for (int iteration = 0; iteration < 10; iteration++)
            {
                var width = rand.Next() % 25 + 25;
                var height = rand.Next() % 25 + 25;

                var maze = generator.Generate(width, height, generatorType);
                var img = MazeToImage.Convert(maze, 8);
                img.Save($"test{iteration}.jpg");

                VerifyNoSquarePathInTheMaze(maze);
            }

        }

        private static void VerifyNoSquarePathInTheMaze(MazeGenerator.Maze maze)
        {
            List<(int x, int y)> errorList = new List<(int x, int y)>();
            for (int x = 0; x < maze.Dimension.X - 1; x++)
            {
                for (int y = 0; y < maze.Dimension.Y - 1; y++)
                {
                    if (maze[y, x] == CaseType.Path &&
                        maze[y, x + 1] == CaseType.Path &&
                        maze[y + 1, x] == CaseType.Path &&
                        maze[y + 1, x + 1] == CaseType.Path)
                    {
                        errorList.Add((x, y));
                    }
                }
            }

            errorList.Should().BeEmpty();
        }
    }
}
