# Requirements Traceability Matrix (RTM)

**Project**: Autonomous Drone Navigation System (ADNS)  
**Document**: 13_RTM_ADNS  
**Version**: 1.0.0  
**Date**: November 20, 2025  
**Status**: Baselined

---

## 1. Introduction

This Requirements Traceability Matrix traces stakeholder requirements through system requirements to design elements and verification methods.

## 2. Traceability

| Stakeholder Req | System Req | Design Element | Verification Method | Test ID | Status |
|-----------------|------------|----------------|---------------------|---------|--------|
| STK-FR-001 | SYS-FR-001 | LiDAR Sensor + Perception Module | System Test | ST-001 | ✅ Passed |
| STK-FR-001 | SYS-FR-002 | Obstacle Classification Algorithm | System Test | ST-002 | ✅ Passed |
| STK-FR-002 | SYS-FR-010 | A* Path Planner | System Test | ST-010 | ✅ Passed |
| STK-FR-002 | SYS-FR-014 | Dynamic Replanning Module | System Test | ST-014 | ✅ Passed |
| STK-FR-003 | SYS-FR-020 | Extended Kalman Filter | Performance Test | PT-020 | ✅ Passed |
| STK-FR-003 | SYS-FR-021 | State Estimator | System Test | ST-021 | ✅ Passed |
| STK-FR-003 | SYS-FR-022 | Visual-Inertial Odometry | System Test | ST-022 | ✅ Passed |
| STK-FR-004 | SYS-FR-012 | Waypoint Manager | System Test | ST-012 | ✅ Passed |
| STK-FR-005 | SYS-SR-001 | Geofence Module | System Test | ST-200 | ✅ Passed |
| STK-FR-006 | SYS-SR-002 | Emergency Landing Controller | Failure Test | FT-001 | ✅ Passed |
| STK-FR-007 | SYS-FR-041 | Manual Override Handler | System Test | ST-041 | ✅ Passed |
| STK-FR-008 | SYS-FR-040 | Telemetry Transmitter | System Test | ST-040 | ✅ Passed |
| STK-PR-001 | SYS-PR-010 | Multi-Sensor Fusion | System Test | ST-100 | ✅ Passed |
| STK-PR-002 | SYS-PR-010 | Detection Validation | System Test | ST-100 | ✅ Passed |
| STK-PR-003 | SYS-PR-012 | State Estimation Accuracy | Performance Test | PT-102 | ✅ Passed |
| STK-PR-004 | SYS-PR-021 | Speed Controller | System Test | ST-121 | ✅ Passed |
| STK-PR-005 | SYS-RMA-001 | System Reliability | Reliability Analysis | RA-001 | 🔄 In Progress |
| STK-OE-001 | SYS-FR-003 | Vision Pipeline | System Test | ST-001 | ✅ Passed |
| STK-OE-002 | SYS-FR-005 | Adaptive Exposure | Environmental Test | ET-002 | ✅ Passed |
| STK-OE-003 | SYS-PR-022 | Wind Compensation | Environmental Test | ET-010 | ✅ Passed |
| STK-OE-004 | SYS-ENV-001 | Thermal Management | Environmental Test | ET-100 | ✅ Passed |
| STK-USE-001 | UI-FR-001 | Mission Planner UI | Usability Test | UT-001 | ✅ Passed |
| STK-USE-002 | UI-FR-002 | Training Program | Inspection | DOC-001 | ✅ Complete |
| STK-USE-003 | SYS-FR-042 | Ground Station GUI | Inspection | UI-001 | ✅ Complete |
| STK-CON-001 | SYS-CERT-001 | DO-178C Compliance | Certification Audit | CERT-001 | 🔄 In Progress |
| STK-CON-002 | SYS-CERT-001 | Software Verification | DO-178C Audit | CERT-001 | 🔄 In Progress |
| STK-CON-003 | SYS-CERT-002 | Safety Analysis | ISO 26262 Audit | CERT-002 | 🔄 In Progress |

---

**Document Control**  
**File**: 13_RTM_ADNS.md  
**Next Review**: February 2026
