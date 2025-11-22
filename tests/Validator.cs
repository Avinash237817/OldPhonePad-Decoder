using System;
using System.IO;
using System.Text;
using OldPhonePad;

namespace OldPhonePad.TestHarness
{
    public static class Validator
    {
        private const string InFile = "testcases.csv";
        private const string LogFile = "validation_log.txt";

        public static void Validate()
        {
            if (!File.Exists(InFile))
            {
                Console.WriteLine("[Validator] ERROR: testcases.csv not found.");
                return;
            }

            using var reader = new StreamReader(InFile);
            using var log = new StreamWriter(LogFile, false, Encoding.UTF8);

            string? header = reader.ReadLine(); // skip header

            long total = 0;
            long failed = 0;

            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                total++;

                var parts = SplitCsv(line);
                string input = parts[0];
                string expected = parts[1];

                string actual = OldPhonePadApi.InvokeDecode(input);

                if (actual != expected)
                {
                    failed++;
                    log.WriteLine($"FAIL: input=\"{input}\" expected=\"{expected}\" actual=\"{actual}\"");
                }
            }

            Console.WriteLine($"[Validator] Total={total}, Failed={failed}");
        }

        // Minimal CSV parser for 2 quoted columns: "input","expected"
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
