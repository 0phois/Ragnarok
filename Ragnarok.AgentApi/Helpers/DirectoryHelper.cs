using System;
using System.IO;

namespace Ragnarok.AgentApi.Helpers
{
    internal static class DirectoryHelper
    {
        public static string GetHomeDirectory() => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        public static string GetCurrentDirectory() => Directory.GetCurrentDirectory();

        public static string DefaultConfigPath() => Path.Combine(GetHomeDirectory(), ".ngrok2", "ngrok.yml");

        public static string DefaultExecutablePath() => Path.Combine(GetCurrentDirectory(), RuntimeHelper.GetNgrokExecutableString());
    }
}
