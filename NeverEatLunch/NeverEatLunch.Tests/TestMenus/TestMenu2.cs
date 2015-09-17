using NeverEatLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch.Tests.TestMenus
{
    public class TestMenu2 : Menu
    {
        private readonly string _name = "test2";

        private readonly Dictionary<int, MenuItem> menuItems = new Dictionary<int, MenuItem>
        {
            {1, new MenuItem("singleMenuItem1", false)},
            {2, new MenuItem("multipleMenuItem1", true)},
            {3, new MenuItem("singleMenuItem2", false)},
            {4, new MenuItem("singleMenuItem3", false)}
        };


        public TestMenu2()
        {
            Name = _name;
            MenuItems = menuItems;
        }
    }
}
