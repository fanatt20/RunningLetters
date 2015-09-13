using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RunningLetters.Core.Tests
{
    [TestClass]
    public class TestLetterManager
    {
        [TestMethod]
        public void TestAddingLetters()
        {
            var letterManager = new LetterManager();
            var letterA = new Letter('a', 0);
            letterManager.AddLetter(letterA);
            var letterB = new Letter('b', 2);
            letterManager.AddLetter(letterB);
            letterManager.AddLetter(new Letter('c', 5));
            Assert.AreEqual(letterA, letterManager.GetElementAt(0));
            Assert.AreEqual(letterB, letterManager.GetElementAt(2));
            Assert.AreEqual(new Letter('c', 5), letterManager.GetElementAt(5));
        }

        [TestMethod]
        public void TestGetString()
        {
            var letterManager = new LetterManager();
            var letterA = new Letter('a', 0);
            letterManager.AddLetter(letterA);
            var letterB = new Letter('b', 2);
            letterManager.AddLetter(letterB);
            letterManager.AddLetter(new Letter('c', 5));

            Assert.AreEqual("a b  c", letterManager.GetString());
        }
    }
}
