using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using MaroonSeal.Maths;

namespace MaroonSealTesting.Maths.Cardinal
{
    public class Cardinal8Tests
    {
        #region Constructors
        [Test]
        public void Constructor_NoParametres_EqualsNorth() {
            var cardinal = new MaroonSeal.Maths.Cardinal8();

            Assert.AreEqual(MaroonSeal.Maths.Cardinal8.Direction.N, cardinal.direction);
            Assert.AreEqual(MaroonSeal.Maths.Cardinal8.N, cardinal);
        }
        #endregion
    }
}
