using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitEnumLogic
{

    public class Item
    {
        public string label { get; set; }
        public int number { get; set; }    
    }

    class Program
    {
        private static List<Item> Items;

        static void Main(string[] args)
        {
            //Items = new List<Item>()
            //{
            //    new Item() {label="1", number = 1},
            //    new Item() {label="2", number = 2},
            //    new Item() {label="4", number = 4},
            //    new Item() {label="8", number = 8},
            //    new Item() {label="16", number = 16},
            //};

            //var output = TraceBitEnum(13);
            //var output2 = TraceBitEnum(0);
            //var output3 = TraceBitEnum(17);
            //var output4 = TraceBitEnum(1);
            //var output5 = TraceBitEnum(2);
            //var output6 = TraceBitEnum(3);
            //var output7 = TraceBitEnum(4);
            //var output8 = TraceBitEnum(7);

            //var output9 = TraceBitAndOperator(15);
            //var output10 = TraceBitEnum(15);

            var items = new List<Item>()
            {
                new Item() {label="16777216", number = 16777216},
                new Item() {label="33554432", number = 33554432},
                new Item() {label="67108864", number = 67108864},
                new Item() {label="134217728", number = 134217728},
                new Item() {label="268435456", number = 268435456},
                new Item() {label="536870912", number = 536870912},
                new Item() {label="1073741824", number = 1073741824},
                new Item() {label="2147483648", number = 2147483648},
            };

            TraceBitEnum2(4278190080, items);

            Console.Read();
        }

        private static string TraceBitAndOperator(int input)
        {
            string test;
            var sb = new StringBuilder();
            
            foreach (var item in Items)
            {
                var value = item.number & input;
                if (value != 0)
                {
                    sb.AppendLine(item.label);
                }
            }

            return sb.ToString();
        }

        private static void TraceBitEnum2(long input, List<Item> options)
        {
            foreach (var option in options)
            {
                //checkbox.checked = ((value & bitValue) == bitValue);

                bool isChecked = (input & option.number) == option.number;
                Console.WriteLine($"option {option.label} - {isChecked}");
            }
        }
    }
}
