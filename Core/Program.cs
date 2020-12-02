using Fclp;
using ImageToPDF.Logger.Core;
using ImageToPDF.Logger.Entities;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;

namespace ImageToPDF.Core
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var fluentCommandLine = new FluentCommandLineParser<Arguments>
            {
                IsCaseSensitive = false
            };
            fluentCommandLine.Setup(a => a.SourceImage).As('s', "source").Required();
            fluentCommandLine.Setup(a => a.FinalPath).As('p', "finalpath");
            fluentCommandLine.SetupHelp("?", "h", "help")
                             .UseForEmptyArgs()
                             .Callback(_ => Console.WriteLine("Command:\n" +
                                                              "        -s or --source [path to image file] indicates the image[bmp, png, jpeg, gif] to convert for PDF.\n" +
                                                              "        -p or --finalpath [path to save the image]"));

            ICommandLineParserResult result = fluentCommandLine.Parse(args);
            if (!result.HasErrors)
            {
                string sourceImageArgument = fluentCommandLine.Object.SourceImage, fileExtension = Path.GetExtension(sourceImageArgument),
                       finalPathArgument = fluentCommandLine.Object.FinalPath;

                try
                {
                    if (!string.IsNullOrEmpty(sourceImageArgument) && File.Exists(sourceImageArgument))
                    {
                        Log.WriteLine($"Transforming the {fileExtension} into PDF.");

                        using (var pdf = new PdfDocument())
                        {
                            using (XImage xImg = XImage.FromFile(sourceImageArgument))
                            {
                                PdfPage page = pdf.AddPage();

                                int width = xImg.PixelWidth, height = xImg.PixelHeight;
                                page.Width = width;
                                page.Height = height;

                                XGraphics.FromPdfPage(page).DrawImage(xImg, 0, 0, width, height);
                            }

                            if (string.IsNullOrEmpty(finalPathArgument))
                            {
                                string finalDirectory = Path.GetDirectoryName(sourceImageArgument), finalCompleteDirectory = $"{finalDirectory}\\{Path.GetFileNameWithoutExtension(sourceImageArgument)}.pdf";
                                pdf.Save(finalCompleteDirectory);

                                Log.WriteLine($"The PDF was saved in the folder: \"{finalDirectory}\"", LoggerEvents.Success);
                            }
                            else
                            {
                                pdf.Save(finalPathArgument);

                                Log.WriteLine($"The PDF was saved in the folder: \"{finalPathArgument}\"", LoggerEvents.Success);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.WriteLine($"An error when trying to turn the .{fileExtension} in PDF. ({e.Message} | StackTrace: {e.StackTrace})", LoggerEvents.Error);
                }
            }
            else
            {
                Log.WriteLine("An error occurred while trying to identify past commands, make sure they were typed correctly.", LoggerEvents.Error);
            }
        }
    }
}