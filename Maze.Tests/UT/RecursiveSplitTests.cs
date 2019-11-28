using Xunit;
using FluentAssertions;
using MazeGenerator;
using System.Collections.Generic;

namespace Maze.Tests.UT
{
    public class RecursiveSplitTests
    {



        [Fact]
        public void GetWidthPossibilitiesWhenSymetricWallReturnAll()
        {
            var dimension = new Dimension(10, 10);
            var maze = MazeGenerator.Maze.Build(dimension);
            for (int x = 0; x < 10; x++)
            {
                maze.Board[0, x] = CaseType.Wall;
                maze.Board[dimension.Y - 1, x] = CaseType.Wall;
            }

            for (int y = 1; y < 9; y++)
            {
                maze.Board[y, 0] = CaseType.Wall;
                maze.Board[y, dimension.Y - 1] = CaseType.Wall;
            }

            var result = RecursiveSplitMazeGenerator.GetPossibilitiesForWidth(maze, new Area(1, 1, 8, 8));

            result.Should().HaveCount(6)
                .And.NotContain(1).And.NotContain(8);
        }

        [Fact]
        public void GetWidthPossibilitiesExcludeSymetricWall()
        {
            var dimension = new Dimension(10, 10);
            var maze = MazeGenerator.Maze.Build(dimension);
            for (int x = 0; x < 10; x++)
            {
                maze.Board[0, x] = CaseType.Wall;
                maze.Board[dimension.Y - 1, x] = CaseType.Wall;
            }

            for (int y = 1; y < 9; y++)
            {
                maze.Board[y, 0] = CaseType.Wall;
                maze.Board[y, dimension.Y - 1] = CaseType.Wall;
            }
            maze.Board[0, 4] = CaseType.Path;
            maze.Board[dimension.Y - 1, 4] = CaseType.Path;



            var result = RecursiveSplitMazeGenerator.GetPossibilitiesForWidth(maze, new Area(1, 1, 8, 8));

            result.Should().HaveCount(5)
                .And.NotContain(1).And.NotContain(8).And.NotContain(4);
        }

        [Fact]
        public void GetWidthWhenNoMiddleThenReturnSide()
        {
            var dimension = new Dimension(5, 5);
            var maze = MazeGenerator.Maze.Build(dimension);
            for (int x = 0; x < 5; x++)
            {
                maze.Board[0, x] = CaseType.Wall;
                maze.Board[dimension.Y - 1, x] = CaseType.Wall;
            }

            for (int y = 1; y < 4; y++)
            {
                maze.Board[y, 0] = CaseType.Wall;
                maze.Board[y, dimension.Y - 1] = CaseType.Wall;
            }
            maze.Board[0, 2] = CaseType.Path;
            maze.Board[dimension.Y - 1, 2] = CaseType.Path;

            var result = RecursiveSplitMazeGenerator.GetPossibilitiesForWidth(maze, new Area(1, 1, 3, 3));

            result.Should().HaveCount(2)
                .And.Contain(1).And.Contain(3);
        }
    }
}
