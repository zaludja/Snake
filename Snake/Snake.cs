using System.Collections.Generic;
using System.Windows;

namespace Snake
{
    public class Had
    {
        public List<Point> Parts { get; private set; } = new List<Point>();
        public Point Direction { get; set; } = new Point(1, 0);
        public int Length { get; set; } = 5;
         
        public Had()
        {
            Reset();
        }
        
        public void Reset()
        {
            Parts.Clear();
            Parts.Add(new Point(5, 5));
            Length = 5;
            Direction = new Point(1, 0);
        }

        public void Move()
        {
            Point head = Parts[0];
            Point newHead = new Point(head.X + Direction.X, head.Y + Direction.Y);
            Parts.Insert(0, newHead);
            if (Parts.Count > Length)
                Parts.RemoveAt(Parts.Count - 1);
        }

        public Point GetHead() => Parts[0];

        public bool IsCollision(Point p)
        {
            return Parts.Contains(p);
        }
    }
}
