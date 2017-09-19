using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AzureVisionDemo.UWPApp
{
    public class AnalisysParameters
    {
        public VisualFeatures VisualFeatures { get; set; }

        public VisualDetails VisualDetails { get; set; }

        public StorageFile StorageFile { get; set; }
    }
}
