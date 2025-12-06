// SysML v2 Block Diagram Example - System Architecture
// Maps to System Architecture documentation (FR-22)

package SystemArchitecture {
    
    private import ScalarValues::*;
    
    // Main system block
    part def DocumentationSystem {
        doc /*
        **Documentation System**
        
        Top-level system for automated document generation and management.
        Composed of multiple subsystems handling import, processing, and export.
        */
        
        // Subsystem blocks
        part importSubsystem : ImportSubsystem;
        part processingSubsystem : ProcessingSubsystem;
        part exportSubsystem : ExportSubsystem;
        part storageSubsystem : StorageSubsystem;
        
        // External interfaces
        port inputPort : DocumentInputPort;
        port outputPort : DocumentOutputPort;
        port apiPort : RestApiPort;
        
        // Internal connections
        connect importSubsystem.outputPort to processingSubsystem.inputPort;
        connect processingSubsystem.outputPort to exportSubsystem.inputPort;
        connect processingSubsystem.storagePort to storageSubsystem.accessPort;
    }
    
    // Import subsystem
    part def ImportSubsystem {
        doc /*
        **Import Subsystem**
        
        Handles import of documents from various formats.
        Supports Markdown, Word, LaTeX, PDF, and SysML v2.
        */
        
        part markdownImporter : MarkdownImporter;
        part wordImporter : WordImporter;
        part pdfImporter : PdfImporter;
        part sysmlImporter : SysMLImporter;
        
        port inputPort : DocumentInputPort;
        port outputPort : InternalModelPort;
        
        // Properties
        attribute supportedFormats : String[*] = ("md", "docx", "pdf", "sysml");
        attribute maxFileSize : Real = 100.0; // MB
    }
    
    // Processing subsystem
    part def ProcessingSubsystem {
        doc /*
        **Processing Subsystem**
        
        Core processing engine that transforms imported documents
        to internal model and applies templates.
        */
        
        part modelTransformer : ModelTransformer;
        part templateEngine : TemplateEngine;
        part manifestProcessor : ManifestProcessor;
        
        port inputPort : InternalModelPort;
        port outputPort : ProcessedModelPort;
        port storagePort : StorageAccessPort;
        
        // Processing constraints
        constraint deterministicProcessing {
            doc /* All processing must be deterministic */
        }
    }
    
    // Export subsystem
    part def ExportSubsystem {
        doc /*
        **Export Subsystem**
        
        Generates final PDF output with templates and branding.
        */
        
        part pdfGenerator : PdfGenerator;
        part watermarkApplicator : WatermarkApplicator;
        
        port inputPort : ProcessedModelPort;
        port outputPort : DocumentOutputPort;
        
        attribute outputFormat : String = "PDF";
    }
    
    // Storage subsystem
    part def StorageSubsystem {
        doc /*
        **Storage Subsystem**
        
        Manages persistent storage of documents, templates, and metadata.
        */
        
        part documentStore : DocumentStore;
        part templateStore : TemplateStore;
        part metadataStore : MetadataStore;
        
        port accessPort : StorageAccessPort;
    }
    
    // Port definitions
    port def DocumentInputPort {
        doc /* Accepts incoming documents in various formats */
    }
    
    port def DocumentOutputPort {
        doc /* Provides generated PDF documents */
    }
    
    port def RestApiPort {
        doc /* REST API for external system integration */
    }
    
    port def InternalModelPort {
        doc /* Internal document model representation */
    }
    
    port def ProcessedModelPort {
        doc /* Processed document ready for export */
    }
    
    port def StorageAccessPort {
        doc /* Access to persistent storage */
    }
    
    // Component definitions
    part def MarkdownImporter;
    part def WordImporter;
    part def PdfImporter;
    part def SysMLImporter;
    part def ModelTransformer;
    part def TemplateEngine;
    part def ManifestProcessor;
    part def PdfGenerator;
    part def WatermarkApplicator;
    part def DocumentStore;
    part def TemplateStore;
    part def MetadataStore;
}
