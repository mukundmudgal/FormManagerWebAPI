using System;
using PDFixSDK.Pdfix;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PDFix.App.Module
{
    class ConvertToImage
    {
        public static List<string> Run(
              String email,
              String licenseKey,
              String openPath,
              String imgPath,
              Double zoom
              //PdfPageRenderParams params
                )
        {
            try
            {
                List<string> imageList = new List<string>();
                Pdfix pdfix = new Pdfix();
                if (pdfix == null)
                    throw new Exception("Pdfix initialization fail");
                if (!pdfix.Authorize(email, licenseKey))
                    throw new Exception(pdfix.GetError());

                PdfDoc doc = pdfix.OpenDoc(openPath, "");
                if (doc == null)
                    throw new Exception(pdfix.GetError());

                for (int i = 0; i < doc.GetNumPages(); i++)
                {

                    PdfPage page = doc.AcquirePage(i);
                    if (page == null)
                        throw new Exception(pdfix.GetError());

                    PdfPageView pageView = page.AcquirePageView(zoom, PdfRotate.kRotate0);
                    if (pageView == null)
                        throw new Exception(pdfix.GetError());

                    int width = pageView.GetDeviceWidth();
                    int height = pageView.GetDeviceHeight();

                    PsImage image = pdfix.CreateImage(width, height,
                       PsImageDIBFormat.kImageDIBFormatArgb);
                    if (image == null)
                        throw new Exception(pdfix.GetError());

                    PdfPageRenderParams pdfPageRenderParams = new PdfPageRenderParams();
                    pdfPageRenderParams.image = image;
                    pdfPageRenderParams.matrix = pageView.GetDeviceMatrix();

                    pdfPageRenderParams.render_flags = Pdfix.kRenderAnnot;

                    if (!page.DrawContent(pdfPageRenderParams, null, IntPtr.Zero))
                        throw new Exception(pdfix.GetError());

                    PsStream stream = pdfix.CreateFileStream(imgPath + i.ToString() + ".jpg", PsFileMode.kPsWrite);

                    imageList.Add(imgPath + i.ToString());

                    PdfImageParams imgParams = new PdfImageParams();
                    imgParams.format = PdfImageFormat.kImageFormatJpg;
                    imgParams.quality = 75;

                    if (!image.SaveToStream(stream, imgParams))
                        throw new Exception(pdfix.GetError());

                    stream.Destroy();

                    pageView.Release();
                    page.Release();

                    
                }
                doc.Close();
                return imageList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
