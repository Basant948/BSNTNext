using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Domain.Entity
{
    public class SeedHistory
    {
        public int Id { get; set; }
        public string SeedKey { get; set; } = null!;
        public DateTime AppliedAt { get; set; }
        public string? Notes { get; set; }
    }
}
