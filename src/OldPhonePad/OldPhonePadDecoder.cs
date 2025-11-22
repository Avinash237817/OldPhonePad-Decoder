using System;
using System.Collections.Generic;
using System.Text;

namespace OldPhonePad
{
    /// <summary>
    /// Core decoding function. Keep this file minimal and standalone.
    /// Public API: OldPhonePadDecoder.Decode(input, optionalMap)
    /// </summary>
    public static class OldPhonePadDecoder
    {
        private static readonly IReadOnlyList<string> ClassicMap = new[]
        {
            " ", "", "ABC", "DEF", "GHI", "JKL", "MNO", "PQRS", "TUV", "WXYZ"
        };

        /// <summary>
        /// Decode the given input sequence of keypad presses into text.
        /// Rules:
        ///  - space ' ' is separator (flush pending group).
        ///  - '*' flushes pending group and backspaces 1 char from output.
        ///  - '#' flushes pending group and stops processing.
        ///  - unknown characters are treated as separators (flush).
        ///  - index = (pressCount - 1) % letters.Length for wrapping.
        /// </summary>
        public static string Decode(string? input, IReadOnlyList<string>? runtimeMap = null)
        {
            var map = PrepareMap(runtimeMap);

            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var sb = new StringBuilder(Math.Min(input.Length, 1024));
            char? currentDigit = null;
            int count = 0;

            void Flush()
            {
                if (currentDigit == null || count == 0)
                {
                    currentDigit = null;
                    count = 0;
                    return;
                }

                int idx = currentDigit.Value - '0';
                if (idx >= 0 && idx < map.Count)
                {
                    var letters = map[idx] ?? string.Empty;
                    if (letters.Length > 0)
                    {
                        int pos = (count - 1) % letters.Length;
                        sb.Append(letters[pos]);
                    }
                }

                currentDigit = null;
                count = 0;
            }

            foreach (var ch in input)
            {
                if (ch == '#')
                {
                    Flush();
                    break;
                }

                if (ch == '*')
                {
                    Flush();
                    if (sb.Length > 0) sb.Length -= 1;
                    continue;
                }

                if (ch == ' ')
                {
                    Flush();
                    continue;
                }

                if (ch >= '0' && ch <= '9')
                {
                    if (currentDigit == ch) count++;
                    else
                    {
                        Flush();
                        currentDigit = ch;
                        count = 1;
                    }
                    continue;
                }

                // unknown -> flush
                Flush();
            }

            Flush(); // final flush
            return sb.ToString();
        }

        private static IReadOnlyList<string> PrepareMap(IReadOnlyList<string>? runtimeMap)
        {
            if (runtimeMap == null) return ClassicMap;
            if (runtimeMap.Count != 10) throw new ArgumentException("runtimeMap must have exactly 10 entries (0..9).", nameof(runtimeMap));
            var arr = new string[10];
            for (int i = 0; i < 10; i++) arr[i] = (runtimeMap[i] ?? string.Empty).Trim();
            return Array.AsReadOnly(arr);
        }
    }
}
