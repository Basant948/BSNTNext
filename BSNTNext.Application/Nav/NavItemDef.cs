using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Nav
{
    public sealed record NavItemDef(
    string Label,
    string Icon,
    string Url,
    string Group);
}
