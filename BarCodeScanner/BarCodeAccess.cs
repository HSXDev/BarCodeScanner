using ImageMagick;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using IronBarCode;

namespace BarCodeScanner
{
    public class BarCodeAccess
    {
        public static string ScanEANPdf(Stream file)
        {
            try
            {
                ArrayList barcodes = new ArrayList();

                var img = ConvertPDFtoImage(file);
                Bitmap b = new Bitmap(img);
                int iScans = b.Height;

                BarcodeImaging.FullScanPage(ref barcodes, b, iScans);

                // Show the results in a message box
                return ShowPDFResults(ref barcodes);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Scan PDF result
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="iScans"></param>
        /// <param name="barcodes"></param>
        public static string ShowPDFResults(ref ArrayList barcodes)
        {
            string Message = string.Empty;
            if (barcodes.Count > 0)
            {
                Message += "Found barcodes:\n";

                foreach (object bc in barcodes)
                {
                    Message += bc + "\n";
                }
            }
            else
            {
                Message += "Failed to find a barcode.";
            }

            return Message;
        }

        public static MemoryStream ConvertPDFtoImage(Stream file)
        {
            MagickReadSettings settings = new MagickReadSettings();
            MemoryStream ms = new MemoryStream();
            // Settings the density to 300 dpi will create an image with a better quality
            settings.Density = new Density(300);

            MagickNET.SetGhostscriptDirectory(@"../BarCodeApi/GhostScriptFiles");

            using (MagickImageCollection images = new MagickImageCollection())
            {
                // Add all the pages of the pdf file to the collection
                images.Read(file, settings);

                using (IMagickImage vertical = images.AppendVertically())
                {
                    // Save result as a png
                    vertical.Write(ms);
                }
            }

            return ms;
        }
    }
}
