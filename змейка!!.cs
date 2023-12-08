using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp.Program;
using System.Xml.Linq;

namespace ConsoleApp
{
    internal enum Wall
    {
        Width = 100,
        Height = 70
    }
    public class Snake
    {
        public static bool isStarted;
        public static Snake snake;
        public enum move { up, down, left, right, stop }
        public static move dir = move.stop;
        public static int futX = 0, futY = 0;

        public class Part
        {
            public int x, y, oldx, oldy;
        }

        public int HeadX, HeadY;
        public List<Part> parts = new List<Part>();
        public static void Init()
        {
            snake = new Snake() { HeadX = (int)Wall.Width / 2, HeadY = (int)Wall.Height / 2, parts = new List<Snake.Part>() { new Snake.Part() { x = ((int)Wall.Width / 2) - 1, y = (int)Wall.Height / 2, oldx = ((int)Wall.Width / 2) - 1, oldy = (int)Wall.Height / 2 } } };
            Console.CursorVisible = false;
            isStarted = true;
            dir = move.stop;


            for (int i = 0; i < (int)Wall.Height; i++)
            {
                for (int j = 0; j < (int)Wall.Width; j++)
                {
                    Console.SetCursorPosition(j, i);
                    if (j == 0 && i == 0) { Console.Write("o"); continue; }
                    if (j == (int)Wall.Width - 1 && i == 0)
                    {
                        Console.Write("o");
                        continue;
                    }
                    if (j == 0 && i == (int)Wall.Height - 1) { Console.Write("o"); continue; }
                    if (j == (int)Wall.Width - 1 && i == (int)Wall.Height - 1)
                    {
                        Console.Write("o");
                        continue;
                    }

                    if (i == 0 || i == (int)Wall.Height - 1)
                    {
                        Console.Write("o");
                        continue;
                    }
                    if ((j == 0 || j == (int)Wall.Width - 1))
                    {
                        Console.Write("o");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n");
            }
            SetFruit();
            Game();
        }

        internal class Program
        {


            static void Main(string[] args)
            {
                Console.SetWindowSize((int)Wall.Width, (int)Wall.Height + 5);
                Snake.Init();
            }
        }

        public static void SetFruit()
        {
            Random rnd = new Random();
            futX = rnd.Next(1, (int)Wall.Width - 1);
            futY = rnd.Next(1, (int)Wall.Height - 1);
        }

        public static void Input()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (dir != move.down)
                            dir = move.up;
                        break;
                    case ConsoleKey.DownArrow:
                        if (dir != move.up)
                            dir = move.down;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (dir != move.right)
                            dir = move.left;
                        break;
                    case ConsoleKey.RightArrow:
                        if (dir != move.left)
                            dir = move.right;
                        break;
                }
            }
        }

        public static void Draw()
        {
            Thread thread = new Thread(_ =>
            {
                Console.SetCursorPosition(snake.HeadX, snake.HeadY);
                Console.Write("*");
                for (int i = 0; i < snake.parts.Count; i++)
                {
                    Console.SetCursorPosition(snake.parts[i].oldx, snake.parts[i].oldy);
                    Console.Write(" ");
                    Console.SetCursorPosition(snake.parts[i].x, snake.parts[i].y);
                    Console.Write("*");
                }
                Console.SetCursorPosition(futX, futY);
                Console.Write("+");
            });
            thread.Start();
        }

        public static void Logic()
        {
            if (dir != move.stop)
            {
                if (snake.parts.FindAll(x => (x.x == snake.HeadX && x.y == snake.HeadY)).Count > 0)
                {
                    isStarted = false;
                }
            }
            int oldX = snake.HeadX, oldY = snake.HeadY;

            if (dir == move.up)
            {
                snake.HeadY--;
            }
            if (dir == move.down)
            {
                snake.HeadY++;
            }
            if (dir == move.left)
            {
                snake.HeadX--;
            }
            if (dir == move.right)
            {
                snake.HeadX++;
            }
            if (dir != move.stop)
            {

                if (snake.HeadX == (int)Wall.Width || snake.HeadX == 0 || snake.HeadY == 0 || snake.HeadY == (int)Wall.Height - 1)
                {
                    isStarted = false;
                }
                for (int i = 0; i < snake.parts.Count; i++)
                {
                    if (i == 0)
                    {
                        snake.parts[i].oldx = snake.parts[i].x;
                        snake.parts[i].oldy = snake.parts[i].y;
                        snake.parts[i].x = oldX;
                        snake.parts[i].y = oldY;
                        continue;
                    };
                    snake.parts[i].oldx = snake.parts[i].x;
                    snake.parts[i].oldy = snake.parts[i].y;

                    snake.parts[i].x = snake.parts[i - 1].oldx;
                    snake.parts[i].y = snake.parts[i - 1].oldy;
                }
                if (snake.HeadX == futX && snake.HeadY == futY)
                {
                    snake.parts.Add(new Snake.Part() { x = snake.parts[snake.parts.Count - 1].oldx, y = snake.parts[snake.parts.Count - 1].oldy });
                    SetFruit();
                }
            }
        }

        public static void Game()
        {
            while (isStarted)
            {
                Draw();
                Input();
                Logic();
                Thread.Sleep(66);
            }
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("играть - ENTER. выход - ESC");

            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Init();
                    Game();
                    break;
                }
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    break;
                    return;
                }
            }
        }

    }
}