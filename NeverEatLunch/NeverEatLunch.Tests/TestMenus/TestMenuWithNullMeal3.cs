using NeverEatLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch.Tests.TestMenus
{
    class TestMenuWithNullMeals : Menu
    {
        private readonly string _name = "testMenuWithNullMeal3";

        private readonly Dictionary<int, MenuItem> menuItems = new Dictionary<int, MenuItem>
        {
            {1, null},
            {2, null},
            {3, null},
            {4, null}
        };


        public TestMenuWithNullMeals()
        {
            Name = _name;
            MenuItems = menuItems;
        }
    }
}
