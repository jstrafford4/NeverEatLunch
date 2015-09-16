using NeverEatLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch
{
    public class Order
    {
        public Dictionary<int, Dish> OrderedDishes;
        public bool ErrorOccurred;

        public Order()
        {
            OrderedDishes = new Dictionary<int, Dish>();
            ErrorOccurred = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dishNumber"></param>
        /// <param name="dishToOrder"></param>
        /// <returns>true if dish successfully ordered.  false if there was an error.</returns>
        public void OrderDish(int dishNumber, MenuItem menuItem)
        {
            if (OrderedDishes.ContainsKey(dishNumber))
            {
                if (OrderedDishes[dishNumber].CanHaveMultiple)
                {
                    OrderedDishes[dishNumber].Count++;
                }
                else
                {
                    ErrorOccurred = true;    
                }
            }
            else
            {
                var dishToAdd = new Dish(menuItem);
                dishToAdd.Count = 1;
                OrderedDishes.Add(dishNumber, dishToAdd);
            }
        }
    }
}
