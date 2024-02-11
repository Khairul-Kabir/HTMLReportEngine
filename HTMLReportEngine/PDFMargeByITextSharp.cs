using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReportEngine
{
    internal class PDFMargeByITextSharp
    {
        public void MargePDF(string SourcePdfPath,string outputPath)
        {
            string[] fileNames = Directory.GetFiles(SourcePdfPath);
            fileNames = fileNames.OrderBy(fileName => int.Parse(Path.GetFileNameWithoutExtension(fileName))).ToArray();

            Document doc = new Document();
            PdfCopy writer = new PdfCopy(doc, new FileStream(outputPath, FileMode.Create));
            if (writer == null)
            {
                return;
            }
            doc.Open();
            foreach (string filename in fileNames)
            {
                PdfReader reader = new PdfReader(filename);
                reader.ConsolidateNamedDestinations();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    writer.AddPage(page);
                }
                reader.Close();
            }
            writer.Close();
            doc.Close();
        }
    }
}
