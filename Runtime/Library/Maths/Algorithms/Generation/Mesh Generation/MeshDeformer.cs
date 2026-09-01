using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal
{
    [RequireComponent(typeof(MeshFilter)), DisallowMultipleComponent]
    public class MeshDeformer : MonoBehaviour
    {
        MeshFilter filter;
        MeshCollider meshCollider;

        Mesh sourceMesh;
        Mesh deformedMesh;

        public int VertexCount => verticesCache.Length;
        public int ColourCount => coloursCache.Length;

        Vector3[] verticesCache;
        private bool verticesDirty;

        Color[] coloursCache;
        private bool coloursDirty;

        Vector3[] normalsCache;
        private bool normalsDirty;

        [SerializeField] private bool copyMesh = true;
        [SerializeField] private bool editCollider = true;

        #region MonoBehaviour
        private void Awake()
        {
            filter = this.GetComponent<MeshFilter>();
            meshCollider = this.GetComponent<MeshCollider>();

            if (!filter) { return; }
            sourceMesh = filter.mesh;
            if (sourceMesh == null) { return; }

            deformedMesh = copyMesh ? CreateMeshCopy(sourceMesh) : sourceMesh;

            verticesDirty = true;
            coloursDirty = true;
            normalsDirty = true;

            RefreshCaches();

            filter.mesh = deformedMesh;
            
            Refresh();
        }
        #endregion

        private Mesh CreateMeshCopy(Mesh _source)
        {
            Mesh copy = new() {
                name = _source.name[..^8] + "Deformed",
                vertices = _source.vertices,
                normals = _source.normals,
                tangents = _source.tangents,
                colors = _source.colors,
                subMeshCount = _source.subMeshCount
            };

            for(int i = 0; i < _source.subMeshCount; i++) {
                copy.SetTriangles(_source.GetTriangles(i), i);
            }

            return copy;
        }

        #region Vertices
        public Vector3 GetVertex(int _index) => verticesCache[_index];
        public void SetVertex(int _index, Vector3 _vertex)
        {
            verticesDirty |= _vertex != verticesCache[_index];
            verticesCache[_index] = _vertex;
        }

        public Vector3 GetWorldVertex(int _index) => this.transform.TransformPoint(GetVertex(_index));
        public void SetWorldVertex(int _index, Vector3 _vertex) => SetVertex(_index, this.transform.InverseTransformPoint(_vertex));
        #endregion

        #region Colours
        public Color GetVertexColour(int _index) => coloursCache[_index];
        public void SetVertexColour(int _index, Color _colour)
        {
            coloursDirty |= _colour != coloursCache[_index];
            coloursCache[_index] = _colour;
        }
        #endregion

        #region Normals
        public Vector3 GetNormal(int _index) => normalsCache[_index];
        public void SetNormal(int _index, Vector3 _normal)
        {
            normalsDirty |= _normal != verticesCache[_index];
            normalsCache[_index] = _normal;
        }

        public Vector3 GetWorldNormal(int _index) => this.transform.TransformDirection(GetVertex(_index));
        public void SetWorldNormal(int _index, Vector3 _normal) => SetVertex(_index, this.transform.InverseTransformDirection(_normal));
        #endregion

        #region Caches
        public void RefreshCaches()
        {
            if(!deformedMesh) { return; }
            verticesCache = deformedMesh.vertices;
            coloursCache = deformedMesh.colors;
            normalsCache = deformedMesh.normals;
        }

        public void ClearCaches()
        {
            verticesCache = null;
            normalsCache = null;
            coloursCache = null;
        }
        #endregion
        public void Refresh()
        {
            if (verticesDirty && verticesCache != null) { deformedMesh.SetVertices(verticesCache); }

            if (coloursDirty && coloursCache != null) { deformedMesh.SetColors(coloursCache); }

            if (normalsDirty && normalsCache != null) { deformedMesh.SetNormals(normalsCache); }
            else { deformedMesh.RecalculateNormals(); }
            
            deformedMesh.RecalculateTangents();

            verticesDirty = false;
            coloursDirty = false;
            normalsDirty = false;
            
            if (editCollider && meshCollider)
            {
                meshCollider.sharedMesh = null;
                meshCollider.sharedMesh = deformedMesh;
            }
        }
    }
}
