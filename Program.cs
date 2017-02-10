using System;
using System.Drawing.Imaging;
using System.IO;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using PdfThumbnail;

namespace Pdf2Img
{
    internal class Program
    {
        private static void Main()
        {
            //Save first page as thumbnail
            PdfToImage.SaveThumbnail("output\\xray_user_manual.pdf", "output\\", "gsdll\\");

            //Save the entire document
            PdfToImage.SaveImage("output\\xray_user_manual.pdf", "output\\", "gsdll\\");
        }
    }
}