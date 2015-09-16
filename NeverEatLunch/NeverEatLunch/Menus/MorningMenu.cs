using NeverEatLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch.Menus
{
    public class MorningMenu : Menu
    {
        private readonly string _name = "morning";

        private readonly Dictionary<int, MenuItem> morningDishes = new Dictionary<int, MenuItem>
        {
            {1, new MenuItem("eggs", false)},
            {2, new MenuItem("toast", false)},
            {3, new MenuItem("coffee", true)}
        };

        public MorningMenu()
        {
            MenuItems = morningDishes;
            Name = _name;

        }
    }
}
