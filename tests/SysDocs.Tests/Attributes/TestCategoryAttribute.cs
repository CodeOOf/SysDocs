using Xunit;

namespace SysDocs.Tests.Attributes;

/// <summary>
/// Categorizes tests for filtering (e.g., Unit, Integration, Determinism)
/// Uses xUnit's Trait system for test filtering
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestCategoryAttribute : Attribute
{
    public string Category { get; }

    public TestCategoryAttribute(string category)
    {
        Category = category;
    }
}

/// <summary>
/// Common test categories
/// </summary>
public static class TestCategories
{
    public const string Unit = "Unit";
    public const string Integration = "Integration";
    public const string System = "System";
    public const string Determinism = "Determinism";
    public const string CrossPlatform = "CrossPlatform";
    public const string Performance = "Performance";
    public const string FileFormat = "FileFormat";
}
