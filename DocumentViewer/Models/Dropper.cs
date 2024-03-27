using System.IO.Compression;

namespace DocumentViewer.Models
{

    public class DocumentDropper
    {
        private static IConfigurationRoot ConfReader() => new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

        public static string SetDirectory(ModuleDirectory directory, ClosingModuleAction action, bool isProduction)
        {
            if (isProduction)
            {
                switch (action)
                {
                    case ClosingModuleAction.Root:
                        return ConfReader().GetSection("DocumentPlacement:Production:Closing:Root").Value!;
                    case ClosingModuleAction.Document:
                        return ConfReader().GetSection("DocumentPlacement:Production:Closing:Document").Value!;
                    case ClosingModuleAction.Payment:
                        return ConfReader().GetSection("DocumentPlacement:Production:Closing:Payment").Value!;
                    default:
                        return ConfReader().GetSection("DocumentPlacement:Production:Root").Value!;
                }
            }
            else
            {
                switch (action)
                {
                    case ClosingModuleAction.Root:
                        return ConfReader().GetSection("DocumentPlacement:Development:Closing:Root").Value!;
                    case ClosingModuleAction.Document:
                        return ConfReader().GetSection("DocumentPlacement:Development:Closing:Document").Value!;
                    case ClosingModuleAction.Payment:
                        return ConfReader().GetSection("DocumentPlacement:Development:Closing:Payment").Value!;
                    default:
                        return ConfReader().GetSection("DocumentPlacement:Development:Root").Value!;
                }
            }

        }

        public static string SetDirectory(ModuleDirectory directory, ClaimModuleAction action, bool isProduction)
        {
            if (isProduction)
            {
                switch (action)
                {
                    case ClaimModuleAction.Root:
                        return ConfReader().GetSection("DocumentPlacement:Production:Claim:Root").Value!;
                    case ClaimModuleAction.Document:
                        return ConfReader().GetSection("DocumentPlacement:Production:Claim:Document").Value!;
                    case ClaimModuleAction.Payment:
                        return ConfReader().GetSection("DocumentPlacement:Production:Claim:Payment").Value!;
                    case ClaimModuleAction.Reserve:
                        return ConfReader().GetSection("DocumentPlacement:Production:Claim:Reserve").Value!;
                    case ClaimModuleAction.Dla:
                        return ConfReader().GetSection("DocumentPlacement:Production:Claim:Dla").Value!;
                    case ClaimModuleAction.Subrogasi:
                        return ConfReader().GetSection("DocumentPlacement:Production:Claim:Subrogasi").Value!;
                    default:
                        return ConfReader().GetSection("DocumentPlacement:Production:Root").Value!;
                }
            }
            else
            {
                switch (action)
                {
                    case ClaimModuleAction.Root:
                        return ConfReader().GetSection("DocumentPlacement:Development:Claim:Root").Value!;
                    case ClaimModuleAction.Document:
                        return ConfReader().GetSection("DocumentPlacement:Development:Claim:Document").Value!;
                    case ClaimModuleAction.Payment:
                        return ConfReader().GetSection("DocumentPlacement:Development:Claim:Payment").Value!;
                    case ClaimModuleAction.Reserve:
                        return ConfReader().GetSection("DocumentPlacement:Development:Claim:Reserve").Value!;
                    case ClaimModuleAction.Dla:
                        return ConfReader().GetSection("DocumentPlacement:Development:Claim:Dla").Value!;
                    case ClaimModuleAction.Subrogasi:
                        return ConfReader().GetSection("DocumentPlacement:Development:Claim:Subrogasi").Value!;
                    default:
                        return ConfReader().GetSection("DocumentPlacement:Development:Root").Value!;
                }
            }

        }

        public static string SetDirectory(ModuleDirectory directory, RefundModuleAction action, bool isProduction)
        {
            if (isProduction)
            {
                switch (action)
                {
                    case RefundModuleAction.Root:
                        return ConfReader().GetSection("DocumentPlacement:Production:Refund:Root").Value!;
                    case RefundModuleAction.Document:
                        return ConfReader().GetSection("DocumentPlacement:Production:Refund:Document").Value!;
                    case RefundModuleAction.Payment:
                        return ConfReader().GetSection("DocumentPlacement:Production:Refund:Payment").Value!;
                    default:
                        return ConfReader().GetSection("DocumentPlacement:Production:Root").Value!;
                }
            }
            else
            {
                switch (action)
                {
                    case RefundModuleAction.Root:
                        return ConfReader().GetSection("DocumentPlacement:Development:Refund:Root").Value!;
                    case RefundModuleAction.Document:
                        return ConfReader().GetSection("DocumentPlacement:Development:Refund:Document").Value!;
                    case RefundModuleAction.Payment:
                        return ConfReader().GetSection("DocumentPlacement:Development:Refund:Payment").Value!;
                    default:
                        return ConfReader().GetSection("DocumentPlacement:Development:Root").Value!;
                }
            }
        }

        public static string? FindFileRecursively(string folderPath, string targetFileName)
        {
            foreach (string file in Directory.GetFiles(folderPath, targetFileName))
            {
                return file; // Return the first match found
            }

            foreach (string subfolder in Directory.GetDirectories(folderPath))
            {
                string targetFilePath = FindFileRecursively(subfolder, targetFileName);
                if (!string.IsNullOrEmpty(targetFilePath))
                {
                    return targetFilePath; // Return the match from the subfolder
                }
            }

            return null; // File not found
        }
    }

    public class DocumentPicker
    {
        public static ExtractAndFindExcelFileCashback ExtractAndFindExcelFile(string compressedFolder, bool isDevelopment, string extractionPath)
        {
            Directory.CreateDirectory(extractionPath);
            ExtractAndFindExcelFileCashback cashback = new ExtractAndFindExcelFileCashback();
            List<string> excel = new List<string>();
            List<string> pdf = new List<string>();
            List<string> other = new List<string>();
            using (ZipArchive archive = ZipFile.OpenRead(compressedFolder))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.Combine(extractionPath, entry.Name);

                    if (entry.FullName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        // This entry is an Excel file, you can process it as needed
                        Console.WriteLine($"Found xlsx file: {entry.FullName}");
                        entry.ExtractToFile(destinationPath, true);
                        excel.Add(destinationPath);
                    }
                    else if (entry.FullName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                    {
                        // Process PDF files or skip them
                        Console.WriteLine($"Found xls file: {entry.FullName}");
                        entry.ExtractToFile(destinationPath, true);
                        excel.Add(destinationPath);
                    }
                    else if (entry.FullName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        // Process PDF files or skip them
                        Console.WriteLine($"Found pdf file: {entry.FullName}");
                        entry.ExtractToFile(destinationPath, true);
                        pdf.Add(destinationPath);
                    }
                    else
                    {
                        Console.WriteLine($"Found other file: {entry.FullName}");
                        // Skip other files
                        entry.ExtractToFile(destinationPath, true);
                        other.Add(destinationPath);
                    }
                }
            }
            cashback.Excel = excel;
            cashback.Pdf = pdf;
            cashback.Other = other;
            return cashback;
        }
    }

    public class ExtractAndFindExcelFileCashback
    {
        public List<string>? Excel { get; set; }
        public List<string>? Pdf { get; set; }
        public List<string>? Other { get; set; }
    }

    public enum ModuleDirectory
    {
        Closing,
        Claim,
        Refund,
        Root
    }

    public enum ClaimModuleAction
    {
        Root,
        Payment,
        Document,
        Reserve,
        Dla,
        Subrogasi,
    }

    public enum ClosingModuleAction
    {
        Root,
        Payment,
        Document
    }

    public enum RefundModuleAction
    {
        Root,
        Payment,
        Document
    }

}
