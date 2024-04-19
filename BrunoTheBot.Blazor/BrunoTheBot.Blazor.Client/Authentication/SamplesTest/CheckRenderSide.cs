using System.Runtime.InteropServices;

namespace BrunoTheBot.Blazor.Client
{
    public class CheckRenderSide
    {
        public static bool IsBlazorServer()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !RuntimeInformation.IsOSPlatform(OSPlatform.Create("WEBASSEMBLY"));
        }

        public static bool IsBlazorWebAssembly()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Create("WEBASSEMBLY"));
        }

        public static string GetOSDescription()
        {
            return RuntimeInformation.OSDescription;
        }

        public static string GetOS()
        {
            return RuntimeInformation.RuntimeIdentifier;
        }
    }
}
