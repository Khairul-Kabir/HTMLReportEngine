using HTMLReportEngine.Model;
using Newtonsoft.Json;
using SelectPdf;

namespace HTMLReportEngine
{
    internal class PDFMaker
    {
        public void GeneratePDF(string folderName, string fileName, string htmlContent, int pageNumber, int totalPages)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                while (!Directory.GetFiles(currentDirectory, "*.csproj").Any())
                {
                    currentDirectory = Directory.GetParent(currentDirectory).FullName;
                }

                string folderPath = Path.Combine(currentDirectory, folderName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string pdfFilePath = Path.Combine(folderPath, fileName + ".pdf");

                PdfDocument pdfDocument = new PdfDocument();

                HtmlToPdf htmlToPdf = new HtmlToPdf();

                htmlToPdf.Options.CssMediaType = HtmlToPdfCssMediaType.Print;
                htmlToPdf.Options.PdfPageSize = PdfPageSize.A4;

                htmlToPdf.Options.DisplayHeader = true;
                htmlToPdf.Options.DisplayFooter = true;

                htmlToPdf.Header.Height = 180;

                htmlToPdf.Footer.Height = 50;

                string headerPath = Path.Combine(currentDirectory, "Template", "Header.html");
                string headerhtmlContent = File.ReadAllText(headerPath);
                PdfHtmlSection headerHtml = new PdfHtmlSection(headerhtmlContent, "");
                headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                htmlToPdf.Header.Add(headerHtml);

                PdfHtmlSection footerHtml = new PdfHtmlSection($"<div style='text-align: center;'>Page: {pageNumber} of {totalPages}</div>", "");
                footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                htmlToPdf.Footer.Add(footerHtml);
                PdfDocument pdfPage = htmlToPdf.ConvertHtmlString(htmlContent);

                foreach (PdfPage page in pdfPage.Pages)
                {
                    pdfDocument.AddPage(page);
                }

                // Save the PDF document
                pdfDocument.Save(pdfFilePath);
                pdfDocument.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while generating PDF: {ex.Message}");
            }
        }

        public List<WorldCheckEntity> ReadDataFromJson(string jsonFilePath)
        {
            try
            {
                string jsonData = File.ReadAllText(jsonFilePath);
                List<WorldCheckEntity> worldCheckEntities = JsonConvert.DeserializeObject<List<WorldCheckEntity>>(jsonData);

                return worldCheckEntities;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading JSON data: {ex.Message}");
                return null;
            }
        }

        public void GeneratePDFBatch(string folderName, string baseFileName, string jsonFilePath)
        {
            List<WorldCheckEntity> worldCheckEntities = ReadDataFromJson(jsonFilePath);

            if (worldCheckEntities == null || worldCheckEntities.Count == 0)
            {
                Console.WriteLine("No data found in JSON file.");
                return;
            }

            // Batch size
            int batchSize = 15;

            // Total batches
            int totalBatches = (int)Math.Ceiling((double)worldCheckEntities.Count / batchSize);

            for (int batchIndex = 0; batchIndex < totalBatches; batchIndex++)
            {
                // Get data for current batch
                List<WorldCheckEntity> batchData = worldCheckEntities.Skip(batchIndex * batchSize).Take(batchSize).ToList();

                // Generate HTML content for the current batch
                string htmlContent = GenerateHtmlContent(batchData, "Template", "DedupeReportTemplate.html");

                // Generate PDF file for the current batch
                string pdfFileName = $"{batchIndex + 1}";
                GeneratePDF(folderName, pdfFileName, htmlContent, batchIndex + 1, totalBatches);

                Console.WriteLine($"Done: {batchIndex}");
            }
            string currentDirectory = Directory.GetCurrentDirectory();
            while (!Directory.GetFiles(currentDirectory, "*.csproj").Any())
            {
                currentDirectory = Directory.GetParent(currentDirectory).FullName;
            }

            string folderPath = Path.Combine(currentDirectory, folderName);
            string outputPath = Path.Combine(currentDirectory, $"{folderName}\\MainPDF.pdf");
            PDFMargeByITextSharp pDFMargeByITextSharp = new PDFMargeByITextSharp();
            pDFMargeByITextSharp.MargePDF(folderPath, outputPath);
        }


        public string GenerateHtmlContent(List<WorldCheckEntity> batchData, string folderName, string fileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            while (!Directory.GetFiles(currentDirectory, "*.csproj").Any())
            {
                currentDirectory = Directory.GetParent(currentDirectory).FullName;
            }
            string folderPath = Path.Combine(currentDirectory, folderName);
            string filePath = Path.Combine(folderPath, fileName);
            string htmlContent = File.ReadAllText(filePath);
            string tbodyData = "";
            foreach (var entity in batchData)
            {
                tbodyData += $"<tr><td>{entity.FULL_NAME}</td><td>{entity.DOB}</td><td>{entity.CITIZENSHIP}</td><td>{entity.ADDRESS}</td><td>{entity.CATEGORY}</td><td>{entity.KEYWORD}</td><td>{entity.CODE}</td><td>{entity.SCORE}</td></tr>";
            }

            htmlContent = htmlContent.Replace("tbodyData", tbodyData);
            return htmlContent;
        }
    }
}
