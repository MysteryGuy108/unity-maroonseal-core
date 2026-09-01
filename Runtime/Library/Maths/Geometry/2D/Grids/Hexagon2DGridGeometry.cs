using UnityEngine;

namespace MaroonSeal.Maths.Geometry
{
    [System.Serializable]
    public struct Hexagon2DGridGeometry : IGridGeometry2D
    {
        const float sqrt3 = 1.73205080757f; // sqrt(3)
        
        [field : SerializeField] public float CellRadius { get; set; }

        #region Constructors
        public Hexagon2DGridGeometry(float _cellRadius) { CellRadius = _cellRadius; }
        #endregion

        #region IGridGeometry
        public readonly Vector2Int WorldToCell(Vector2 _worldPoint) { 
            // Axial Coordinates
            float q = 2f / 3f * _worldPoint.y / CellRadius;
            float r = (-1f / 3f * _worldPoint.y + sqrt3 / 3f * _worldPoint.x) / CellRadius;

            // Cube Coordinates
            Vector3 cube = new(q, -q-r, r);

            // Cube Rounding
            Vector3Int cubeR = Vector3Maths.RoundToInt(cube);
            Vector3 cubeD = Vector3Maths.Abs(cubeR - cube);

            // Error Checking (rx + ry + rz = 0)
            if (cubeD.x > cubeD.y && cubeD.x > cubeD.z) { cubeR.x = -cubeR.y - cubeR.z; }
            else if (cubeD.y > cubeD.z) { cubeR.y = -cubeR.x - cubeR.z; }
            else { cubeR.z = -cubeR.x - cubeR.y; }

            return new Vector2Int(
                cubeR.z + (cubeR.x - (cubeR.x & 1)) / 2,
                cubeR.x
            );
        }

        public Vector2 GetCellCentre(Vector2Int _cell) {
            float x = _cell.x * sqrt3 * CellRadius;

            if ((_cell.y & 1) == 1)
            {
                x += sqrt3 * CellRadius / 2;
            }

            float y = _cell.y * 1.5f * CellRadius;

            return new Vector2(x, y);
        }

        public IShape2D GetCellShape(Vector2Int _cell)
        {
            return new Hexagon2D(new Transform2D(GetCellCentre(_cell), 90.0f), CellRadius);
        }

        public TShape2D GetCellShape<TShape2D>(Vector2Int _cell) where TShape2D : IShape2D {
            return (TShape2D)GetCellShape(_cell);
        } 
        #endregion
    }
}
