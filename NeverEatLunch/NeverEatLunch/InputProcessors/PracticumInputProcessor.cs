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

        /// <summary>
        /// Read a line at a time from the input TextReader, until given null (Ctrl-Z) input
        /// Calls ProcessLine on each line.
        /// </summary>
        public void ProcessLines()
        {
            var currentLine = Input.ReadLine();

            while (currentLine != null)
            {
                ProcessLine(currentLine);

                currentLine = Input.ReadLine();
            }
        }


        /// <summary>
        /// Splits each line into individual strings delimited by commas.
        /// If there is not a comma, or there is nothing after the first comma, output "error" without going to the order processor.
        /// If there is an argument after the first comma, have the order processor process the array of argument strings.
        /// Finally, write the processed order to output.
        /// </summary>
        /// <param name="line">the input string obtained by the TextReader input</param>
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
