using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths.Geometry;
using MaroonSeal.Maths.Geometry.Shapes;
using MaroonSeal.Maths.Geometry.Paths;

namespace MaroonSeal.Maths.Geometry.Paths.ReedsShepp {

    /// <summary>
    /// Path comprised of a series of vehicle manoeuvres for a 
    /// Reeds Shepp vehicle with a fixed minimum turn radius. 
    /// </summary>
    [System.Serializable]
    public class ReedsSheppPath : CompositePath<ShapePath>
    {
        override public bool IsLoop => false;

        [Space]
        [SerializeField][Min(0.0f)] private float minRadius = 1.0f;
        [Space]
        private PointTransform2D startPoint;
        public PointTransform2D StartPoint => startPoint;

        [SerializeField] private List<RSManoeuvre> manoeuvreList;
        
        int gearChangeCount;
        public int GearChangeCount => gearChangeCount;

        #region Constructors
        public ReedsSheppPath() : base() {
            manoeuvreList = new();
        }

        public ReedsSheppPath(PointTransform2D? _start = null, float _minRadius = 1.0f, List<RSManoeuvre> _manoeuvreList = null) : this() {
            
            startPoint = _start ?? new PointTransform2D();
            minRadius = _minRadius;

            SetManoeuvres(_manoeuvreList);
        }
        #endregion

        #region GeometryPathBase
        public override void Clear()
        {
            base.Clear();
            manoeuvreList?.Clear();
            gearChangeCount = 0;
        }
        #endregion

        #region ReedsSheppPath
        public string GetWord() {
            string word = "";
            for(int i = 0; i < manoeuvreList.Count; i++) {
                if (i > 0) { 
                    if (manoeuvreList[i].gear != manoeuvreList[i-1].gear) { word += "|"; } 
                }
                else if (manoeuvreList[i].gear == RSManoeuvre.Gear.Backward) { word += "|"; } 

                switch(manoeuvreList[i].steer) {
                    case RSManoeuvre.Steer.Left: 
                        word += 'L'; break;
                    case RSManoeuvre.Steer.Straight: 
                        word += 'S'; break;
                    case RSManoeuvre.Steer.Right: 
                        word += 'R'; break;
                }
            }
            return word;
        }
        #endregion

        #region Manoeuvres
        public void SetManoeuvres(List<RSManoeuvre> _manoeuvreList, PointTransform2D? _start = null, float? _minRadius = null) {
            Clear();
            startPoint = _start ?? startPoint;
            minRadius = _minRadius ?? minRadius;

            foreach (RSManoeuvre manoeuvre in _manoeuvreList) {
                AddManoeuvre(manoeuvre, false);
            }

            Refresh();
        }

        public void AddManoeuvre(RSManoeuvre _manoeuvre) =>
            AddManoeuvre(_manoeuvre, true);

        private void AddManoeuvre(RSManoeuvre _manoeuvre, bool _refresh) {
            if (_manoeuvre.travel == 0.0f) { return; }
            
            PointTransform2D anchorPoint;

            if (manoeuvreList.Count == 0) {
                anchorPoint = startPoint;
                gearChangeCount += _manoeuvre.gear == RSManoeuvre.Gear.Backward ? 1 : 0;
            }
            else {
                anchorPoint = segments[^1].GetPoint2DAtTime(1.0f);
                gearChangeCount += _manoeuvre.gear != manoeuvreList[^1].gear ? 1 : 0;
            }

            ShapePath manoeuvrePath = _manoeuvre.steer == RSManoeuvre.Steer.Straight ? 
                GetStraightManoeuvrePath(anchorPoint, _manoeuvre) :
                GetTurnManoeuvrePath(anchorPoint, _manoeuvre);

            manoeuvreList.Add(_manoeuvre);
            segments.Add(manoeuvrePath);

            if (_refresh) { Refresh(); }
        }
        #endregion

        #region Manoeuvres Queries
        public List<RSManoeuvre> GetManoeuvres() => new(manoeuvreList);

        public RSManoeuvre GetManoeuvreAtIndex(int _index) => manoeuvreList[_index];

        public RSManoeuvre GetManoeuvreAtTime(float _t)
        {
            if (manoeuvreList == null) { return new RSManoeuvre(); }
            int index = GetSegmentIndexAtTime(_t, out float _localTime);
            return manoeuvreList[index];
        }

        public RSManoeuvre GetManoeuvreAtDistance(float _distance) => 
            GetManoeuvreAtTime(distanceLUT.EvaulateTimeAtValue(_distance));
        #endregion

        #region Manoeuvre Paths
        private LinePath GetStraightManoeuvrePath(PointTransform2D _start, RSManoeuvre _manoeuvre) {
            Vector3 delta = _manoeuvre.travel * minRadius * (int)_manoeuvre.gear * _start.Right;
            Line line = new(_start.position, (Vector3)_start.position + delta);
            return new LinePath(line) { flipTangent = _manoeuvre.gear == RSManoeuvre.Gear.Backward };
        }

        private ArcPath GetTurnManoeuvrePath(PointTransform2D _start, RSManoeuvre _manoeuvre) {
            float theta = _manoeuvre.travel * (int)_manoeuvre.gear * Mathf.Rad2Deg;

            PointTransform2D arcTransform = new(_start.position, _start.angle - 90.0f, new(-(int)_manoeuvre.steer, 1));
            arcTransform.position += minRadius * -(int)_manoeuvre.steer * _start.Up;

            Arc arc = new(arcTransform.ToXY(), minRadius, 0.0f, theta);
            return new ArcPath(arc);
        }
        #endregion

        #region Static Functions
        static public ReedsSheppPath FindShortestPath(PointTransform2D _start, PointTransform2D _end, float _minRadius = 1.0f, float _gearChangeWeight = 0.0f) {
            List<ReedsSheppPath> pathCases = GetPathCases(_start, _end, _minRadius);
            ReedsSheppPath shortestPath = null;
            float shortestTravelWeight = Mathf.Infinity;

            for(int i = 0; i < pathCases.Count; i++) {
                float travelWeight = pathCases[i].Length + (pathCases[i].GearChangeCount * _gearChangeWeight);

                if (travelWeight < shortestTravelWeight) { 
                    shortestPath?.Clear();

                    shortestPath = pathCases[i];
                    shortestTravelWeight = travelWeight;
                }
                else {
                    pathCases[i].Clear(); 
                }
            }

            pathCases.Clear();
            return shortestPath;
        }

        static public List<ReedsSheppPath> GetOrderedPathCases(PointTransform2D _start, PointTransform2D _end, float _minRadius = 1.0f, float _gearChangeWeight = 0.0f) {
            List<ReedsSheppPath> pathCases = GetPathCases(_start, _end, _minRadius);
            List<ReedsSheppPath> orderedCases = pathCases.OrderBy(x => (x.GearChangeCount * _gearChangeWeight) + x.Length).ToList();

            foreach(ReedsSheppPath path in pathCases) { path.Clear(); }
            pathCases.Clear();
            return orderedCases;
        }

        static public List<ReedsSheppPath> GetPathCases(PointTransform2D _start, PointTransform2D _end, float _minRadius = 1.0f) =>
            ReedsSheppPathFinder.GetAllPaths(_start, _end, _minRadius);
        #endregion
    }
}