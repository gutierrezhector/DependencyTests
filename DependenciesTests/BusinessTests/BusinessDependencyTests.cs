using Xunit;
using Xunit.Abstractions;

namespace DependencyTests.BusinessTests;

public class BusinessDependencyTests(ITestOutputHelper output)
{
    [Theory(DisplayName = "")]
    [MemberData(nameof(DependenciesTests))]
    public void Should_Not_Have_Forbidden_Dependency(DependencyTest dependencyTest)
    {
        var testedNamespace = dependencyTest.TestedNamespace;
        var forbiddenDependancies = dependencyTest.ForbiddenDependencies;

        dependencyTest.Assert(output, testedNamespace, forbiddenDependancies);
    }

    public static readonly TheoryData<DependencyTest> DependenciesTests =
        new TheoryData<DependencyTest>
        {
            new DependencyTest
            (
                testedNamespace: FakeProjectNamespaces.FakeProject_Application,
                allowedDependencies: new []
                {
                    FakeProjectNamespaces.FakeProject_Infra,
                    FakeProjectNamespaces.FakeProject_Domain,
                }
            ),
            new DependencyTest
            (
                testedNamespace: FakeProjectNamespaces.FakeProject_Infra,
                allowedDependencies: new []
                {
                    FakeProjectNamespaces.FakeProject_Application,
                    FakeProjectNamespaces.FakeProject_Domain,
                }
            ),
            new DependencyTest
            (
                testedNamespace: FakeProjectNamespaces.FakeProject_Domain,
                allowedDependencies: FakeProjectNamespaces.None
            ),
        };
}
