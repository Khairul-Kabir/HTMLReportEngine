using SelectPdf;

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

        HtmlToPdf htmlToPdf = new HtmlToPdf();
        SelectPdf.PdfDocument pdfDocument = htmlToPdf.ConvertHtmlString(htmlContent);
        byte[] pdf = pdfDocument.Save();

        File.WriteAllBytes(pdfFilePath, pdf);
    }
}
