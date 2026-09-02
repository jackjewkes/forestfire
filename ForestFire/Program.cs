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
            NewFire,
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
            Cell[,] grid = ReadFile(isDefault: answer == "1");
            Console.WriteLine("Thank you, we are loading your forest grid now. Press enter to display the grid");
            Console.ReadLine();
            DrawGrid(grid);
            Console.WriteLine("It's time to start the fire simulation.");
            int fireStartingX, fireStartingY;
            while (true)
            {
                Console.Write("Please enter the x co-ordinate of the fire: ");
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out fireStartingX) && fireStartingX >= 0 && fireStartingX <= grid[].getlength(0) - 1)
                    {
                        break;
                    }
                    Console.Write("The x coordinate must be an integer within the width of the forest. Please retry: ");
                }
                Console.Write("Please enter the y co-ordinate of the fire: ");
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out fireStartingY) && fireStartingY >= 0 && fireStartingY <= grid[].getlength(1) - 1)
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
            DrawGrid(grid);
            Console.WriteLine("The fire spread simulation has now started. Press enter to see the next time step.");
            AdvanceFire(grid);
        }

        static Cell[,] ReadFile(bool isDefault)
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
                        return grid;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void DrawGrid(Cell[,] grid)
        {
            int width = grid[].getlength(0);
            int height = grid[].getlength(1);
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

        static bool AdvanceFire(Cell[,] grid)
        {
            bool fireAdvanced = false;
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x,y] == Cell.Fire)
                    {
                        if (grid[x - 1, y] == Cell.Tree)
                        {
                            grid[x - 1, y] = Cell.NewFire;
                        }
                    }
                }
            }
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x, y] == Cell.NewFire)
                    {
                        grid[x, y] = Cell.Fire;
                        fireAdvanced = true;
                    }
                }
            }
            return fireAdvanced;
        }
    }
}
