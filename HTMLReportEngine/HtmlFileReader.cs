using HTMLReportEngine.Model;
using SelectPdf;
using System;
using System.Text.Json;

public class HtmlFileReader
{

    public string ReadHtmlFile(string folderName, string fileName)
    {
        try
        {
            // Get the current directory where the application is running
            string currentDirectory = Directory.GetCurrentDirectory();
            while (!Directory.GetFiles(currentDirectory, "*.csproj").Any())
            {
                currentDirectory = Directory.GetParent(currentDirectory).FullName;
            }
            // Combine the current directory with the folder name and file name to get the full path
            string folderPath = Path.Combine(currentDirectory, folderName);
            string filePath = Path.Combine(folderPath, fileName);

            // Read the contents of the HTML file
            string htmlContent = File.ReadAllText(filePath);

            string dataPath = Path.Combine(currentDirectory, "Template/DummyData/DedupeResponse.json");
            string jsonData = File.ReadAllText(dataPath);

            // Deserialize JSON to list of WorldCheckEntity objects
            List<WorldCheckEntity>? worldCheckEntities = JsonSerializer.Deserialize<List<WorldCheckEntity>>(jsonData);

            // Here you can manipulate the HTML content and map it with the WorldCheckEntity list

            // Replace "tbodyData" placeholder with actual data from WorldCheckEntity list

            // Example:
            string tbodyData = "";
            foreach (var entity in worldCheckEntities)
            {
                tbodyData += $"<tr><td>{entity.FULL_NAME}</td><td>{entity.DOB}</td><td>{entity.CITIZENSHIP}</td><td>{entity.ADDRESS}</td><td>{entity.CATEGORY}</td><td>{entity.KEYWORD}</td><td>{entity.CODE}</td><td>{entity.SCORE}</td></tr>";
            }

            htmlContent = htmlContent.Replace("tbodyData", tbodyData);

            return htmlContent;
        }
        catch (Exception ex)
        {
            // Handle exceptions, such as if the file does not exist or cannot be read
            Console.WriteLine($"An error occurred while reading the HTML file: {ex.Message}");
            return null;
        }
    }
    public void GeneratePDF(string folderName, string fileName, string htmlContent)
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        while (!Directory.GetFiles(currentDirectory, "*.csproj").Any())
        {
            currentDirectory = Directory.GetParent(currentDirectory).FullName;
        }

        // Combine the current directory with the folder name to get the full path
        string folderPath = Path.Combine(currentDirectory, folderName);

        // If the folder does not exist, create it
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Combine the folder path with the file name and extension to get the full path
        string pdfFilePath = Path.Combine(folderPath, fileName + ".pdf");

        // Create a new PdfDocument object
        PdfDocument pdfDocument = new PdfDocument();

        // Create a new HtmlToPdf object
        HtmlToPdf htmlToPdf = new HtmlToPdf();

        // Set the CSS media type
        htmlToPdf.Options.CssMediaType = HtmlToPdfCssMediaType.Print;

        // Set the page size to A4
        htmlToPdf.Options.PdfPageSize = PdfPageSize.A4;

        // Convert HTML content to PDF
        PdfDocument pdfPage = htmlToPdf.ConvertHtmlString(htmlContent);


        // footer settings
        htmlToPdf.Options.DisplayFooter = true;
        htmlToPdf.Footer.DisplayOnFirstPage = true;
        htmlToPdf.Footer.DisplayOnOddPages = true;
        htmlToPdf.Footer.DisplayOnEvenPages = true;
        htmlToPdf.Footer.Height = 50;
        // add some html content to the footer
        PdfHtmlSection footerHtml = new PdfHtmlSection("Test Footer");
        footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        htmlToPdf.Footer.Add(footerHtml);

        // page numbers can be added using a PdfTextSection object
        PdfTextSection text = new PdfTextSection(0, 10, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
        text.HorizontalAlign = PdfTextHorizontalAlign.Right;
        htmlToPdf.Footer.Add(text);

        // Add each page of the converted PDF to the main PDF document
        foreach (PdfPage page in pdfPage.Pages)
        {
            pdfDocument.AddPage(page);
        }

        // Save the PDF document
        //pdfDocument.Save(pdfFilePath);

        byte[] pdf = pdfDocument.Save();
        File.WriteAllBytes(pdfFilePath, pdf);

        // Close the PDF document
        //pdfDocument.Close();
    }

    public void GeneratePDFV2(string folderName, string fileName, string htmlContent)
    {
        try
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            while (!Directory.GetFiles(currentDirectory, "*.csproj").Any())
            {
                currentDirectory = Directory.GetParent(currentDirectory).FullName;
            }

            // Combine the current directory with the folder name to get the full path
            string folderPath = Path.Combine(currentDirectory, folderName);

            // If the folder does not exist, create it
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            // Get the full path for the folder and the PDF file
            string pdfFilePath = Path.Combine(folderPath, fileName + ".pdf");

            // Create a new PDF document
            PdfDocument pdfDocument = new PdfDocument();

            // Create a new HTML to PDF converter
            HtmlToPdf htmlToPdf = new HtmlToPdf();

            // Set CSS media type and page size
            htmlToPdf.Options.CssMediaType = HtmlToPdfCssMediaType.Print;
            htmlToPdf.Options.PdfPageSize = PdfPageSize.A4;

            // Enable header and footer
            htmlToPdf.Options.DisplayHeader = true;
            htmlToPdf.Options.DisplayFooter = true;

            // Set header height
            htmlToPdf.Header.Height = 300;

            // Set footer height
            htmlToPdf.Footer.Height = 50;

            // Add HTML content to header
            string headerPath = Path.Combine(currentDirectory, "Template", "Header.html");
            string headerhtmlContent = File.ReadAllText(headerPath);
            PdfHtmlSection headerHtml = new PdfHtmlSection(headerhtmlContent, "");
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            htmlToPdf.Header.Add(headerHtml);

            // Add page numbers to footer
            PdfHtmlSection footerHtml = new PdfHtmlSection("<div style='text-align: center;'>Page: {page_number} of {total_pages}</div>", "");
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            htmlToPdf.Footer.Add(footerHtml);

            // Convert HTML content to PDF
            PdfDocument pdfPage = htmlToPdf.ConvertHtmlString(htmlContent);

            // Add each page of the converted PDF to the main PDF document
            foreach (PdfPage page in pdfPage.Pages)
            {
                pdfDocument.AddPage(page);
            }

            // Save the PDF document
            pdfDocument.Save(pdfFilePath);

            //byte[] pdf = pdfDocument.Save();
            //File.WriteAllBytes(pdfFilePath, pdf);
            // Close the PDF document
            pdfDocument.Close();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while generating PDF: {ex.Message}");
        }
    }

}
