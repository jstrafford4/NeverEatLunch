using NeverEatLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch.Tests.TestMenus
{
    class EmptyTestMenu : Menu
    {
        private readonly string _name = "empty";

        private readonly Dictionary<int, MenuItem> testMenuItems = new Dictionary<int, MenuItem>
        {
        };

        public EmptyTestMenu()
        {
            MenuItems = testMenuItems;
            Name = _name;

        }
    }
}
