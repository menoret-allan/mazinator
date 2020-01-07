using MazeGenerator;
using System;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Maze.Tests.PropertyMazeValidator;

namespace Maze.Tests
{
    public class MazePropertyTesting
    {
        const int NumberOfIteration = 10;

        [Theory]
        [MemberData(nameof(GenerateSeeAndMazeType), NumberOfIteration)]
        public void ShouldContentOnlyPathAndWall(int seed, GeneratorType generatorType)
        {
            var rand = new Rand(new Random(seed));
            var generator = new Generator(rand);
            var width = rand.Next() % 25 + 50;
            var height = rand.Next() % 25 + 50;

            MazeGenerator.Maze maze = generator.Generate(width, height, generatorType);

            maze.AssertContainOnlyWallAndPath();
        }

        [Theory]
        [MemberData(nameof(GenerateSeeAndMazeType), NumberOfIteration)]
        public void ShouldHave2BlocksOfWall(int seed, GeneratorType generatorType)
        {
            var rand = new Rand(new Random(seed));
            var generator = new Generator(rand);

            var width = rand.Next() % 50 + 50;
            var height = rand.Next() % 50 + 50;

            var maze = generator.Generate(width, height, generatorType);

            maze.AssertThatWallAreSplittedIn2Blocks();
        }

        [Theory]
        [MemberData(nameof(GenerateSeeAndMazeType), NumberOfIteration)]
        public void MazeGeneratorShouldGenerateMazeWithout4PathCasesInSquare(int seed, GeneratorType generatorType)
        {
            var rand = new Rand(new Random(seed));
            var generator = new Generator(rand);
            var width = rand.Next() % 25 + 25;
            var height = rand.Next() % 25 + 25;

            var maze = generator.Generate(width, height, generatorType);

            maze.AssertNoSquarePathExist();
        }

        [Theory]
        [MemberData(nameof(GenerateSeeAndMazeType), NumberOfIteration)]
        public void MazeGeneratorShouldGenerateMazeWithEnytranceLinkedWithPathToTheExit(int seed, GeneratorType generatorType)
        {
            var rand = new Rand(new Random(seed));
            var generator = new Generator(rand);
            var width = rand.Next() % 50 + 50;
            var height = rand.Next() % 50 + 50;

            var maze = generator.Generate(width, height, generatorType);

            maze.AssertEntranceLinkedToExit();
        }

        [Theory]
        [InlineData(GeneratorType.Random)]
        [InlineData(GeneratorType.RecursiveSplit)]
        public void MazeGeneratorShouldGenerateMazeWithCorrectPathSizeAndHave2PathOnTheBorder(GeneratorType generatorType)
        {
            var rand = new Rand(new Random());
            var generator = new Generator(rand);

            for (int iteration = 0; iteration < 15; iteration++)
            {
                var width = rand.Next() % 50 + 50;
                var height = rand.Next() % 50 + 50;

                var maze = generator.Generate(width, height, generatorType);

                VerifyMazeSize(maze,(ushort) width, (ushort) height);
                VerifyHasEntranceAndExit(maze);
            }
        }

        private static void VerifyMazeSize(MazeGenerator.Maze maze, ushort width, ushort height)
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
            var rand = new Rand(new Random());
            var test = new Generator(rand);
            var mazeRandom = test.Generate(width, height, GeneratorType.Random);
            return mazeRandom;
        }

        public static IEnumerable<object[]> GenerateSeeAndMazeType(int repeat)
        {
            var rand = new Random();

            for (int i = 0; i < repeat; i++)
            {
                yield return new object[] { rand.Next(), GeneratorType.RecursiveSplit };
                yield return new object[] { rand.Next(), GeneratorType.Random };
            };
        }
    }
}
