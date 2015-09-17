using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch
{
    /// <summary>
    /// May want to process orders in a variety of ways.
    /// </summary>
    public interface IOrderProcessor
    {
        string ProcessOrder(string[] orderStrings);
    }
}
