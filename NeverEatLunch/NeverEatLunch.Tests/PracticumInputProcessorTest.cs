using Autofac;
using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NeverEatLunch.Tests
{
    [TestFixture]
    class PracticumInputProcessorTest
    {

        [Test]
        public void Input_With_No_Comma_Should_Output_Error_Before_Calling_ProcessOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var strBuilder = new StringBuilder("");
                var output = new StringWriter(strBuilder);
                mock.Provide<TextWriter>(output);

                string testInput = "NoComma 1 2 3";
                var input = new StringReader(testInput);
                mock.Provide<TextReader>(input);

                // Arrange - configure the mock
                var sut = mock.Create<PracticumInputProcessor>();

                sut.ProcessLines();
                mock.Mock<IOrderProcessor>().Verify(x => x.ProcessOrder(It.IsAny<string[]>()), Times.Never()); 
                Assert.That(strBuilder.ToString(), Is.EqualTo("error\r\n"));
            }
        }

        [Test]
        public void No_Input_After_Comma_Should_Output_Error_Before_Calling_ProcessLine()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var strBuilder = new StringBuilder("");
                var output = new StringWriter(strBuilder);
                mock.Provide<TextWriter>(output);

                string testInput = "menu name,";
                var input = new StringReader(testInput);
                mock.Provide<TextReader>(input);

                var sut = mock.Create<PracticumInputProcessor>();

                sut.ProcessLines();
                mock.Mock<IOrderProcessor>().Verify(x => x.ProcessOrder(It.IsAny<string[]>()), Times.Never()); 
                Assert.That(strBuilder.ToString(), Is.EqualTo("error\r\n"));
            }
        }

        [Test]
        public void Should_Process_Multiple_Valid_Lines()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var strBuilder = new StringBuilder("");
                var output = new StringWriter(strBuilder);
                mock.Provide<TextWriter>(output);

                string testInput = "Good, 1\r\n";
                testInput += "Good, 1\r\n";
                testInput += "Good, 1";
                var input = new StringReader(testInput);
                mock.Provide<TextReader>(input);

                // Arrange - configure the mock
                mock.Mock<IOrderProcessor>().Setup(x => x.ProcessOrder(It.IsAny<string[]>())).Returns("Great!");
                var sut = mock.Create<PracticumInputProcessor>();

                sut.ProcessLines();
                Assert.That(strBuilder.ToString(), Is.EqualTo("Great!\r\nGreat!\r\nGreat!\r\n"));
            }
        }

        [Test]
        public void Should_Process_Valid_And_Invalid_Lines_Intermixed()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var strBuilder = new StringBuilder("");
                var output = new StringWriter(strBuilder);
                mock.Provide<TextWriter>(output);

                string testInput = "Valid, 1\r\n";
                testInput += "No Comma\r\n";
                testInput += "Valid, 1";
                var input = new StringReader(testInput);
                mock.Provide<TextReader>(input);

                // Arrange - configure the mock
                mock.Mock<IOrderProcessor>().Setup(x => x.ProcessOrder(It.IsAny<string[]>())).Returns("Great!");
                var sut = mock.Create<PracticumInputProcessor>();

                sut.ProcessLines();
                Assert.That(strBuilder.ToString(), Is.EqualTo("Great!\r\nerror\r\nGreat!\r\n"));
            }
        }

        [Test]
        public void Should_End_Upon_Null_Input()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var strBuilder = new StringBuilder("");
                var output = new StringWriter(strBuilder);
                mock.Provide<TextWriter>(output);

                string nullString = null;
                mock.Mock<TextReader>().Setup(x => x.ReadLine()).Returns(nullString);
                
                // Arrange - configure the mock
                mock.Mock<IOrderProcessor>().Setup(x => x.ProcessOrder(It.IsAny<string[]>())).Returns("Great!");
                var sut = mock.Create<PracticumInputProcessor>();

                sut.ProcessLines();
                Assert.That(strBuilder.ToString(), Is.EqualTo(String.Empty));
            }
        }



    }
}
