using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ForestFire
{
    internal class Program
    {
        enum Cell
        {
            Empty,
            Tree,
            Fire,
            Water
        }

        static void Main(string[] args)
        {
            var (width, height, grid) = ReadFile();
            DrawGrid(grid, width, height);
        }

        static (int width, int height, Cell[,] grid) ReadFile()
        {
            int width, height;
            string path;
            while (true)
            {
                Console.Write("Please enter the name of the forest file: ");
                path = Console.ReadLine();
                try
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line = sr.ReadLine();
                        if (!int.TryParse(line[0].ToString(), out width))
                        {
                            throw new FormatException("Incorrectly formatted forest width");
                        }
                        if (!int.TryParse(line[2].ToString(), out height))
                        {
                            throw new FormatException("Incorrectly formatted forest height");
                        }
                        Cell[,] grid = new Cell[width, height];
                        int y = height - 1;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] splat = line.Split(' ');
                            if (splat.Length == width)
                            {
                                for (int x = 0; x < width; x++)
                                {
                                    switch (splat[x])
                                    {
                                        case ".":
                                            grid[x, y] = Cell.Empty;
                                            break;
                                        case "T":
                                            grid[x, y] = Cell.Tree;
                                            break;
                                        default:
                                            Console.WriteLine("Splat error");
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }
                            }
                            else
                            {
                                throw new FormatException();
                            }
                            y--;
                        }
                        return (width, height, grid);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }

            }
        }

        static void DrawGrid(Cell[,] grid, int width, int height)
        {
            string line = "+" + new string('-', width * 3) + "+";
            Console.WriteLine(line);
            for (int y = height - 1; y >= 0; y--)
            {
                Console.Write("|");
                for (int x =  0; x < width; x++)
                {
                    Console.Write($" {FindTile(grid, x, y)} ");
                }
                Console.Write("|");
                Console.WriteLine();
            }
            Console.WriteLine(line);
        }

        static string FindTile(Cell[,] grid, int x, int y)
        {
            switch (grid[x, y])
            {
                case Cell.Empty:
                    return ".";
                case Cell.Tree:
                    return "T";
                case Cell.Fire:
                    return "F";
                case Cell.Water:
                    return "W";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
