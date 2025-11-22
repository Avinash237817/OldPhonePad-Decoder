using System;
using System.IO;
using System.Text;
using OldPhonePad;

namespace OldPhonePad.TestHarness
{
    public static class Generator
    {
        private static readonly Random R = new Random();
        private static readonly string OutFile = "testcases.csv";

        public static void Generate(long count)
        {
            using var w = new StreamWriter(OutFile, false, Encoding.UTF8);

            // CSV header
            w.WriteLine("input,expected");

            for (long i = 0; i < count; i++)
            {
                string input = RandomInput();
                string expected = OldPhonePadApi.InvokeDecode(input);

                // Always quote for safety
                w.WriteLine($"\"{input}\",\"{expected}\"");
            }

            Console.WriteLine($"[Generator] Created {count} cases → {OutFile}");
        }

        private static string RandomInput()
        {
            int len = R.Next(1, 25); // random length between 1–25
            char[] chars = "0123456789*# ".ToCharArray();

            var sb = new StringBuilder();
            for (int i = 0; i < len; i++)
                sb.Append(chars[R.Next(chars.Length)]);

            return sb.ToString();
        }
    }
}
