using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Number_Pattern
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintPattern1();

            LineFeed();

            PrintPattern2();

            Console.ReadKey();
        }

        private static void PrintPattern2()
        {
            int printable = 1;
            int count = 0;

            for (int i = 0; i < 5; i++)
            {
               

                for (int j = 0; j < 5; j++)
                {
                    if (j > count)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(printable);
                    }
                }

                LineFeed();
                count = count + 1;
                printable = printable + 1;
            }
        }

        private static void PrintPattern1()
        {
            int width = 9;
            int height = 5;

            int startIndex = 4;
            int endIndex = 4;
            int medianIndex = 1;

            for (int i = 0; i < height; i++)
            {
                int printable = 0;
                bool isDecrement = false;
                                    
                for (int j = 0; j < width; j++)
                {
                    if (j >= startIndex && j <= endIndex)
                    {
                        if (printable >= medianIndex)
                        {
                            isDecrement = true;
                        }
                       
                        if (isDecrement)
                        {
                            printable = printable - 1;
                        }
                        else
                        {
                            printable = printable + 1;
                        }

                        Console.Write(printable);
                        
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                LineFeed();
                startIndex = startIndex - 1;
                endIndex = endIndex + 1;
                medianIndex = medianIndex + 1;
            }
        }

        private static void LineFeed()
        {
            Console.WriteLine();
        }
    }
}
