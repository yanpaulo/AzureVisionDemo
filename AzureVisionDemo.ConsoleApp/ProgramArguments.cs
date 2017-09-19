using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AzureVisionDemo.ConsoleApp
{
    public class ProgramArguments
    {
        public VisualFeatures VisualFeatures { get; set; }

        public VisualDetails VisualDetails { get; set; }

        public string Path { get; set; }

        public bool IsUrl { get; set; }

        public static ProgramArguments Parse(string[] args)
        {
            var ret = new ProgramArguments();
            foreach (var arg in args)
            {
                if (arg.StartsWith("-f"))
                {
                    ret.VisualFeatures = (VisualFeatures)ArgToEnum(typeof(VisualFeatures), arg);
                }
                else if (arg.StartsWith("-d"))
                {
                    ret.VisualDetails = (VisualDetails)ArgToEnum(typeof(VisualDetails), arg);
                }
                else if (arg == "-url")
                {
                    ret.IsUrl = true;
                }
                else if(!arg.StartsWith("-"))
                {
                    var regex = new Regex("http[s]?://.*");

                    if (ret.Path == null)
                    {
                        ret.Path = arg;

                        if (regex.IsMatch(arg))
                        {
                            ret.IsUrl = true;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Múltiplos caminhos foram passados. Está tentando me fazer de besta?");
                    }
                }
                else
                {
                    throw new ArgumentException("Argumento(s) inválido(s).");
                }
            }

            return ret;
        }

        private static object ArgToEnum(Type type, string arg)
        {
            var codes = arg.Remove(0, 2);
            var names = Enum.GetNames(type);
            int e = 0;
            foreach (var c in codes)
            {
                e = e | (int)Enum.Parse(type, names.First(n => n[0] == char.ToUpper(c)));
            }

            return e;
        }
    }
}
