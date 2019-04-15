using System;
using System.IO;
using PreMailer;

namespace CssInliner
{
    static class CssInliner
    {
        static void Main(string[] args)
        {
            string sourcePath = "./";
            string cssPath = "./styles.css";
            string destinationPath = "./inlined/";

            Console.WriteLine();

            try
            {
                if (args.Length > 0)
                    sourcePath = args[0];
                if (args.Length > 1)
                    cssPath = args[1];
                if (args.Length > 2)
                    destinationPath = args[2];

                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                string mailCss = File.ReadAllText(cssPath);
                foreach (string file in Directory.GetFiles(sourcePath, "*.html"))
                {
                    string mailHtml = File.ReadAllText(file);
                    var results = PreMailer.Net.PreMailer.MoveCssInline(mailHtml, css: mailCss, ignoreElements: "link");
                    System.IO.File.WriteAllText(Path.Combine(destinationPath, file), results.Html);
                }
            }
            catch
            {
                Console.WriteLine("Incorrect arguments");
                Console.WriteLine("Usage : ./CssInliner.exe <html sourcePath> <cssPath> <destinationPath>");
            }
        }


    }
}
