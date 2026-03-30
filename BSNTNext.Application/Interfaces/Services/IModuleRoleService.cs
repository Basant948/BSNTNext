using BSNTNext.Application.Dtos.RoleManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Interfaces.Services
{
    public interface IModuleRoleService
    {
        Task<ModuleRolePageDto> GetPageDataAsync(string areaName);
        Task<List<int>> GetUserPermissionsAsync(string areaName, Guid userId);
        Task<bool> ToggleModuleAccessAsync(string areaName, Guid userId, Guid adminId);
        Task SaveUserPermissionsAsync(string areaName, Guid userId, List<int> permissionIds, Guid adminId);
    }
}
