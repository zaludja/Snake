using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame
{
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        List<Point> snakeParts = new List<Point>();
        Point currentDirection = new Point(1, 0); // startuje doprava
        Point food;
        int snakeLength = 5;
        Random rand = new Random();
        int cellSize = 20;

        public MainWindow()
        {
            InitializeComponent();
            gameTimer.Interval = TimeSpan.FromMilliseconds(100);
            gameTimer.Tick += GameLoop;
            StartGame();
        }

        void StartGame()
        {
            snakeParts.Clear();
            snakeLength = 5;
            snakeParts.Add(new Point(5, 5));
            PlaceFood();
            gameTimer.Start();
        }

        void GameLoop(object sender, EventArgs e)
        {
            Point head = snakeParts[0];
            Point newHead = new Point(head.X + currentDirection.X, head.Y + currentDirection.Y);

            // Konec hry při kolizi se zdí nebo se sebou
            if (newHead.X < 0 || newHead.Y < 0 ||
                newHead.X >= GameCanvas.ActualWidth / cellSize ||
                newHead.Y >= GameCanvas.ActualHeight / cellSize ||
                snakeParts.Contains(newHead))
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over!");
                StartGame();
                return;
            }

            snakeParts.Insert(0, newHead);

            // Snězení jídla
            if (newHead == food)
            {
                snakeLength++;
                PlaceFood();
            }

            // Zkrácení hada na správnou délku
            while (snakeParts.Count > snakeLength)
                snakeParts.RemoveAt(snakeParts.Count - 1);

            Draw();
        }

        void Draw()
        {
            GameCanvas.Children.Clear();

            foreach (Point p in snakeParts)
            {
                Rectangle rect = new Rectangle
                {
                    Width = cellSize,
                    Height = cellSize,
                    Fill = Brushes.Green
                };
                Canvas.SetLeft(rect, p.X * cellSize);
                Canvas.SetTop(rect, p.Y * cellSize);
                GameCanvas.Children.Add(rect);
            }

            // jídlo
            Rectangle foodRect = new Rectangle
            {
                Width = cellSize,
                Height = cellSize,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(foodRect, food.X * cellSize);
            Canvas.SetTop(foodRect, food.Y * cellSize);
            GameCanvas.Children.Add(foodRect);
        }

        void PlaceFood()
        {
            int maxX = (int)(GameCanvas.ActualWidth / cellSize);
            int maxY = (int)(GameCanvas.ActualHeight / cellSize);
            food = new Point(rand.Next(maxX), rand.Next(maxY));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (currentDirection.Y != 1) currentDirection = new Point(0, -1);
                    break;
                case Key.Down:
                    if (currentDirection.Y != -1) currentDirection = new Point(0, 1);
                    break;
                case Key.Left:
                    if (currentDirection.X != 1) currentDirection = new Point(-1, 0);
                    break;
                case Key.Right:
                    if (currentDirection.X != -1) currentDirection = new Point(1, 0);
                    break;
            }
        }
    }
}
