using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diamond_Pattern
{
    class Program
    {
        static void Main(string[] args)
        {
            int width= 11;
            int height = width;

            int median = width/2;

            int u_start = median;
            int u_end = median;

            int l_start = 1;
            int l_end = width - 2;

            for (int i = 0; i < height; i++)
            {
                if (i < median + 1)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (j >= u_start && j <= u_end)
                        {
                            Print("*");
                        }
                        else
                        {
                            Print();
                        }
                    }

                    u_start = u_start - 1;
                    u_end = u_end + 1;
                }
                else
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (j >= l_start && j <= l_end)
                        {
                            Print("*");
                        }
                        else
                        {
                            Print();
                        }
                    }

                    l_start = l_start + 1;
                    l_end = l_end - 1;

                }

                LineFeed();
            }


            Console.ReadKey();
        }


        static void Print(string input = " ")
        {
            Console.Write(input);
        }

        static void LineFeed()
        {
            Console.Write("\n");
        }
    }
}
