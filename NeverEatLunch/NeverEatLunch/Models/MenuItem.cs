using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch.Models
{
    /// <summary>
    /// MenuItems are used by Menus
    /// </summary>
    public class MenuItem
    {
        public string Name { get; private set; }

        public bool CanHaveMultiple { get; private set; }


        public MenuItem(string name, bool canHaveMultiple)
        {
            Name = name;
            CanHaveMultiple = canHaveMultiple;
        }

    }
}
