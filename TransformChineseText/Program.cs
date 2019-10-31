using System;
using System.IO;

namespace TransformChineseText
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = @"D:\Test Dir\NovelParserCore\诡境主宰 2.txt";
            string[] lines = File.ReadAllLines(input);
            for (int i=0; i<lines.Length; i++)
            {
                lines[i] = lines[i].Replace(',', '，');
                lines[i] = lines[i].Replace('.', '．');
                lines[i] = lines[i].Replace('-', '一');
                lines[i] = lines[i].Replace('/', ' ');
                lines[i] = lines[i].Replace('!', '！');
                lines[i] = lines[i].Replace('?', '？');
                lines[i] = lines[i].Replace('[', '【');
                lines[i] = lines[i].Replace(']', '】');
                lines[i] = lines[i].Replace(':', '：');
                lines[i] = lines[i].Replace("后金斯", "詹金斯");
                lines[i] = lines[i].Replace("信金斯", "詹金斯");
                lines[i] = lines[i].Replace("危金斯", "詹金斯");
                lines[i] = lines[i].Replace("言金斯", "詹金斯");
                lines[i] = lines[i].Replace("金更", "金屬");
                lines[i] = lines[i].Replace("金展", "金屬");
                lines[i] = lines[i].Replace("蜡虫", "蠟燭");
                //lines[i] = lines[i].Replace("", "");

                /*if (lines[i].StartsWith("    \"") || lines[i].StartsWith("    “"))
                {
                    if (!(lines[i].EndsWith("\"n") || lines[i].EndsWith("”")))
                    {
                        if (lines[i].StartsWith("    \""))
                        {
                            lines[i] += "\"";
                        }
                        else
                        {
                            lines[i] += "”";
                        }
                    }
                }*/
            }


            File.WriteAllLines(@"D:\Test Dir\NovelParserCore\诡境主宰 2 new.txt", lines);
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
