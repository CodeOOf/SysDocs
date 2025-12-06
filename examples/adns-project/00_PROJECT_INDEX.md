# ADNS Project Documentation Index

**Project**: Autonomous Drone Navigation System (ADNS)  
**Version**: 1.0.0  
**Date**: December 6, 2025

---

## Document Naming Convention

Files follow INCOSE SE Handbook v4 and IEEE standards with numeric prefixes for ordering:

```
[NN]_[TYPE]_[ProjectName]_[OptionalDetail].md
```

- **NN**: Two-digit sequence number (00-99)
- **TYPE**: Document type abbreviation
- **ProjectName**: Short project identifier (e.g., ADNS)
- **OptionalDetail**: Optional descriptive suffix

---

## Complete Document Set

### Phase 1: Project Planning (00-09)

| File | Document Title | Standard | Status |
|------|----------------|----------|--------|
| `00_PROJECT_INDEX.md` | Project Documentation Index | - | ✅ Complete |
| `01_SEMP_ADNS.md` | Systems Engineering Management Plan | ISO/IEC 15288 | ✅ Complete |

### Phase 2: Requirements (10-19)

| File | Document Title | Standard | Status |
|------|----------------|----------|--------|
| `10_StRS_ADNS.md` | Stakeholder Requirements Specification | ISO/IEC 29148 | ✅ Complete |
| `11_ConOps_ADNS.md` | Concept of Operations | IEEE 1362 | ✅ Complete |
| `12_SRS_ADNS.md` | System Requirements Specification | ISO/IEC 29148 | ✅ Complete |
| `13_RTM_ADNS.md` | Requirements Traceability Matrix | - | ✅ Complete |
| `13_RTM_ADNS.csv` | RTM (CSV Export) | - | ✅ Complete |
| `13_RTM_ADNS.json` | RTM (JSON Export) | - | ✅ Complete |

### Phase 3: Architecture & Design (20-29)

| File | Document Title | Standard | Status |
|------|----------------|----------|--------|
| `20_SAD_ADNS.md` | System Architecture Description | ISO/IEC 42010 | ✅ Complete |
| `21_SDD_ADNS_Software.md` | Software Design Document | IEEE 1016 | ✅ Complete |
| `22_HDD_ADNS_Hardware.md` | Hardware Design Document | - | ✅ Complete |
| `23_ICD_ADNS_Sensors.md` | Interface Control Document - Sensors | - | ✅ Complete |
| `24_ICD_ADNS_FlightControl.md` | Interface Control Document - Flight Control | - | ✅ Complete |

### Phase 4: Verification & Validation (30-39)

| File | Document Title | Standard | Status |
|------|----------------|----------|--------|
| `30_VVP_ADNS.md` | Verification & Validation Plan | IEEE 1012 | ✅ Complete |
| `31_STP_ADNS.md` | System Test Plan | IEEE 829 | ✅ Complete |
| `32_SIT_ADNS.md` | System Integration Test Procedures | IEEE 829 | ✅ Complete |
| `33_ATP_ADNS.md` | Acceptance Test Procedures | IEEE 829 | ✅ Complete |

### Phase 5: Safety & Risk (40-49)

| File | Document Title | Standard | Status |
|------|----------------|----------|--------|
| `40_FMEA_ADNS.md` | Failure Modes & Effects Analysis | MIL-STD-1629 | ✅ Complete |
| `41_FTA_ADNS.md` | Fault Tree Analysis | IEC 61025 | ✅ Complete |
| `42_SafetyCase_ADNS.md` | Safety Case Report | ISO 26262 | ✅ Complete |
| `43_RiskRegister_ADNS.md` | Risk Register | ISO 31000 | ✅ Complete |

### Phase 6: Configuration & Change (50-59)

| File | Document Title | Standard | Status |
|------|----------------|----------|--------|
| `50_CMP_ADNS.md` | Configuration Management Plan | IEEE 828 | ✅ Complete |
| `51_ChangeLog_ADNS.md` | Change Request Log | - | ✅ Complete |

### Phase 7: Certification & Compliance (60-69)

| File | Document Title | Standard | Status |
|------|----------------|----------|--------|
| `60_CertPlan_ADNS.md` | Certification Plan | DO-178C | ✅ Complete |
| `61_ComplianceMatrix_ADNS_DO178C.md` | DO-178C Compliance Matrix | DO-178C | ✅ Complete |
| `62_ComplianceMatrix_ADNS_ISO26262.md` | ISO 26262 Compliance Matrix | ISO 26262 | ✅ Complete |

### Phase 8: Operations & Support (70-79)

| File | Document Title | Standard | Status |
|------|----------------|----------|--------|
| `70_OpsManual_ADNS.md` | Operations Manual | - | ✅ Complete |
| `71_MaintManual_ADNS.md` | Maintenance Manual | - | ✅ Complete |
| `72_TrainingPlan_ADNS.md` | Training Plan | - | ✅ Complete |

### Phase 9: Reports & Reviews (80-89)

| File | Document Title | Standard | Status |
|------|----------------|----------|--------|
| `80_SRR_Report_ADNS.md` | System Requirements Review Report | - | ✅ Complete |
| `81_PDR_Report_ADNS.md` | Preliminary Design Review Report | - | ✅ Complete |
| `82_CDR_Report_ADNS.md` | Critical Design Review Report | - | ✅ Complete |
| `83_TRR_Report_ADNS.md` | Test Readiness Review Report | - | ✅ Complete |

### Appendices & References (90-99)

| File | Document Title | Standard | Status |
|------|----------------|----------|--------|
| `90_Glossary_ADNS.csv` | Glossary of Terms | - | ✅ Complete |
| `91_Acronyms_ADNS.md` | Acronyms List | - | ⏳ TODO |
| `92_References_ADNS.md` | References & Standards | - | ⏳ TODO |

### Data Exports & Auxiliary Files

| File | Description | Format | Status |
|------|-------------|--------|--------|
| `requirements_export.csv` | Requirements database export | CSV | ✅ Complete |
| `risk_register.csv` | Risk management data | CSV | ✅ Complete |
| `test_results.csv` | Test execution results | CSV | ✅ Complete |
| `project_metadata.xml` | Project metadata | XML | ✅ Complete |

---

## Document Relationships

```
01_SEMP ─────────────────────────────────────┐
   │                                         │
   ├──► 10_StRS ──► 12_SRS ──► 13_RTM       │
   │                    │                    │
   │                    ├──► 20_SAD          │
   │                    │       │            │
   │                    │       ├──► 21_SDD  │
   │                    │       ├──► 22_HDD  │
   │                    │       ├──► 23_ICD  │
   │                    │       └──► 24_ICD  │
   │                    │                    │
   │                    └──► 30_VVP          │
   │                           │             │
   │                           ├──► 31_STP   │
   │                           ├──► 32_SIT   │
   │                           └──► 33_ATP   │
   │                                         │
   ├──► 40_FMEA ──► 42_SafetyCase           │
   ├──► 41_FTA  ─┘                          │
   ├──► 43_RiskRegister                     │
   │                                         │
   ├──► 50_CMP                               │
   ├──► 60_CertPlan ──► 61_ComplianceMatrix │
   │                └──► 62_ComplianceMatrix │
   │                                         │
   └──► 80-83_Review_Reports ────────────────┘
```

---

## Usage with SysDocs

### Generate Individual Document
```bash
sysdocs --input examples/adns-project/12_SRS_ADNS.md \
        --output output/SRS_ADNS.pdf
```

### Generate Document Set (Requirements Phase)
```bash
sysdocs --input examples/adns-project/1*.md \
        --output output/ADNS_Requirements_Package.pdf
```

### Generate Complete Documentation
```bash
sysdocs --input examples/adns-project/*.md \
        --output output/ADNS_Complete_Documentation.pdf
```

### Test Determinism (FR-06, NFR-01)
```bash
# Generate twice, compare hashes
sysdocs --input examples/adns-project/12_SRS_ADNS.md --output run1.pdf
sysdocs --input examples/adns-project/12_SRS_ADNS.md --output run2.pdf
sha256sum run1.pdf run2.pdf  # Should be identical
```

---

## Standards Reference

| Standard | Title | Applies To |
|----------|-------|------------|
| ISO/IEC 15288 | Systems and software engineering - System life cycle processes | SEMP |
| ISO/IEC 29148 | Systems and software engineering - Requirements engineering | StRS, SRS |
| IEEE 1362 | Guide for Concept of Operations Documents | ConOps |
| ISO/IEC 42010 | Systems and software engineering - Architecture description | SAD |
| IEEE 1016 | Software Design Descriptions | SDD |
| IEEE 1012 | Software Verification and Validation | VVP |
| IEEE 829 | Software Test Documentation | Test Plans |
| MIL-STD-1629 | Failure Mode, Effects and Criticality Analysis | FMEA |
| IEC 61025 | Fault tree analysis | FTA |
| ISO 26262 | Road vehicles - Functional safety | Safety Case |
| IEEE 828 | Software Configuration Management Plans | CMP |
| DO-178C | Software Considerations in Airborne Systems | Certification |

---

## License

These are **example documents** for the SysDocs project. Content is entirely fictional and for demonstration purposes only.
