using System.Collections.ObjectModel;

namespace AACSB.WebApi.Shared.Authorization;

public static class AACSBAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class AACSBResource
{
    public const string Tenants = nameof(Tenants);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Brands = nameof(Brands);
}

public static class AACSBPermissions
{
    private static readonly AACSBPermission[] _all = new AACSBPermission[]
    {
        new("View Dashboard", AACSBAction.View, AACSBResource.Dashboard),
        new("View Hangfire", AACSBAction.View, AACSBResource.Hangfire),
        new("View Users", AACSBAction.View, AACSBResource.Users),
        new("Search Users", AACSBAction.Search, AACSBResource.Users),
        new("Create Users", AACSBAction.Create, AACSBResource.Users),
        new("Update Users", AACSBAction.Update, AACSBResource.Users),
        new("Delete Users", AACSBAction.Delete, AACSBResource.Users),
        new("Export Users", AACSBAction.Export, AACSBResource.Users),
        new("View UserRoles", AACSBAction.View, AACSBResource.UserRoles),
        new("Update UserRoles", AACSBAction.Update, AACSBResource.UserRoles),
        new("View Roles", AACSBAction.View, AACSBResource.Roles),
        new("Create Roles", AACSBAction.Create, AACSBResource.Roles),
        new("Update Roles", AACSBAction.Update, AACSBResource.Roles),
        new("Delete Roles", AACSBAction.Delete, AACSBResource.Roles),
        new("View RoleClaims", AACSBAction.View, AACSBResource.RoleClaims),
        new("Update RoleClaims", AACSBAction.Update, AACSBResource.RoleClaims),
        new("View Products", AACSBAction.View, AACSBResource.Products, IsBasic: true),
        new("Search Products", AACSBAction.Search, AACSBResource.Products, IsBasic: true),
        new("Create Products", AACSBAction.Create, AACSBResource.Products),
        new("Update Products", AACSBAction.Update, AACSBResource.Products),
        new("Delete Products", AACSBAction.Delete, AACSBResource.Products),
        new("Export Products", AACSBAction.Export, AACSBResource.Products),
        new("View Brands", AACSBAction.View, AACSBResource.Brands, IsBasic: true),
        new("Search Brands", AACSBAction.Search, AACSBResource.Brands, IsBasic: true),
        new("Create Brands", AACSBAction.Create, AACSBResource.Brands),
        new("Update Brands", AACSBAction.Update, AACSBResource.Brands),
        new("Delete Brands", AACSBAction.Delete, AACSBResource.Brands),
        new("Generate Brands", AACSBAction.Generate, AACSBResource.Brands),
        new("Clean Brands", AACSBAction.Clean, AACSBResource.Brands),
        new("View Tenants", AACSBAction.View, AACSBResource.Tenants, IsRoot: true),
        new("Create Tenants", AACSBAction.Create, AACSBResource.Tenants, IsRoot: true),
        new("Update Tenants", AACSBAction.Update, AACSBResource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", AACSBAction.UpgradeSubscription, AACSBResource.Tenants, IsRoot: true)
    };

    public static IReadOnlyList<AACSBPermission> All { get; } = new ReadOnlyCollection<AACSBPermission>(_all);
    public static IReadOnlyList<AACSBPermission> Root { get; } = new ReadOnlyCollection<AACSBPermission>(_all.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<AACSBPermission> Admin { get; } = new ReadOnlyCollection<AACSBPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<AACSBPermission> Basic { get; } = new ReadOnlyCollection<AACSBPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record AACSBPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
