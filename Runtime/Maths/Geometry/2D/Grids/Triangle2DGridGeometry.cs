using UnityEngine;

using MaroonSeal.DataStructures.Grid;

namespace MaroonSeal.Maths.Geometry
{
    public struct Triangle2DGridGeometry : IGridGeometry2D
    {
        const float sqrt3 = 1.73205080757f; // sqrt(3)
        const float heightCoeff = sqrt3 / 2.0f; // sqrt(3) / 2

        public float CellSideLength { get; set; }

        public readonly float CellHeight => heightCoeff * CellSideLength;

        #region Constructors
        public Triangle2DGridGeometry(float _cellRadius) { CellSideLength = _cellRadius; }
        #endregion

        #region IGridGeometry
        public readonly Vector2Int WorldToCell(Vector2 _worldPoint) {
            float s = CellSideLength;
            float h = CellHeight;
            if (s == 0.0f) { return Vector2Int.zero; }

            // Converting to lattice space.
            float v = _worldPoint.y / h;
            float u = (_worldPoint.x - v * s * 0.5f) / s;

            // Getting lattice index.
            int uR = (int)Mathf.Floor(u);
            int vR = (int)Mathf.Floor(v);

            // Getting local lattice coordinates.
            float fu = u - uR;
            float fv = v - vR;

            // Checking if it is the first triangle in the parallelogram.
            bool firstTriangle = fu + fv < 1f;

            // Getting grid index row and column
            int row = vR;
            int column = uR * 2 + (firstTriangle ? 0 : 1) + row; // Do not know why I have to add the row here but it's the only way I can get this to work.
        
            return new Vector2Int(column, row);
        }

        public readonly Vector2 GetCellCentre(Vector2Int _cell)
        {
            float s = CellSideLength;
            float h = CellHeight;
            
            float x = _cell.x / 2.0f * s;
            float y = _cell.y * h;

            bool pointsUp = TriangleGridTopology.GetIsUpTri(_cell);

            x += s * 0.5f;

            if (pointsUp) { y += h * (1.0f / 3.0f); }
            else { y += h * (2.0f / 3.0f); }
            
            return new(x, y);
        }

        public readonly IShape2D GetCellShape(Vector2Int _cell)
        {
            bool isUp = TriangleGridTopology.GetIsUpTri(_cell);
            Transform2D transform = new(GetCellCentre(_cell), isUp ? 0.0f : 180.0f);

            return new Triangle2D(CellSideLength, transform);
        }
        #endregion
    }
}
