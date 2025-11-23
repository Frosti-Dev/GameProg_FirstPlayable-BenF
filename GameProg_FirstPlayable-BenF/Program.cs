using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameProg_FirstPlayable_BenF
{
    internal class Program
    {
        static bool playerAlive = true;
        static bool playerTurn;
        static int playerHealth = 10;
        static int playerPosX = 5;
        static int playerPosY = 5;


        static int enemy1Health = 2;
        static int enemy2Health = 3;
        static int enemy3Health = 1;
        static (int,int) enemy1Pos = (1, 12);
        static (int,int) enemy2Pos = (23, 1);
        static (int,int) enemy3Pos = (23, 12);

        static bool enemy1Alive = true;
        static bool enemy2Alive = true;
        static bool enemy3Alive = true;

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

            //Displays
            //Console.WriteLine("Map legend: ");
            //Console.WriteLine("^ = mountain");
            //Console.WriteLine("` = grass");
            //Console.WriteLine("~ = water");
            //Console.WriteLine("* = trees");
            Console.WriteLine(" ");

            Console.WriteLine("Predict where they will go to hurt them!!! But watch your postition...if they get to you first then you'll take damage!");
            Console.WriteLine("Head on collsions will result in you taking damage.");
            Console.WriteLine(" ");


            Console.WriteLine("Controls: WASD to move");
            Console.WriteLine("          Space to stall");
            Console.WriteLine(" ");


            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Health: {playerHealth}");
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enemy 1 Health: {enemy1Health}");
            Console.WriteLine($"Enemy 2 Health: {enemy2Health}");
            Console.WriteLine($"Enemy 3 Health: {enemy3Health}");

            Console.ForegroundColor = ConsoleColor.White;
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
            if (Enemy == enemy1Pos)
            {
                Console.Write("1");
            }

            if (Enemy == enemy2Pos)
            {
                Console.Write("2");
            }

            if (Enemy == enemy3Pos)
            {
                Console.Write("3");
            }
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

            if (enemy1Health == 0)
            {
                enemy1Alive = false;
                enemy1Pos = (50, 0);
            }

            if (enemy2Health == 0)
            {
                enemy2Alive = false;
                enemy2Pos = (50, 0);

            }

            if (enemy3Health == 0)
            {
                enemy3Alive = false;
                enemy3Pos = (50, 0);

            }

            return Enemy;
        }

        
        static void CheckCollides(bool PlayerTurn)
        {
            (int, int) playerPos = (playerPosX, playerPosY);

            if (PlayerTurn)
            {
                if (playerPos == enemy1Pos)
                {
                    enemy1Health -= 1;
                    
                    //back to spawn
                    enemy1Pos = (1, 12);
                }

                else if (playerPos == enemy2Pos)
                {
                    enemy2Health -= 1;

                    //back to spawn
                    enemy2Pos = (23, 1);
                }

                else if (playerPos == enemy3Pos)
                {
                    enemy3Health -= 1;

                    //back to spawn
                    enemy3Pos = (23, 12);
                }

                else
                {
                    //do nothing
                }

                playerTurn = false;
            }

            else
            {
                if (playerPos == enemy1Pos)
                {
                    playerHealth -= 1;

                    //back to spawn
                    enemy1Pos = (1, 12);
                }

                else if (playerPos == enemy2Pos)
                {
                    playerHealth -= 1;

                    //back to spawn
                    enemy2Pos = (23, 1);
                }

                else if (playerPos == enemy3Pos)
                {
                    playerHealth -= 1;

                    //back to spawn
                    enemy3Pos = (23, 12);
                }

                else
                {
                    //do nothing
                }

                playerTurn = true;
            }
        }

        static void Main(string[] args)
        {
            
            while (playerAlive)
            {
                Console.Clear();
                DisplayMap(scale);

                if (enemy1Alive)
                {
                    EnemyDraw(enemy1Pos);
                }

                if (enemy2Alive)
                {
                    EnemyDraw(enemy2Pos);
                }

                if (enemy3Alive)
                {
                    EnemyDraw(enemy3Pos);
                }

                PlayerDraw(playerPosX, playerPosY);

                //enemy turn

                if (enemy1Alive)
                {
                    enemy1Pos = EnemyUpdate(enemy1Pos);
                }
                
                if(enemy2Alive)
                {
                    enemy2Pos = EnemyUpdate(enemy2Pos);
                }
                
                if (enemy3Alive)
                {
                    enemy3Pos = EnemyUpdate(enemy3Pos);
                }
                
                CheckCollides(playerTurn);

                //player turn

                PlayerUpdate();
                CheckCollides(playerTurn);

                if (playerHealth == 0)
                {
                    playerAlive = false;
                }

                if (!enemy1Alive && !enemy2Alive && !enemy3Alive)
                {
                    break;
                }

                Debug.Print($"Cords: {playerPosX},{playerPosY}");
            }

            Console.Clear();

            if (playerAlive)
            {
                Console.WriteLine("You win!");
            }

            else
            {
                Console.WriteLine("You lose");
            }
                
        }
    }
}
