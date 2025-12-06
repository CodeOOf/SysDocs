// SysML v2 Sequence Diagram Example - System Behavior
// Maps to System Architecture documentation (FR-23)

package SystemBehavior {
    
    import SystemArchitecture::*;
    import ScalarValues::*;
    
    // Document generation workflow
    interaction DocumentGenerationWorkflow {
        doc /*
        **Document Generation Workflow**
        
        Main sequence showing the flow from document import through
        processing to final PDF generation.
        */
        
        // Participants (lifelines)
        participant user : User;
        participant api : RestApiPort;
        participant importSys : ImportSubsystem;
        participant processSys : ProcessingSubsystem;
        participant exportSys : ExportSubsystem;
        participant storage : StorageSubsystem;
        
        // Message sequence
        message start from user to api {
            doc /* User initiates document generation request */
        }
        
        message submitDocument from api to importSys {
            doc /* API submits document for import */
            
            attribute payload : DocumentPayload;
            attribute format : String;
        }
        
        message importComplete from importSys to processSys {
            doc /* Import subsystem passes internal model */
            
            attribute model : InternalDocumentModel;
        }
        
        message loadTemplate from processSys to storage {
            doc /* Processing requests template from storage */
            
            attribute templateId : String;
        }
        
        message templateData from storage to processSys {
            doc /* Storage returns template */
            
            attribute template : DocumentTemplate;
        }
        
        message processComplete from processSys to exportSys {
            doc /* Processing complete, ready for export */
            
            attribute processedModel : ProcessedDocument;
        }
        
        message generatePDF from exportSys to exportSys {
            doc /* Export subsystem generates PDF */
        }
        
        message savePDF from exportSys to storage {
            doc /* Save generated PDF to storage */
            
            attribute pdf : PdfDocument;
        }
        
        message confirmSave from storage to exportSys {
            doc /* Storage confirms successful save */
            
            attribute documentId : String;
        }
        
        message deliverResult from api to user {
            doc /* API returns result to user */
            
            attribute status : String = "Success";
            attribute downloadUrl : String;
        }
        
        // Timing constraints
        constraint responseTime {
            doc /* Total workflow must complete within 10 seconds */
            
            first start;
            then deliverResult;
            
            attribute maxDuration : Real = 10.0; // seconds
        }
    }
    
    // Error handling sequence
    interaction ErrorHandlingWorkflow {
        doc /*
        **Error Handling Workflow**
        
        Demonstrates exception handling when import fails.
        */
        
        participant user : User;
        participant api : RestApiPort;
        participant importSys : ImportSubsystem;
        
        message submitInvalidDocument from user to api;
        
        message attemptImport from api to importSys;
        
        message importError from importSys to api {
            doc /* Import fails with error */
            
            attribute errorCode : String = "UNSUPPORTED_FORMAT";
            attribute errorMessage : String;
        }
        
        message returnError from api to user {
            attribute status : String = "Error";
            attribute error : ErrorDetails;
        }
    }
    
    // Concurrent processing sequence
    interaction ParallelProcessing {
        doc /*
        **Parallel Processing Workflow**
        
        Shows concurrent processing of multiple documents.
        */
        
        participant scheduler : ProcessScheduler;
        participant worker1 : ProcessingWorker;
        participant worker2 : ProcessingWorker;
        participant worker3 : ProcessingWorker;
        
        message distributeWork from scheduler to worker1;
        message distributeWork from scheduler to worker2;
        message distributeWork from scheduler to worker3;
        
        // Parallel execution
        par {
            message processDoc1 from worker1 to worker1;
            message processDoc2 from worker2 to worker2;
            message processDoc3 from worker3 to worker3;
        }
        
        message reportComplete from worker1 to scheduler;
        message reportComplete from worker2 to scheduler;
        message reportComplete from worker3 to scheduler;
        
        message aggregateResults from scheduler to scheduler;
    }
    
    // Supporting types
    part def User;
    part def ProcessScheduler;
    part def ProcessingWorker;
    
    attribute def DocumentPayload;
    attribute def InternalDocumentModel;
    attribute def DocumentTemplate;
    attribute def ProcessedDocument;
    attribute def PdfDocument;
    attribute def ErrorDetails;
}
