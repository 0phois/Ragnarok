using System;
using System.IO;

namespace Ragnarok.AgentApi.Helpers
{
    internal static class DirectoryHelper
    {
        public static string GetHomeDirectory() => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        public static string GetLocalAppDataDirectory() => Path.Combine(GetHomeDirectory(), Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

        public static string GetCurrentDirectory() => Directory.GetCurrentDirectory();

        public static string DefaultConfigPath() => Path.Combine(GetLocalAppDataDirectory(), "ngrok", "ngrok.yml");

        public static string DefaultExecutablePath() => Path.Combine(GetCurrentDirectory(), RuntimeHelper.GetNgrokExecutableString());
    }
}
