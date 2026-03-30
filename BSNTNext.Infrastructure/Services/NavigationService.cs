using BSNTNext.Application.Dtos.Nav;
using BSNTNext.Application.Interfaces.Services;
using BSNTNext.Application.Nav;
using BSNTNext.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Infrastructure.Services
{

    public class NavigationService : INavigationService
    {
        private readonly ApplicationDbContext _db;

        public NavigationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<NavItemDto>> GetNavItemsAsync(Guid userId, bool isAdmin, string areaName)
        {
            var navMap = NavRegistry.GetMap(areaName);
            if (navMap == null) return new List<NavItemDto>();

            if (isAdmin)
                return navMap
                    .Select(kv => ToDto(kv.Key, kv.Value))
                    .ToList();

            var prefix = NavRegistry.GetPermKeyPrefix(areaName) ?? string.Empty;

            var moduleId = await _db.AppModules
                .Where(m => m.AreaName == areaName)
                .Select(m => (int?)m.Id)
                .FirstOrDefaultAsync();

            if (moduleId == null) return new List<NavItemDto>();

            var grantedKeys = await _db.UserModulePermissions
                .Where(up => up.UserId == userId)
                .Join(
                    _db.ModulePermissions.Where(p =>
                        p.ModuleId == moduleId &&
                        p.IsActive &&
                        p.PermissionKey.StartsWith(prefix)),
                    up => up.ModulePermissionId,
                    p => p.Id,
                    (up, p) => p.PermissionKey)
                .ToListAsync();

            return grantedKeys
                .Where(k => navMap.ContainsKey(k))
                .OrderBy(k => Array.IndexOf(navMap.Keys.ToArray(), k))
                .Select(k => ToDto(k, navMap[k]))
                .ToList();
        }

        public async Task SetViewBagAsync(Guid userId, bool isAdmin, string areaName, dynamic viewBag)
        {
            viewBag.ModuleNav = await GetNavItemsAsync(userId, isAdmin, areaName);
            viewBag.IsAdmin = isAdmin;
        }

        public List<NavItemDto> GetFullNavForArea(string areaName)
        {
            var navMap = NavRegistry.GetMap(areaName);
            if (navMap == null) return new List<NavItemDto>();
            return navMap.Select(kv => ToDto(kv.Key, kv.Value)).ToList();
        }

        private static NavItemDto ToDto(string key, NavItemDef def) => new()
        {
            Key = key,
            Label = def.Label,
            Icon = def.Icon,
            Url = def.Url,
            Group = def.Group
        };
    }
}