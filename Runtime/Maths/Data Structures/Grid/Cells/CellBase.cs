using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    [System.Serializable]
    abstract public class CellBase<TValue, TEdge> : ISerializationCallbackReceiver
    {
        abstract public int EdgeCount { get; }
        abstract public int NeighbourCount { get; }

        [SerializeField] private TValue value;
        [SerializeField][FixedListView(true, true)] private CompassArray<TEdge> edges;

        #region Constructors
        public CellBase()
        {
            edges = new(EdgeCount);
        }
        #endregion

        #region ISerializationCallbackReceiver
        public void OnBeforeSerialize() {}

        public void OnAfterDeserialize()
        {
            if (edges.Length == EdgeCount) { return; }
            TEdge[] edgeValues = edges.ToArray();
            edges = new(EdgeCount, edgeValues);
        }
        #endregion
    }
}
