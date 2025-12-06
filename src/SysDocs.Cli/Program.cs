using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SysDocs.Cli;

/// <summary>
/// Entry point for the SysDocs CLI application.
/// </summary>
internal class Program
{
    public static async Task<int> Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        
        var rootCommand = new RootCommand("SysDocs - Deterministic Systems Engineering Documentation Pipeline");
        
        var inputOption = new Option<FileInfo>(
            aliases: new[] { "--input", "-i" },
            description: "Input document file (Markdown, Word, LaTeX, or PDF)")
        {
            IsRequired = true
        };
        
        var outputOption = new Option<FileInfo>(
            aliases: new[] { "--output", "-o" },
            description: "Output PDF file path")
        {
            IsRequired = true
        };
        
        var templateOption = new Option<string>(
            aliases: new[] { "--template", "-t" },
            description: "Template name (default, incose, vmodel)",
            getDefaultValue: () => "default");
        
        var verboseOption = new Option<bool>(
            aliases: new[] { "--verbose", "-v" },
            description: "Enable verbose logging");
        
        rootCommand.AddOption(inputOption);
        rootCommand.AddOption(outputOption);
        rootCommand.AddOption(templateOption);
        rootCommand.AddOption(verboseOption);
        
        rootCommand.SetHandler(async (input, output, template, verbose) =>
        {
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            
            logger.LogInformation("SysDocs v0.1.0-alpha");
            logger.LogInformation("Input: {Input}", input.FullName);
            logger.LogInformation("Output: {Output}", output.FullName);
            logger.LogInformation("Template: {Template}", template);
            
            // TODO: Implement document processing pipeline
            logger.LogWarning("Document processing not yet implemented - placeholder only");
            
            await Task.CompletedTask;
            
        }, inputOption, outputOption, templateOption, verboseOption);
        
        return await rootCommand.InvokeAsync(args);
    }
    
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register services here
                // services.AddSingleton<IDocumentProcessor, DocumentProcessor>();
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });
}
