using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using MaroonSeal.DataStructures.NodeGraphs;

namespace MaroonSealTesting.DataStructures.NodeGraphs
{
    public class NodeGraphWeightedTests
    {
        #region Constructors
        [Test]
        public void Constructor_NoParametres_NodeCountZero() {
            var graph = new NodeGraphWeighted<int>();
            Assert.AreEqual(0, graph.NodeCount);
        }
        #endregion
    }
}
