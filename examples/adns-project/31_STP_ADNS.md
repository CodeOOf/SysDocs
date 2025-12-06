# System Test Plan (STP)

**Project**: Autonomous Drone Navigation System (ADNS)  
**Document**: 31_STP_ADNS  
**Version**: 1.5.0  
**Date**: November 20, 2025  
**Status**: Approved

---

## 1. Introduction

### 1.1 Purpose

This System Test Plan defines the strategy, scope, resources, and schedule for testing the Autonomous Drone Navigation System (ADNS) at the system level.

### 1.2 Scope

System testing verifies all system requirements from 12_SRS_ADNS through integrated hardware/software testing in representative operational environments.

### 1.3 Test Objectives

1. Verify all system requirements are met
2. Validate system performance in operational scenarios
3. Demonstrate DO-178C Level C compliance
4. Identify defects before delivery
5. Provide evidence for certification

---

## 2. Test Strategy

### 2.1 Test Levels

```
Unit Tests (UT)
    ↓ [Pass: 95% coverage]
Component Integration Tests (IT)
    ↓ [Pass: All interfaces verified]
Subsystem Integration Tests (SIT)
    ↓ [Pass: Subsystems interact correctly]
System Tests (ST) ◄── THIS DOCUMENT
    ↓ [Pass: Requirements verified]
Acceptance Tests (AT)
    ↓ [Pass: Customer acceptance]
```

### 2.2 Test Types

| Test Type | Purpose | Environment | Responsibility |
|-----------|---------|-------------|----------------|
| **Functional Tests** | Verify functional requirements (SYS-FR-*) | HIL + Flight Range | Test Team |
| **Performance Tests** | Verify timing, throughput, accuracy (SYS-PR-*) | HIL Lab | Test Team |
| **Environmental Tests** | Verify operation in environmental conditions | Environmental Chamber | Test Team |
| **Safety Tests** | Verify safety requirements (SYS-SR-*) and fault handling | HIL Lab | Safety Engineer + IV&V |
| **Security Tests** | Verify encryption, authentication (SYS-SEC-*) | Security Lab | Security Team |
| **Interface Tests** | Verify interface requirements (SYS-IF-*) | Integration Lab | Integration Team |

---

## 3. Test Environment

### 3.1 Hardware-in-the-Loop (HIL) Lab

**Components**:
- Real sensors (LiDAR: Velodyne VLP-16, Camera: IMX477, IMU: BMI088)
- Real flight controller (Pixhawk 4)
- Real compute platform (NVIDIA Jetson AGX Orin 64GB)
- Simulated physics (X-Plane 12 + ROS2 Humble Gazebo)
- Simulated environment (virtual warehouse, urban, forest scenarios)

**Capabilities**:
- Repeatable test scenarios (deterministic simulation)
- Fault injection (sensor failures, communication loss, CPU faults)
- Performance measurement (latency, throughput, accuracy)
- 24/7 automated regression testing (Jenkins CI/CD)

### 3.2 Indoor Test Range

**Specifications**:
- Size: 20m x 20m x 5m (height)
- Motion capture system (OptiTrack Prime 17W, 16 cameras) for ground truth
- Static obstacles (poles, walls, boxes)
- Dynamic obstacles (motorized platforms, 2 m/s max speed)
- Controlled lighting (LED panels, 10 lux to 100,000 lux variable)
- Safety nets (perimeter and ceiling)

### 3.3 Outdoor Flight Test Range

**Specifications**:
- Size: 500m x 500m
- FAA-approved airspace (Certificate of Authorization #COA-2025-AER-001)
- Safety observer stations (4 positions)
- Weather station (wind, temperature, humidity logging)
- Video recording from multiple angles (4K, 60 FPS)
- Telemetry ground station (real-time monitoring)
- Emergency landing zones (3 designated areas)

---

## 4. Test Cases

### 4.1 Functional Test Cases

#### ST-001: Obstacle Detection Range

| Field | Value |
|-------|-------|
| **ID** | ST-001 |
| **Requirement** | SYS-FR-001 |
| **Objective** | Verify obstacle detection within 50m radius |
| **Preconditions** | System initialized, LiDAR operational, HIL simulation active |
| **Test Steps** | 1. Spawn obstacle (1m cube) at 10m distance<br>2. Verify system detects obstacle<br>3. Repeat at 20m, 30m, 40m, 50m, 60m<br>4. Record detection success/failure |
| **Expected Result** | Obstacles detected at 10m-50m, NOT detected at 60m |
| **Pass Criteria** | 100% detection within 50m, 0% detection beyond 50m |
| **Test Data** | HIL scenario: warehouse_obstacle_range.xml |

#### ST-010: Collision-Free Trajectory Computation

| Field | Value |
|-------|-------|
| **ID** | ST-010 |
| **Requirement** | SYS-FR-010 |
| **Objective** | Verify trajectory computation within 200ms of obstacle detection |
| **Preconditions** | System in flight, autonomous mode active |
| **Test Steps** | 1. Drone flying at 5 m/s toward waypoint<br>2. Inject obstacle suddenly in path (30m ahead)<br>3. Timestamp obstacle detection<br>4. Timestamp new trajectory published<br>5. Calculate latency<br>6. Verify trajectory clears obstacle by > 2m |
| **Expected Result** | New trajectory computed < 200ms, clearance > 2m, no collision |
| **Pass Criteria** | Latency < 200ms AND clearance > 2m AND no collision |
| **Test Data** | HIL scenario: sudden_obstacle_injection.xml |

#### ST-030: Autonomous Takeoff

| Field | Value |
|-------|-------|
| **ID** | ST-030 |
| **Requirement** | SYS-FR-030 |
| **Objective** | Verify autonomous takeoff to 5m altitude |
| **Preconditions** | Drone on ground, system armed, mission uploaded |
| **Test Steps** | 1. Send "Start Mission" command<br>2. Observe autonomous takeoff<br>3. Measure altitude reached<br>4. Verify stability (hover for 10 seconds) |
| **Expected Result** | Ascends to 5m ± 0.2m, stable hover |
| **Pass Criteria** | Altitude 4.8m - 5.2m AND hover stability < 0.5m drift |
| **Test Data** | Flight test at outdoor range |

### 4.2 Performance Test Cases

#### PT-001: Camera Frame Rate

| Field | Value |
|-------|-------|
| **ID** | PT-001 |
| **Requirement** | SYS-FR-003 |
| **Objective** | Verify camera processing at ≥ 30 FPS |
| **Preconditions** | System operational, camera streaming, vision pipeline active |
| **Test Steps** | 1. Enable FPS counter (ROS2 topic /camera/fps)<br>2. Measure frame timestamps over 60 seconds<br>3. Calculate average FPS<br>4. Calculate min FPS (worst-case) |
| **Expected Result** | Average FPS ≥ 30, Min FPS ≥ 28 |
| **Pass Criteria** | Average FPS ≥ 30 for entire 60-second duration |
| **Test Data** | Automated test in HIL (camera_fps_test.py) |

#### PT-100: End-to-End Latency

| Field | Value |
|-------|-------|
| **ID** | PT-100 |
| **Requirement** | SYS-PR-001 |
| **Objective** | Verify latency < 100ms from sensor input to control output |
| **Preconditions** | System operational in HIL, latency measurement harness installed |
| **Test Steps** | 1. Inject sensor stimulus (obstacle appears)<br>2. Measure time to control output (new trajectory)<br>3. Repeat 100 times<br>4. Calculate mean, max, 95th percentile |
| **Expected Result** | Mean < 100ms, Max < 120ms, 95th percentile < 100ms |
| **Pass Criteria** | 100% of samples < 100ms |
| **Test Data** | Automated test (latency_profiler.py) |
| **Current Status** | ⚠️ FAILING - Mean 62ms (PASS), but 95th percentile 112ms (FAIL)<br>**Action**: Optimize vision pipeline (GPU acceleration) |

#### PT-102: Position Estimation RMSE

| Field | Value |
|-------|-------|
| **ID** | PT-102 |
| **Requirement** | SYS-PR-012 |
| **Objective** | Verify position RMSE < 0.3m |
| **Preconditions** | Indoor test range, OptiTrack motion capture (ground truth) |
| **Test Steps** | 1. Fly predefined trajectory (100m total path)<br>2. Log ADNS position estimates (100 Hz)<br>3. Log OptiTrack ground truth (240 Hz, downsampled to 100 Hz)<br>4. Calculate RMSE: sqrt(mean((x_est - x_true)^2 + (y_est - y_true)^2 + (z_est - z_true)^2)) |
| **Expected Result** | RMSE < 0.3m |
| **Pass Criteria** | RMSE < 0.3m over entire 100m trajectory |
| **Test Data** | Flight test (indoor_trajectory_100m.yaml) |

### 4.3 Safety Test Cases

#### FT-001: Emergency Landing on Critical Failure

| Field | Value |
|-------|-------|
| **ID** | FT-001 |
| **Requirement** | SYS-SR-002 |
| **Objective** | Verify emergency landing on critical system failure |
| **Preconditions** | System in flight at 20m AGL, HIL simulation |
| **Test Steps** | 1. Inject critical failure (simulate CPU fault, watchdog timeout)<br>2. Verify emergency landing initiated<br>3. Measure time to landing sequence start<br>4. Verify safe landing (no crash) |
| **Expected Result** | Landing sequence starts within 500ms, lands within 30 seconds, no crash |
| **Pass Criteria** | Emergency landing executed AND landing gear touches down safely |
| **Test Data** | HIL scenario (critical_failure_injection.xml) |

#### FT-002: Single Sensor Failure Tolerance

| Field | Value |
|-------|-------|
| **ID** | FT-002 |
| **Requirement** | SYS-SR-003 |
| **Objective** | Verify obstacle detection continues if single sensor fails |
| **Preconditions** | System operational, all sensors healthy |
| **Test Steps** | 1. Place obstacles in flight path<br>2. Disable LiDAR (simulate failure)<br>3. Verify obstacle detection via camera<br>4. Measure detection accuracy<br>5. Re-enable LiDAR, disable camera<br>6. Verify obstacle detection via LiDAR<br>7. Measure detection accuracy |
| **Expected Result** | Detection continues with reduced accuracy (> 90%) |
| **Pass Criteria** | Detection accuracy > 90% with single sensor failure |
| **Test Data** | HIL scenario (sensor_failure_tolerance.xml) |

#### FT-005: Watchdog Timer

| Field | Value |
|-------|-------|
| **ID** | FT-005 |
| **Requirement** | SYS-SR-005 |
| **Objective** | Verify watchdog detects software hang within 1 second |
| **Preconditions** | System operational, watchdog enabled |
| **Test Steps** | 1. Inject infinite loop in navigation code<br>2. Timestamp hang injection<br>3. Timestamp watchdog reset trigger<br>4. Calculate detection time<br>5. Verify system recovers (reboots) |
| **Expected Result** | Watchdog triggers within 1 second, system recovers |
| **Pass Criteria** | Detection time < 1 second AND system operational after recovery |
| **Test Data** | Unit test (watchdog_test.cpp) + HIL validation |

### 4.4 Security Test Cases

#### SEC-001: Communication Encryption

| Field | Value |
|-------|-------|
| **ID** | SEC-001 |
| **Requirement** | SYS-SEC-001 |
| **Objective** | Verify AES-256 encryption on ground station communications |
| **Preconditions** | System operational, ground station connected |
| **Test Steps** | 1. Capture radio traffic (software-defined radio)<br>2. Attempt to decrypt without key<br>3. Verify encryption algorithm (AES-256-GCM)<br>4. Verify key exchange (Diffie-Hellman) |
| **Expected Result** | Traffic encrypted, decryption without key fails |
| **Pass Criteria** | AES-256-GCM verified AND plaintext not recoverable |
| **Test Data** | Security test (packet_capture_analysis.py) |

---

## 5. Test Schedule

### 5.1 Test Phases

| Phase | Start Date | End Date | Milestone |
|-------|------------|----------|-----------|
| Unit Testing | Jul 1, 2025 | Sep 30, 2025 | Code complete |
| Component Integration | Sep 1, 2025 | Oct 31, 2025 | Integration complete |
| **System Testing** | **Oct 1, 2025** | **Dec 31, 2025** | **Current Phase** |
| Flight Testing | Jan 15, 2026 | Mar 31, 2026 | Flight test complete |
| Acceptance Testing | Apr 1, 2026 | May 31, 2026 | Customer acceptance |

### 5.2 Current Status (Dec 6, 2025)

**System Testing Progress**:
- Functional Tests: 82% complete (130/159 tests passed)
- Performance Tests: 68% complete (27/40 tests passed, 1 failing)
- Safety Tests: 60% complete (12/20 tests passed)
- Environmental Tests: 35% complete (7/20 tests passed)
- Security Tests: 100% complete (5/5 tests passed)

**Failing Tests**:
- **PT-100** (End-to-end latency): 95th percentile 112ms (target < 100ms)
  - **Root Cause**: Vision pipeline optimization needed
  - **Action**: GPU acceleration implementation in progress
  - **ETA**: Dec 15, 2025

**Blocked Tests**:
- **ET-020** (Radio range): Outdoor testing blocked by weather
  - **Action**: Scheduled for next clear weather window
  - **ETA**: Dec 10-15, 2025

---

## 6. Test Resources

### 6.1 Personnel

| Role | Name | Allocation | Responsibility |
|------|------|------------|----------------|
| Test Manager | Emily Watson | 100% | Test planning, execution, reporting |
| Test Lead | David Lee | 100% | HIL testing, test automation |
| Test Engineer | Maria Garcia | 100% | Test case execution, defect tracking |
| Test Engineer | John Smith | 100% | Environmental testing, flight tests |
| Safety Analyst | Dr. Anna Schmidt | 50% | Safety test review, fault injection |
| IV&V Engineer | Tom Johnson | 25% | Independent verification (AeroVerify Inc.) |

### 6.2 Equipment

| Item | Quantity | Location | Status |
|------|----------|----------|--------|
| HIL Test Rig | 1 | Integration Lab (Bldg 3, Room 201) | Operational |
| Drone (Alpha Prototype) | 2 | Flight Test Range | Operational |
| Drone (Beta Prototype) | 3 | Flight Test Range | Delivery Dec 15 |
| Environmental Chamber | 1 | Test Lab (Bldg 2, Room 105) | Operational |
| OptiTrack Motion Capture | 1 | Indoor Range (Bldg 4, Hangar A) | Operational |
| Ground Station (Primary) | 2 | Flight Range + Lab | Operational |
| Ground Station (Backup) | 1 | Lab | Operational |
| SDR Equipment (Security Testing) | 1 | Security Lab (Bldg 1, Room 301) | Operational |

---

## 7. Test Deliverables

### 7.1 Test Documentation

- ✅ Test Plan (this document) - Approved Nov 20, 2025
- ✅ Test Procedures (159 procedures) - 100% complete
- 🔄 Daily Test Reports - Ongoing (automated via Jenkins)
- 🔄 Weekly Test Summary - Ongoing (every Friday)
- ⏳ Test Summary Report - Due Jan 15, 2026
- ⏳ IV&V Report (Independent Verification & Validation) - Due May 15, 2026
- ⏳ DO-178C Test Compliance Matrix - Due May 31, 2026

### 7.2 Test Artifacts

- Test cases (automated: 127, manual: 32)
- Test data (input scenarios, expected outputs)
- Test logs (timestamped execution records, stored in GitLab)
- Defect reports (JIRA project: ADNS-TEST)
- Traceability matrix (Requirements ↔ Test Cases, maintained in DOORS Next)
- Video recordings (all flight tests, 4K 60fps, archived for 10 years)
- Performance profiles (latency, CPU, memory, network usage)

---

## 8. Defect Management

### 8.1 Severity Classification

| Severity | Definition | Response Time | Approval to Close |
|----------|------------|---------------|-------------------|
| **Critical** | System crash, safety violation, certification blocker | Immediate (< 4 hours) | Program Manager |
| **High** | Requirement failure, major functional issue | 24 hours | Test Manager |
| **Medium** | Minor functional issue, workaround exists | 1 week | Test Lead |
| **Low** | Cosmetic, documentation, nice-to-have | Next release | Test Lead |

### 8.2 Current Defect Status (Dec 6, 2025)

| Severity | Open | In Progress | Resolved | Closed | Total |
|----------|------|-------------|----------|--------|-------|
| Critical | 0 | 0 | 3 | 3 | 6 |
| High | 3 | 6 | 15 | 15 | 39 |
| Medium | 12 | 10 | 28 | 28 | 78 |
| Low | 24 | 4 | 48 | 48 | 124 |
| **Total** | **39** | **20** | **94** | **94** | **247** |

**Defect Trend**: ✅ Declining (Good - nearing test completion)

**Critical Defects (Historical)**:
1. ADNS-TEST-001: LiDAR driver crash on point cloud overflow → Fixed Sep 15
2. ADNS-TEST-012: Watchdog not triggering on hang → Fixed Oct 3
3. ADNS-TEST-045: Geofence breach in edge case → Fixed Nov 8

---

## 9. Entry and Exit Criteria

### 9.1 System Test Entry Criteria

- ✅ All unit tests passed (95% code coverage achieved)
- ✅ All integration tests passed (all interfaces verified)
- ✅ Code review 100% complete (all code reviewed, approved)
- ✅ Static analysis passed (no critical/high findings)
- ✅ Test environment ready (HIL, indoor range, outdoor range operational)
- ✅ Test procedures approved (all 159 procedures reviewed, baselined)
- ✅ Test data prepared (scenarios, inputs, expected outputs)

### 9.2 System Test Exit Criteria

- 🔄 All test cases executed (82% complete - **IN PROGRESS**)
- ⚠️ All critical/high defects resolved (3 high still open - **BLOCKER**)
- ⏳ 95% of test cases passed (currently 84% - need 11% more)
- ⏳ Test summary report approved
- ⏳ Customer sign-off obtained (planned Jan 2026)

**Current Blockers**:
- 3 high-severity defects must be resolved before exit
- PT-100 performance test must pass (optimization in progress)

---

## 10. Risks and Mitigation

| Risk | Probability | Impact | Mitigation | Owner |
|------|-------------|--------|------------|-------|
| Weather delays for outdoor flight tests | High | Medium | Schedule buffer (2 weeks), indoor backup tests, weather forecasting | John Smith |
| HIL equipment failure | Medium | High | Spare components inventory, vendor support contract (4-hour response) | David Lee |
| Defect backlog delays exit | Medium | High | Prioritize critical defects, add contractor support if needed | Emily Watson |
| Regulatory delays (DO-178C DER review) | Low | Critical | Early engagement with DER (completed), IV&V reviews (ongoing) | Sarah Chen |
| Beta prototype delivery delay | Medium | Medium | Continue testing with Alpha prototypes, parallel test planning | Emily Watson |
| Key personnel unavailable (illness, leave) | Low | Medium | Cross-training, backup assignments, documentation | Emily Watson |

---

## 11. Approval

| Role | Name | Signature | Date |
|------|------|-----------|------|
| Test Manager | Emily Watson | _/s/ Emily Watson_ | Nov 18, 2025 |
| Program Manager | Sarah Chen | _/s/ Sarah Chen_ | Nov 19, 2025 |
| Quality Assurance | Michael Brown | _/s/ Michael Brown_ | Nov 20, 2025 |
| Customer Representative | Linda Torres | _/s/ Linda Torres_ | Nov 20, 2025 |

---

**Document Control**  
**File**: 31_STP_ADNS.md  
**Classification**: Company Confidential  
**Next Review**: January 15, 2026
