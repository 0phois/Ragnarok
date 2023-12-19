using System;
using System.Runtime.InteropServices;

namespace Ragnarok.AgentApi.Helpers
{
    internal static class RuntimeHelper
    {
        public static string GetArchitectureString()
        {
            var architecture = RuntimeInformation.ProcessArchitecture;
            switch (architecture)
            {
                case Architecture.X64: return "amd64";
                case Architecture.X86: return "386";
                case Architecture.Arm or Architecture.Arm64:
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) throw new PlatformNotSupportedException();

                    return architecture.ToString().ToLower();
                default:
                    throw new PlatformNotSupportedException();
            }
        }

        public static string GetOsString()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return "windows";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)) return "freebsd";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return "linux";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return "darwin";

            throw new PlatformNotSupportedException();
        }

        public static string GetOsArchitectureString() => $"{GetOsString()}-{GetArchitectureString()}";

        public static string GetNgrokExecutableString() => "ngrok" + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : string.Empty);
    }
}
