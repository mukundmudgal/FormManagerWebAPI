using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PDFixSDK.Pdfix;

// using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace PDFix.App.Module
{
    class ConversionJSONToPDF
    {

        public static void Run(
        String email,                               // authorization email   
        String licenseKey,                          // authorization license key
        String openPath,                            // source PDF document
        String savePath,                            // output PDF document
        String configPath,                          // configuration file
        PdfFlattenAnnotsParams flattenAnnotsParams
        )
        {
            Pdfix pdfix = new Pdfix();
            if (pdfix == null)
                throw new Exception("Pdfix initialization fail");

            if (!pdfix.Authorize(email, licenseKey))
                throw new Exception(pdfix.GetError());

            PdfDoc doc = pdfix.OpenDoc(openPath, "");
            if (doc == null)
                throw new Exception(pdfix.GetError());

            try
            {
                using (StreamReader r = new StreamReader(configPath + "\\Sample.json"))
                {
                    string json = r.ReadToEnd();
                    var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PDF>>(json);

                    for (int i = 0; i < doc.GetNumFormFields(); i = i + 1)
                    {
                        String name, value;

                        PdfFormField field = doc.GetFormField(i);
                        name = field.GetFullName();
                        //var pdfObject = jsonObject.Find(obj => obj.name == name);
                        //value = pdfObject.values;
                        //if (field != null)
                        //{
                        //    doc.GetFormField(i).SetValue(value);
                        //    doc.SetInfo(name, value);
                        //}
                        //else
                        //{
                        //    field.SetValue("");
                        //}
                    }

                    if (!doc.Save(savePath, PdfSaveFlags.kSaveFull))
                    {
                        throw new Exception(pdfix.GetError());
                    }

                    if (!doc.FlattenAnnots(flattenAnnotsParams))
                        throw new Exception(pdfix.GetError());

                    if (!doc.Save(savePath, PdfSaveFlags.kSaveFull))
                        throw new Exception(pdfix.GetError());

                    doc.Close();
                    pdfix.Destroy();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
