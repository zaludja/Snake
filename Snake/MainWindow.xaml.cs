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
        DispatcherTimer gameTimer = new DispatcherTimer();  // herní smyčka - opakuje se každých pár millisekund (kontroluje pozici hada)
        List<Point> snakeParts = new List<Point>(); // seznam částí hada
        Point currentDirection = new Point(1, 0); // směr pohybu, startuje doprava   ???
        Point food; // pozice jablka
        Point powerup; // pozice hrusky
        int snakeLength = 5; 
        Random rand = new Random();
        int cellSize = 20;  // velikost buněk po kterych se had pohybuje
        int score = 0; // skóre
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            gameTimer.Interval = TimeSpan.FromMilliseconds(200);
            gameTimer.Tick += GameLoop; // zapina časovač
            StartGame();
        }

        void StartGame()  // vyčištení předchozí hry a nastavení nové
        {
            MessageBox.Show("jablko(červené) jen dává body a zvětšuje hada, boruvka(modrá) dává body, zmenšuje hada, ale zrychluje pohyb"); 
            txbScore.Text = score.ToString();
            snakeParts.Clear(); 
            snakeLength = 5;
            snakeParts.Add(new Point(5, 5));
            PlaceFood();
            PlacePowerup();
            gameTimer.Start();
        }

        void GameLoop(object sender, EventArgs e) // beží každých 100ms
        {
            Point head = snakeParts[0];
            Point newHead = new Point(head.X + currentDirection.X, head.Y + currentDirection.Y); // vypočítává pozici hlavy podle směru

            // Konec hry při kolizi se zdí nebo se sebou
            if (newHead.X < 0 || newHead.Y < 0 ||
                newHead.X >= GameCanvas.ActualWidth / cellSize ||
                newHead.Y >= GameCanvas.ActualHeight / cellSize ||
                snakeParts.Contains(newHead))
            {
                gameTimer.Stop(); // ukaže se okno že hra skončila
                MessageBox.Show("Game Over!");
                StartGame();
                return;
            }

            snakeParts.Insert(0, newHead);
          

            // Snězení jídla
            if (newHead == food) // přidává 1 bod a zvětšuje hada
            {
                score++;
                txbScore.Text = score.ToString();
                snakeLength++;
                PlaceFood();
            }

            if (newHead == powerup) // přidává 2 body ale delá vetší rychlost a delšího hada
            {
                double currentSpeed = gameTimer.Interval.TotalMilliseconds;
                double newSpeed = Math.Max(currentSpeed * 0.95, 50); 
                gameTimer.Interval = TimeSpan.FromMilliseconds(newSpeed);
                score = score + 2;
                txbScore.Text = score.ToString();
                snakeLength++;
                PlacePowerup();
            }

            // Zkrácení hada na správnou délku
            while (snakeParts.Count > snakeLength)
                snakeParts.RemoveAt(snakeParts.Count - 1);

            Draw();
        }

        void Draw() // vykresluje hada a jídlo
        {
            GameCanvas.Children.Clear();

            foreach (Point p in snakeParts) // tvar hada
            {
                Ellipse snake = new Ellipse
                {
                    Width = cellSize,
                    Height = cellSize,
                    Fill = Brushes.Green
                };
                Canvas.SetLeft(snake, p.X * cellSize);
                Canvas.SetTop(snake, p.Y * cellSize);
                GameCanvas.Children.Add(snake);
            }



            Ellipse apple = new Ellipse // tvar jablka
            {
                Width = cellSize,
                Height = cellSize,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(apple, food.X * cellSize);
            Canvas.SetTop(apple, food.Y * cellSize);
            GameCanvas.Children.Add(apple);

            Ellipse blueberry = new Ellipse
            {
                Width = cellSize,
                Height = cellSize,
                Fill = Brushes.Blue,
            };
            Canvas.SetLeft(blueberry, powerup.X * cellSize);
            Canvas.SetTop(blueberry, powerup.Y * cellSize);
            GameCanvas.Children.Add(blueberry);
        }

        void PlaceFood()    // pokládání jídla na random podle velikosti plochy
        {
            int maxX = (int)(GameCanvas.ActualWidth / cellSize);
            int maxY = (int)(GameCanvas.ActualHeight / cellSize);
            food = new Point(rand.Next(maxX), rand.Next(maxY));
            
        }

        void PlacePowerup()    // pokládání powerupu na random podle velikosti plochy
        {
            int maxX = (int)(GameCanvas.ActualWidth / cellSize);
            int maxY = (int)(GameCanvas.ActualHeight / cellSize);
            powerup = new Point(rand.Next(maxX), rand.Next(maxY));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)  // pohybování
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
