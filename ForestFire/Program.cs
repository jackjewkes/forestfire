using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestFire
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DrawGrid(4, 4);
        }

        static void DrawGrid(int width, int height)
        {
            Console.Write(string.Concat("+", new string('-', width * 3), "+"));
            for (int y = 0; y <= height; y++)
            {
                Console.Write()
                for (int x =  0; x <= width; x++)
                {
                    Console.Write("s");
                }
                Console.WriteLine();
            }
            Console.WriteLine("");
        }
    }
}
