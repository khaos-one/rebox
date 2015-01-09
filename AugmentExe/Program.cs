using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AugmentExe
{
    class Program
    {
        static void PrintUsage()
        {
            Console.WriteLine("USAGE: AugmentExe.exe <INPUT_FILE> <OUTPUT_FILE> Name=File [Name=File ...]");
        }

        static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.Error.WriteLine("Insufficient parameters.");
                PrintUsage();
                return 1;
            }

            var inputFilePath = args[0];

            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine("Failed to find input file `{0}`.", inputFilePath);
                PrintUsage();
                return 1;
            }

            var outputFilePath = args[1];
            var sections = new Dictionary<string, byte[]>(args.Length - 2);

            for (var i = 2; i < args.Length; i++)
            {
                var temp = args[i].Split('=');

                if (temp.Length < 2)
                    continue;

                if (!File.Exists(temp[1]))
                {
                    Console.Error.WriteLine("Warning: file `{0}` was not found.", temp[1]);
                    continue;
                }

                var fileContent = File.ReadAllBytes(temp[1]);
                sections.Add(temp[0], fileContent);
            }

            var inputFile = File.ReadAllBytes(inputFilePath);
            byte[] augmentArray;

            using (var ms = new MemoryStream())
            {
                using (var zip = new ZipArchive(ms, ZipArchiveMode.Create))
                {
                    foreach (var kv in sections)
                    {
                        var entry = zip.CreateEntry(kv.Key);

                        using (var writer = new BinaryWriter(entry.Open()))
                        {
                            writer.Write(kv.Value);
                        }
                    }
                }

                augmentArray = ms.ToArray();
            }

            using (var fs = new FileStream(outputFilePath, FileMode.Create))
            {
                fs.Write(inputFile, 0, inputFile.Length);
                fs.Write(augmentArray, 0, augmentArray.Length);

                var lenBytes = BitConverter.GetBytes(augmentArray.Length);
                fs.Write(lenBytes, 0, lenBytes.Length);
            }

            Console.WriteLine("Done!");

            return 0;
        }
    }
}
