using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using NeverEatLunch.Tests.TestMenus;
using System.Collections.Generic;
using NeverEatLunch.Models;

namespace NeverEatLunch.Tests
{

    [TestFixture]
    public class OrderTests
    {
        private Menu testMenu;
        private Order sut;

        [TestFixtureSetUp]
        public void Init()
        {
            testMenu = new TestMenu1();
        }

        [SetUp]
	    public void RunBeforeAnyTests()
	    {
	      sut = new Order();
	    }

        [Test]
        public void Ordering_Each_Dish_Should_Be_Ok()
        {
            foreach(KeyValuePair<int, MenuItem> entry in testMenu.MenuItems)
            {
                sut.OrderDish(entry.Key, entry.Value);
            }
            NUnit.Framework.Assert.IsFalse(sut.ErrorOccurred);
        }

        [Test]
        public void Multiple_Unique_Dishes_Should_Cause_Error()
        {
            MenuItem singleMenuItem = new MenuItem("singleMenuItem", false);
            sut.OrderDish(1, singleMenuItem);
            NUnit.Framework.Assert.IsFalse(sut.ErrorOccurred);
            sut.OrderDish(1, singleMenuItem);
            NUnit.Framework.Assert.IsTrue(sut.ErrorOccurred);
        }

        [Test]
        public void Multiple_CanHaveMultiple_Dishes_Should_Be_Ok()
        {
            MenuItem multipleMenuItem = new MenuItem("multipleMenuItem", true);
            sut.OrderDish(1, multipleMenuItem);
            sut.OrderDish(1, multipleMenuItem);
            NUnit.Framework.Assert.IsFalse(sut.ErrorOccurred);
        }

        [Test]
        public void Dish_Count_Should_Be_Correct_Before_Error()
        {
            var singleItem1 = testMenu.MenuItems[1];
            var singleItem2 = testMenu.MenuItems[2];
            var multipleItem1 = testMenu.MenuItems[3];

            sut.OrderDish(1, singleItem1);
            sut.OrderDish(2, singleItem2);
            sut.OrderDish(3, multipleItem1);
            sut.OrderDish(3, multipleItem1);

            NUnit.Framework.Assert.That(sut.OrderedDishes[1].Count, Is.EqualTo(1));
            NUnit.Framework.Assert.That(sut.OrderedDishes[2].Count, Is.EqualTo(1));
            NUnit.Framework.Assert.That(sut.OrderedDishes[3].Count, Is.EqualTo(2));

        }

        [Test]
        public void Dish_Count_Should_Be_Correct_After_Errors()
        {
            var singleItem1 = testMenu.MenuItems[1];
            var singleItem2 = testMenu.MenuItems[2];
            var multipleItem1 = testMenu.MenuItems[3];

            sut.OrderDish(1, singleItem1);
            sut.OrderDish(2, singleItem2);
            sut.OrderDish(3, multipleItem1);
            sut.OrderDish(3, multipleItem1);

            sut.OrderDish(1, singleItem1);
            sut.OrderDish(2, singleItem2);

            NUnit.Framework.Assert.That(sut.OrderedDishes[1].Count, Is.EqualTo(1));
            NUnit.Framework.Assert.That(sut.OrderedDishes[2].Count, Is.EqualTo(1));
            NUnit.Framework.Assert.That(sut.OrderedDishes[3].Count, Is.EqualTo(2));

        }

    }
}
