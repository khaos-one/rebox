using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeaugmentExe
{
    class Program
    {
        static void PrintUsage()
        {
            Console.WriteLine("USAGE: DeaugmentExe.exe <INPUT_FILE> <OUTPUT_FILE> <OUTPUT_PAYLOAD_FILE>");
        }

        static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.Error.WriteLine("Insufficient parameters.");
                PrintUsage();
                return 1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("Input file was not found.");
                PrintUsage();
                return 1;
            }

            using (var fs = new FileStream(args[0], FileMode.Open))
            {
                fs.Seek(-sizeof (int), SeekOrigin.End);
                var lenBytes = new byte[sizeof (int)];
                fs.Read(lenBytes, 0, lenBytes.Length);
                var augmentArrayLength = BitConverter.ToInt32(lenBytes, 0);

                if (augmentArrayLength > fs.Length)
                {
                    Console.Error.WriteLine("Augment footer incorrect.");
                    return 2;
                }

                fs.Seek(-augmentArrayLength - sizeof (int), SeekOrigin.End);
                var augmentArray = new byte[augmentArrayLength];
                fs.Read(augmentArray, 0, augmentArrayLength);

                var fileLength = (int) fs.Length - (augmentArrayLength + sizeof (int));

                fs.Seek(0, SeekOrigin.Begin);
                var fileContent = new byte[fileLength];
                fs.Read(fileContent, 0, fileLength);

                File.WriteAllBytes(args[1], fileContent);
                File.WriteAllBytes(args[2], augmentArray);
            }

            Console.WriteLine("Done!");

            return 0;
        }
    }
}
