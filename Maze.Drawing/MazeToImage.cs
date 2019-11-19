
using System.Drawing;

namespace Maze.Drawing
{
    public class MazeToImage
    {
        static public Image Convert(MazeGenerator.Maze maze, int pixelSize = 1)
        {
            Bitmap img = new Bitmap(maze.Dimension.X * pixelSize, maze.Dimension.Y * pixelSize);

            for (int x = 0; x < maze.Dimension.X; x++)
            {
                for (int y = 0; y < maze.Dimension.Y; y++)
                {
                    switch (maze.Board[y, x])
                    {
                        case MazeGenerator.CaseType.Path:
                            DrawPixel(img, x, y, pixelSize, Color.White);
                            break;
                        case MazeGenerator.CaseType.Wall:
                            DrawPixel(img, x, y, pixelSize, Color.Black);
                            break;
                        case MazeGenerator.CaseType.Debug:
                            DrawPixel(img, x, y, pixelSize, Color.Green);
                            break;
                    }
                }
            }
            return img;
        }

        private static void DrawPixel(Bitmap img, int x, int y, int pixelSize, Color color)
        {
            for (int posX = 0; posX < pixelSize; posX++)
                for (int posY = 0; posY < pixelSize; posY++)
                    img.SetPixel(x * pixelSize + posX, y * pixelSize + posY, color);
        }
    }
}
