using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    // Enum for the direction of the snake
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    class Program
    {
        // Console window width and height
        const int WindowWidth = 40;
        const int WindowHeight = 20;

        // Snake variables
        static List<int> SnakeXPositions = new List<int>();
        static List<int> SnakeYPositions = new List<int>();
        static int SnakeLength;
        static Direction SnakeDirection;

        // Food variables
        static int FoodXPosition;
        static int FoodYPosition;
        static bool FoodEaten;

        // Game control variables
        static bool GameOverFlag;
        static bool RestartFlag;
        static int GameSpeed;

        // Input variables
        static ConsoleKeyInfo LastKeyPressed;

        static void Main(string[] args)
        {
            Console.Title = "Snake Game";
            Console.CursorVisible = false;
            Console.SetWindowSize(WindowWidth, WindowHeight);
            Console.SetBufferSize(WindowWidth, WindowHeight);

            DifficultyMenu();
            while (true)
            {
                InitializeGame();
                RunGameLoop();

                Console.Clear();
                Console.WriteLine("Game over! Press 'R' to restart or any other key to exit.");
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.R)
                    continue;
                else
                    break;
            }
        }

        static void DifficultyMenu()
        {
            Console.WriteLine("Select difficulty level:");
            Console.WriteLine("1. Beginner (Slow)");
            Console.WriteLine("2. Average (Medium)");
            Console.WriteLine("3. Expert (Fast)");

            ConsoleKeyInfo selection;
            do
            {
                selection = Console.ReadKey(true);
            } while (selection.Key != ConsoleKey.D1 && selection.Key != ConsoleKey.D2 && selection.Key != ConsoleKey.D3);

            switch (selection.Key)
            {
                case ConsoleKey.D1:
                    GameSpeed = 200; // Beginner (Slow)
                    break;
                case ConsoleKey.D2:
                    GameSpeed = 100; // Average (Medium)
                    break;
                case ConsoleKey.D3:
                    GameSpeed = 50; // Expert (Fast)
                    break;
            }
        }

        static void InitializeGame()
        {
            // Initialize snake
            SnakeXPositions.Clear();
            SnakeYPositions.Clear();
            SnakeXPositions.Add(WindowWidth / 2);
            SnakeYPositions.Add(WindowHeight / 2);
            SnakeLength = 1;
            SnakeDirection = Direction.Right;

            // Initialize food
            GenerateFood();

            // Initialize game control variables
            GameOverFlag = false;
            RestartFlag = false;

            // Initialize input variables
            LastKeyPressed = new ConsoleKeyInfo();
        }

        static void GenerateFood()
        {
            Random random = new Random();
            FoodXPosition = random.Next(0, WindowWidth);
            FoodYPosition = random.Next(0, WindowHeight);
            FoodEaten = false;
        }

        static void RunGameLoop()
        {
            while (!GameOverFlag && !RestartFlag)
            {
                if (Console.KeyAvailable)
                {
                    LastKeyPressed = Console.ReadKey(true);
                    ProcessKeyPress();
                }

                MoveSnake();
                CheckCollision();
                RenderGame();

                Thread.Sleep(GameSpeed);
            }
        }

        static void ProcessKeyPress()
        {
            switch (LastKeyPressed.Key)
            {
                case ConsoleKey.W:
                    if (SnakeDirection != Direction.Down)
                        SnakeDirection = Direction.Up;
                    break;
                case ConsoleKey.S:
                    if (SnakeDirection != Direction.Up)
                        SnakeDirection = Direction.Down;
                    break;
                case ConsoleKey.A:
                    if (SnakeDirection != Direction.Right)
                        SnakeDirection = Direction.Left;
                    break;
                case ConsoleKey.D:
                    if (SnakeDirection != Direction.Left)
                        SnakeDirection = Direction.Right;
                    break;
                case ConsoleKey.R:
                    RestartFlag = true;
                    break;
                case ConsoleKey.P:
                    PauseGame();
                    break;
            }
        }

        static void MoveSnake()
        {
            int snakeHeadX = SnakeXPositions.First();
            int snakeHeadY = SnakeYPositions.First();

            switch (SnakeDirection)
            {
                case Direction.Up:
                    snakeHeadY--;
                    break;
                case Direction.Down:
                    snakeHeadY++;
                    break;
                case Direction.Left:
                    snakeHeadX--;
                    break;
                case Direction.Right:
                    snakeHeadX++;
                    break;
            }

            if (snakeHeadX == FoodXPosition && snakeHeadY == FoodYPosition)
            {
                // Food eaten, increase snake length
                SnakeLength++;
                GenerateFood();
            }
            else
            {
                // Remove tail segment
                SnakeXPositions.RemoveAt(SnakeLength - 1);
                SnakeYPositions.RemoveAt(SnakeLength - 1);
            }

            SnakeXPositions.Insert(0, snakeHeadX);
            SnakeYPositions.Insert(0, snakeHeadY);
        }

        static void CheckCollision()
        {
            int snakeHeadX = SnakeXPositions.First();
            int snakeHeadY = SnakeYPositions.First();

            // Check collision with walls or self
            if (snakeHeadX >= WindowWidth || snakeHeadX < 0 ||
                snakeHeadY >= WindowHeight || snakeHeadY < 0 ||
                SnakeXPositions.Skip(1).Contains(snakeHeadX) &&
                SnakeYPositions.Skip(1).Contains(snakeHeadY))
            {
                GameOverFlag = true;
            }
        }

        static void RenderGame()
        {
            Console.Clear();

            // Render snake
            for (int i = 0; i < SnakeLength; i++)
            {
                int snakeX = SnakeXPositions[i];
                int snakeY = SnakeYPositions[i];
                Console.SetCursorPosition(snakeX, snakeY);
                Console.Write("*");
            }

            // Render food
            Console.SetCursorPosition(FoodXPosition, FoodYPosition);
            Console.Write("@");
        }

        static void PauseGame()
        {
            Console.Clear();
            Console.WriteLine("Game paused. Press any key to resume...");
            Console.ReadKey(true);
        }
    }
}
