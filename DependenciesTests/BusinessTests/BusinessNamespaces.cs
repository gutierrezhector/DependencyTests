namespace DependencyTests.BusinessTests;

internal static class FakeProjectNamespaces
{
    public const string FakeProject_Web = "FakeProject.Web";
    public const string FakeProject_Infra = "FakeProject.Infra";
    public const string FakeProject_Application = "FakeProject.Application";
    public const string FakeProject_Domain = "FakeProject.Domain";
    public static IReadOnlyCollection<string> All => _all;

    public static IReadOnlyCollection<string> None = new List<string>();

    private static readonly List<string> _all =
    [
        FakeProject_Web,
        FakeProject_Infra,
        FakeProject_Application,
        FakeProject_Domain
    ];
}
