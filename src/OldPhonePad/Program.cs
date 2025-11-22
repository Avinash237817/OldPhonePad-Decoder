using System;
using System.IO;
using System.Text;

namespace OldPhonePad
{
    internal class Program
    {
        static int Main(string[] args)
        {
            // file-processing mode:
            // dotnet run --project src\OldPhonePad\OldPhonePad.csproj -- --file "<in.csv>" --out "<out.csv>"
            if (args.Length >= 2 && args[0] == "--file")
            {
                string inPath = args[1];
                string outPath = (args.Length >= 4 && args[2] == "--out") ? args[3] : "results.csv";
                if (!File.Exists(inPath))
                {
                    Console.Error.WriteLine("Input file not found: " + inPath);
                    return 2;
                }

                using var r = new StreamReader(inPath, Encoding.UTF8);
                using var w = new StreamWriter(outPath, false, Encoding.UTF8);
                // assume CSV header "input,expected" (generator format)
                string? header = r.ReadLine(); // skip header if present
                w.WriteLine("input,expected,actual,match");

                string? line;
                long count = 0;
                long fails = 0;
                while ((line = r.ReadLine()) != null)
                {
                    count++;
                    var parts = SplitCsv(line);
                    var input = parts[0];
                    var expected = parts[1];

                    // call main logic
                    var actual = OldPhonePadApi.InvokeDecode(input);

                    var match = string.Equals(expected, actual, StringComparison.Ordinal) ? "PASS" : "FAIL";
                    if (match == "FAIL") fails++;

                    // CSV-safe quoting
                    w.WriteLine($"\"{input}\",\"{expected}\",\"{actual}\",\"{match}\"");
                    if ((count % 10000) == 0) Console.WriteLine($"Processed {count:N0} rows...");
                }

                Console.WriteLine($"Processing done. Processed={count}, Fails={fails}. Wrote results -> {outPath}");
                return 0;
            }

            // fallback: existing CLI behavior
            string inputArg = args.Length > 0 ? string.Join(' ', args) : Console.ReadLine() ?? "";
            var output = OldPhonePadApi.InvokeDecode(inputArg);
            Console.WriteLine(output);
            return 0;
        }

        // minimal CSV parser for generator format: "input","expected"
        private static string[] SplitCsv(string line)
        {
            int a = line.IndexOf('"');
            int b = line.IndexOf('"', a + 1);
            string input = line.Substring(a + 1, b - a - 1);

            int c = line.IndexOf('"', b + 1);
            int d = line.IndexOf('"', c + 1);
            string expected = line.Substring(c + 1, d - c - 1);

            return new[] { input, expected };
        }
    }
}
