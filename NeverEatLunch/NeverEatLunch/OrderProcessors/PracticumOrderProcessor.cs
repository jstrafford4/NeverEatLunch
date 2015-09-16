using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch
{
    public class PracticumOrderProcessor : IOrderProcessor
    {
        private IEnumerable<Menu> _menus;


        public PracticumOrderProcessor(IEnumerable<Menu> menus)
        {
            this._menus = menus;
        }


        public virtual string ProcessOrder(string[] orderStrings)
        {
            Order order = FormOrder(orderStrings);
            return PrintOrder(order);
        }

        public virtual Order FormOrder(string[] orderStrings)
        {
            Order currentOrder = new Order();
            var currentMenu = _menus.FirstOrDefault(menu => string.Equals(orderStrings[0], menu.Name, StringComparison.CurrentCultureIgnoreCase));
            if (currentMenu == null)
            {
                currentOrder.ErrorOccurred = true;
                return currentOrder;
            }
            for (int i = 1; i < orderStrings.Length; i++)
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
                    currentOrder.OrderDish(dishNumber, currentDish);
                    if(currentOrder.ErrorOccurred)
                    {
                        break;
                    }
                }
                else //couldn't parse integer from the orderString
                {
                    currentOrder.ErrorOccurred = true;
                    break;
                }
            }

            return currentOrder;

        }

        public virtual string PrintOrder(Order order)
        {
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
