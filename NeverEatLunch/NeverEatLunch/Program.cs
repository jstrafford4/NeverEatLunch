using Autofac;
using NeverEatLunch.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch
{
    class Program
    {
        //private static IContainer Container { get; set; }
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            //Register all menus that will be served.
            builder.RegisterType<MorningMenu>().As<Menu>();
            builder.RegisterType<NightMenu>().As<Menu>();
            builder.RegisterType<PracticumOrderProcessor>().As<IOrderProcessor>();

            builder.RegisterInstance(Console.Out).As<TextWriter>().ExternallyOwned();
            builder.RegisterInstance(Console.In).As<TextReader>().ExternallyOwned();
            builder.RegisterType<PracticumInputProcessor>().As<IInputProcessor>();

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var inputProcessor = scope.Resolve<IInputProcessor>();
                inputProcessor.ProcessLines();
            }
        }
    }
}
