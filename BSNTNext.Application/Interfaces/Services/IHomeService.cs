using BSNTNext.Application.Dtos.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Interfaces.Services
{
    public interface IHomeService
    {
        Task<DashboardResult?> GetDashboardAsync(string userId);
    }
}
