using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Autofac;
using NeverEatLunch.Tests.TestMenus;
using System.Collections.Generic;

namespace NeverEatLunch.Tests
{
    [TestFixture]
    public class FormOrderTests
    {
        private PracticumOrderProcessor sut;
        private static IContainer Container { get; set; }
        
        [TestFixtureSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            //TestMenu1 MUST be registered for tests below to run
            builder.RegisterType<TestMenu1>().As<Menu>();
            builder.RegisterType<TestMenu2>().As<Menu>();
            builder.RegisterType<EmptyTestMenu>().As<Menu>();
            builder.RegisterType<TestMenuWithNullMeals>().As<Menu>();
            builder.RegisterType<TestMenuWithNullMealNames>().As<Menu>();
            builder.RegisterType<PracticumOrderProcessor>().As<PracticumOrderProcessor>();

            Container = builder.Build();

        }

        [SetUp]
        public void RunBeforeAnyTests()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // When processor is resolved, it'll have all of the
                // registered handlers passed in to the constructor.
                sut = scope.Resolve<PracticumOrderProcessor>();
            }
        }


        [Test]
        public void TestMenu1_Standard_Order_Should_Form_Valid_Order()
        {
            var testMenu = new TestMenu1();
            string[] orderStrings = {testMenu.Name, "1", "2", "3"};

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsFalse(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.That(testOrder.OrderedDishes[1].Name, Is.EqualTo(testMenu.MenuItems[1].Name));
            NUnit.Framework.Assert.That(testOrder.OrderedDishes[2].Name, Is.EqualTo(testMenu.MenuItems[2].Name));
            NUnit.Framework.Assert.That(testOrder.OrderedDishes[3].Name, Is.EqualTo(testMenu.MenuItems[3].Name));

        }

        [Test]
        public void Should_Form_Order_Until_Error_Then_Stop()
        {
            var testMenu = new TestMenu1();
            string[] orderStrings = { testMenu.Name, "1", "2", "1", "3" };

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.That(testOrder.OrderedDishes[1].Count, Is.EqualTo(1));
            NUnit.Framework.Assert.That(testOrder.OrderedDishes[2].Count, Is.EqualTo(1));
            NUnit.Framework.Assert.IsFalse(testOrder.OrderedDishes.ContainsKey(3));
        }

        [Test]
        public void Should_Trim_WhiteSpace_For_OrderNumbers()
        {
            var testMenu = new TestMenu1();
            string[] orderStrings = { testMenu.Name, " 1   ", "2 ", "  3" };

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsFalse(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.That(testOrder.OrderedDishes[1].Name, Is.EqualTo(testMenu.MenuItems[1].Name));
            NUnit.Framework.Assert.That(testOrder.OrderedDishes[2].Name, Is.EqualTo(testMenu.MenuItems[2].Name));
            NUnit.Framework.Assert.That(testOrder.OrderedDishes[3].Name, Is.EqualTo(testMenu.MenuItems[3].Name));
        }

        [Test]
        public void Should_Not_Trim_WhiteSpace_For_Menu_Names()
        {
            var testMenu = new TestMenu1();
            string[] orderStrings = { testMenu.Name + " ", "1" };

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.That(testOrder.OrderedDishes.Count, Is.EqualTo(0));
        }

        [Test]
        public void Should_Return_Empty_Order_With_Error_Upon_Invalid_Menu_Name()
        {
            string[] orderStrings = { "Not A Registered Menu Name", "1", "2"};

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.That(testOrder.OrderedDishes.Count, Is.EqualTo(0));
        }

        [Test]
        public void Should_Return_Error_Upon_Invalid_OrderNumber_String1()
        {
            var testMenu = new TestMenu1();
            string[] orderStrings = { testMenu.Name, "Three" };

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);

            NUnit.Framework.Assert.IsFalse(testOrder.OrderedDishes.ContainsKey(3));
        }
        [Test]
        public void Should_Return_Error_Upon_Invalid_OrderNumber_String2()
        {
            var testMenu = new TestMenu1();
            string[] orderStrings = { testMenu.Name, "1.0" };

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.IsFalse(testOrder.OrderedDishes.ContainsKey(1));
        }

        [Test]
        public void Should_Return_Error_Ordering_From_Empty_Menu()
        {
            var testMenu = new EmptyTestMenu();
            string[] orderStrings = { testMenu.Name, "1"};

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.IsFalse(testOrder.OrderedDishes.ContainsKey(1));
        }

        [Test]
        public void Should_Not_Allow_Ordering_Null_Meal()
        {
            var testMenu = new TestMenuWithNullMeals();
            string[] orderStrings = { testMenu.Name, "1"};

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.IsFalse(testOrder.OrderedDishes.ContainsKey(1));
        }

        [Test]
        public void Should_Not_Allow_Ordering_Meal_With_Null_Name()
        {
            var testMenu = new TestMenuWithNullMealNames();
            string[] orderStrings = { testMenu.Name, "1"};

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.IsFalse(testOrder.OrderedDishes.ContainsKey(1));
        }
    }


    [TestFixture]
    public class PrintOrderTests
    {
        private PracticumOrderProcessor sut;
        private static IContainer Container { get; set; }

        private Order testOrder;
        private string printedOrder;

        [TestFixtureSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<PracticumOrderProcessor>().As<PracticumOrderProcessor>();

            Container = builder.Build();

        }

        [SetUp]
        public void RunBeforeAnyTests()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                sut = scope.Resolve<PracticumOrderProcessor>();
            }
        }

        [Test]
        public void No_Duplicate_Dishes_Should_Print_Without_Error()
        {
            var dishDictionary = new Dictionary<int, Dish>()
            {
                {1, new Dish("singleItem1", false, 1)},
                {2, new Dish("singleItem2", false, 1)},
                {3, new Dish("multipleItem1", true, 1)}
            };

            testOrder = new Order(dishDictionary);

            printedOrder = sut.PrintOrder(testOrder);
            NUnit.Framework.Assert.That(printedOrder, Is.EqualTo("singleItem1, singleItem2, multipleItem1"));
        }
        [Test]
        public void Should_Print_Multiples_When_Allowed()
        {
            var dishDictionary = new Dictionary<int, Dish>()
            {
                {1, new Dish("singleItem1", false, 1)},
                {2, new Dish("singleItem2", false, 1)},
                {3, new Dish("multipleItem1", true, 3)}
            };

            testOrder = new Order(dishDictionary);

            printedOrder = sut.PrintOrder(testOrder);
            NUnit.Framework.Assert.That(printedOrder, Is.EqualTo("singleItem1, singleItem2, multipleItem1(x3)"));
        }

        [Test]
        public void Should_Print_Error_Whenever_ErrorOccurred_Equals_True()
        {
            var dishDictionary = new Dictionary<int, Dish>()
            {
                {1, new Dish("singleItem1", false, 1)},
                {2, new Dish("singleItem2", false, 1)},
                {3, new Dish("multipleItem1", true, 1)}
            };

            testOrder = new Order(dishDictionary);
            testOrder.ErrorOccurred = true;

            printedOrder = sut.PrintOrder(testOrder);
            NUnit.Framework.Assert.That(printedOrder, Is.EqualTo("singleItem1, singleItem2, multipleItem1, error"));
        }

        [Test]
        public void Should_Not_Print_Dishes_With_Zero_Count()
        {
            var dishDictionary = new Dictionary<int, Dish>()
            {
                {1, new Dish("singleItem1", false, 0)},
                {2, new Dish("singleItem2", false, 0)},
                {3, new Dish("multipleItem1", true, 0)}
            };

            testOrder = new Order(dishDictionary);

            printedOrder = sut.PrintOrder(testOrder);
            NUnit.Framework.Assert.That(printedOrder, Is.EqualTo(""));
        }

        
        [Test]
        public void Should_Not_Print_Anything_For_No_Ordered_Dishes()
        {
            var dishDictionary = new Dictionary<int, Dish>()
            {
            };

            testOrder = new Order(dishDictionary);

            printedOrder = sut.PrintOrder(testOrder);
            NUnit.Framework.Assert.That(printedOrder, Is.EqualTo(""));
        }

        [Test]
        public void Should_Sort_Keys_Before_Output()
        {
            var dishDictionary = new Dictionary<int, Dish>()
            {
                {100, new Dish("singleItem1", false, 1)},
                {1, new Dish("singleItem2", false, 1)},
                {50, new Dish("multipleItem1", true, 3)}
            };

            testOrder = new Order(dishDictionary);

            printedOrder = sut.PrintOrder(testOrder);
            NUnit.Framework.Assert.That(printedOrder, Is.EqualTo("singleItem2, multipleItem1(x3), singleItem1"));
        }

        [Test]
        public void Should_Return_Error_For_Null_Order()
        {
            printedOrder = sut.PrintOrder(null);
            NUnit.Framework.Assert.That(printedOrder, Is.EqualTo("error"));
        }
    }

   


}
