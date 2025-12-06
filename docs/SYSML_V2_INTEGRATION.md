# SysML v2 Integration

**Requirements Coverage**: FR-20, FR-21, FR-22, FR-23, FR-24, FR-25  
**Status**: ⏳ Specified - Pending Implementation  
**Test Coverage**: 16 test cases defined (all skipped pending implementation)

## Overview

SysDocs supports importing SysML v2 (Systems Modeling Language version 2) models and mapping them to Systems Engineering documentation artifacts aligned with the V-Model and INCOSE SE Handbook.

### Supported SysML v2 Elements

| SysML v2 Element | Maps To | Requirements |
|------------------|---------|--------------|
| **Requirements** | Stakeholder Requirements, System Requirements | FR-20 |
| **Use Cases** | Stakeholder Requirements, System Requirements | FR-21 |
| **Block Diagrams** | System Architecture, Detailed Design | FR-22 |
| **Sequence Diagrams** | System Architecture (behavioral), Detailed Design (interactions) | FR-23 |
| **Traceability Links** | Preserved across all document types | FR-24 |

### File Format Support

SysDocs supports three SysML v2 file formats (FR-25):

1. **`.sysml`** - SysML v2 textual syntax (primary format)
2. **`.kerml`** - Kernel Modeling Language files
3. **`.json`** - SysML v2 API JSON format

All three formats are parsed to the same internal representation, ensuring equivalent models produce identical output regardless of source format.

## V-Model Mapping

### Left Side - Requirements & Design

```
┌─────────────────────────────┐
│  Stakeholder Requirements   │ ← SysML Requirements (stakeholder-level)
│  (FR-20, FR-21)             │ ← SysML Use Cases (stakeholder scenarios)
└─────────────────────────────┘
              ↓
┌─────────────────────────────┐
│  System Requirements        │ ← SysML Requirements (system-level)
│  (FR-20, FR-21)             │ ← SysML Use Cases (system scenarios)
└─────────────────────────────┘
              ↓
┌─────────────────────────────┐
│  System Architecture        │ ← SysML Block Diagrams (structure)
│  (FR-22, FR-23)             │ ← SysML Sequence Diagrams (behavior)
└─────────────────────────────┘
              ↓
┌─────────────────────────────┐
│  Detailed Design            │ ← SysML Block Diagrams (internal design)
│  (FR-22, FR-23)             │ ← SysML Sequence Diagrams (interactions)
└─────────────────────────────┘
```

### Traceability (FR-24)

All SysML v2 traceability relationships are preserved:

- **satisfy** - Design element satisfies requirement
- **refine** - Requirement refines higher-level requirement
- **derive** - Requirement derived from another
- **verify** - Test verifies requirement
- **realize** - Component realizes design element

## Requirements Details

### FR-20: Import SysML v2 Requirements

**Capability**: Import SysML v2 requirement definitions and map to documentation

**Supported Attributes**:
- Requirement ID
- Requirement text/description
- Priority
- Source/rationale
- Verification method (for system requirements)
- Verification criteria
- Traceability links (satisfy, refine, derive)

**Document Mapping**:
- **Stakeholder Requirements**: High-level needs, business objectives
- **System Requirements**: Derived functional/non-functional requirements with verification approach

**Example**:
```sysml
requirement STK_REQ_001 : StakeholderNeed {
    doc /* System shall be highly available */
    attribute priority = "High";
    attribute source = "COO";
}

requirement SYS_REQ_001 : SystemRequirement {
    doc /* System shall maintain 99.9% uptime */
    attribute verificationMethod = "Test";
    satisfy STK_REQ_001;
}
```

### FR-21: Import SysML v2 Use Cases

**Capability**: Import SysML v2 use case definitions and map to documentation

**Supported Attributes**:
- Use case name
- Actors (primary and secondary)
- Preconditions
- Postconditions
- Main success scenario
- Alternative flows
- Exception handling
- System boundary (for system use cases)
- System functions referenced

**Document Mapping**:
- **Stakeholder Requirements**: User-facing scenarios, business workflows
- **System Requirements**: System-level use cases with technical details

**Example**:
```sysml
usecase GenerateDocument {
    doc /* User generates a PDF document from sources */
    
    actor :> User;
    subject :> DocumentationSystem;
    
    attribute preconditions = "User authenticated, sources available";
    attribute postconditions = "PDF generated and available for download";
}
```

### FR-22: Import SysML v2 Block Diagrams

**Capability**: Import SysML v2 block structure and relationships

**Supported Elements**:
- Block definitions (parts)
- Ports and interfaces
- Properties and attributes
- Composition relationships
- Association relationships
- Generalization hierarchies
- Internal parts
- Constraints
- Operations

**Document Mapping**:
- **System Architecture**: High-level system decomposition, subsystems, interfaces
- **Detailed Design**: Component internal structure, detailed interfaces, operations

**Output Format**:
- Embedded diagram images (SVG or PNG)
- Textual descriptions of blocks and relationships
- Port connection tables
- Interface specifications

**Example**:
```sysml
part def DocumentationSystem {
    part importSubsystem : ImportSubsystem;
    part processingSubsystem : ProcessingSubsystem;
    
    port inputPort : DocumentInputPort;
    port outputPort : DocumentOutputPort;
    
    connect importSubsystem.output to processingSubsystem.input;
}
```

### FR-23: Import SysML v2 Sequence Diagrams

**Capability**: Import SysML v2 interaction specifications

**Supported Elements**:
- Interactions (sequence scenarios)
- Lifelines (participating elements)
- Messages (synchronous, asynchronous, return)
- Message signatures and parameters
- Sequence numbers
- Timing constraints
- Preconditions and postconditions
- Exception flows
- Parallel fragments (par)
- Alternative fragments (alt)
- Loop fragments (loop)

**Document Mapping**:
- **System Architecture**: System-level behavior, subsystem interactions
- **Detailed Design**: Detailed message flows, timing specifications, exception handling

**Output Format**:
- Embedded sequence diagram images
- Message sequence tables
- Timing constraint specifications
- Exception handling documentation

**Example**:
```sysml
interaction DocumentGenerationWorkflow {
    participant user : User;
    participant api : RestAPI;
    participant importer : ImportSubsystem;
    
    message submitDocument from user to api;
    message processDocument from api to importer;
    message importComplete from importer to api;
    
    constraint responseTime {
        maxDuration = 10.0; // seconds
    }
}
```

### FR-24: SysML v2 Traceability Preservation

**Capability**: Preserve all traceability relationships from SysML v2 models

**Preserved Relationships**:
- **satisfy**: Design satisfies requirement
- **refine**: Lower-level requirement refines higher-level
- **derive**: Requirement derived from another
- **verify**: Test/verification verifies requirement
- **realize**: Implementation realizes design

**Traceability Output**:
- Bidirectional links in documents (forward and backward traces)
- Traceability matrices
- Impact analysis support
- Change tracking foundation

**Example**:
```sysml
requirement SYS_REQ_001 {
    satisfy STK_REQ_001;  // Traces to stakeholder requirement
}

part ImportSubsystem {
    satisfy SYS_REQ_001;  // Traces to system requirement
}
```

### FR-25: SysML v2 File Format Support

**Capability**: Support multiple SysML v2 file formats

**Format Details**:

1. **`.sysml` (SysML v2 Textual Syntax)**
   - Human-readable textual representation
   - Primary authoring format
   - Direct KerML + SysML v2 syntax

2. **`.kerml` (Kernel Modeling Language)**
   - Foundation layer of SysML v2
   - Core metamodel elements
   - Can be extended with SysML v2 constructs

3. **`.json` (SysML v2 API Format)**
   - JSON representation following SysML v2 API specification
   - Machine-generated and processed
   - Interoperability with SysML v2 tools via REST API

**Determinism Guarantee**:
Equivalent models in different formats produce byte-for-byte identical PDF output (NFR-01).

## Implementation Architecture

### Importer Structure

```
src/SysDocs.Core/Importers/SysML/
├── SysMLImporter.cs              # Main importer interface implementation
├── Parsers/
│   ├── SysMLTextParser.cs       # .sysml parser
│   ├── KerMLParser.cs           # .kerml parser
│   └── SysMLJsonParser.cs       # .json parser
├── Model/
│   ├── SysMLRequirement.cs      # Requirement representation
│   ├── SysMLUseCase.cs          # Use case representation
│   ├── SysMLBlock.cs            # Block definition
│   ├── SysMLSequence.cs         # Sequence diagram
│   └── TraceLink.cs             # Traceability relationship
├── Transformers/
│   ├── RequirementTransformer.cs   # SysML → Internal model
│   ├── UseCaseTransformer.cs
│   ├── BlockTransformer.cs
│   └── SequenceTransformer.cs
└── Renderers/
    ├── DiagramRenderer.cs       # Generate diagram images
    └── TraceabilityRenderer.cs  # Generate trace matrices
```

### Processing Pipeline

```
┌──────────────┐
│ .sysml       │
│ .kerml       │──→ Parse ──→ SysML Model ──→ Transform ──→ Internal Model ──→ PDF Export
│ .json        │
└──────────────┘
      ↓
  [Diagrams] ──→ Render ──→ SVG/PNG ──→ Embed in PDF
      ↓
[Traceability] ──→ Extract ──→ Trace Matrix ──→ Include in Output
```

## Test Coverage

### Test Structure

```
tests/SysDocs.Tests/Integration/SysML/
├── FR20_FR21_SysMLRequirementsUseCasesTests.cs
│   ├── TC-01: Import SysML v2 Requirements as Stakeholder Requirements
│   ├── TC-02: Import SysML v2 Requirements as System Requirements
│   ├── TC-03: Import SysML v2 Use Cases as Stakeholder Requirements
│   ├── TC-04: Import SysML v2 Use Cases as System Requirements
│   ├── TC-05: Preserve SysML v2 Traceability Links
│   ├── TC-06: Support Multiple SysML v2 File Formats
│   └── TC-07: SysML v2 Import Produces Deterministic Output
│
└── FR22_FR23_SysMLDiagramsTests.cs
    ├── TC-01: Import SysML v2 Block Diagrams as System Architecture
    ├── TC-02: Import SysML v2 Block Diagrams as Detailed Design
    ├── TC-03: Extract Block Relationships and Connections
    ├── TC-04: Import SysML v2 Sequence Diagrams as System Architecture
    ├── TC-05: Import SysML v2 Sequence Diagrams as Detailed Design
    ├── TC-06: Extract Message Sequences and Timing
    ├── TC-07: Preserve Diagram-to-Requirement Traceability
    ├── TC-08: Diagram Import Produces Deterministic Output
    └── TC-09: Convert SysML v2 Diagrams to Embedded Images
```

**Total Test Cases**: 16 (all currently skipped pending implementation)

### Test Fixtures

```
examples/sysml-v2-project/
├── requirements/
│   ├── stakeholder_requirements.sysml
│   ├── system_requirements.sysml
│   └── requirements_with_traces.sysml
├── use-cases/
│   ├── stakeholder_use_cases.sysml
│   └── system_use_cases.sysml
├── architecture/
│   ├── system_architecture_blocks.sysml
│   ├── system_behavior_sequences.sysml
│   └── traced_architecture.sysml
├── design/
│   ├── detailed_design_blocks.sysml
│   └── interaction_specifications.sysml
└── format-variants/
    ├── requirements.sysml
    ├── requirements.kerml
    └── requirements.json
```

## Integration with Existing Features

### Determinism (NFR-01, FR-06)

SysML v2 import must produce deterministic output:
- Diagram rendering uses fixed coordinates
- Element ordering is stable
- Timestamps normalized
- UUIDs replaced with deterministic IDs

### Manifest-Based Assembly (FR-16-19)

SysML v2 files can be referenced in manifests:

```json
{
  "documents": [
    {
      "output": "System_Requirements.pdf",
      "sources": [
        {
          "file": "requirements/system_requirements.sysml",
          "type": "sysml-requirements"
        }
      ]
    },
    {
      "output": "System_Architecture.pdf",
      "sources": [
        {
          "file": "architecture/system_architecture_blocks.sysml",
          "type": "sysml-blocks"
        },
        {
          "file": "architecture/system_behavior_sequences.sysml",
          "type": "sysml-sequences"
        }
      ]
    }
  ]
}
```

### Templates (FR-05)

SysML v2 content can be styled using templates:
- Requirement formatting
- Use case layout
- Diagram placement and sizing
- Traceability matrix styling

## Standard Compliance

### SysML v2 Specification

Implementation follows the [OMG SysML v2 specification](https://www.omgsysml.org/):
- SysML v2 Language Specification
- KerML (Kernel Modeling Language) Specification
- SysML v2 API & Services Specification

### Systems Engineering Standards

Aligns with:
- **INCOSE SE Handbook**: Document structure and artifact types
- **V-Model**: Requirements → Design → Verification mapping
- **ISO/IEC/IEEE 15288**: Systems and software engineering processes

## Implementation Priority

Per `.github/RELEASE_CHECKLIST.md`:

- **v1.0.0-alpha**: Not included (focus on core importers/exporters)
- **v1.0.0-beta**: SysML v2 import implementation
  - Requirements and use cases (FR-20, FR-21)
  - Basic block diagram import (FR-22)
  - Traceability preservation (FR-24)
  - File format support (FR-25)
- **v1.0.0 production**: 
  - Sequence diagram import (FR-23)
  - Complete diagram rendering
  - Full traceability features
  - Performance optimization

## Dependencies

### Required Libraries

1. **SysML v2 Parser**
   - Options: ANTLR-based parser, official SysML v2 parser
   - Must support .sysml, .kerml, .json formats

2. **Diagram Rendering**
   - GraphViz or PlantUML for diagram generation
   - SVG/PNG output for embedding

3. **Traceability Engine**
   - Graph-based relationship tracking
   - Bidirectional link resolution

### Integration Points

- `IDocumentImporter` interface
- Internal document model extensions
- Template system integration
- PDF embedding capabilities

## Related Documentation

- **[REQUIREMENTS.md](../project/REQUIREMENTS.md)** - FR-20 to FR-25 detailed specs
- **[REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md)** - SysML v2 V&V status
- **[TRACEABILITY.md](../project/TRACEABILITY.md)** - Implementation mapping
- **[.github/RELEASE_CHECKLIST.md](../.github/RELEASE_CHECKLIST.md)** - Implementation roadmap
- **[examples/sysml-v2-project/](../examples/sysml-v2-project/)** - Test fixtures and examples

## References

- [OMG SysML v2](https://www.omgsysml.org/)
- [SysML v2 Release on GitHub](https://github.com/Systems-Modeling/SysML-v2-Release)
- [INCOSE SE Handbook](https://www.incose.org/products-publications/se-handbook)
- [ISO/IEC/IEEE 15288:2015](https://www.iso.org/standard/63711.html)
