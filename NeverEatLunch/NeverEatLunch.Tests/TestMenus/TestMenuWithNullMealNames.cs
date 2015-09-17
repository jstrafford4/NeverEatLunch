using NeverEatLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch.Tests.TestMenus
{
    class TestMenuWithNullMealNames : Menu
    {
        private readonly string _name = "testMenuWithNullMeal3";

        private readonly Dictionary<int, MenuItem> menuItems = new Dictionary<int, MenuItem>
        {
            {1, new MenuItem(null, false)},
            {2, new MenuItem(null, true)},
            {3, new MenuItem(null, true)},
            {4, new MenuItem(null, false)}
        };


        public TestMenuWithNullMealNames()
        {
            Name = _name;
            MenuItems = menuItems;
        }
    }
}
