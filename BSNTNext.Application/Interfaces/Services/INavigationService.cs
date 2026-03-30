using BSNTNext.Application.Dtos.Nav;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Interfaces.Services
{
    public interface INavigationService
    {
        Task<List<NavItemDto>> GetNavItemsAsync(Guid userId, bool isAdmin, string areaName);
        Task SetViewBagAsync(Guid userId, bool isAdmin, string areaName, dynamic viewBag);
        List<NavItemDto> GetFullNavForArea(string areaName);
    }
}
