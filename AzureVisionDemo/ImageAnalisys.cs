using System;
using System.Collections.Generic;
using System.Text;

namespace AzureVisionDemo
{

    public class ImageAnalisys
    {
        public string RequestId { get; set; }

        public Description Description { get; set; }

        public Category[] Categories { get; set; }

        public Tag[] Tags { get; set; }

        public Face[] Faces { get; set; }

        public Adult Adult { get; set; }
        
        public Color Color { get; set; }

        public Imagetype ImageType { get; set; }

        public Metadata Metadata { get; set; }
    }

    public class Adult
    {
        public bool IsAdultContent { get; set; }
        public bool IsRacyContent { get; set; }
        public float AdultScore { get; set; }
        public float RacyScore { get; set; }
    }

    public class Description
    {
        public Caption[] Captions { get; set; }
        public string[] Tags { get; set; }
    }

    public class Caption
    {
        public string Text { get; set; }
        public float Confidence { get; set; }
    }

    public class Metadata
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; }
    }

    public class Color
    {
        public string DominantColorForeground { get; set; }
        public string DominantColorBackground { get; set; }
        public string[] DominantColors { get; set; }
        public string AccentColor { get; set; }
        public bool IsBWImg { get; set; }
    }

    public class Imagetype
    {
        public int ClipArtType { get; set; }
        public int LineDrawingType { get; set; }
    }

    public class Category
    {
        public string Name { get; set; }
        public float Score { get; set; }
        public Detail Detail { get; set; }
    }

    public class Detail
    {
        public Celebrity[] Celebrities { get; set; }
        public Landmark[] Landmarks { get; set; }
    }

    public class Celebrity
    {
        public string Name { get; set; }
        public Facerectangle FaceRectangle { get; set; }
        public float Confidence { get; set; }
    }

    public class Facerectangle
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Landmark
    {
        public string Name { get; set; }
        public float Confidence { get; set; }
    }

    public class Tag
    {
        public string Name { get; set; }
        public float Confidence { get; set; }
    }

    public class Face
    {
        public int Age { get; set; }
        public string Gender { get; set; }
        public Facerectangle FaceRectangle { get; set; }
    }

}
