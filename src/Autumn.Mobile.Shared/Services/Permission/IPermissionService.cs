namespace Autumn.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}