using MazeGenerator;
using System;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace Maze.Tests
{
    public class EntranceAndExitAreLinkedByPathPropertyTests
    {
        [Fact]
        public void MazeGeneratorShouldGenerateMazeWithEnytranceLinkedWithPathToTheExit()
        {
            var rand = new Random();

            for (int iteration = 0; iteration < 4; iteration++)
            {
                var width = rand.Next() % 50 + 50;
                var height = rand.Next() % 50 + 50;

                MazeGenerator.Maze mazeRandom = CreateMaze(width, height);

                VerifyEntranceLinkedToExit(mazeRandom);
            }

        }

        private static void VerifyEntranceLinkedToExit(MazeGenerator.Maze maze)
        {
            var wallEntrance = maze.Entrance;
            var processedPath = new List<(int x, int y)>();
            var toBeProcessPath = new List<(int x, int y)> { wallEntrance };
            while (toBeProcessPath.Any())
            {
                var first = toBeProcessPath.First();
                processedPath.Add(first);
                toBeProcessPath.Remove(first);
                var nextMoves = GetPathNeihboors(maze, first).Where(pos => !processedPath.Contains(pos) && !toBeProcessPath.Contains(pos));
                toBeProcessPath.AddRange(nextMoves.ToList());
            }
            processedPath.Should().Contain(maze.Exit);
        }

        private static List<(int x, int y)> GetPathNeihboors(MazeGenerator.Maze maze, (int, int) pos)
        {
            (int x, int y) = pos;
            var walls = new List<(int x, int y)> {
                (x,y-1),
                (x-1,y),
                (x+1,y),
                (x,y+1),
            };

            return walls
                .Where(pos => pos.x >= 0 && pos.x < maze.Dimension.X)
                .Where(pos => pos.y >= 0 && pos.y < maze.Dimension.Y)
                .Where(pos => maze[pos.y, pos.x] == CaseType.Path)
                .ToList();
        }

        private static MazeGenerator.Maze CreateMaze(int width, int height)
        {
            var test = new Generator();
            var mazeRandom = test.Generate(width, height, GeneratorType.Random);
            return mazeRandom;
        }
    }
}
