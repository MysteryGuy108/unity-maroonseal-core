using UnityEngine;

using MaroonSeal.Maths.Geometry;

namespace MaroonSeal.Maths.DataStructures.Grid
{
    public struct TriGridGeometry : IGridGeometry
    {
        const float sqrt3 = 1.73205080757f; // sqrt(3)
        const float heightCoeff = sqrt3 / 2.0f; // sqrt(3) / 2

        public float CellSideLength { get; set; }

        public readonly float CellHeight => heightCoeff * CellSideLength;

        #region Constructors
        public TriGridGeometry(float _cellRadius) { CellSideLength = _cellRadius; }
        #endregion

        #region IGridGeometry
        public readonly Vector2Int WorldToCell(Vector2 _worldPoint) {
            float s = CellSideLength;
            float h = CellHeight;

            float v = _worldPoint.y / h;
            float u = (_worldPoint.x - v * s * 0.5f) / s;

            int uR = (int)Mathf.Floor(u);
            int vR = (int)Mathf.Floor(v);

            float fu = u - uR;
            float fv = v - vR;

            bool firstTriangle = fu + fv < 1f;

            int row = vR;
            int column = uR * 2 + (firstTriangle ? 0 : 1);
            column += row; 
        
            return new Vector2Int(column, row);
        }

        public readonly Vector2 GetCellCentre(Vector2Int _cell)
        {
            float s = CellSideLength;
            float h = CellHeight;
            
            float x = (_cell.x / 2.0f) * s;
            float y = _cell.y * h;

            bool pointsUp = TriGridTopology.GetIsUpTri(_cell);

            x += s * 0.5f;

            if (pointsUp) { y += h * (1.0f / 3.0f); }
            else { y += h * (2.0f / 3.0f); }
            
            return new(x, y);
        }

        public readonly IShape2D GetCellShape(Vector2Int _cell)
        {
            bool isUp = TriGridTopology.GetIsUpTri(_cell);
            Transform2D transform = new(GetCellCentre(_cell), isUp ? 0.0f : 180.0f);

            return new Triangle2D(CellSideLength, transform);
        }
        #endregion
    }
}
