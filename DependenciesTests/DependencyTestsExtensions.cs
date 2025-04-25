using DependencyTests.BusinessTests;
using FluentAssertions.Execution;
using NetArchTest.Rules;
using Xunit.Abstractions;
using RuleTypes = NetArchTest.Rules.Types;

namespace DependencyTests;

internal static class DependencyTestsExtensions
{
    public static void Assert(this DependencyTest dependencyTest, ITestOutputHelper output, string testedNamespace, string[] forbiddenDependancies)
    {
        // needed so InCurrentDomain() can find assembly types
        ExplicitlyLoadAssemblies();

        var types = RuleTypes
            .InCurrentDomain()
            .That()
            .ResideInNamespace(testedNamespace);

        var tmp = types.GetTypes().ToList();
        
        if (!types.GetTypes().Any())
        {
            output.WriteLine(@"Nothing to test");
            return;
        }

        types.Should()
            .NotHaveDependencyOnAny(forbiddenDependancies)
            .GetResultAndDisplayTestSummary(dependencyTest, output);
    }

    private static void ExplicitlyLoadAssemblies()
    {
        foreach (var fakeProjectNamespaces in FakeProjectNamespaces.All)
        {
            AppDomain.CurrentDomain.Load(fakeProjectNamespaces);
        }
    }

    private static void GetResultAndDisplayTestSummary(
            this ConditionList subject,
            DependencyTest dependenciesTest,
            ITestOutputHelper output)
    {
        var testedNamespace = dependenciesTest.TestedNamespace;
        var forbiddenDependencies = dependenciesTest.ForbiddenDependencies;
        var result = subject.GetResult();

        if (result.IsSuccessful)
        {
            var allowedDependencies = dependenciesTest.AllowedDependencies;
            var allowedDependenciesSentence = allowedDependencies.Length != 0 ? $"\nit has dependency only on :\n\t{string.Join("\n\t", allowedDependencies)}\n" : "";

            output.WriteLine(
                $"Namespace tested :\n\t{testedNamespace}\n" +
                $"{allowedDependenciesSentence}" +
                $"\nit has no dependency on :\n\t{string.Join("\n\t", forbiddenDependencies)}");

            return;
        }

        var failingTypeNames = result.FailingTypes!.Select(t => t.FullName);

        Execute.Assertion
            .Given(() => result)
            .ForCondition(result => result.IsSuccessful)
            .FailWith(
                $"Namespace tested :\n\t{testedNamespace}\n" +
                $"\nthese types use forbidden dependencies :\n\t{string.Join("\n\t", failingTypeNames)}\n" +
                $"\nforbidden dependencies to reference are :\n\t{string.Join("\n\t", forbiddenDependencies)}"
            );
    }
}
