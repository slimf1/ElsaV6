using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ElsaV6.Utils;

namespace Tests
{
    class TextTest
    {
        [Test]
        public void TestToAlphaNum()
        {
            Assert.AreEqual(Text.ToLowerAlphaNum("bcde éçàAééa"), "bcdeaa");
            Assert.AreEqual(Text.ToLowerAlphaNum(" "), "");
            Assert.AreEqual(Text.ToLowerAlphaNum("0132 tést"), "0132tst");
        }
    }
}
