using System.Collections.Generic;

namespace OldPhonePad
{

    public static class OldPhonePadApi
    {
        public static string InvokeDecode(string input, IReadOnlyList<string>? runtimeMap = null)
        {

            return OldPhonePadDecoder.Decode(input, runtimeMap);
        }
    }
}
