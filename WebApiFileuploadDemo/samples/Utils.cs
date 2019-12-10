//\cond INTERNAL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFix.App.Module
{
    class Utils
    {
        public static String GetAbsolutePath(String name)
        {
            String path = AppDomain.CurrentDomain.BaseDirectory;
            return path + name;
        }
    }

    public class PDF
    {
        public string Page { get; set; }
        public List<PDFObject> pdfObjList { get; set; }
        public string imageUrl { get; set; }
    }



    public class PDFObject
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string OptionName { get; set; }
        public int TabOrder { get; set; }
        public int MaxLength { get; set; }
        public bool MultiLine { get; set; }
        public bool IsFormatted { get; set; }
        public bool IsChecked { get; set; }
        public bool Required { get; set; }
        public bool ReadOnly { get; set; }
        public bool IsCalculation { get; set; }
        public string FieldType { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Tooltip { get; set; }
        public string DisplayName { get; set; }
        public List<string> optionList { get; set; }
    }
}
//\endcond
