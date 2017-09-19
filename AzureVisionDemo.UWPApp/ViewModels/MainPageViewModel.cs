using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVisionDemo.UWPApp.ViewModels
{
    public class MainPageViewModel
    {
        public SelectListItem[] VisualFeaturesList { get; set; } = new[]
        {
            new SelectListItem{ Value = VisualFeatures.Categories,  Text = "Categorias" },
            new SelectListItem{ Value = VisualFeatures.Tags,        Text = "Tags" },
            new SelectListItem{ Value = VisualFeatures.Description, Text = "Descrição" },
            new SelectListItem{ Value = VisualFeatures.Faces,       Text = "Rostos" },
            new SelectListItem{ Value = VisualFeatures.ImageType,   Text = "Tipo" },
            new SelectListItem{ Value = VisualFeatures.Adult,       Text = "Adulto" },
        };

        public SelectListItem[] VisualDetailsList { get; set; } = new[]
        {
            new SelectListItem{ Value = VisualDetails.Celebrities,  Text = "Celebridades" },
            new SelectListItem{ Value = VisualDetails.Landmarks,    Text = "Locais" },
        };
    }
}
