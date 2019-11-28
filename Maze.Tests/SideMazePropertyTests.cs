using MazeGenerator;
using System;
using Xunit;
using FluentAssertions;

namespace Maze.Tests
{
    public class SideMazePropertyTests
    {
        [Theory]
        [InlineData(GeneratorType.Random)]
        [InlineData(GeneratorType.RecursiveSplit)]
        [InlineData(GeneratorType.Split)]
        public void MazeGeneratorShouldGenerateMazeWithCorrectPathSizeAndHave2PathOnTheBorder(GeneratorType generatorType)
        {
            var rand = new Random();
            var generator = new Generator();

            for (int iteration = 0; iteration < 15; iteration++)
            {
                var width = rand.Next() % 50 + 50;
                var height = rand.Next() % 50 + 50;

                var maze = generator.Generate(width, height, generatorType);

                VerifyMazeSize(maze, width, height);
                VerifyHasEntranceAndExit(maze);
            }
        }

        private static void VerifyMazeSize(MazeGenerator.Maze maze, int width, int height)
        {
            maze.Dimension.X.Should().Be(width);
            maze.Dimension.Y.Should().Be(height);

            int pathCount = 0;
            for (int x = 0; x < maze.Dimension.X; x++)
            {
                if (maze[0, x] == CaseType.Path) pathCount++;
                if (maze[maze.Dimension.Y - 1, x] == CaseType.Path) pathCount++;
            }
            for (int y = 1; y < maze.Dimension.Y - 1; y++)
            {
                if (maze[y, 0] == CaseType.Path) pathCount++;
                if (maze[y, maze.Dimension.X - 1] == CaseType.Path) pathCount++;
            }

            pathCount.Should().Be(2);
        }

        private static void VerifyHasEntranceAndExit(MazeGenerator.Maze maze)
        {
            int pathCount = 0;
            for (int x = 0; x < maze.Dimension.X; x++)
            {
                if (maze[0, x] == CaseType.Path) pathCount++;
                if (maze[maze.Dimension.Y - 1, x] == CaseType.Path) pathCount++;
            }
            for (int y = 1; y < maze.Dimension.Y - 1; y++)
            {
                if (maze[y, 0] == CaseType.Path) pathCount++;
                if (maze[y, maze.Dimension.X - 1] == CaseType.Path) pathCount++;
            }

            pathCount.Should().Be(2);
        }

        private static MazeGenerator.Maze CreateMaze(int width, int height)
        {
            var test = new Generator();
            var mazeRandom = test.Generate(width, height, GeneratorType.Random);
            return mazeRandom;
        }
    }
}
