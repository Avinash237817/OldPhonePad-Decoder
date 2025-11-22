using System.Collections.Generic;

namespace OldPhonePad
{
    /// <summary>
    /// Thin API wrapper / invoker. Tests will call this file.
    /// Keeps the Decode implementation separate from test harness calling code.
    /// </summary>
    public static class OldPhonePadApi
    {
        public static string InvokeDecode(string input, IReadOnlyList<string>? runtimeMap = null)
        {
            // we just forward to the core function
            return OldPhonePadDecoder.Decode(input, runtimeMap);
        }
    }
}
