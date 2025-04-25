using DependencyTests.BusinessTests;
using Xunit.Abstractions;

namespace DependencyTests;

/// <summary>
/// Represents a test for a namespace and a list of namespaces it is allowed to reference
/// </summary>
public class DependencyTest : IXunitSerializable
{
    private string[] _forbiddenDependencies;

    // Needed for deserializer, do not use
    public DependencyTest()
    { }

    public DependencyTest(string testedNamespace, IReadOnlyCollection<string> allowedDependencies)
    {
        TestedNamespace = testedNamespace;
        AllowedDependencies = allowedDependencies.ToArray();

        _forbiddenDependencies = FakeProjectNamespaces.All
            .Except(allowedDependencies)
            .Where(dep => dep != TestedNamespace)
            .ToArray();
    }

    public string TestedNamespace { get; private set; }

    public string[] AllowedDependencies { get; private set; }

    public string[] ForbiddenDependencies => _forbiddenDependencies;

    public void Deserialize(IXunitSerializationInfo info)
    {
        TestedNamespace = info.GetValue<string>(nameof(TestedNamespace));
        AllowedDependencies = info.GetValue<string[]>(nameof(AllowedDependencies));
        _forbiddenDependencies = info.GetValue<string[]>(nameof(_forbiddenDependencies));
    }

    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(TestedNamespace), TestedNamespace);
        info.AddValue(nameof(AllowedDependencies), AllowedDependencies);
        info.AddValue(nameof(_forbiddenDependencies), _forbiddenDependencies);
    }

    public override string ToString()
    {
        return TestedNamespace;
    }
}
