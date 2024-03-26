using System;
using System.IO;

public class PdfMergerTest
{
    static void Test()
    {
        // Single string simulating the command line input
        string commandLine = "-o outputTest.pdf -i pdf1.pdf:1-2 pdf2.pdf:1,3-4";

        // Split the string into an array of arguments
        // Note: This simple split method works here because there are no spaces in file names or paths.
        // For more complex scenarios, consider a method that correctly handles quoted arguments.
        string[] testArgs = commandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


        // Call the Main method with the simulated arguments
        Program.Main(testArgs);

        // Check if the output PDF has been created to verify the test result
        string outputPath = "outputTest.pdf";
        if (File.Exists(outputPath))
        {
            Console.WriteLine("Test Passed: Output PDF file created successfully.");
        }
        else
        {
            Console.WriteLine("Test Failed: Output PDF file was not created.");
        }

        // Here you could add additional checks, such as verifying the content or page count of the output PDF
    }
}
