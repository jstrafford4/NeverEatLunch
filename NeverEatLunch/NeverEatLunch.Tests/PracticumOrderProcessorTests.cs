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
        public void FormOrder_Valid_For_TestMenu1_Standard_Order()
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
        public void FormOrder_Continues_Until_Error_Then_Stops()
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
        public void FormOrder_Doesnt_Care_About_WhiteSpace_For_OrderNumbers()
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
        public void FormOrder_Does_Care_About_WhiteSpace_For_Menu_Names()
        {
            var testMenu = new TestMenu1();
            string[] orderStrings = { testMenu.Name + " ", "1" };

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.That(testOrder.OrderedDishes.Count, Is.EqualTo(0));
        }

        [Test]
        public void FormOrder_Returns_Empty_Order_With_Error_Upon_Invalid_Menu_Name()
        {
            string[] orderStrings = { "Not A Registered Menu Name", "1", "2"};

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.That(testOrder.OrderedDishes.Count, Is.EqualTo(0));
        }

        [Test]
        public void FormOrder_Returns_Error_Upon_Invalid_OrderNumber_String1()
        {
            var testMenu = new TestMenu1();
            string[] orderStrings = { testMenu.Name, "1", "2", "Three" };

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.That(testOrder.OrderedDishes[1].Name, Is.EqualTo(testMenu.MenuItems[1].Name));
            NUnit.Framework.Assert.That(testOrder.OrderedDishes[2].Name, Is.EqualTo(testMenu.MenuItems[2].Name));
            NUnit.Framework.Assert.IsFalse(testOrder.OrderedDishes.ContainsKey(3));
        }
        [Test]
        public void FormOrder_Returns_Error_Upon_Invalid_OrderNumber_String2()
        {
            var testMenu = new TestMenu1();
            string[] orderStrings = { testMenu.Name, "1.0" };

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.IsFalse(testOrder.OrderedDishes.ContainsKey(1));
        }

        [Test]
        public void FormOrder_Returns_Error_Ordering_From_Empty_Menu()
        {
            var testMenu = new EmptyTestMenu();
            string[] orderStrings = { testMenu.Name, "1"};

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.IsFalse(testOrder.OrderedDishes.ContainsKey(1));
        }

        [Test]
        public void FormOrder_Doesnt_Allow_Ordering_Null_Meal()
        {
            var testMenu = new TestMenuWithNullMeals();
            string[] orderStrings = { testMenu.Name, "1"};

            var testOrder = sut.FormOrder(orderStrings);

            NUnit.Framework.Assert.IsTrue(testOrder.ErrorOccurred);
            NUnit.Framework.Assert.IsFalse(testOrder.OrderedDishes.ContainsKey(1));
        }

        [Test]
        public void FormOrder_Doesnt_Allow_Ordering_Meal_With_Null_Name()
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
            //TestMenu1 MUST be registered for tests below to run
            //builder.RegisterType<TestMenu1>().As<Menu>();
            //builder.RegisterType<TestMenu2>().As<Menu>();
            //builder.RegisterType<EmptyTestMenu>().As<Menu>();
            //builder.RegisterType<TestMenuWithNullMeals>().As<Menu>();
            //builder.RegisterType<TestMenuWithNullMealNames>().As<Menu>();
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
        public void PrintOrder_No_Duplicates_No_Errors_Test()
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
        public void PrintOrder_Duplicates_No_Errors_Test()
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
        public void PrintOrder_No_Duplicates_But_Error_Test()
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
        public void PrintOrder_DishCountZero_But_Error_Test()
        {
            var dishDictionary = new Dictionary<int, Dish>()
            {
                {1, new Dish("singleItem1", false, 0)},
                {2, new Dish("singleItem2", false, 0)},
                {3, new Dish("multipleItem1", true, 0)}
            };

            testOrder = new Order(dishDictionary);
            testOrder.ErrorOccurred = true;

            printedOrder = sut.PrintOrder(testOrder);
            NUnit.Framework.Assert.That(printedOrder, Is.EqualTo("error"));
        }

        [Test]
        public void PrintOrder_DishCountZero_No_Errors_Test()
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
        public void PrintOrder_No_Dishes_No_Errors_Test()
        {
            var dishDictionary = new Dictionary<int, Dish>()
            {
            };

            testOrder = new Order(dishDictionary);

            printedOrder = sut.PrintOrder(testOrder);
            NUnit.Framework.Assert.That(printedOrder, Is.EqualTo(""));
        }

        [Test]
        public void PrintOrder_No_Dishes_But_Errors_Test()
        {
            var dishDictionary = new Dictionary<int, Dish>()
            {
            };

            testOrder = new Order(dishDictionary);
            testOrder.ErrorOccurred = true;

            printedOrder = sut.PrintOrder(testOrder);
            NUnit.Framework.Assert.That(printedOrder, Is.EqualTo("error"));
        }

        [Test]
        public void PrintOrder_Keys_Should_Be_Sorted_Before_Output()
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
