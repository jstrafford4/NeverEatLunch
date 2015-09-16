using NeverEatLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch.Menus
{
    public class NightMenu : Menu
    {
        private readonly string _name = "night";

        private readonly Dictionary<int, MenuItem> menuItems = new Dictionary<int, MenuItem>
        {
            {1, new MenuItem("steak", false)},
            {2, new MenuItem("potato", true)},
            {3, new MenuItem("wine", false)},
            {4, new MenuItem("cake", false)}
        };


        public NightMenu()
        {
            Name = _name;
            MenuItems = menuItems;
        }
    }
}
