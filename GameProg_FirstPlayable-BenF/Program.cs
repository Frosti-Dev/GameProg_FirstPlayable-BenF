using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameProg_FirstPlayable_BenF
{
    internal class Program
    {
        static int playerHealth;
        static int playerPosX = 5;
        static int playerPosY = 5;
        static (int,int) enemy1Pos = (1, 12);
        static (int,int) enemy2Pos = (23, 1);
        static (int,int) enemy3Pos = (23, 12);

        static int scale = 1;

        static char[,] map = new char[,] // dimensions defined by following data:
    {
        {'^','^','^','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
        {'^','^','`','`','`','`','*','*','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','~','~','~','`','`','`'},
        {'^','^','`','`','`','*','*','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','~','~','~','`','`','`','`','`'},
        {'^','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
        {'`','`','`','`','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
        {'`','`','`','`','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
        {'`','`','`','~','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','^','^','`','`','`','`','`','`'},
        {'`','`','`','`','`','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','^','^','^','^','`','`','`','`','`'},
        {'`','`','`','`','`','~','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','^','^','^','^','`','`','`'},
        {'`','`','`','`','`','`','`','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
        {'`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
        {'`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
    };
        static void DisplayMap(int scale)
        {
           
            //border top
            Console.Write("+");
            for (int k = 0; k < map.GetLength(0) * scale * 2; k++)
            {
                Console.Write('-');
            }
            Console.Write('+');
            Console.WriteLine(" ");

            //print map
            for (int i = 0; i < map.GetLength(0); i++)
            {

                int row = 0;
                while (row < scale)
                {
                    Console.Write('|');
                    for (int j = 0; j < map.GetLength(0); j++)
                    {
                        int timer = 0;
                        while (timer < scale)
                        {
                            Console.Write(map[i, j] + " ");
                            timer++;
                        }
                    }
                    Console.Write('|');
                    Console.WriteLine(" ");
                    row++;
                }
            }

            //bottom border
            Console.Write("+");
            for (int f = 0; f < map.GetLength(0) * scale * 2; f++)
            {
                Console.Write('-');
            }
            Console.Write('+');
            Console.WriteLine(" ");

            Console.WriteLine("Map legend: ");
            Console.WriteLine("^ = mountain");
            Console.WriteLine("` = grass");
            Console.WriteLine("~ = water");
            Console.WriteLine("* = trees");
        }

        static void PlayerDraw(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("X");
            Console.SetCursorPosition(x, y);
        }

        static void PlayerUpdate()
        {

            ConsoleKeyInfo keyinfo = Console.ReadKey(true);

            switch (keyinfo.Key)
            {
                case ConsoleKey.W:
                    playerPosY -= 1;
                    if(playerPosY <= 0)
                    {
                        playerPosY += 1;
                    }
                    break;

                case ConsoleKey.A:

                    playerPosX -= 1;

                    if(playerPosX <= 0)
                    {
                        playerPosX += 1;
                    }

                    break;

                case ConsoleKey.S:

                    playerPosY += 1;

                    if(playerPosY > 12 * scale)
                    {
                        playerPosY -= 1;
                    }
                    break;

                case ConsoleKey.D:

                    playerPosX += 1;

                    if(playerPosX > 24 * scale)
                    {
                        playerPosX -= 1;
                    }
                    break;

                case ConsoleKey.Escape:

                    Console.Clear();
                    Console.WriteLine("You Quit.");
                    Environment.Exit(0);
                    break;
            }
        }

        static void EnemyDraw((int, int) Enemy)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(Enemy.Item1, Enemy.Item2);
            Console.Write("X");
            Console.SetCursorPosition(Enemy.Item1, Enemy.Item2);
            Console.ForegroundColor = ConsoleColor.White;

        }

        static (int,int) EnemyUpdate((int,int) Enemy)
        {
            //aligns enemy x with player x
            if(Enemy.Item1 > playerPosX)
            {
                Enemy.Item1 -= 1;
            }

            else if (Enemy.Item1 < playerPosX)
            {
                Enemy.Item1 += 1;
            }
            else
            {
                //do nothing
            }

            //aligns enemy y with player y
            if (Enemy.Item2 > playerPosY)
            {
                Enemy.Item2 -= 1;
            }

            else if (Enemy.Item2 < playerPosY)
            {
                Enemy.Item2 += 1;
            }
            else
            {
                //do nothing
            }

            return Enemy;
        }

        static void Main(string[] args)
        {
            
            while (true)
            {
                Console.Clear();
                DisplayMap(scale);



                //enemy turn

                

                EnemyDraw(enemy1Pos);
                EnemyDraw(enemy2Pos);
                EnemyDraw(enemy3Pos);

                enemy1Pos = EnemyUpdate(enemy1Pos);
                enemy2Pos = EnemyUpdate(enemy2Pos);
                enemy3Pos = EnemyUpdate(enemy3Pos);

                //player turn
                PlayerDraw(playerPosX, playerPosY);
                PlayerUpdate();

                
                Debug.Print($"Cords: {playerPosX},{playerPosY}");
            }
        }
    }
}
