using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web;
using WebApiFileuploadDemo.UploadFile;
using System.IO;
using PDFix.App.Module;
using PDFixSDK.OcrTesseract;
using PDFixSDK.Pdfix;
using PDFixSDK.PdfToHtml;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApiFileuploadDemo.Controllers
{
    [RoutePrefix("api/fileupload")]
    public class FileUploadController : ApiController
    {

        // asynchronous function 
        
        [HttpPost]
        [Mime]
        public async Task<List<PDF>> Post()
        {
            try
            {
                List<PDF> pdfList = new List<PDF>();
                // file path
                var fileuploadPath = HttpContext.Current.Server.MapPath("~/UploadedFiles");

                var multiFormDataStreamProvider = new MultiFileUploadProvider(fileuploadPath);

                // Read the MIME multipart asynchronously 
                await Request.Content.ReadAsMultipartAsync(multiFormDataStreamProvider);

                string uploadingFileName = multiFormDataStreamProvider
                    .FileData.Select(x => x.LocalFileName).FirstOrDefault();
 
                pdfList = GetExtractedData();


                return pdfList.ToList();
            }
            catch(Exception ex)
            {
                throw  ex;
            }
        }
        //[HttpGet]
        //[Route("GetExtractedData")]
        private List<PDF> GetExtractedData()
        {
            List<PDF> pdfList = new List<PDF>();
            List<string> imageList = new List<string>();
            string email = "raghavendra.naidu@evry.com";                      
            string licenseKey = "2C8WQBLS76CfW8mMg";

            var resourcesDir = HttpContext.Current.Server.MapPath("~/UploadedFiles/");
            var outputDir = HttpContext.Current.Server.MapPath("~/JSON/");

            string openPath = resourcesDir + "SampleFillablePDF.pdf";

            Array.ForEach(Directory.GetFiles(outputDir), File.Delete);
            try
            {
                Directory.CreateDirectory(outputDir);
               
                Initialization.Run(email, licenseKey);


                //Convert to Image;
                imageList = ConvertToImage.Run(email, licenseKey, openPath, outputDir + "image", 1.0);

                //Convert to JSON
                pdfList = ConvertToJson.Run(email, licenseKey, openPath, outputDir + "Sample.json", imageList);

                return pdfList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        //[HttpGet]
        //[Route("GetJsonData")]
        public HttpResponseMessage GetJsonData()
        {
            var filepath = HttpContext.Current.Server.MapPath("~/JSON/Sample.json");

            var stream = new FileStream(filepath, FileMode.Open);

            var result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return result;
        }

        public HttpResponseMessage SaveJsonFile(List<PDF> pdfList)
        {
            bool isAdmin = false; bool isUser = false;
            if (pdfList.Count > 0)
            {
                string path = HttpContext.Current.Server.MapPath("~/JSON/ ");

                if (isAdmin)
                {
                    path = path + "AdminConfig\\";
                }
                if (isUser)
                {
                    path = path + "UserConfig\\";
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string json = JsonConvert.SerializeObject(pdfList);
                path = path + "Updated.json";
                File.WriteAllText(path, json);
            }

            var result = Request.CreateResponse(HttpStatusCode.OK);

            return result;
        }
    }
}
