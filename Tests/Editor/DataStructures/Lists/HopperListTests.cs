using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using MaroonSeal.DataStructures;

namespace MaroonSealTesting.DataStructures.Lists
{
    public class HopperListTests
    {
        #region Constructors
        [Test]
        public void Constructor_EmptyList_SupplyCountZero() {
            var hopper = new HopperList<int>(new List<int>());
            Assert.AreEqual(0, hopper.SupplyCount);
        }
        #endregion
    }
}
