using Maze.Drawing;
using MazeGenerator;

namespace Maze.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Generator();
            var maze = test.Generate(42, 42, GeneratorType.Split);
            var img = MazeToImage.Convert(maze);
            img.Save("zimgSplitMaze.jpg");

            for (int i = 0; i <20; i++)
            {
                var mazeRandom = test.Generate(42, 42, GeneratorType.Random);
                var imgRandomMaze = MazeToImage.Convert(mazeRandom, 8);
                imgRandomMaze.Save($"zimgRandomMaze{i}.png");
            }

            var fuckme = test.Generate(150, 150, GeneratorType.Random);
            var fuckmeMaze = MazeToImage.Convert(fuckme, 8);
            fuckmeMaze.Save($"zimgRandomMaze{42}.png");
        }
    }
}
