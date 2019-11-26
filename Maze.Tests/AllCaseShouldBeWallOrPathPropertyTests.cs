using MazeGenerator;
using System;
using Xunit;
using FluentAssertions;

namespace Maze.Tests
{
    public class AllCaseShouldBeWallOrPathPropertyTests
    {
        [Theory]
        [InlineData(GeneratorType.Random)]
        [InlineData(GeneratorType.Split)]
        public void MazeGeneratorShouldWithPathAndWallOnly(GeneratorType generatorType)
        {
            var rand = new Random();
            var generator = new Generator();

            for (int iteration = 0; iteration < 15; iteration++)
            {
                var width = rand.Next() % 25 + 50;
                var height = rand.Next() % 25 + 50;

                MazeGenerator.Maze mazeRandom = generator.Generate(width, height, generatorType);
                VerifyMazeContent(mazeRandom);
            }
        }

        private static void VerifyMazeContent(MazeGenerator.Maze maze)
        {
            int caseFound = 0;
            foreach (var caseType in maze.Board){
                if (caseType != CaseType.Path && caseType != CaseType.Wall) caseFound++;
            }
            caseFound.Should().Be(0);
        }
    }
}