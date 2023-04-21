namespace ProjectCMS.Core.Services.Interfaces
{
    public interface IRoleService
    {
        bool CheckRole(int[] roleId, string userId);
    }
}
