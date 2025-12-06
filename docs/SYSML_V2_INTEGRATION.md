# SysML v2 Integration

**Requirements Coverage**: FR-20, FR-21, FR-22, FR-23, FR-24, FR-25  
**Status**: ⏳ Specified - Pending Implementation  
**Test Coverage**: 16 test cases defined (all skipped pending implementation)

## Overview

SysDocs supports importing SysML v2 (Systems Modeling Language version 2) models and rendering them in Systems Engineering documentation aligned with the V-Model and INCOSE SE Handbook.

### Core Capabilities

| Capability | Description | Requirements |
|------------|-------------|--------------|
| **Model Import** | Import complete SysML v2 models with traceability | FR-20 |
| **Diagram Support** | 9 SysML v2 diagram types (Requirements, Use Case, BDD, IBD, Parametric, Activity, Sequence, State Machine, Package) | FR-21 |
| **File Formats** | Support .sysml, .kerml, .json, .sysmlv2 formats | FR-22 |
| **Diagram Rendering** | Render diagrams as embedded SVG/PNG/PDF images | FR-23 |
| **Property Extraction** | Extract and present model element properties | FR-24 |
| **Deterministic Output** | Byte-for-byte identical output across formats | FR-25 |

### Supported Diagram Types (FR-21)

1. **Requirements Diagram** - Requirements hierarchy and relationships
2. **Use Case Diagram** - Actors, use cases, and associations
3. **Block Definition Diagram (BDD)** - System structure and composition
4. **Internal Block Diagram (IBD)** - Internal connections and flows
5. **Parametric Diagram** - Constraint equations and parameters
6. **Activity Diagram** - Behavioral workflows and actions
7. **Sequence Diagram** - Temporal message sequences
8. **State Machine Diagram** - State transitions and behaviors
9. **Package Diagram** - Model organization and dependencies

### File Format Support (FR-22)

SysDocs supports four SysML v2 file formats:

1. **`.sysml`** - SysML v2 textual syntax (primary format)
2. **`.kerml`** - Kernel Modeling Language files
3. **`.json`** - SysML v2 API JSON format
4. **`.sysmlv2`** - Alternative textual syntax

All formats are parsed to the same internal representation, ensuring equivalent models produce identical output (FR-25).

## Model-Based Documentation Approach

SysDocs treats SysML v2 models as **source artifacts** that can be rendered into various documentation formats. The tool does NOT prescribe specific document mappings but provides flexibility to:

- Render any diagram type in any document
- Extract model elements for tabular presentations
- Generate traceability matrices
- Embed diagrams as images
- Present element properties in structured formats

**User Control**: Documentation structure is defined by:
- **Templates** (FR-05) - Control layout and styling
- **Manifests** (FR-16-19) - Define document assembly
- **User Configuration** - Specify what to include and where

## Traceability Relationships

SysDocs preserves all SysML v2 traceability relationships defined in models (FR-20):

- **satisfy** - Design element satisfies requirement
- **refine** - Requirement refines higher-level requirement
- **derive** - Requirement derived from another
- **verify** - Test verifies requirement
- **realize** - Component realizes design element
- **compose** - Part is composed within whole
- **associate** - General association between elements
- **generalize** - Specialization/generalization relationship

These relationships are maintained in the internal model and can be presented in various ways:
- Traceability matrices
- Cross-reference tables
- Inline references in element descriptions
- Graphical representations in diagrams

## Requirements Details

### FR-20: Import SysML v2 Models

**Capability**: Import complete SysML v2 models, preserving all element relationships and traceability links

**Supported Elements**:
- Requirements (requirement definitions with attributes)
- Use Cases (actors, scenarios, conditions)
- Blocks (parts, ports, properties, constraints, operations)
- Interactions (lifelines, messages, timing)
- States (state machines, transitions, behaviors)
- Activities (actions, flows, decisions)
- Parameters (constraints, equations)
- Packages (model organization)
- All SysML v2 relationships (satisfy, refine, derive, verify, realize, compose, associate, generalize)

**Core Functions**:
- Parse SysML v2 models from supported file formats
- Build internal representation of model structure
- Extract all model elements with properties
- Maintain element relationships and traceability
- Support model queries and navigation

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
    satisfy STK_REQ_001;  // Traceability link preserved
}
```

---

### FR-21: Support SysML v2 Diagram Types

**Capability**: Support all 9 core SysML v2 diagram types as defined by OMG specification

**1. Requirements Diagram**
- Visualizes requirement hierarchy and relationships
- Shows containment, derivation, satisfaction, verification
- Displays requirement attributes inline

**2. Use Case Diagram**
- Shows actors and their interactions with use cases
- Displays system boundaries
- Shows use case relationships (include, extend, generalization)

**3. Block Definition Diagram (BDD)**
- Structural view of system composition
- Shows blocks, value types, and relationships
- Displays generalization, association, composition, aggregation

**4. Internal Block Diagram (IBD)**
- Internal structure and connections within blocks
- Shows parts, ports, and connectors
- Displays item flows between ports

**5. Parametric Diagram**
- Constraint equations and parameter bindings
- Shows constraint blocks and value properties
- Displays mathematical relationships

**6. Activity Diagram**
- Behavioral workflows and processes
- Shows actions, control flow, object flow
- Displays decision points, forks, joins

**7. Sequence Diagram**
- Temporal ordering of interactions
- Shows lifelines, messages, execution specifications
- Displays timing constraints and guards

**8. State Machine Diagram**
- State-based behavior specifications
- Shows states, transitions, events, guards, effects
- Displays entry/exit behaviors

**9. Package Diagram**
- Model organization and dependencies
- Shows packages and their relationships
- Displays import/access dependencies

**Output**: Each diagram type is rendered as an embedded image (SVG/PNG/PDF) with optional textual descriptions

---

### FR-22: Support SysML v2 File Formats

**Capability**: Parse and import SysML v2 models from multiple file formats

**Supported Formats**:

1. **`.sysml` - SysML v2 Textual Syntax**
   - Human-readable textual representation
   - Primary authoring format
   - Direct KerML + SysML v2 syntax

2. **`.kerml` - Kernel Modeling Language**
   - Foundation layer of SysML v2
   - Core metamodel elements
   - Can be extended with SysML v2 constructs

3. **`.json` - SysML v2 API Format**
   - JSON representation following SysML v2 API specification
   - Machine-generated and processed
   - Interoperability with SysML v2 tools via REST API

4. **`.sysmlv2` - Alternative Textual Syntax**
   - Alternative file extension for SysML v2 textual format
   - Same syntax as .sysml
   - Used by some tools for clarity

**Format Detection**: Automatic format detection based on file extension and content structure

**Equivalence**: Models with identical content produce identical output regardless of source format (FR-25)

---

### FR-23: Render SysML v2 Diagrams

**Capability**: Render SysML v2 diagrams as embedded images in output documents

**Supported Output Formats**:
- **SVG** (Scalable Vector Graphics) - Preferred for quality and scalability
- **PNG** (Portable Network Graphics) - Fallback for compatibility
- **PDF** (Portable Document Format) - Direct embedding for PDF outputs

**Rendering Features**:
- High-quality vector graphics (SVG preferred)
- Configurable resolution and sizing
- Consistent styling across diagrams
- Support for diagram annotations and notes
- Deterministic rendering (FR-25)

**Image Embedding**:
- Diagrams embedded directly in PDF output
- Maintains aspect ratio and readability
- Supports captions and references
- Controlled via templates (FR-05)

**Example Configuration**:
```json
{
  "sysml": {
    "diagramFormat": "svg",
    "defaultWidth": 800,
    "defaultHeight": 600,
    "quality": "high"
  }
}
```

---

### FR-24: Extract SysML v2 Element Properties

**Capability**: Extract and present model element properties in structured formats

**Requirements Properties**:
- ID, text, description
- Priority, status, risk level
- Source, rationale
- Verification method and criteria
- Traceability relationships

**Use Case Properties**:
- Name, description
- Actors (primary, secondary, supporting)
- Preconditions, postconditions
- Main scenario, alternative flows
- Exception handling
- System boundary

**Block Properties**:
- Name, type, stereotype
- Ports (name, type, direction)
- Properties (name, type, multiplicity, default value)
- Constraints (mathematical, behavioral)
- Operations (signature, parameters, return type)
- Internal parts and connections

**Interaction Properties**:
- Scenario name, description
- Lifelines (representing blocks/actors)
- Messages (signature, parameters, sequence number, message type)
- Timing constraints (duration, deadline)
- Guards and conditions

**Output Formats**:
- Tabular presentations (requirements tables, property lists)
- Structured text (descriptions, specifications)
- Traceability matrices
- Cross-reference indices

---

### FR-25: Deterministic SysML v2 Output

**Capability**: Ensure byte-for-byte identical output for equivalent SysML v2 models

**Determinism Guarantees**:
- Same model → Same output (regardless of import timing)
- Different formats (.sysml, .kerml, .json) → Same output (if equivalent content)
- Cross-platform → Same output (Windows, Linux, macOS)
- Repeated runs → Same output (no timestamps, no random elements)

**Implementation Approach**:
- Normalize diagram layouts (fixed coordinates, stable ordering)
- Remove timestamps from metadata
- Stable element sorting (alphabetical or by ID)
- Deterministic UUID handling
- Consistent font rendering
- Normalized line endings and whitespace

**Verification**: SHA-256 hash comparison confirms byte-for-byte identity

**Example**:
```bash
# Import same model twice
sysdocs import model.sysml -o output1.pdf
sysdocs import model.sysml -o output2.pdf

# Verify determinism
sha256sum output1.pdf output2.pdf
# Both should have identical hash
```

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
