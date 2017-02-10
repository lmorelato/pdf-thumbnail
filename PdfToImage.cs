using System;
using System.Drawing.Imaging;
using System.IO;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;

namespace PdfThumbnail
{
    public class PdfToImage
    {
        /// <summary>
        /// Shortcut method for generating pdf thumbnails in png format
        /// The first page of pdf submitted will be saved as image
        /// See SaveImage method for additional helping
        /// </summary>
        /// <param name="file">File to be saved as png Image</param>
        /// <param name="outputPath">Output path</param>
        /// <param name="gsdllPath">Path to ghostscript dll</param>
        public static void SaveThumbnail(string file, string outputPath, string ghostPath)
        {
            SaveImage(file, outputPath, ghostPath, 1);
        }

        /// <summary>
        /// Converts a pdf document in separated .png images
        /// </summary>
        /// <param name="file">File to be saved as png Image</param>
        /// <param name="outputPath">Output path</param>
        /// <param name="gsdllPath">Path to ghostscript dll</param>
        /// <param name="page">Page to be rendered, if no page submitted, generates all</param>
        /// <param name="dpi">Quality of output image, default 100dpi</param>
        public static void SaveImage(string file, string outputPath, string gsdllPath, int? page = null, int dpi = 100)
        {
            //Checks if output exists
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            using (var rasterizer = new GhostscriptRasterizer())
            {
                //Selects the version of dll
                var dll = Environment.Is64BitProcess ? "gsdll64.dll" : "gsdll32.dll";
                var versionInfo = new GhostscriptVersionInfo(new Version(0, 0, 0), gsdllPath + dll, string.Empty,
                    GhostscriptLicense.GPL);
                rasterizer.Open(file, versionInfo, false);

                //If no page submitted
                var pages = page ?? rasterizer.PageCount;
                var guid = $"{Guid.NewGuid():N}";

            for (var i = 1; i <= pages; i++)
                {
                    //Sets file number in output image
                    var number = page == null ? "_p" + i : string.Empty;

                    //Constructs output image name
                    var pageFilePath = Path.Combine(outputPath, 
                        $"thumbnail_{Path.GetFileNameWithoutExtension(file)}{number}_{guid}.png");

                    //Checks if file already exists
                    if (File.Exists(pageFilePath))
                        File.Delete(pageFilePath);

                    //Save the image in .png format
                    var img = rasterizer.GetPage(dpi, dpi, i);
                    img.Save(pageFilePath, ImageFormat.Png);
                }
                rasterizer.Close();
            }
        }
    }
}