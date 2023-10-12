namespace MyOrg.WebApp.AppUtils;

public interface IContext
{
    string TenantName { get; set; }
    string UserName { get; set; }
}