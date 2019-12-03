using Xunit;
using FluentAssertions;
using MazeGenerator;

namespace Maze.Tests.UT
{
    public class RecursiveSplitTests
    {
        [Fact]
        public void GetPossibilitiesForbidenX()
        {
            var dimension = new Dimension(4, 4);
            var maze = MazeGenerator.Maze.Build(dimension);
            maze.Board[0, 2] = CaseType.Wall;
            maze.Board[1, 0] = CaseType.Wall;
            maze.Board[3, 1] = CaseType.Wall;

            var result = RecursiveSplitMazeGenerator.GetSquare2PossibilitiesX(maze, new Area(1, 1, 2, 2), 3);

            result.Should().ContainSingle().Which.Should().Be((2, 1));
        }

        [Fact]
        public void GetPossibilitiesForbidenX2()
        {
            var dimension = new Dimension(4, 4);
            var maze = MazeGenerator.Maze.Build(dimension);
            maze.Board[0, 2] = CaseType.Wall;
            maze.Board[1, 0] = CaseType.Wall;
            maze.Board[2, 3] = CaseType.Wall;
            maze.Board[3, 2] = CaseType.Wall;

            var result = RecursiveSplitMazeGenerator.GetSquare2PossibilitiesX(maze, new Area(1, 1, 2, 2), 0);

            result.Should().ContainSingle().Which.Should().Be((2, 2));
        }

        [Fact]
        public void GetAllPossibilities()
        {
            var dimension = new Dimension(4, 4);
            var maze = MazeGenerator.Maze.Build(dimension);
            maze.Board[0, 1] = CaseType.Wall;
            maze.Board[0, 2] = CaseType.Wall;
            maze.Board[1, 0] = CaseType.Wall;
            maze.Board[2, 0] = CaseType.Wall;
            maze.Board[3, 1] = CaseType.Wall;
            maze.Board[3, 2] = CaseType.Wall;
            maze.Board[1, 3] = CaseType.Wall;
            maze.Board[2, 3] = CaseType.Wall;

            var result = RecursiveSplitMazeGenerator.GetSquare2Possibilities(maze, new Area(1, 1, 2, 2));

            result.Should().HaveCount(4);
        }

        [Fact]
        public void GetAllPossibilitiesNone()
        {
            var dimension = new Dimension(4, 4);
            var maze = MazeGenerator.Maze.Build(dimension);
            maze.Board[0, 1] = CaseType.Wall;
            maze.Board[2, 0] = CaseType.Wall;
            maze.Board[3, 2] = CaseType.Wall;
            maze.Board[1, 3] = CaseType.Wall;

            var result = RecursiveSplitMazeGenerator.GetSquare2Possibilities(maze, new Area(1, 1, 2, 2));

            result.Should().BeEmpty();
        }

        [Fact]
        public void GetAllPossibilitiesNone2()
        {
            var dimension = new Dimension(4, 4);
            var maze = MazeGenerator.Maze.Build(dimension);
            maze.Board[0, 2] = CaseType.Wall;
            maze.Board[1, 0] = CaseType.Wall;
            maze.Board[3, 1] = CaseType.Wall;
            maze.Board[2, 3] = CaseType.Wall;

            var result = RecursiveSplitMazeGenerator.GetSquare2Possibilities(maze, new Area(1, 1, 2, 2));

            result.Should().BeEmpty();
        }
    }
}
