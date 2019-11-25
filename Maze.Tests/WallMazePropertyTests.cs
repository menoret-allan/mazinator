using MazeGenerator;
using System;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace Maze.Tests
{
    public class WallMazePropertyTests
    {
        [Theory]
        [InlineData(GeneratorType.Random)]
        [InlineData(GeneratorType.Split)]
        public void MazeGeneratorShouldGenerateMazeWithCorrectPathSizeAndHave2PathOnTheBorder(GeneratorType generatorType)
        {
            var rand = new Random();
            var generator = new Generator();

            for (int iteration = 0; iteration < 4; iteration++)
            {
                var width = rand.Next() % 50 + 50;
                var height = rand.Next() % 50 + 50;

                var maze = generator.Generate(width, height, generatorType);

                VerifyThatBlackAreIn2Blocks(maze);
            }

        }

        private static void VerifyThatBlackAreIn2Blocks(MazeGenerator.Maze maze)
        {
            var wallEntrance = GetWallEntranceNeihboors(maze, maze.Entrance);
            wallEntrance.Count.Should().Be(2);

            var AllBlackWalls = GetAllBlack(maze).ToList();
            var firstSide = GetLinkedWalls(maze, wallEntrance.First(), new List<(int x, int y)>(AllBlackWalls)).Distinct().ToList();
            var secondSide = GetLinkedWalls(maze, wallEntrance.Last(), new List<(int x, int y)>(AllBlackWalls)).Distinct().ToList();

            foreach (var first in firstSide)
            {
                AllBlackWalls.Should().Contain(first);
                AllBlackWalls.Remove(first);
            }
            foreach (var second in secondSide)
            {
                AllBlackWalls.Should().Contain(second);
                AllBlackWalls.Remove(second);
            }


            foreach (var test in AllBlackWalls)
            {
                maze.Board[test.y, test.x] = CaseType.Debug;
            }
           
            AllBlackWalls.Should().BeEmpty();
        }

        private static List<(int x, int y)> GetLinkedWalls(MazeGenerator.Maze maze, (int x, int y) pos, List<(int x, int y)> AllBlackWalls)
        {
            var wallsLinked = new List<(int x, int y)> { pos };
            var result = new List<(int x, int y)>();
            while (wallsLinked.Any())
            {
                var first = wallsLinked.First();
                result.Add(first);
                wallsLinked.Remove(first);

                var voisins = GetWallNeihboors(maze, first);
                var fetch = AllBlackWalls.Where(pos => voisins.Contains(pos)).ToList();
                foreach (var lal in fetch)
                {
                    wallsLinked.Add(lal);
                    AllBlackWalls.Remove(lal);
                }
            }
            return result;
        }

        private static IEnumerable<(int x, int y)> GetAllBlack(MazeGenerator.Maze maze)
        {
            for (int x = 0; x < maze.Dimension.X; x++)
            {
                for (int y = 0; y < maze.Dimension.Y; y++)
                {
                    if (maze[y, x] == CaseType.Wall)
                        yield return (x, y);
                }
            }
        }

        private static List<(int x, int y)> GetWallNeihboors(MazeGenerator.Maze maze, (int, int) pos)
        {
            (int x, int y) = pos;
            var walls = new List<(int x, int y)> {
                (x-1,y-1),
                (x,y-1),
                (x+1,y-1),
                (x-1,y),
                (x+1,y),
                (x-1,y+1),
                (x,y+1),
                (x+1,y+1),
            };

            return walls
                .Where(pos => pos.x >= 0 && pos.x < maze.Dimension.X)
                .Where(pos => pos.y >= 0 && pos.y < maze.Dimension.Y)
                .Where(pos => {
                    return maze[pos.y, pos.x] == CaseType.Wall;
                })
                .ToList();
        }

        private static List<(int x, int y)> GetWallEntranceNeihboors(MazeGenerator.Maze maze, (int, int) entrance)
        {
            (int x, int y) = entrance;
            var walls = new List<(int x, int y)> {
                (x,y-1),
                (x-1,y),
                (x+1,y),
                (x,y+1),
            };

            return walls
                .Where(pos => pos.x >= 0 && pos.x < maze.Dimension.X)
                .Where(pos => pos.y >= 0 && pos.y < maze.Dimension.Y)
                .Where(pos => {
                    return maze[pos.y, pos.x] == CaseType.Wall;
                })
                .ToList();
        }
    }
}
