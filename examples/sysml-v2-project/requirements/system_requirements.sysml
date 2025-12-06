// SysML v2 System Requirements Example
// Maps to System Requirements documentation (FR-20)

package SystemRequirements {
    
    private import ScalarValues::*;
    private import StakeholderRequirements::*;
    
    // System-level requirements with verification methods
    requirement def SystemRequirement {
        doc /* A derived system requirement with verification approach */
        
        attribute verificationMethod : String;
        attribute verificationCriteria : String;
    }
    
    requirement SYS_REQ_001 : SystemRequirement {
        doc /*
        **SYS-REQ-001: System Uptime**
        
        The system shall maintain 99.9% uptime, measured as:
        (Total Time - Downtime) / Total Time >= 0.999
        
        **Verification Method**: Test
        **Verification Criteria**: Continuous monitoring over 30-day period shows >= 99.9% availability
        */
        
        attribute :>> verificationMethod = "Test";
        attribute :>> verificationCriteria = "30-day monitoring >= 99.9% availability";
        
        // Traceability to stakeholder requirement
        satisfy STK_REQ_001;
    }
    
    requirement SYS_REQ_002 : SystemRequirement {
        doc /*
        **SYS-REQ-002: Response Time**
        
        The system shall respond to user requests within 2 seconds under normal load (< 50% capacity).
        
        **Verification Method**: Test
        **Verification Criteria**: 95th percentile response time <= 2.0 seconds
        */
        
        attribute :>> verificationMethod = "Test";
        attribute :>> verificationCriteria = "95th percentile response time <= 2.0 seconds";
        
        satisfy STK_REQ_002;
    }
    
    requirement SYS_REQ_003 : SystemRequirement {
        doc /*
        **SYS-REQ-003: Authentication**
        
        The system shall require multi-factor authentication for all user access.
        
        **Verification Method**: Inspection and Test
        **Verification Criteria**: Code review confirms MFA implementation; penetration testing validates security
        */
        
        attribute :>> verificationMethod = "Inspection and Test";
        attribute :>> verificationCriteria = "Code review + penetration test";
        
        satisfy STK_REQ_003;
    }
    
    requirement SYS_REQ_004 : SystemRequirement {
        doc /*
        **SYS-REQ-004: Data Encryption**
        
        The system shall encrypt all data at rest using AES-256 and in transit using TLS 1.3.
        
        **Verification Method**: Inspection
        **Verification Criteria**: Configuration review confirms AES-256 and TLS 1.3
        */
        
        attribute :>> verificationMethod = "Inspection";
        attribute :>> verificationCriteria = "Configuration audit";
        
        satisfy STK_REQ_003;
    }
    
    requirement SYS_REQ_005 : SystemRequirement {
        doc /*
        **SYS-REQ-005: Horizontal Scaling**
        
        The system shall support horizontal scaling by adding compute nodes without downtime.
        
        **Verification Method**: Test
        **Verification Criteria**: Load test demonstrates linear scaling from 1K to 100K users
        */
        
        attribute :>> verificationMethod = "Test";
        attribute :>> verificationCriteria = "Load test 1K-100K users";
        
        satisfy STK_REQ_004;
    }
    
    requirement SYS_REQ_006 : SystemRequirement {
        doc /*
        **SYS-REQ-006: Database Replication**
        
        The system shall replicate database across 3+ geographic regions with automatic failover.
        
        **Verification Method**: Test
        **Verification Criteria**: Failover test completes within 30 seconds
        */
        
        attribute :>> verificationMethod = "Test";
        attribute :>> verificationCriteria = "Failover < 30 seconds";
        
        satisfy STK_REQ_001;
    }
}
