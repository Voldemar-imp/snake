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
            int snakePositionX;
            int snakePositionY;
            int snakeDirectionX = 0;
            int snakeDirectionY = 1;
            int tailValue = 1;
            int mausCount = 5;
            int[] snakeTailPositionsX = new int[tailValue];
            int[] snakeTailPositionsY = new int[tailValue];
            int sleep1 = 100;
            int sleep2 = 3000;
            bool isPlaying = true;

            char[,] map1 = ReadMap("map1", out snakePositionX, out snakePositionY);
            Console.CursorVisible = false;
            DrawMap(map1);
            snakeTailPositionsX[0] = snakePositionX;
            snakeTailPositionsY[0] = snakePositionY;

            for (int i = 0; i < mausCount; i++)
            {
                DrawNewMaus(map1);
            }

            while (isPlaying)
            {
                DrawMap(map1);
                GetDirection(ref  snakeDirectionX, ref  snakeDirectionY);             

                if (map1[snakePositionX + snakeDirectionX, snakePositionY + snakeDirectionY] != '#' && map1[snakePositionX + snakeDirectionX, snakePositionY + snakeDirectionY] != '&')
                {
                    DrawSnakeTail(map1, snakeTailPositionsX, snakeTailPositionsY,  snakePositionY, snakePositionX);
                    MoveSnake(map1, ref snakePositionY, ref snakePositionX, snakeDirectionY, snakeDirectionX);
                }
                else if (map1[snakePositionX + snakeDirectionX, snakePositionY + snakeDirectionY] == '&')
                {
                    tailValue++;
                    snakeTailPositionsX = ElargeArray(snakeTailPositionsX, tailValue);
                    snakeTailPositionsY = ElargeArray(snakeTailPositionsY, tailValue);
                    DrawSnakeTail(map1, snakeTailPositionsX, snakeTailPositionsY, snakePositionY, snakePositionX);
                    MoveSnake(map1, ref snakePositionY, ref snakePositionX, snakeDirectionY, snakeDirectionX);
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

        static char[,] ReadMap(string mapName, out int positionX, out int positionY)
        {
            positionX = 0;
            positionY = 0;
            string[] newFile = File.ReadAllLines($"maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];
                    if (map[i, j] == '@')
                    {
                        positionX = i;
                        positionY = j;
                    }
                }
            }

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

        static void GetDirection(ref int snakeDirectionX, ref int snakeDirectionY)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        snakeDirectionX = -1;
                        snakeDirectionY = 0;
                        break;
                    case ConsoleKey.DownArrow:
                        snakeDirectionX = 1;
                        snakeDirectionY = 0;
                        break;
                    case ConsoleKey.RightArrow:
                        snakeDirectionX = 0;
                        snakeDirectionY = 1;
                        break;
                    case ConsoleKey.LeftArrow:
                        snakeDirectionX = 0;
                        snakeDirectionY = -1;
                        break;
                }
            }
        }

        static void DrawSnakeTail(char[,] map1, int [] snakeTailPositionsX, int[] snakeTailPositionsY, int snakePositionY, int snakePositionX)
        {
            snakeTailPositionsX = ShiftArray(snakeTailPositionsX, snakePositionX);
            snakeTailPositionsY = ShiftArray(snakeTailPositionsY, snakePositionY);
            int cursorPositionX = snakeTailPositionsX [0];
            int cursorPositionY = snakeTailPositionsY [0];
            Console.SetCursorPosition(cursorPositionY, cursorPositionX);
            map1[cursorPositionX, cursorPositionY] = '#';
            СhangeTextСolor("#", ConsoleColor.Red);
            cursorPositionX = snakeTailPositionsX[snakeTailPositionsX.Length - 1];
            cursorPositionY = snakeTailPositionsY[snakeTailPositionsY.Length - 1];
            Console.SetCursorPosition(cursorPositionY, cursorPositionX);
            map1[cursorPositionX, cursorPositionY] = ' ';
            Console.Write(" ");
        }

        static void MoveSnake(char[,] map1, ref int snakePositionsY, ref int snakePositionsX, int snakeDirectionY, int snakeDirectionX)
        {
            snakePositionsY += snakeDirectionY;
            snakePositionsX += snakeDirectionX;
            Console.SetCursorPosition(snakePositionsY, snakePositionsX);
            СhangeTextСolor("@", ConsoleColor.Yellow);
            map1[snakePositionsX, snakePositionsY] = '@';
        }

        static void DrawNewMaus(char[,] map)
        {
            Random random = new Random();
            int positionX = 0;
            int positionY = 0;

            while (map[positionX, positionY] != ' ')
            {
                positionX = random.Next(map.GetLength(0) - 1);
                positionY = random.Next(map.GetLength(1) - 1);
            }

            map[positionX, positionY] = '&';
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
            Console.SetCursorPosition(0, 0);
            СhangeTextСolor("Game over", ConsoleColor.Yellow);
            Console.SetCursorPosition(0, 1);
            СhangeTextСolor("Вы поймали " + (tailValue - 1) + " мышек", ConsoleColor.Green);
            System.Threading.Thread.Sleep(sleep);
        }

        static void СhangeTextСolor(string text, ConsoleColor сolor)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = сolor;
            Console.Write(text);
            Console.ForegroundColor = defaultColor;
        }
    }
}
