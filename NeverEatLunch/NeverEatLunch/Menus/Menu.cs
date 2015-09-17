using NeverEatLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch
{
    /// <summary>
    /// Each menu should be initialized with hard-coded MenuItems in their class definition file.
    /// Register all Menus being served with the IoC container to pass them in to the order processor.
    /// </summary>
    public abstract class Menu
    {
        /// <summary>
        /// Name corresponds to the first argument of an order input
        /// e.g. "morning, 1, 2" corresponds to the Menu with the Name "morning"
        /// </summary>
        public string Name { get; protected set; }
        public Dictionary<int, MenuItem> MenuItems {get; protected set;}
    }
}
