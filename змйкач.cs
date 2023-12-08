using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    private static int windowWidth = 80;
    private static int windowHeight = 25;
    private static int snakeSpeed = 150;
    private static List<(int, int)> snake = new List<(int, int)>();
    private static (int, int) food;
    private static int score = 0;
    private static Direction currentDirection = Direction.Right;
    private static bool gameover = false;
    private static Random random = new Random();

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    static void Main()
    {
        Console.Title = "Змейка";
        Console.CursorVisible = false;
        Console.SetWindowSize(windowWidth, windowHeight);
        Console.SetBufferSize(windowWidth, windowHeight);
        InitializeSnake();
        PlaceFood();
        while (!gameover)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentDirection != Direction.Down)
                            currentDirection = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentDirection != Direction.Up)
                            currentDirection = Direction.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (currentDirection != Direction.Right)
                            currentDirection = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        if (currentDirection != Direction.Left)
                            currentDirection = Direction.Right;
                        break;
                }
            }
            MoveSnake();
            CheckCollision();
            Draw();
            Thread.Sleep(snakeSpeed);
        }
    }

    static void InitializeSnake()
    {
        int startX = windowWidth / 2;
        int startY = windowHeight / 2;
        for (int i = 0; i < 4; i++)
        {
            var tailSegment = (startX - i, startY);
            snake.Add(tailSegment);
        }
    }

    static void PlaceFood()
    {
        int x = random.Next(1, windowWidth - 1);
        int y = random.Next(1, windowHeight - 1);
        food = (x, y);
    }

    static void MoveSnake()
    {
        var head = snake[0];
        switch (currentDirection)
        {
            case Direction.Up:
                head.Item2--;
                break;
            case Direction.Down:
                head.Item2++;
                break;
            case Direction.Left:
                head.Item1--;
                break;
            case Direction.Right:
                head.Item1++;
                break;
        }
        snake.Insert(0, head);
        if (head.Item1 == food.Item1 && head.Item2 == food.Item2)
        {
            score++;
            PlaceFood();
        }
        else
        {
            snake.RemoveAt(snake.Count - 1);
        }
    }

    static void CheckCollision()
    {
        var head = snake[0];
        if (head.Item1 <= 0 || head.Item1 >= windowWidth - 1 ||
            head.Item2 <= 0 || head.Item2 >= windowHeight - 1 ||
            snake.Skip(1).Any(segment => segment.Item1 == head.Item1 && segment.Item2 == head.Item2))
        {
            gameover = true;
        }
        
    }

    static void Draw()
    {
        Console.Clear();
        for (int i = 0; i < windowWidth; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("|");
        }
        for (int i = 0; i < windowWidth; i++)
        {
            Console.SetCursorPosition(i, windowHeight - 1);
            Console.Write("|");
        }
        foreach (var segment in snake)
        {
            Console.SetCursorPosition(segment.Item1, segment.Item2);
            Console.Write("=");
            Console.SetCursorPosition(0, segment.Item2);
            Console.Write("|");
            Console.SetCursorPosition(windowWidth - 1, segment.Item2);
            Console.Write("|");
        }
        Console.SetCursorPosition(food.Item1, food.Item2);
        Console.Write("@");
        Console.SetCursorPosition(windowWidth - 10, windowHeight - 1);
        Console.Write($"Score: {score}");
    }
}
