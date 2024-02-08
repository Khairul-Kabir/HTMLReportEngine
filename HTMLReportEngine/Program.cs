using System;

var htmlFileReader = new HtmlFileReader();
string htmlContent = htmlFileReader.ReadHtmlFile("Template", "DedupeReportTemplate.html");

//if (htmlContent != null)
//{
//    Console.WriteLine("HTML file content:");
//    Console.WriteLine(htmlContent);
//}

htmlFileReader.GeneratePDFV2("Output", "Report", htmlContent);

