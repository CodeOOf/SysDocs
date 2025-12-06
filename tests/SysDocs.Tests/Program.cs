using SysDocs.Tests.Tools;

namespace SysDocs.Tests;

/// <summary>
/// Test project entry point for running traceability reports and license compliance checks
/// Usage: dotnet run --project tests/SysDocs.Tests -- [command]
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Contains("--generate-traceability-report") || args.Contains("--traceability"))
        {
            Console.WriteLine("Generating Traceability Report...");
            Console.WriteLine();
            
            var report = TraceabilityReportGenerator.GenerateReport();
            
            // Write to console
            Console.WriteLine(report);
            
            // Write to file
            var repoRoot = FindRepositoryRoot();
            var outputPath = Path.Combine(repoRoot, "reports", "TEST_TRACEABILITY.md");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
            File.WriteAllText(outputPath, report);
            
            Console.WriteLine();
            Console.WriteLine($"Report written to: {outputPath}");
        }
        else if (args.Contains("--license-compliance") || args.Contains("--licenses"))
        {
            await LicenseComplianceChecker.GenerateLicenseReportAsync();
        }
        else
        {
            Console.WriteLine("SysDocs Test Project");
            Console.WriteLine();
            Console.WriteLine("Available commands:");
            Console.WriteLine("  --traceability          Generate requirement-to-test traceability report");
            Console.WriteLine("  --license-compliance    Verify all dependencies use open-source licenses");
            Console.WriteLine();
            Console.WriteLine("Or run tests normally:");
            Console.WriteLine("  dotnet test");
        }
    }

    private static string FindRepositoryRoot()
    {
        var dir = Directory.GetCurrentDirectory();
        while (dir != null && !File.Exists(Path.Combine(dir, "LICENSE")))
        {
            dir = Directory.GetParent(dir)?.FullName;
        }
        return dir ?? throw new InvalidOperationException("Cannot find repository root (LICENSE file not found)");
    }
}
