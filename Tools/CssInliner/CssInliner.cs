using System;
using System.IO;

namespace CssInliner
{
    internal static class CssInliner
    {
        private static void Main(string[] args)
        {
            var sourcePath = "./";
            var cssPath = "./styles.css";
            var destinationPath = "./inlined/";

            Console.WriteLine();

            try
            {
                if (args.Length > 0)
                {
                    sourcePath = args[0];
                }

                if (args.Length > 1)
                {
                    cssPath = args[1];
                }

                if (args.Length > 2)
                {
                    destinationPath = args[2];
                }
            }
            catch
            {
                Console.WriteLine("Incorrect arguments");
                Console.WriteLine("Usage : ./CssInliner.exe <html sourcePath> <cssPath> <destinationPath>");
            }

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            var mailCss = File.ReadAllText(cssPath);
            foreach (var file in Directory.GetFiles(sourcePath, "*.html"))
            {
                var mailHtml = File.ReadAllText(file);
                var results = PreMailer.Net.PreMailer.MoveCssInline(mailHtml, css: mailCss, ignoreElements: "link");
                File.WriteAllText(Path.Combine(destinationPath, Path.GetFileName(file)), results.Html);
            }
        }
    }
}