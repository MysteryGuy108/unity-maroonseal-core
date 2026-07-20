using UnityEngine;

using MaroonSeal.Maths.Geometry;

namespace MaroonSeal
{
    public class HexagonGridLayout : MonoBehaviour
    {
        [SerializeField] private Vector2Int size;
        [SerializeField] Hexagon2DGridGeometry geometry;

        #region MonoBehaviour
        private void OnValidate() {
            Refresh();
        }

        private void OnDrawGizmosSelected()
        {
            Matrix4x4 TRS = Matrix4x4.TRS(this.transform.position, this.transform.rotation * Quaternion.Euler(90.0f, 0.0f, 0.0f), this.transform.localScale);

            Gizmos.matrix = TRS;
            Gizmos.color = new(0.25f, 0.25f, 0.25f, 1.0f);

            for(int y = 0; y < size.y; y++)
            {
                for(int x = 0; x < size.x; x++)
                {
                    GeometryGizmos2D.DrawPolygon(geometry.GetCellShape<IPolygon2D>(new(x, y)));
                }
            }

            Gizmos.matrix = Matrix4x4.identity;
        }
        #endregion

        public void Refresh()
        {
            foreach(Transform child in this.transform)
            {
                Vector2Int cell = geometry.WorldToCell(child.localPosition.ToXZ());
                child.localPosition = geometry.GetCellCentre(cell).ToXZ();
            }
        }
    }
}
