using BSNTNext.Application.Dtos.RoleManagement;
using BSNTNext.Application.Interfaces.Services;
using BSNTNext.Domain.Entity;
using BSNTNext.Infrastructure.Data;
using BSNTNext.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Infrastructure.Services
{
    public class ModuleRoleService : IModuleRoleService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ModuleRoleService(ApplicationDbContext db, UserManager<ApplicationUser> um)
        {
            _db = db;
            _userManager = um;
        }

        public async Task<ModuleRolePageDto> GetPageDataAsync(string areaName)
        {
            var module = await _db.AppModules.FirstOrDefaultAsync(m => m.AreaName == areaName)
                         ?? throw new InvalidOperationException($"Module '{areaName}' not found.");

            var allUsers = await _userManager.Users
                .OrderBy(u => u.FirstName).ThenBy(u => u.LastName).ToListAsync();

            var moduleUserIds = await _db.UserModuleAccesses
                .Where(a => a.ModuleId == module.Id)
                .Select(a => a.UserId)
                .ToListAsync();

            var permissions = await _db.ModulePermissions
                .Where(p => p.ModuleId == module.Id && p.IsActive)
                .OrderBy(p => p.SortOrder)
                .ToListAsync();

            var userDtos = allUsers.Select(u => new RoleUserRowDto
            {
                UserId = u.Id,
                FirstName = u.FirstName,
                Email = u.Email ?? "",
                HasAccess = moduleUserIds.Contains(u.Id)
            }).ToList();

            var permGroups = permissions
                .GroupBy(p => p.GroupName)
                .OrderBy(g => g.Key)
                .Select(g => new RolePermGroupDto
                {
                    GroupName = g.Key,
                    Permissions = g.Select(p => new RolePermDto
                    {
                        Id = p.Id,
                        DisplayName = p.DisplayName,
                        GroupName = p.GroupName,
                        IconClass = p.IconClass,
                        PermissionKey = p.PermissionKey
                    }).ToList()
                }).ToList();

            return new ModuleRolePageDto
            {
                ModuleId = module.Id,
                ModuleDisplayName = module.DisplayName,
                ModuleAreaName = areaName,
                AllUsers = userDtos,
                PermissionGroups = permGroups
            };
        }

        public async Task<List<int>> GetUserPermissionsAsync(string areaName, Guid userId)
        {
            var module = await _db.AppModules.FirstOrDefaultAsync(m => m.AreaName == areaName);
            if (module == null) return new();

            var modulePermIds = await _db.ModulePermissions
                .Where(p => p.ModuleId == module.Id)
                .Select(p => p.Id).ToListAsync();

            return await _db.UserModulePermissions
                .Where(up => up.UserId == userId && modulePermIds.Contains(up.ModulePermissionId))
                .Select(up => up.ModulePermissionId).ToListAsync();
        }

        public async Task<bool> ToggleModuleAccessAsync(string areaName, Guid userId, Guid adminId)
        {
            var module = await _db.AppModules.FirstOrDefaultAsync(m => m.AreaName == areaName);
            if (module == null) return false;

            var existing = await _db.UserModuleAccesses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.ModuleId == module.Id);

            if (existing != null)
            {
                _db.UserModuleAccesses.Remove(existing);
                var permIds = await _db.ModulePermissions
                    .Where(p => p.ModuleId == module.Id).Select(p => p.Id).ToListAsync();
                var userPerms = await _db.UserModulePermissions
                    .Where(up => up.UserId == userId && permIds.Contains(up.ModulePermissionId))
                    .ToListAsync();
                _db.UserModulePermissions.RemoveRange(userPerms);
                await _db.SaveChangesAsync();
                return false;
            }
            else
            {
                _db.UserModuleAccesses.Add(new UserModuleAccess
                {
                    UserId = userId,
                    ModuleId = module.Id,
                    GrantedByUserId = adminId,
                    GrantedAt = DateTime.UtcNow
                });
                await _db.SaveChangesAsync();
                return true;
            }
        }

        public async Task SaveUserPermissionsAsync(
            string areaName, Guid userId, List<int> permissionIds, Guid adminId)
        {
            var module = await _db.AppModules.FirstOrDefaultAsync(m => m.AreaName == areaName);
            if (module == null) return;

            var modulePermIds = await _db.ModulePermissions
                .Where(p => p.ModuleId == module.Id).Select(p => p.Id).ToListAsync();

            var existing = await _db.UserModulePermissions
                .Where(up => up.UserId == userId && modulePermIds.Contains(up.ModulePermissionId))
                .ToListAsync();
            _db.UserModulePermissions.RemoveRange(existing);

            foreach (var permId in permissionIds.Where(p => modulePermIds.Contains(p)))
            {
                _db.UserModulePermissions.Add(new UserModulePermission
                {
                    UserId = userId,
                    ModulePermissionId = permId,
                    GrantedByUserId = adminId,
                    GrantedAt = DateTime.UtcNow
                });
            }
            await _db.SaveChangesAsync();
        }
    }
}
