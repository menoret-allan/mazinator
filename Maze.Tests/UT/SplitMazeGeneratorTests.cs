using FluentAssertions;
using MazeGenerator;
using System;
using Xunit;

namespace Maze.Tests.UT
{
    public class SplitMazeGeneratorTests
    {
        private SplitMazeGenerator mazeGenerator = new SplitMazeGenerator();

        [Fact]
        public void GetWallYieldPos()
        {
            var area = new Area(1, 1, 10, 10);
            var rand = new Random();
            (int, int) pos = (4, 5);
            var test = mazeGenerator.GetWalls(area, pos, rand);

            test.Should().Contain(pos);
        }

        [Fact]
        public void GetWallYieldUniqueResult()
        {
            var area = new Area(1, 1, 10, 10);
            var rand = new Random();
            (int, int) pos = (4, 5);
            var test = mazeGenerator.GetWalls(area, pos, rand);

            test.Should().HaveCount(16).And.OnlyHaveUniqueItems();
        }

        [Fact]
        public void GetWallDoNotYieldBoarder()
        {
            var area = new Area(1, 1, 8, 8);
            var rand = new Random();
            (int, int) pos = (4, 5);
            var test = mazeGenerator.GetWalls(area, pos, rand);

            test.Should().NotContain(position => position.x <= 0 || position.y <= 0 || position.x >= 9 || position.y >= 9);
        }
    }
}
