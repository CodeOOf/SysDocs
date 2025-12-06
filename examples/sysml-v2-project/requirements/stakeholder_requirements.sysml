// SysML v2 Stakeholder Requirements Example
// Maps to Stakeholder Requirements documentation (FR-20)

package StakeholderRequirements {
    
    import ScalarValues::*;
    
    // Stakeholder needs and expectations
    requirement def StakeholderNeed {
        doc /* A high-level need expressed by stakeholders */
        
        attribute priority : String;
        attribute source : String;
        attribute rationale : String;
    }
    
    // Concrete stakeholder requirements
    requirement STK_REQ_001 : StakeholderNeed {
        doc /*
        **STK-REQ-001: System Availability**
        
        The system shall be available for use 99.9% of the time during operational hours.
        
        **Priority**: High
        **Source**: Chief Operations Officer
        **Rationale**: Minimal downtime is critical for business continuity.
        */
        
        attribute priority = "High";
        attribute source = "Chief Operations Officer";
        attribute rationale = "Minimal downtime is critical for business continuity";
    }
    
    requirement STK_REQ_002 : StakeholderNeed {
        doc /*
        **STK-REQ-002: User-Friendly Interface**
        
        The system shall provide an intuitive user interface that requires minimal training.
        
        **Priority**: Medium
        **Source**: End User Representative
        **Rationale**: Reduce training costs and improve user adoption.
        */
        
        attribute priority = "Medium";
        attribute source = "End User Representative";
        attribute rationale = "Reduce training costs and improve user adoption";
    }
    
    requirement STK_REQ_003 : StakeholderNeed {
        doc /*
        **STK-REQ-003: Data Security**
        
        The system shall protect sensitive data from unauthorized access.
        
        **Priority**: Critical
        **Source**: Chief Information Security Officer
        **Rationale**: Compliance with data protection regulations and customer trust.
        */
        
        attribute priority = "Critical";
        attribute source = "Chief Information Security Officer";
        attribute rationale = "Compliance with data protection regulations";
    }
    
    requirement STK_REQ_004 : StakeholderNeed {
        doc /*
        **STK-REQ-004: Scalability**
        
        The system shall support growth from 1,000 to 100,000 concurrent users.
        
        **Priority**: High
        **Source**: Chief Technology Officer
        **Rationale**: Support business growth over 5-year horizon.
        */
        
        attribute priority = "High";
        attribute source = "Chief Technology Officer";
        attribute rationale = "Support business growth over 5-year horizon";
    }
}
