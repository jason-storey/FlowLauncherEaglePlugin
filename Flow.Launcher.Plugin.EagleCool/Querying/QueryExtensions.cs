using System;
using System.Linq;

namespace Flow.Launcher.Plugin.EagleCool
{
#pragma warning disable CS1591
    public static class QueryExtensions
    {
        public static bool Is(this Query q, string val) => q.Search.Trim().Equals(val.Trim(), StringComparison.OrdinalIgnoreCase);
        public static bool IsAnyOf(this Query q, params string[] val) => val.Any(q.Is);
    }
}