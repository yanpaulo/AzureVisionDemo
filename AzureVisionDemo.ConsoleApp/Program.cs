using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVisionDemo.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var ws = new WebService(ConfigurationManager.AppSettings["location"], ConfigurationManager.AppSettings["key"]);
            ImageAnalisys analisys;
            string saveFileName;
            ProgramArguments options;

            #region Read args or display help
            if (HelpRequested(args))
            {
                PrintHelp();
                return;
            }

            try
            {
                options = ProgramArguments.Parse(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                PrintHelp();
                return;
            } 
            #endregion

            if (options.IsUrl)
            {
                analisys = ws.AnalyzeImageAsync(options.Path, options.VisualFeatures, options.VisualDetails).Result;
                saveFileName = options.Path.Remove(0, options.Path.LastIndexOf('/') + 1);
            }
            else
            {
                var stream = new FileStream(options.Path, FileMode.Open);
                analisys = ws.AnalyzeImageAsync(stream, options.VisualFeatures, options.VisualDetails).Result;
                saveFileName = Path.GetFileNameWithoutExtension(options.Path);
            }
            var stringAnalisys = JsonConvert.SerializeObject(analisys, Formatting.Indented);
            Console.WriteLine(stringAnalisys);
            File.WriteAllText(saveFileName + ".analisys.json", stringAnalisys);
        }

        private static bool HelpRequested(string[] args) =>
            args.Length == 0 || new[] { "?", "/?", "-?", "help", "-help", "--help" }.Any(a => a == args[0]);

        private static void PrintHelp()
        {
            Console.WriteLine(
@"Uso: vision.exe [path] [-f[c][t][d][f][i][c][a]] [-d[c][l]]
-----------
path: caminho ou url do arquivo a analizar

-f: Visual Features
    c: Categories - categorizes image content according to a taxonomy defined in documentation. 
    t: Tags - tags the image with a detailed list of words related to the image content. 
    d: Description - describes the image content with a complete English sentence. 
    f: Faces - detects if faces are present. If present, generate coordinates, gender and age.
    i: ImageType - detects if image is clipart or a line drawing.
    c: Color - determines the accent color, dominant color, and whether an image is black&white.
    a: Adult - detects if the image is pornographic in nature (depicts nudity or a sex act). Sexually suggestive content is also detected.

-d: Details
    c: Celebrities - identifies celebrities if detected in the image.
    l: Landmarks - identifies landmarks if detected in the image.

--------------
Exemplo:
    vision.exe http://www.popsci.com/sites/popsci.com/files/bill_gates_july_2014.jpg -fcdf -dc
    Analiza a imagem e retorna informações com as seguintes Visual Features: Categories, Description e Faces (-fcdf).
    Informações sobre celebridades serão retornadas detalhadamente (-dc).
");
        }
    }
}
