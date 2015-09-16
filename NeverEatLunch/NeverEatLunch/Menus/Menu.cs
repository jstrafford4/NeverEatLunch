using NeverEatLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch
{
    public abstract class Menu
    {
        public string Name { get; protected set; }
        public Dictionary<int, MenuItem> MenuItems {get; protected set;}
    }
}
