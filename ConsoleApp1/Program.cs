using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

// Define a class to represent a page range
public class PageRange
{
    public string FilePath { get; set; }
    public IEnumerable<int> Pages { get; set; }
}

// Define options for the command line parser
public class Options
{
    [Option('o', "output", Required = true, HelpText = "Output PDF file.")]
    public string OutputFile { get; set; }

    [Option('i', "input", Required = true, HelpText = "Input PDF files and page ranges in the format 'file.pdf:1-3,5'.")]
    public IEnumerable<string> InputFiles { get; set; }
}

// Define an interface for collecting PDF merge information
public interface IPdfMerger
{
    ICollection<PageRange> CollectMergeInfo(string[] args);
}

// Implement the interface for command line usage
public class CommandLinePdfMerger : IPdfMerger
{
    public ICollection<PageRange> CollectMergeInfo(string[] args)
    {
        var pageRanges = new List<PageRange>();
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(opts =>
            {
                foreach (var inputFile in opts.InputFiles)
                {
                    var parts = inputFile.Split(':');
                    var filePath = parts[0];
                    var pages = ParsePageRanges(parts[1]);
                    pageRanges.Add(new PageRange { FilePath = filePath, Pages = pages });
                }
            });
        return pageRanges;
    }

    private IEnumerable<int> ParsePageRanges(string pageRanges)
    {
        var pages = new List<int>();
        var ranges = pageRanges.Split(',');

        foreach (var range in ranges)
        {
            if (range.Contains("-"))
            {
                var tokens = range.Split('-');
                int start = int.Parse(tokens[0]);
                int end = int.Parse(tokens[1]);

                for (int i = start; i <= end; i++)
                {
                    pages.Add(i);
                }
            }
            else
            {
                pages.Add(int.Parse(range));
            }
        }

        return pages;
    }
}

// A class that performs the actual PDF merging
public class PdfMerger
{
    public static void MergePdfs(ICollection<PageRange> pageRanges, string outputFilePath)
    {
        var mergedPdf = new PdfDocument();
        foreach (var pageRange in pageRanges)
        {
            if (!File.Exists(pageRange.FilePath))
            {
                Console.WriteLine($"File not found: {pageRange.FilePath}");
                continue;
            }

            var inputDocument = PdfReader.Open(pageRange.FilePath, PdfDocumentOpenMode.Import);
            foreach (int pageNumber in pageRange.Pages)
            {
                if (pageNumber > 0 && pageNumber <= inputDocument.PageCount)
                {
                    mergedPdf.AddPage(inputDocument.Pages[pageNumber - 1]);
                }
            }
        }

        mergedPdf.Save(outputFilePath);
        Console.WriteLine($"PDFs merged successfully into '{outputFilePath}'.");
    }
}

// Example usage with command line arguments
public class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(opts =>
            {
                IPdfMerger mergerInterface = new CommandLinePdfMerger();
                var pageRanges = mergerInterface.CollectMergeInfo(args);
                string outputFilePath = opts.OutputFile; // Get the output file path from the parsed options

                PdfMerger.MergePdfs(pageRanges, outputFilePath);
            })
            .WithNotParsed<Options>((errs) =>
            {
                // Handle errors or invalid arguments
                Console.WriteLine("Error parsing command line arguments.");
                foreach (var err in errs)
                {
                    Console.WriteLine(err.ToString());
                }
            });

        
    }
}
