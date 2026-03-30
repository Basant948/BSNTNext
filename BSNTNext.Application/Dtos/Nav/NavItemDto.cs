using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Dtos.Nav
{
    public class NavItemDto
    {
        public string Key { get; set; } = "";
        public string Label { get; set; } = "";
        public string Icon { get; set; } = "";
        public string Url { get; set; } = "";
        public string Group { get; set; } = "";
    }
}
