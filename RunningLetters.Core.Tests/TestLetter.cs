using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RunningLetters.Core.Tests
{
    [TestClass]
    public class TestLetter
    {
        [TestMethod]
        public void Test_Letter_Constructor()
        {
            var letter = new Letter('a', 5);
            Assert.AreEqual('a', letter.Value);
            Assert.AreEqual(5, letter.Position);
        }
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_Letter_MoveNextAndMovePrevious()
        {
            var letter = new Letter('a', 0);

            letter.MoveNext();
            Assert.AreEqual(1, letter.Position);
            letter.MovePrevious();
            Assert.AreEqual(0, letter.Position);
            letter.MovePrevious();
            Assert.AreEqual(0, letter.Position);

        }
    }
}
