using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;

namespace NovelSiteParser
{
    static class Utilities
    {
        public static string TrimEnd(string source, string trimChars)
        {
            if (string.IsNullOrEmpty(source)) return source;
            if (string.IsNullOrEmpty(trimChars)) return source;
            return source.EndsWith(trimChars) ? source.Remove(source.LastIndexOf(trimChars, StringComparison.Ordinal)) : source;
        }

        public static string TrimStart(string source, string trimChars)
        {
            if (string.IsNullOrEmpty(source)) return source;
            if (string.IsNullOrEmpty(trimChars)) return source;
            return source.TrimStart(trimChars.ToCharArray());
        }

        public static string TrimIllegalPath(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            path = r.Replace(path, "");
            return path;
        }

        public static string ToSimplified(string argSource)
        {
            var t = ChineseConverter.Convert(argSource, ChineseConversionDirection.TraditionalToSimplified);
            return t;
        }

        public static string ToTraditional(string argSource)
        {
            var t = ChineseConverter.Convert(argSource, ChineseConversionDirection.SimplifiedToTraditional);
            return t;
        }
    }
}
