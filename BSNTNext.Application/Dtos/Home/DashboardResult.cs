using BSNTNext.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Dtos.Home
{
    public class DashboardResult
    {
        public string UserFullName { get; set; } = string.Empty;
        public List<AppModule> Modules { get; set; } = new();
    }
}
