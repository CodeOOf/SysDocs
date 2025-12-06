# Systems Engineering Artifact Definitions

**📖 Navigation**: [🏠 README](../README.md) | [🗺️ Docs Navigation](DOCUMENTATION_NAVIGATION.md)

> 📝 **TECHNICAL REFERENCE**  
> This document defines SE artifacts for FR-10 (INCOSE alignment) and FR-11 (V-Model phases).  
> Machine-parsable definitions: [src/SysDocs.Core/Model/SeArtifactDefinitions.json](../src/SysDocs.Core/Model/SeArtifactDefinitions.json)

**Related Requirements**: FR-10 (INCOSE SE Handbook alignment), FR-11 (V-Model phases)

---

## 📋 Overview

SysDocs aligns with industry-standard Systems Engineering practices:

- **FR-10**: Outputs aligned with **INCOSE SE Handbook** (5th Edition)
- **FR-11**: Documentation follows **V-Model** development methodology

This document provides both human-readable explanations and references to machine-parsable definitions used by the tool.

---

## 🔄 V-Model Development Phases

The V-Model represents system development as a series of phases with corresponding verification activities:

```
Validation ←─────────────────────────────────→ Stakeholder Requirements
    ↑                                                      ↓
System Verification ←──────────────────────→ System Requirements
    ↑                                                      ↓
Integration Verification ←─────────→ Architecture
    ↑                                                      ↓
Unit Verification ←───────→ Detailed Design
```

### Left Side: Development Phases

#### 1. Stakeholder Requirements
- **INCOSE Process**: Business or Mission Analysis
- **Purpose**: Capture stakeholder needs and expectations
- **Typical Artifacts**:
  - Stakeholder Requirements Document
  - Concept of Operations (ConOps)
  - User Stories
  - Use Case Diagrams
- **Verification Phase**: Validation (right side)

#### 2. System Requirements
- **INCOSE Process**: System Requirements Definition
- **Purpose**: Translate stakeholder needs into technical requirements
- **Typical Artifacts**:
  - System Requirements Specification (SRS)
  - Functional Requirements (FR)
  - Non-Functional Requirements (NFR)
  - Constraints (C)
  - Build Requirements (BR)
- **Verification Phase**: System Verification
- **Example**: [project/REQUIREMENTS.md](../project/REQUIREMENTS.md)

#### 3. System Architecture
- **INCOSE Process**: Architecture Definition
- **Purpose**: Define system structure and component allocation
- **Typical Artifacts**:
  - System Architecture Document
  - Component Diagrams
  - Interface Definitions
  - Allocation Tables
- **Verification Phase**: Integration Verification

#### 4. Detailed Design
- **INCOSE Process**: Design Definition
- **Purpose**: Component-level implementation specifications
- **Typical Artifacts**:
  - Detailed Design Document
  - Component Specifications
  - Interface Control Documents (ICD)
  - Data Models
- **Verification Phase**: Unit Verification

### Right Side: Verification & Validation Phases

#### 5. Unit Verification
- **INCOSE Process**: Verification
- **Purpose**: Verify individual components meet design
- **Typical Artifacts**:
  - Unit Test Plans
  - Unit Test Reports
  - Code Review Records
  - Coverage Reports
- **Verifies**: Detailed Design

#### 6. Integration Verification
- **INCOSE Process**: Integration and Verification
- **Purpose**: Verify components work together
- **Typical Artifacts**:
  - Integration Test Plans
  - Integration Test Reports
  - Interface Test Results
- **Verifies**: Architecture

#### 7. System Verification
- **INCOSE Process**: System Verification
- **Purpose**: Verify system meets requirements
- **Typical Artifacts**:
  - System Test Plans
  - System Test Reports
  - Verification Matrix
  - Requirements Verification Reports
- **Verifies**: System Requirements
- **Example**: [project/VERIFICATION_VALIDATION.md](../project/VERIFICATION_VALIDATION.md)

#### 8. System Validation
- **INCOSE Process**: Validation
- **Purpose**: Validate system meets stakeholder needs
- **Typical Artifacts**:
  - Validation Test Plans
  - Validation Reports
  - User Acceptance Tests (UAT)
  - Operational Readiness Reviews
- **Validates**: Stakeholder Requirements

---

## 🔗 Cross-Cutting Artifacts

These artifacts span multiple V-Model phases:

### Traceability Matrix
- **INCOSE Process**: Technical Planning (Configuration Management)
- **Purpose**: Document relationships between artifacts across phases
- **Types**:
  - Requirements Traceability Matrix (RTM)
  - Verification Matrix
- **Output Formats**: PDF, Markdown, Excel
- **Automation**: ✅ Supported via test attributes
- **Example**: [project/TRACEABILITY.md](../project/TRACEABILITY.md)

### Verification & Validation Matrix
- **INCOSE Process**: Verification and Validation Planning
- **Purpose**: Map requirements to V&V methods
- **Types**:
  - V&V Matrix
  - Test Coverage Matrix
- **Output Formats**: PDF, Markdown
- **Automation**: ✅ Supported via test reporting
- **Example**: [project/REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md)

### Change Management
- **INCOSE Process**: Configuration Management
- **Purpose**: Track changes across all phases and artifacts
- **Types**:
  - Change Requests
  - Change Impact Analysis
  - Version History
- **Output Formats**: PDF, Markdown
- **Automation**: ✅ Supported via Git integration

---

## 📊 INCOSE SE Handbook Alignment

### Process Groups

SysDocs supports artifacts for all INCOSE Technical Process Groups:

| INCOSE Process | V-Model Phase | SysDocs Support |
|----------------|---------------|-----------------|
| **Business or Mission Analysis** | Stakeholder Requirements | ✅ Template support |
| **System Requirements Definition** | System Requirements | ✅ Template support |
| **Architecture Definition** | Architecture | ✅ Template support |
| **Design Definition** | Detailed Design | ✅ Template support |
| **Verification** | Unit/Integration/System Verification | ✅ Automated test reporting |
| **Validation** | System Validation | ✅ Template support |
| **Configuration Management** | Cross-cutting | ✅ Git integration |

### Lifecycle Stages

SysDocs artifacts can be generated for any INCOSE lifecycle stage:

1. **Concept** - Initial exploration and feasibility
2. **Development** - Design, build, and verification
3. **Production** - Manufacturing and deployment
4. **Utilization** - Operational use
5. **Support** - Maintenance and upgrades
6. **Retirement** - End-of-life disposal

---

## 🤖 Machine-Parsable Definitions

### JSON Schema Location

All SE artifact definitions are defined in a machine-parsable JSON file:

**File**: `src/SysDocs.Core/Model/SeArtifactDefinitions.json`

### Schema Structure

```json
{
  "vModelPhases": [
    {
      "id": "system-requirements",
      "name": "System Requirements",
      "vModelLevel": "left-mid-upper",
      "sequence": 2,
      "incoseProcess": "System Requirements Definition",
      "typicalArtifacts": ["SRS", "FR", "NFR"],
      "verification": "system-verification",
      "traceabilityLinks": ["stakeholder-requirements", "architecture"]
    }
  ],
  "crossCuttingArtifacts": [...],
  "documentTemplates": {...}
}
```

### Using Definitions in Code

```csharp
// Example: Load SE artifact definitions
var definitions = SeArtifactDefinitions.Load();

// Generate artifact based on V-Model phase
var phase = definitions.GetPhase("system-requirements");
var template = definitions.GetTemplate(phase.Id);

// Validate traceability
bool isValid = definitions.ValidateTraceability(
    sourcePhase: "system-requirements",
    targetPhase: "architecture"
);
```

### Automation Benefits

1. **Template Selection**: Automatically select correct template for V-Model phase
2. **Traceability Validation**: Ensure proper links between phases
3. **Completeness Checks**: Verify all required artifacts for phase
4. **Report Generation**: Auto-generate V&V matrices and traceability reports

---

## 📝 Document Templates

SysDocs includes standardized templates for each artifact type:

### Stakeholder Requirements Template
- Introduction and Purpose
- Stakeholder Identification
- Operational Concepts
- Stakeholder Needs
- Requirements
- Constraints
- Assumptions

### System Requirements Template
- Introduction
- Functional Requirements
- Non-Functional Requirements
- Constraints
- Interface Requirements
- Traceability to Stakeholder Requirements

### Architecture Document Template
- Introduction
- Architectural Views
- Component Descriptions
- Interface Definitions
- Allocation of Requirements
- Design Rationale

### Verification Report Template
- Introduction
- Verification Approach
- Test Cases and Results
- Requirements Coverage
- Deviations and Waivers
- Conclusions

---

## 🔍 Artifact Metadata

All generated artifacts include standard metadata:

| Attribute | Type | Description |
|-----------|------|-------------|
| `version` | string | Semantic version (e.g., "1.2.0") |
| `author` | string | Primary author |
| `lastModified` | ISO8601 | Last modification timestamp |
| `approver` | string | Approval authority |
| `status` | enum | draft, review, approved, obsolete |
| `classification` | enum | public, internal, confidential |
| `relatedRequirements` | array | Linked requirement IDs |
| `traceabilityLinks` | array | Linked artifact IDs |

---

## 🎯 Implementation Examples

### SysDocs Project Usage

This project uses the same SE artifacts it generates:

| Artifact | Location | V-Model Phase |
|----------|----------|---------------|
| **System Requirements** | [project/REQUIREMENTS.md](../project/REQUIREMENTS.md) | System Requirements |
| **Requirements Matrix** | [project/REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md) | V&V Matrix |
| **Traceability** | [project/TRACEABILITY.md](../project/TRACEABILITY.md) | Traceability Matrix |
| **V&V Strategy** | [project/VERIFICATION_VALIDATION.md](../project/VERIFICATION_VALIDATION.md) | System Verification |
| **Test Reports** | [reports/TEST_TRACEABILITY.md](../reports/TEST_TRACEABILITY.md) | Automated V&V |

---

## 🔗 Related Documentation

- **[project/REQUIREMENTS.md](../project/REQUIREMENTS.md)** - Complete requirements (FR-10, FR-11)
- **[project/REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md)** - V&V matrix example
- **[project/TRACEABILITY.md](../project/TRACEABILITY.md)** - Traceability matrix example
- **[project/VERIFICATION_VALIDATION.md](../project/VERIFICATION_VALIDATION.md)** - V&V approach
- **[reports/TEST_TRACEABILITY.md](../reports/TEST_TRACEABILITY.md)** - Automated test-to-requirement mapping

---

## 📚 References

1. **INCOSE Systems Engineering Handbook**: A Guide for System Life Cycle Processes and Activities, 5th Edition
2. **ISO/IEC/IEEE 15288:2023**: Systems and software engineering — System life cycle processes
3. **V-Model**: Systems Development Methodology
4. **IEEE 29148**: Requirements engineering standard

---

**Machine-Parsable Schema**: [SeArtifactDefinitions.json](../src/SysDocs.Core/Model/SeArtifactDefinitions.json)
