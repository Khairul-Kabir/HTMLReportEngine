using HTMLReportEngine;
using System;

//var htmlFileReader = new HtmlFileReader();
//string htmlContent = htmlFileReader.ReadHtmlFile("Template", "DedupeReportTemplate.html");

//htmlFileReader.GeneratePDFV2("Output", "Report", htmlContent);


PDFMaker fileReader = new PDFMaker();

// Set folder name and base file name
string folderName = "PDFs";
string baseFileName = "WorldCheck";

// Path to JSON file
string currentDirectory = Directory.GetCurrentDirectory();
while (!Directory.GetFiles(currentDirectory, "*.csproj").Any())
{
    currentDirectory = Directory.GetParent(currentDirectory).FullName;
}
string jsonFilePath = Path.Combine(currentDirectory, "Template", "DummyData", "DedupeResponse.json");

// Generate PDF files in batches
fileReader.GeneratePDFBatch(folderName, baseFileName, jsonFilePath);


