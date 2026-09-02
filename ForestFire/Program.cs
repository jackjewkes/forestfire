using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
            Console.WriteLine("Welcome to the Fire Spread Simulation!");
            Console.WriteLine("You are an environmental scientist tracking a wildfire spreading through a forest. Your mission is to simulate the spread of fire across a grid-based map of the forest and find strategies to slow or stop the blaze.");
            Console.Write("Press 1 to load a default 4x4 forest grid, press 2 to load a forest grid from file: ");
            string answer = Console.ReadLine();
            while (answer != "1" && answer != "2")
            {
                Console.Write("Incorrect input, press 1 to load a default 4x4 forest grid, press 2 to load a forest grid from file: ");
                answer = Console.ReadLine();
            }
            var (width, height, grid) = ReadFile(isDefault: answer == "1");
            Console.WriteLine("Thank you, we are loading your forest grid now. Press enter to display the grid");
            Console.ReadLine();
            DrawGrid(grid, width, height);
            Console.WriteLine("It's time to start the fire simulation.");
            int fireStartingX, fireStartingY;
            while (true)
            {
                Console.Write("Please enter the x co-ordinate of the fire: ");
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out fireStartingX) && fireStartingX >= 0 && fireStartingX <= width - 1)
                    {
                        break;
                    }
                    Console.Write("The x coordinate must be an integer within the width of the forest. Please retry: ");
                }
                Console.Write("Please enter the y co-ordinate of the fire: ");
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out fireStartingY) && fireStartingY >= 0 && fireStartingY <= height - 1)
                    {
                        break;
                    }
                    Console.Write("The y coordinate must be an integer within the height of the forest. Please retry: ");
                }
                if (grid[fireStartingX,fireStartingY] == Cell.Tree)
                {
                    grid[fireStartingX, fireStartingY] = Cell.Fire;
                    break;
                }
                Console.WriteLine("Whoops, you can only start the fire simulation in a cell that contains a tree. Please try again.");
            }
            Console.WriteLine("Thank you, we are loading your forest grid now. Press enter to display the grid");
            Console.ReadLine();
            DrawGrid(grid, width, height);
        }

        static (int width, int height, Cell[,] grid) ReadFile(bool isDefault)
        {
            int width, height;
            string path;
            while (true)
            {
                if (isDefault)
                {
                    path = "forest_grid_1.txt";
                }
                else
                {
                    Console.Write("Please enter the name of the forest file: ");
                    path = Console.ReadLine();
                }
                try
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line = sr.ReadLine();
                        string[] splat = line.Split(' ');
                        if (!int.TryParse(splat[0], out width))
                        {
                            throw new FormatException("Incorrectly formatted forest width");
                        }
                        if (!int.TryParse(splat[1], out height))
                        {
                            throw new FormatException("Incorrectly formatted forest height");
                        }
                        Cell[,] grid = new Cell[width, height];
                        int y = height - 1;
                        while ((line = sr.ReadLine()) != null)
                        {
                            splat = line.Split(' ');
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
                                throw new FormatException("Unknown error");
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
