////////////////////////////////////////////////////////////////////////////////////////////////////
// OcrWithTesseract.cs
// Copyright (c) 2018 PDFix. All Rights Reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////

/*!
\page CS_Samples C# Samples
- \subpage OcrWithTesseract_cs
*/
/*!
\page OcrWithTesseract_cs Ocr With Tesseract Sample
Example how to convert an image based PDF to searchable document.
NOTE: If your tessdata dir is in the /usr/share/tesseract-ocr dir, data_path should be set to /usr/share/tesseract-ocr.
\snippet /OcrWithTesseract.cs OcrWithTesseract_cs
*/

//\cond INTERNAL
//! [OcrWithTesseract_cs]
using PDFixSDK.OcrTesseract;
using PDFixSDK.Pdfix;
using System;

namespace PDFix.App.Module
{
    class OcrWithTesseract
    {
        public static void Run(
            String email,                               // authorization email   
            String licenseKey,                          // authorization license key
            String openPath,                            // source PDF document
            String savePath,                            // output PDF document
            String dataPath,                            // path to OCR data
            String language                             // default OCR language
            )
        {
            Pdfix pdfix = new Pdfix();
            if (pdfix == null)
                throw new Exception("Pdfix initialization fail");

            if (!pdfix.Authorize(email, licenseKey))
                throw new Exception(pdfix.GetError());

            OcrTesseract ocr = new OcrTesseract();
            if (ocr == null)
                throw new Exception("OcrTesseract initialization fail");

            if (!ocr.Initialize(pdfix))
                throw new Exception(pdfix.GetError());

            PdfDoc doc = pdfix.OpenDoc(openPath, "");
            if (doc == null)
                throw new Exception(pdfix.GetError());

            ocr.SetLanguage(language);
            ocr.SetDataPath(dataPath);

            TesseractDoc ocrDoc = ocr.OpenOcrDoc(doc);
            if (ocrDoc == null)
                throw new Exception(pdfix.GetError());


            //if (!ocrDoc.Save(savePath, ocrParams, null, IntPtr.Zero))
            //    throw new Exception(pdfix.GetError());

            ocrDoc.Close();
            doc.Close();
            pdfix.Destroy();
        }
    }
}
//! [OcrWithTesseract_cs]
//\endcond
