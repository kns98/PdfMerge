using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class PdfMergerTest
    {
        static void TestMergePdfs()
        {
            // Simulate command line arguments: output file and input files with page ranges
            string[] testArgs = new string[]
            {
            "-o", "outputTest.pdf", // Output file
            "-i", "input1.pdf:1-2", "input2.pdf:1,3-4" // Input files and their page ranges
            };

            // Set up a directory for test files (adjust the path as needed)
            string testDirectory = Path.Combine(Environment.CurrentDirectory, "TestFiles");
            Directory.CreateDirectory(testDirectory);

            // Update the paths in testArgs to point to the actual test PDF files
            for (int i = 2; i < testArgs.Length; i += 2)
            {
                testArgs[i] = Path.Combine(testDirectory, testArgs[i]);
            }

            // Call the Main method with the test arguments
            Program.Main(testArgs);

            // Verify the output
            string outputPath = Path.Combine(testDirectory, "outputTest.pdf");
            if (File.Exists(outputPath))
            {
                Console.WriteLine("Test Passed: Output PDF file created successfully.");
            }
            else
            {
                Console.WriteLine("Test Failed: Output PDF file was not created.");
            }

            // Additional verifications can be added here, such as checking the page count of the output PDF
        }
    }

}
