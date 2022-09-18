using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;

namespace Dany2000.Models
{
    internal class SongDurationHelper
    {
        private static TimeSpan GetDuration(string filePath)
        {
            using (var shell = ShellObject.FromParsingName(filePath))
            {
                IShellProperty prop = shell.Properties.System.Media.Duration;
                var t = (ulong)prop.ValueAsObject;
                return TimeSpan.FromTicks((long)t);
            }
        }
    }
}
