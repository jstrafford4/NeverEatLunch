using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch
{
    public class PracticumOrderProcessor : IOrderProcessor
    {

        /// <summary>
        /// All menus being served.
        /// Users order off of a menu by using the menu's Name as the first argument.
        /// </summary>
        private IEnumerable<Menu> _menus;


        public PracticumOrderProcessor(IEnumerable<Menu> menus)
        {
            this._menus = menus;
        }

        /// <summary>
        /// Form an order from the array of input argument strings
        /// Print out the string summarizing that order.
        /// </summary>
        /// <param name="orderStrings">input arguments.  first argument is the menu name, all other arguments are dish numbers.</param>
        /// <returns>string summarizing the best attempt to process the input arguments</returns>
        public virtual string ProcessOrder(string[] orderStrings)
        {
            Order order = FormOrder(orderStrings);
            return PrintOrder(order);
        }

        /// <summary>
        /// Takes in the input arguments, and adds Dishes to an Order until done or error.
        /// </summary>
        /// <param name="orderStrings">input arguments.  first argument is the menu name, all other arguments are dish numbers.</param>
        /// <returns>A new Order object, with tabulated dish counts for each dish ordered, and error information.</returns>
        public virtual Order FormOrder(string[] orderStrings)
        {
            Order currentOrder = new Order();
            var currentMenu = _menus.FirstOrDefault(menu => string.Equals(orderStrings[0], menu.Name, StringComparison.CurrentCultureIgnoreCase));
            if (currentMenu == null)
            {
                currentOrder.ErrorOccurred = true;
                return currentOrder;
            }
            for (int i = 1; ((i < orderStrings.Length) && (!currentOrder.ErrorOccurred)); i++)
            {
                var dishNumber = 0;
                var trimmedArgument = orderStrings[i].Trim();

                if (Int32.TryParse(trimmedArgument, out dishNumber))
                {
                    if (!currentMenu.MenuItems.ContainsKey(dishNumber))
                    {
                        currentOrder.ErrorOccurred = true;
                        break;
                    }

                    var currentDish = currentMenu.MenuItems[dishNumber];
                    if(currentDish == null || currentDish.Name == null)
                    {
                        currentOrder.ErrorOccurred = true;
                        break;
                    }
                    currentOrder.OrderDish(dishNumber, currentDish);
                }
                else //couldn't parse integer from the orderString
                {
                    currentOrder.ErrorOccurred = true;
                }
            }

            return currentOrder;

        }

        /// <summary>
        /// Gets the string summary of the order.  Doesn't actually "print" it to an output.
        /// </summary>
        /// <param name="order">The Order object to print out</param>
        /// <returns>The string summary of the order.</returns>
        public virtual string PrintOrder(Order order)
        {
            if (order == null) return "error";

            var dishStrings = new List<string>();
            List<int> dishKeys = order.OrderedDishes.Keys.ToList();
            dishKeys.Sort();

            for (int i = 0; i < dishKeys.Count(); i++)
            {
                var currentKey = dishKeys[i];
                var currentDish = order.OrderedDishes[currentKey];
                if (currentDish == null) continue;
                if (currentDish.Count > 0)
                {
                    var dishStringToAdd = currentDish.Name;

                    if (currentDish.Count > 1)
                    {
                        dishStringToAdd += "(x" + currentDish.Count + ")";
                    }
                    dishStrings.Add(dishStringToAdd);
                }
            }
            if (order.ErrorOccurred)
            {
                dishStrings.Add("error");
            }

            var joinedString = string.Join(", ", dishStrings.ToArray());
            return joinedString;
        }


    }
}
