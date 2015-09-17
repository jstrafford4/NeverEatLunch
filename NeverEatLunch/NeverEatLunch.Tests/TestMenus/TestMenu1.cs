using NeverEatLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch.Tests.TestMenus
{
    public class TestMenu1 : Menu
    {
        private readonly string _name = "test1";

        private readonly Dictionary<int, MenuItem> testMenuItems = new Dictionary<int, MenuItem>
        {
            {1, new MenuItem("singleItem1", false)},
            {2, new MenuItem("singleItem2", false)},
            {3, new MenuItem("multipleItem1", true)}
        };

        public TestMenu1()
        {
            MenuItems = testMenuItems;
            Name = _name;

        }
    }
}
