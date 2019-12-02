using System;
using System.Collections.Generic;
using System.Text;

namespace MazeGenerator
{
    public interface IRand
    {
        int Next();
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
    }


    public class Rand : IRand
    {
        private readonly Random rand;

        public Rand(Random rand)
        {
            this.rand = rand;
        }

        public int Next() => this.rand.Next();

        public int Next(int maxValue) => this.rand.Next(maxValue);

        public int Next(int minValue, int maxValue) => this.rand.Next(minValue, maxValue);
    }
}
