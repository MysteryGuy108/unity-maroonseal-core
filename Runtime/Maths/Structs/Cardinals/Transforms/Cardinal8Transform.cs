using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using MaroonSeal.Maths.Geometry.Shapes;
using MaroonSeal.Maths.DataStructures;
using MaroonSeal.Maths.DataStructures.Grid;
using MaroonSeal.Maths.DataStructures.Graphs;

namespace MaroonSeal.Maths {
    public class Cardinal8Transform : MonoBehaviour
    {
        Vector2Graph graph;
        [SerializeField] private List<Vector2> points;

        [SerializeField] private Triangle2D tri;

        [Header("Cardinal Transform")]
        [SerializeField] protected Cardinal8 cardinalDirection;
        virtual public Cardinal8 Direction { 
            get => cardinalDirection; 
            set {
                cardinalDirection = value;
                this.transform.rotation = cardinalDirection.ToRotation();
            }
        }

        #region MonoBehaviour
        private void Awake() => Direction = cardinalDirection;

        private void Start()
        {
            points = new();
            for(int i = 0; i < 8; i++)
            {
                points.Add(Random.insideUnitCircle * 5);
            }

            graph = GraphGenerator.CreateTriangulation2DGraph(points);
        }

        private void OnDrawGizmosSelected()
        {
            //ShapeGizmos.DrawTriangle(tri);
            //ShapeGizmos.DrawCircle(tri.GetCircumcircle());

            if (graph == null) { return; }

            foreach(Vector2 point in points) { Gizmos.DrawSphere(point, 0.125f); }

            foreach(Vector2 node in graph.Nodes)
            {
                foreach(KeyValuePair<Vector2, float> neighbour in graph.GetNodeNeighbors(node))
                {
                    Gizmos.DrawLine(node, neighbour.Key);
                }
            }
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            Direction = cardinalDirection;
        }
        #endif
        #endregion
    }
}