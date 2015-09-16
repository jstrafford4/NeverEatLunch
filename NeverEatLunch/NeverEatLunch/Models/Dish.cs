using NeverEatLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch
{
    public class Dish : MenuItem
    {

        public int Count;


        public Dish(string name, bool canHaveMultiple, int count) : base(name, canHaveMultiple)
        {
            Count = count;
        }



        public Dish(MenuItem menuItem) : base(menuItem.Name, menuItem.CanHaveMultiple)
        {
            Count = 0;
        }


    }
}
