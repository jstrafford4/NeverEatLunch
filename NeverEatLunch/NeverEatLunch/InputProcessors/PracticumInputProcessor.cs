using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeverEatLunch
{
    public class PracticumInputProcessor : IInputProcessor
    {
        //protected TextWriter Error;
        protected TextReader Input;
        protected TextWriter Output;
        protected IOrderProcessor orderProcessor;

        public PracticumInputProcessor(TextReader input, TextWriter output, IOrderProcessor orderProcessor)
        {
            //Error = error;
            Output = output;
            Input = input;
            this.orderProcessor = orderProcessor;
        }

        public void ProcessLines()
        {
            var currentLine = Input.ReadLine();

            while (currentLine != null)
            {
                ProcessLine(currentLine);

                currentLine = Input.ReadLine();
            }


        }

        protected void ProcessLine(string line)
        {

            var delimitedLine = line.Split(',');
            //input must specify a meal time and have at least one argument
            if (delimitedLine.Length < 2 || delimitedLine[1].Equals(String.Empty))
            {
                Output.WriteLine("error");
                return;
            }

            var orderOutput = orderProcessor.ProcessOrder(delimitedLine);
            Output.WriteLine(orderOutput);

        }


    }
}
