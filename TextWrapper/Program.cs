using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TextWrapper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int lineLength = 80;
            if (args.Length > 1) 
            {
                lineLength = Convert.ToInt32(args[1]);
            }

            var path = Path.GetFullPath(args[0]);

            IEnumerable<FileInfo> files;
            if (File.Exists(path))
            {
                files = new[] { new FileInfo(path) };
            }
            else { 
                var dir = new DirectoryInfo(Path.GetFullPath(args[0]));
                files = dir.GetFiles("*.txt", SearchOption.TopDirectoryOnly).Union(dir.GetFiles("*.md", SearchOption.TopDirectoryOnly));
            }

            foreach (var file in files)
            {
                File.WriteAllText(file.FullName + ".out", File.ReadAllText(file.FullName).Wrap(lineLength));
            }
        }
    }

    public static class StringExtensions
    {
        public static string Wrap(this string text, int maxLength = 72)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (text.Length == 0) return string.Empty;

            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var fittedLines = lines.SelectMany(l => l.LinesWithinLength(maxLength)).ToArray();
            return String.Join(Environment.NewLine, fittedLines);
        }

        public static IEnumerable<string> LinesWithinLength(this string text, int length)
        {
            if (text.Length == 0) 
                yield return String.Empty;
            else if (text.Length <= length)
                yield return text;
            else
            {
                int position = length;
                char c = text[position];
                if (c != ' ')
                {
                    while (c != ' ')
                    {
                        if (position > 0)
                        {
                            position--;
                            c = text[position];
                        }
                        else
                        {
                            position = length - 1;
                            break;
                        }
                    }

                    position++; // Keep the last space
                }
                yield return text.Substring(0, position);
                foreach (var fittedLine in text.Substring(position).TrimStart().LinesWithinLength(length))
                    yield return fittedLine;
            }
        }

        public static string Wrap2(this string text, int maxLength = 72)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (text.Length == 0) return string.Empty;

            var sb = new StringBuilder();
            foreach (var unwrappedLine in text.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                var line = new StringBuilder();
                foreach (var word in unwrappedLine.Split(' '))
                {
                    var needsLeadingSpace = line.Length > 0;

                    var extraLength = (needsLeadingSpace ? 1 : 0) + word.Length;
                    if (line.Length + extraLength > maxLength)
                    {
                        sb.AppendLine(line.ToString());
                        line.Clear();
                        needsLeadingSpace = false;
                    }

                    if (needsLeadingSpace)
                        line.Append(" ");

                    line.Append(word);
                }

                sb.AppendLine(line.ToString());
            }

            return sb.ToString();
        }
    }
}