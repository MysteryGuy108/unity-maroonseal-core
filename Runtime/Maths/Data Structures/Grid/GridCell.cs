using System.Collections.ObjectModel;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    [System.Serializable]
    public class GridCell<TValue, TEdge> : ISerializationCallbackReceiver
    {
        public int EdgeCount { get; }

        [SerializeField][FixedListView(true, true)] private CompassArray<TEdge> edges;

        [field : SerializeField] public TValue Value {get; set; }

        public TEdge this[int _edgeIndex]
        {
            get => edges[_edgeIndex];
            set => edges[_edgeIndex] = value;
        }

        #region Constructors
        public GridCell(int _edgeCount) {
            EdgeCount = _edgeCount;
            edges = new(_edgeCount); 
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
