using NeverEatLunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch
{
    /// <summary>
    /// Tabulates the dishes ordered, and whether an invalid order was requested
    /// </summary>
    public class Order
    {
        public Dictionary<int, Dish> OrderedDishes;
        public bool ErrorOccurred;

        public Order()
        {
            OrderedDishes = new Dictionary<int, Dish>();
            ErrorOccurred = false;
        }

        public Order(Dictionary<int, Dish> orderedDishes)
        {
            OrderedDishes = orderedDishes;
            ErrorOccurred = false;

        }

        /// <summary>
        /// MenuItems that have yet to be ordered will add a new dish to the order.
        /// MenuItems that have been ordered will increase the Count of the ordered dish, if allowed.
        /// Ordering another dish that can't be ordered anymore will set ErrorOccurred.
        /// </summary>
        /// <param name="dishNumber">Parsed argument from user input</param>
        /// <param name="menuItem">MenuItem corresponding to dishNumber in Menu used</param>
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
