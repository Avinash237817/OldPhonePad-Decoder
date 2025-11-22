using System;

namespace OldPhonePad.TestHarness
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: --gen <N> | --validate");
                return;
            }

            if (args[0] == "--gen")
            {
                long n = args.Length > 1 ? long.Parse(args[1]) : 1000;
                Generator.Generate(n);
                return;
            }

            if (args[0] == "--validate")
            {
                Validator.Validate();
                return;
            }

            Console.WriteLine("Unknown command.");
        }
    }
}
