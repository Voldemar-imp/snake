using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Map
{
    internal class Program
    {
        static void Main(string[] args)
        {     
            int snakeX;
            int snakeY;
            int snakeDX = 0;
            int snakeDY = 1;
            int tailValue = 1;
            int mausCount = 5;
            int[] snakeTailX = new int[tailValue];
            int[] snakeTailY = new int[tailValue];
            int sleep1 = 100;
            int sleep2 = 3000;
            bool isPlaying = true;

            char[,] map1 = ReadMap("map1", out snakeX, out snakeY);
            Console.CursorVisible = false;
            DrawMap(map1);
            snakeTailX[0] = snakeX;
            snakeTailY[0] = snakeY;

            for (int i = 0; i < mausCount; i++)
            {
                DrawNewMaus(map1);
            }

            while (isPlaying)
            {
                DrawMap(map1);

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            snakeDX = -1;
                            snakeDY = 0;
                            break;
                        case ConsoleKey.DownArrow:
                            snakeDX = 1;
                            snakeDY = 0;
                            break;
                        case ConsoleKey.RightArrow:
                            snakeDX = 0;
                            snakeDY = 1;
                            break;
                        case ConsoleKey.LeftArrow:
                            snakeDX = 0;
                            snakeDY = -1;
                            break;
                    }
                }                

                if (map1[snakeX + snakeDX, snakeY + snakeDY] != '#' && map1[snakeX + snakeDX, snakeY + snakeDY] != '&')
                {
                    DrawSnakeTail(map1, snakeTailX, snakeTailY,  snakeY, snakeX);
                    MoveSnake(map1, ref snakeY, ref snakeX, snakeDY, snakeDX);
                }
                else if (map1[snakeX + snakeDX, snakeY + snakeDY] == '&')
                {
                    tailValue++;
                    snakeTailX = ElargeArray(snakeTailX, tailValue);
                    snakeTailY = ElargeArray(snakeTailY, tailValue);
                    DrawSnakeTail(map1, snakeTailX, snakeTailY, snakeY, snakeX);
                    MoveSnake(map1, ref snakeY, ref snakeX, snakeDY, snakeDX);
                    DrawNewMaus(map1);
                }
                else
                {
                    isPlaying = false;
                }

                System.Threading.Thread.Sleep(sleep1);
            }

            FinishGame(tailValue, sleep2);
            Console.Clear();
        }

        static char[,] ReadMap(string mapName, out int snakeX, out int snakeY)
        {
            snakeX = 0;
            snakeY = 0;
            string[] newFile = File.ReadAllLines($"maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];
                    if (map[i, j] == '@')
                    {
                        snakeX = i;
                        snakeY = j;
                    }
                }
            }
;
            return map;
        }

        static void DrawMap(char[,] map)
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    string simbol = Convert.ToString(map[i, j]);
                    Console.SetCursorPosition(j, i);

                    switch (simbol)
                    {
                        case "#":
                            СhangeTextСolor(simbol, ConsoleColor.Red);
                            break;
                        case "@":
                            СhangeTextСolor(simbol, ConsoleColor.Yellow);
                            break;
                        case "&":
                            СhangeTextСolor(simbol, ConsoleColor.Green);
                            break;
                    }
                }

                Console.WriteLine();
            }
        }       

        static void DrawSnakeTail(char[,] map1, int [] snakeTailX, int[] snakeTailY, int snakeY, int snakeX)
        {
            snakeTailX = ShiftArray(snakeTailX, snakeX);
            snakeTailY = ShiftArray(snakeTailY, snakeY);
            int cursorX = snakeTailX [0];
            int cursorY = snakeTailY [0];
            Console.SetCursorPosition(cursorY, cursorX);
            map1[cursorX, cursorY] = '#';
            СhangeTextСolor("#", ConsoleColor.Red);
            cursorX = snakeTailX[snakeTailX.Length - 1];
            cursorY = snakeTailY[snakeTailY.Length - 1];
            Console.SetCursorPosition(cursorY, cursorX);
            map1[cursorX, cursorY] = ' ';
            Console.Write(" ");
        }

        static void MoveSnake(char[,] map1, ref int snakeY, ref int snakeX, int snakeDY, int snakeDX)
        {
            snakeY += snakeDY;
            snakeX += snakeDX;
            Console.SetCursorPosition(snakeY, snakeX);
            СhangeTextСolor("@", ConsoleColor.Yellow);
            map1[snakeX, snakeY] = '@';
        }

        static void DrawNewMaus(char[,] map)
        {
            Random random = new Random();
            int X = 0;
            int Y = 0;
            while (map[X, Y] != ' ')
            {
                X = random.Next(map.GetLength(0) - 1);
                Y = random.Next(map.GetLength(1) - 1);
            }
            map[X, Y] = '&';
        }

        static int[] ShiftArray(int[] array, int value)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                array[i] = array[i - 1];
            }

            array[0] = value;
            return array;
        }

        static int [] ElargeArray(int[] array, int value)
        {
            int[] tempArray = new int[value];            

            for (int i = 0; i < array.Length; i++)
            {
                tempArray [i] = array [i];                
            }

            tempArray [tempArray.Length - 1] = tempArray[tempArray.Length - 2];
            return tempArray;
        }    

        static void FinishGame(int tailValue, int sleep)
        {
            Console.SetCursorPosition(16, 0);
            СhangeTextСolor("Game over", ConsoleColor.Yellow);
            Console.SetCursorPosition(11, 1);
            СhangeTextСolor("Вы поймали " + (tailValue - 1) + " мышек", ConsoleColor.Green);
            System.Threading.Thread.Sleep(sleep);
        }

        static void СhangeTextСolor(string message, ConsoleColor сolor)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = сolor;
            Console.Write(message);
            Console.ForegroundColor = defaultColor;
        }
    }
}
