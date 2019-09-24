using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_File_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var deviceImagePath =
                @"C:\ProgramData\YOKOGAWA\FDT_YDTM\DD_LIB\Packages\438b36f5-761f-43f6-86c5-034c52164e01\fdipackage\attachments";

            var ext = new List<string> { ".jpg", ".gif", ".png", ".bmp" };
            var myFiles = Directory.GetFiles(deviceImagePath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => ext.Contains(Path.GetExtension(s)));

            var imageFile = myFiles.FirstOrDefault();

            if (imageFile != null)
            {
                var bytes = File.ReadAllBytes(imageFile);
                var data = BitConverter.ToString(bytes).Replace("-", "");
                Console.Write(data);
            }

           
            Console.Read();
        }
    }
}
