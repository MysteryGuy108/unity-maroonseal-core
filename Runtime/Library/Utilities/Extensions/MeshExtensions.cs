using UnityEngine;

namespace MaroonSeal
{
    public static class MeshExtensions
    {
        public static int FindTriangleSubmesh(this Mesh _mesh, int _triangleIndex)
        {
            // There are 3 indices stored per triangle
            int limit = _triangleIndex * 3;
            int submesh;
            for(submesh = 0; submesh < _mesh.subMeshCount; submesh++)
            {
                int numIndices = _mesh.GetTriangles(submesh).Length;
                if(numIndices > limit) { break; }

                limit -= numIndices;   
            }

            return submesh;
        }
    }
}
