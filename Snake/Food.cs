using System;
using System.Windows;

namespace Snake
{
    public class Food
    {
        public Point Position { get; private set; }
        private Random rand = new Random();

        public void Place(int maxX, int maxY)
        {
            Position = new Point(rand.Next(maxX), rand.Next(maxY));
        }
    }
}
