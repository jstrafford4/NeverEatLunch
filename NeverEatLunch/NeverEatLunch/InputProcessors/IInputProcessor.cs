using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch
{
    public interface IInputProcessor
    {
        //TextWriter Error { get; private set; }
        //TextReader Input { get; private set; }
        //TextWriter Output { get; private set; }
        //IOrderProcessor orderProcessor { get; private set; }

        //public IInputProcessor(TextReader input, TextWriter output, TextWriter error, OrderProcessor orderProcessor)
        //{
        //    Error = error;
        //    Output = output;
        //    Input = input;
        //    this.orderProcessor = orderProcessor;
        //}

        void ProcessLines();
        
            /*
            var currentLine = Input.ReadLine();

            while (currentLine != null)
            {
                ProcessLine(currentLine);

                currentLine = Input.ReadLine();
            }
             * */


        

        /*void ProcessLine(string line)
        {

            var delimitedLine = line.Split(',');
            //input must specify a meal time and have at least one argument
            if (delimitedLine.Length < 2)
            {
                Console.WriteLine("error");
                return;
            }

            var orderOutput = orderProcessor.ProcessOrder(delimitedLine);
            Output.WriteLine(orderOutput);

        }*/


    }
}
