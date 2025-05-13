using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Snake
{
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        Had snake;
        Food food;
        Food powerup;
        int cellSize = 20;
        int score = 0;
            

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            gameTimer.Interval = TimeSpan.FromMilliseconds(200);
            gameTimer.Tick += GameLoop;
            StartGame();
        }

        void StartGame()
        {
            MessageBox.Show("jablko (červené) = +1 bod\nborůvka (modrá) = +2 body + zrychlení");
            snake = new Had();
            food = new Food();
            powerup = new Food();
            score = 0;
            txbScore.Text = score.ToString();

            PlaceFood();
            PlacePowerup();

            gameTimer.Start();
        }

        void GameLoop(object sender, EventArgs e)
        {
            snake.Move();
            Point head = snake.GetHead();

            int maxX = (int)(GameCanvas.Width / cellSize);
            int maxY = (int)(GameCanvas.Height / cellSize);

            // Kolize se stěnou nebo se sebou
            if (head.X < 0 || head.Y < 0 || head.X >= maxX || head.Y >= maxY ||
                (snake.Parts.Count(x => x == head) > 1) && head != snake.Parts[1])
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over!");
                StartGame();
                return;
            }

            if (head == food.Position)
            {
                score++;
                txbScore.Text = score.ToString();
                snake.Length++;
                PlaceFood();
            }

            if (head == powerup.Position)
            {
                score += 2;
                txbScore.Text = score.ToString();
                snake.Length++;

                double currentSpeed = gameTimer.Interval.TotalMilliseconds;
                double newSpeed = Math.Max(currentSpeed * 0.95, 50);
                gameTimer.Interval = TimeSpan.FromMilliseconds(newSpeed);

                PlacePowerup();
            }

            Draw();
        }

        void Draw()
        {
            GameCanvas.Children.Clear();

            // Had
            for (int i = 0; i < snake.Parts.Count; i++)
            {
                Ellipse segment = new Ellipse
                {
                    Width = cellSize,
                    Height = cellSize,
                    Fill = Brushes.Green,
                };
                Canvas.SetLeft(segment, snake.Parts[i].X * cellSize);
                Canvas.SetTop(segment, snake.Parts[i].Y * cellSize);
                GameCanvas.Children.Add(segment);
            }

            // Jablko
            Ellipse apple = new Ellipse
            {
                Width = cellSize,
                Height = cellSize,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(apple, food.Position.X * cellSize);
            Canvas.SetTop(apple, food.Position.Y * cellSize);
            GameCanvas.Children.Add(apple);

            // Borůvka
            Ellipse blueberry = new Ellipse
            {
                Width = cellSize,
                Height = cellSize,
                Fill = Brushes.Blue
            };
            Canvas.SetLeft(blueberry, powerup.Position.X * cellSize);
            Canvas.SetTop(blueberry, powerup.Position.Y * cellSize);
            GameCanvas.Children.Add(blueberry);
        }

        void PlaceFood()
        {
            int maxX = (int)(GameCanvas.Width / cellSize);
            int maxY = (int)(GameCanvas.Height / cellSize);
            food.Place(maxX, maxY);
        }

        void PlacePowerup()
        {
            int maxX = (int)(GameCanvas.Width / cellSize);
            int maxY = (int)(GameCanvas.Height / cellSize);
            powerup.Place(maxX, maxY);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (snake.Direction.Y != 1) snake.Direction = new Point(0, -1);
                    break;
                case Key.Down:
                    if (snake.Direction.Y != -1) snake.Direction = new Point(0, 1);
                    break;
                case Key.Left:
                    if (snake.Direction.X != 1) snake.Direction = new Point(-1, 0);
                    break;
                case Key.Right:
                    if (snake.Direction.X != -1) snake.Direction = new Point(1, 0);
                    break;
            }
        }
    }
}
