# Failure Modes and Effects Analysis (FMEA)

**Project**: Autonomous Drone Navigation System (ADNS)  
**Document**: 40_FMEA_ADNS  
**Version**: 1.1.0  
**Date**: October 30, 2025  
**Status**: Approved

---

## 1. Introduction

### 1.1 Purpose

This Failure Modes and Effects Analysis (FMEA) identifies potential failure modes in the ADNS, evaluates their effects on system operation and safety, and recommends mitigations.

### 1.2 Scope

Analysis covers:
- Sensor subsystem (LiDAR, camera, IMU)
- Navigation subsystem (perception, path planning, state estimation)
- Communication subsystem (radio link, ground station)
- Power subsystem (battery, power distribution)
- Flight control subsystem (autopilot interface)

### 1.3 Standards

- MIL-STD-1629A: Procedures for Performing FMEA
- SAE ARP4761: Guidelines for Conducting FMEA on Civil Airborne Systems
- ISO 26262: Functional Safety (ASIL-B target)

---

## 2. Risk Priority Number (RPN) Calculation

**RPN = Severity × Occurrence × Detection**

### 2.1 Severity Scale (1-10)

| Rating | Description | Effect |
|--------|-------------|--------|
| **10** | Catastrophic | Loss of life, system destroyed |
| **9** | Critical | Severe injury, major system damage |
| **8** | Serious | Minor injury, significant mission failure |
| **7** | Major | System degraded, mission compromised |
| **6** | Moderate | System performance reduced |
| **5** | Minor | Slight performance reduction |
| **3-4** | Low | Minimal effect |
| **1-2** | Negligible | No effect |

### 2.2 Occurrence Scale (1-10)

| Rating | Description | Probability |
|--------|-------------|-------------|
| **10** | Very High | > 1 in 10 missions |
| **8-9** | High | 1 in 100 missions |
| **6-7** | Moderate | 1 in 1,000 missions |
| **4-5** | Low | 1 in 10,000 missions |
| **2-3** | Remote | 1 in 100,000 missions |
| **1** | Nearly Impossible | < 1 in 1,000,000 missions |

### 2.3 Detection Scale (1-10)

| Rating | Description | Detection Method |
|--------|-------------|------------------|
| **10** | None | No detection method |
| **8-9** | Very Low | Detected after failure impact |
| **6-7** | Low | Detected during operation |
| **4-5** | Moderate | Automatic detection, operator action required |
| **2-3** | High | Automatic detection and mitigation |
| **1** | Very High | Failure prevented by design |

---

## 3. Sensor Subsystem FMEA

### 3.1 LiDAR Failures

| ID | Component | Failure Mode | Effect | Cause | Severity | Occurrence | Detection | RPN | Mitigation |
|----|-----------|--------------|--------|-------|----------|------------|-----------|-----|------------|
| **FM-001** | LiDAR | Complete failure (no data) | Loss of primary obstacle detection, degraded to camera-only (90% accuracy) | Power loss, hardware fault, driver crash | 7 | 4 | 3 | **84** | Redundant sensors (camera backup), watchdog monitors LiDAR heartbeat, automatic failover |
| **FM-002** | LiDAR | Noisy data (>20% outliers) | False obstacle detections, path planning inefficient | EMI, sensor degradation, rain | 5 | 6 | 4 | **120** | Outlier filtering (RANSAC), sensor fusion weights, data validation |
| **FM-003** | LiDAR | Reduced range (< 30m) | Late obstacle detection, reduced reaction time | Rain, fog, sensor aging | 7 | 7 | 5 | **245** | **HIGH RISK** - Slow down when range degrades, weather detection, proactive maintenance |
| **FM-004** | LiDAR | Intermittent dropouts | Perception gaps, jerky navigation | Loose connector, cable damage | 6 | 5 | 4 | **120** | Connector locking, cable strain relief, periodic inspection |

### 3.2 Camera Failures

| ID | Component | Failure Mode | Effect | Cause | Severity | Occurrence | Detection | RPN | Mitigation |
|----|-----------|--------------|--------|-------|----------|------------|-----------|-----|------------|
| **FM-010** | Camera | Complete failure (no images) | Loss of visual navigation, degraded to LiDAR-only | Hardware fault, driver crash, lens obstruction | 6 | 4 | 3 | **72** | Sensor fusion (LiDAR backup), image validity check, automatic failover |
| **FM-011** | Camera | Overexposure (bright sun) | Vision algorithms fail, no feature detection | Automatic exposure failure, sun in FOV | 6 | 7 | 5 | **210** | **MODERATE RISK** - Adaptive exposure, sun detection, HDR processing, LiDAR primary in bright conditions |
| **FM-012** | Camera | Underexposure (low light) | Vision algorithms fail, no feature detection | Low light (< 10 lux), exposure settings | 6 | 6 | 4 | **144** | IR camera (future), low-light optimization, LiDAR primary at night |
| **FM-013** | Camera | Motion blur | Poor feature tracking, state estimation errors | High speed, vibration, exposure too long | 5 | 5 | 6 | **150** | Shorter exposure, mechanical dampening, optical stabilization (gimbal) |

### 3.3 IMU Failures

| ID | Component | Failure Mode | Effect | Cause | Severity | Occurrence | Detection | RPN | Mitigation |
|----|-----------|--------------|--------|-------|----------|------------|-----------|-----|------------|
| **FM-020** | IMU | Gyro bias drift | State estimation error accumulates over time, position error grows | Temperature change, sensor aging | 6 | 5 | 5 | **150** | Gyro calibration, temperature compensation, sensor fusion with visual odometry |
| **FM-021** | IMU | Accelerometer bias | Velocity estimation error, position drift | Vibration, temperature, aging | 5 | 4 | 5 | **100** | Calibration, vibration isolation, Kalman filter tuning |
| **FM-022** | IMU | Complete failure | Loss of attitude estimation, flight unstable | Hardware fault, power loss | 9 | 2 | 2 | **36** | Redundant IMU in autopilot, watchdog, emergency landing |

---

## 4. Navigation Subsystem FMEA

### 4.1 Perception Failures

| ID | Component | Failure Mode | Effect | Cause | Severity | Occurrence | Detection | RPN | Mitigation |
|----|-----------|--------------|--------|-------|----------|------------|-----------|-----|------------|
| **FM-030** | Perception | False negative (obstacle missed) | Collision with obstacle | Sensor blind spot, algorithm failure, small obstacle | **10** | 3 | 7 | **210** | **MODERATE RISK** - Multi-sensor fusion, conservative obstacle buffer (2m clearance), pre-flight checks |
| **FM-031** | Perception | False positive (phantom obstacle) | Path planning detour, mission inefficiency | Sensor noise, shadows, reflections | 4 | 6 | 8 | **192** | Confidence thresholds, multi-frame persistence, sensor fusion voting |
| **FM-032** | Perception | Delayed detection (> 200ms) | Insufficient reaction time at high speed | CPU overload, algorithm complexity | 8 | 4 | 4 | **128** | Performance optimization, GPU acceleration, watchdog on processing time |

### 4.2 Path Planning Failures

| ID | Component | Failure Mode | Effect | Cause | Severity | Occurrence | Detection | RPN | Mitigation |
|----|-----------|--------------|--------|-------|----------|------------|-----------|-----|------------|
| **FM-040** | Path Planner | No solution found | Drone stops, mission fails | Complex environment, algorithm limitation | 6 | 4 | 3 | **72** | Fallback to simpler planner, manual override, operator decision |
| **FM-041** | Path Planner | Suboptimal path (oscillation) | Mission time increases, jerky flight | Algorithm tuning, dynamic obstacles | 4 | 5 | 6 | **120** | Path smoothing, hysteresis in planning, parameter tuning |
| **FM-042** | Path Planner | Computation timeout (> 500ms) | Delayed response to obstacles | CPU overload, complex scenario | 7 | 3 | 4 | **84** | Anytime algorithm (returns best solution so far), timeout warnings |

### 4.3 State Estimation Failures

| ID | Component | Failure Mode | Effect | Cause | Severity | Occurrence | Detection | RPN | Mitigation |
|----|-----------|--------------|--------|-------|----------|------------|-----------|-----|------------|
| **FM-050** | State Estimator | Position drift (> 1m) | Waypoint navigation inaccurate, collision risk | Sensor errors accumulate, poor sensor fusion | 7 | 5 | 5 | **175** | Periodic re-localization (visual landmarks), loop closure, tighter sensor fusion |
| **FM-051** | State Estimator | Covariance collapse (overconfident) | Ignores sensor updates, diverges from truth | Filter tuning error, numerical issues | 8 | 3 | 6 | **144** | Covariance monitoring, filter health checks, periodic resets |
| **FM-052** | State Estimator | Filter divergence | Position estimate wildly wrong, mission abort | Bad sensor data, initialization error | 9 | 2 | 3 | **54** | Sanity checks on estimates, divergence detection, automatic reset |

---

## 5. Communication Subsystem FMEA

### 5.1 Radio Link Failures

| ID | Component | Failure Mode | Effect | Cause | Severity | Occurrence | Detection | RPN | Mitigation |
|----|-----------|--------------|--------|-------|----------|------------|-----------|-----|------------|
| **FM-060** | Radio | Complete loss (> 10 sec) | Auto return-to-home triggered, mission aborted | Out of range, interference, hardware fault | 6 | 5 | 2 | **60** | Automatic return-to-home, link margin monitoring, range warnings |
| **FM-061** | Radio | High latency (> 500ms) | Delayed telemetry, sluggish manual override | Interference, congestion, poor signal | 5 | 6 | 4 | **120** | Latency monitoring, frequency hopping, link quality display |
| **FM-062** | Radio | Packet loss (> 10%) | Telemetry gaps, choppy video | Interference, multipath, range | 4 | 7 | 5 | **140** | Forward error correction, retransmission, graceful degradation |

### 5.2 Ground Station Failures

| ID | Component | Failure Mode | Effect | Cause | Severity | Occurrence | Detection | RPN | Mitigation |
|----|-----------|--------------|--------|-------|----------|------------|-----------|-----|------------|
| **FM-070** | Ground Station | Software crash | Loss of monitoring, manual override unavailable | Software bug, OS crash | 7 | 3 | 3 | **63** | Backup controller (RC transmitter), automatic flight termination after 30 sec |
| **FM-071** | Ground Station | Display freeze | Operator loses situational awareness | GUI bug, GPU hang | 6 | 4 | 5 | **120** | Watchdog restarts GUI, audio alerts continue, telemetry logging independent |

---

## 6. Power Subsystem FMEA

### 6.1 Battery Failures

| ID | Component | Failure Mode | Effect | Cause | Severity | Occurrence | Detection | RPN | Mitigation |
|----|-----------|--------------|--------|-------|----------|------------|-----------|-----|------------|
| **FM-080** | Battery | Rapid discharge (< 20% remaining) | Auto return-to-home, mission aborted | Battery aging, cold temperature, high load | 6 | 6 | 2 | **72** | Battery health monitoring, conservative capacity thresholds (20% trigger), pre-flight checks |
| **FM-081** | Battery | Cell failure (voltage drop) | Immediate emergency landing | Manufacturing defect, damage, thermal runaway | **9** | 2 | 2 | **36** | Battery management system (BMS), cell voltage monitoring, thermal sensors |
| **FM-082** | Battery | Connector failure (power loss) | Instant power loss, crash | Vibration, corrosion, poor connection | **10** | 2 | 10 | **200** | **MODERATE RISK** - Locking connectors, periodic inspection, redundant connections (future) |

---

## 7. High-Risk Failure Modes (RPN > 200)

| Rank | ID | Failure Mode | RPN | Severity | Action |
|------|----|--------------|-----|----------|--------|
| 1 | **FM-003** | LiDAR reduced range | **245** | 7 | Weather detection algorithm (in progress), speed reduction in degraded conditions, proactive maintenance schedule |
| 2 | **FM-011** | Camera overexposure | **210** | 6 | HDR processing (completed), sun detection (in progress), LiDAR priority mode (completed) |
| 3 | **FM-030** | Perception false negative | **210** | 10 | Multi-sensor fusion (completed), 2m obstacle clearance buffer (completed), pre-flight sensor checks (documented) |
| 4 | **FM-082** | Battery connector failure | **200** | 10 | Locking connectors (retrofitted), inspection checklist (added), redundant power (future upgrade) |

**Risk Reduction Target**: All RPN < 200 by system delivery (Jun 2026)

---

## 8. Mitigation Summary

### 8.1 Design Mitigations (Implemented)

- ✅ Sensor fusion (LiDAR + Camera + IMU) for redundancy
- ✅ Watchdog timers on all critical processes
- ✅ Automatic return-to-home on comm loss
- ✅ Battery management system with cell-level monitoring
- ✅ Geofence enforcement (software + hardware)
- ✅ Emergency landing algorithm
- ✅ Locking connectors (battery, sensors)

### 8.2 Operational Mitigations (Procedures)

- ✅ Pre-flight checklists (30-point checklist)
- ✅ Weather assessment (no-go criteria documented)
- ✅ Operator training (8-hour course + 10 supervised missions)
- ✅ Periodic maintenance schedule (weekly, monthly, annual)
- ✅ Backup controller (RC transmitter) always available

### 8.3 Planned Mitigations (Future Enhancements)

- ⏳ Redundant IMU (under evaluation for v2.0)
- ⏳ IR camera for low-light operation (v2.0)
- ⏳ Redundant battery connections (v2.0)
- ⏳ Parachute system for emergency landing (v3.0)

---

## 9. FMEA Review Schedule

| Review | Date | Attendees | Outcome |
|--------|------|-----------|---------|
| Initial FMEA | Aug 15, 2025 | Systems team, Safety engineer | 65 failure modes identified |
| PDR Update | Sep 30, 2025 | + Customer, IV&V | Added 18 failure modes, updated severities |
| CDR Update | Oct 30, 2025 | + Test team | Finalized RPN rankings, mitigation plan approved |
| **Next Review** | **Jan 31, 2026** | All stakeholders | Post-system test update |

---

## 10. Approval

| Role | Name | Signature | Date |
|------|------|-----------|------|
| Chief Systems Engineer | James Park | _/s/ James Park_ | Oct 28, 2025 |
| Safety Engineer | Dr. Anna Schmidt | _/s/ Dr. Anna Schmidt_ | Oct 29, 2025 |
| Program Manager | Sarah Chen | _/s/ Sarah Chen_ | Oct 30, 2025 |
| Customer Representative | Linda Torres | _/s/ Linda Torres_ | Oct 30, 2025 |

---

**Document Control**  
**File**: 40_FMEA_ADNS.md  
**Classification**: Company Confidential  
**Next Review**: January 31, 2026
