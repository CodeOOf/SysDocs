namespace SysDocs.Tests.Attributes;

/// <summary>
/// Marks a test as verifying a specific requirement from REQUIREMENTS.md
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RequirementTestAttribute : Attribute
{
    /// <summary>
    /// The requirement ID being tested (e.g., "FR-01", "NFR-02")
    /// </summary>
    public string RequirementId { get; }

    /// <summary>
    /// The type of test (Unit, Integration, System)
    /// </summary>
    public TestType TestType { get; }

    /// <summary>
    /// Optional description of what aspect of the requirement is being verified
    /// </summary>
    public string? Description { get; set; }

    public RequirementTestAttribute(string requirementId, TestType testType = TestType.Unit)
    {
        RequirementId = requirementId;
        TestType = testType;
    }
}

/// <summary>
/// Types of tests in the V&V hierarchy
/// </summary>
public enum TestType
{
    /// <summary>Unit test - tests individual component in isolation</summary>
    Unit,
    
    /// <summary>Integration test - tests multiple components working together</summary>
    Integration,
    
    /// <summary>System test - tests complete end-to-end workflow</summary>
    System,
    
    /// <summary>Acceptance test - validates against stakeholder requirements</summary>
    Acceptance
}
