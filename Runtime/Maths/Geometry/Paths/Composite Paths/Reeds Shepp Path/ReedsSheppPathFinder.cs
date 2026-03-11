using System;
using System.Collections.Generic;

using UnityEngine;

using MaroonSeal.Maths;
using MaroonSeal.Maths.Geometry;


namespace MaroonSeal.Maths.Geometry.Paths.ReedsShepp {
    internal class ReedsSheppPathFinder
    {
        private static readonly Func<float, float ,float, bool, bool, List<RSManoeuvre>>[] reedsSheppCases = 
            new Func<float, float ,float, bool, bool, List<RSManoeuvre>>[12] {
                PathCase1, PathCase2, PathCase3, PathCase4, PathCase5, PathCase6,
                PathCase7, PathCase8, PathCase9, PathCase10, PathCase11, PathCase12 
            };
        
        private static readonly Func<float, float ,float, bool, bool, List<RSManoeuvre>>[] dubinsCases = 
            new Func<float, float ,float, bool, bool, List<RSManoeuvre>>[2] { PathCase1, PathCase2 };

        static public List<ReedsSheppPath> GetAllPaths(PointTransform2D _start, PointTransform2D _end, float _minRadius = 1.0f)
        {
            List<ReedsSheppPath> pathCases = new();

            PointTransform2D target = ChangeOfBasis(_start, _end, _minRadius);
            float targetPhi = target.angle * Mathf.Deg2Rad;

            for (int i = 0; i < 12; i++)
            {
                List<RSManoeuvre> pathManoeuvres = reedsSheppCases[i](target.position.x, target.position.y, targetPhi, false, false);
                if (pathManoeuvres != null) { pathCases.Add(new ReedsSheppPath(_start, _minRadius, pathManoeuvres)); pathManoeuvres.Clear(); }

                // Flipped Gear
                pathManoeuvres = reedsSheppCases[i](-target.position.x, target.position.y, -targetPhi, false, true);
                if (pathManoeuvres != null) { pathCases.Add(new ReedsSheppPath(_start, _minRadius, pathManoeuvres)); pathManoeuvres.Clear(); }

                // Flipped Steer
                pathManoeuvres = reedsSheppCases[i](target.position.x, -target.position.y, -targetPhi, true, false);
                if (pathManoeuvres != null) { pathCases.Add(new ReedsSheppPath(_start, _minRadius, pathManoeuvres)); pathManoeuvres.Clear(); }

                // Flipped Gear and Steer
                pathManoeuvres = reedsSheppCases[i](-target.position.x, -target.position.y, targetPhi, true, true);
                if (pathManoeuvres != null) { pathCases.Add(new ReedsSheppPath(_start, _minRadius, pathManoeuvres)); pathManoeuvres.Clear(); }
            }

            return pathCases;
        }

        static public List<ReedsSheppPath> GetDubinsPaths(PointTransform2D _start, PointTransform2D _end, float _minRadius = 1.0f)
        {
            List<ReedsSheppPath> pathCases = new();

            PointTransform2D target = ChangeOfBasis(_start, _end, _minRadius);
            float targetPhi = target.angle * Mathf.Deg2Rad;

            for (int i = 0; i < 2; i++) {
                List<RSManoeuvre> pathManoeuvres = dubinsCases[i](target.position.x, target.position.y, targetPhi, false, false);
                if (pathManoeuvres != null) { pathCases.Add(new ReedsSheppPath(_start, _minRadius, pathManoeuvres)); pathManoeuvres.Clear(); }
                
                // Flipped Steer
                pathManoeuvres = dubinsCases[i](target.position.x, -target.position.y, -targetPhi, true, false);
                if (pathManoeuvres != null) { pathCases.Add(new ReedsSheppPath(_start, _minRadius, pathManoeuvres)); pathManoeuvres.Clear();  }
            }

            return pathCases;
        }

        static public List<ReedsSheppPath> GetReverseDubinsPaths(PointTransform2D _start, PointTransform2D _end, float _minRadius = 1.0f)
        {
            List<ReedsSheppPath> pathCases = new();

            PointTransform2D target = ChangeOfBasis(_start, _end, _minRadius);
            float targetPhi = target.angle * Mathf.Deg2Rad;

            for (int i = 0; i < 2; i++)
            {
                List<RSManoeuvre> pathManoeuvres = dubinsCases[i](-target.position.x, target.position.y, targetPhi, false, true);
                if (pathManoeuvres != null) { pathCases.Add(new ReedsSheppPath(_start, _minRadius, pathManoeuvres)); pathManoeuvres.Clear(); }

                // Flipped Steer
                pathManoeuvres = dubinsCases[i](-target.position.x, -target.position.y, -targetPhi, true, true);
                if (pathManoeuvres != null) { pathCases.Add(new ReedsSheppPath(_start, _minRadius, pathManoeuvres)); pathManoeuvres.Clear(); }
            }

            return pathCases;
        }

        static private PointTransform2D ChangeOfBasis(PointTransform2D _base, PointTransform2D _current, float _minRadius)
        {
            Vector2 delta = (_current.position - _base.position) / _minRadius;

            float theta = _base.angle * Mathf.Deg2Rad;
            Vector2 position = new(
                delta.x * Mathf.Cos(theta) + delta.y * Mathf.Sin(theta),
                -delta.x * Mathf.Sin(theta) + delta.y * Mathf.Cos(theta));

            float angle = _current.angle - _base.angle;

            return new(position, angle);
        }

        #region Path Cases
        // Formula 8.1: CSC (same turns)
        static private List<RSManoeuvre> PathCase1(float _x, float _y, float _phi, bool _flipSteer, bool _flipGear) { 
            PolarVector2 polar = new(new(_x - Mathf.Sin(_phi), _y - 1.0f + Mathf.Cos(_phi)));

            float u = polar.radius;
            float t = polar.theta;
            float v = AngleMaths.ModPI(_phi - polar.theta);

            return new() {
                RSManoeuvre.Mirror(new(t, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _flipSteer, _flipGear),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Straight, RSManoeuvre.Gear.Forward), _flipSteer, _flipGear),
                RSManoeuvre.Mirror(new(v, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _flipSteer, _flipGear)
            };
        }

        // Formula 8.2: CSC (opposite turns)
        static private List<RSManoeuvre> PathCase2(float _x, float _y, float _phi, bool _flipSteer, bool _flipGear) {
            PolarVector2 polar = new(new(_x + Mathf.Sin(_phi), _y - 1.0f - Mathf.Cos(_phi)));

            float rho = polar.radius;
            float t1 = polar.theta;

            if (rho * rho < 4.0f) { return null; }

            float u = Mathf.Sqrt((rho * rho) - 4.0f);
            float t = AngleMaths.ModPI(t1 + Mathf.Atan2(2.0f, u));
            float v = AngleMaths.ModPI(t - _phi);

            return new() {
                RSManoeuvre.Mirror(new(t, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _flipSteer, _flipGear),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Straight, RSManoeuvre.Gear.Forward), _flipSteer, _flipGear),
                RSManoeuvre.Mirror(new(v, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Forward), _flipSteer, _flipGear)
            };
        }

        // Formula 8.3: C|C|C
        static private List<RSManoeuvre> PathCase3(float _x, float _y, float _phi, bool _flipSteer, bool _flipGear) {

            float xi = _x - Mathf.Sin(_phi);
            float eta = _y - 1.0f + Mathf.Cos(_phi);

            PolarVector2 polar = new(new(xi, eta));
            float rho = polar.radius;
            float theta = polar.theta;

            if (rho > 4.0f) { return null; }

            float A = Mathf.Acos(rho / 4.0f);

            float t = AngleMaths.ModPI(theta + (Mathf.PI/2.0f) + A);
            float u = AngleMaths.ModPI(Mathf.PI - (2.0f*A));
            float v = AngleMaths.ModPI(_phi - t - u);

            if (float.IsNaN(t) || float.IsNaN(u) || float.IsNaN(v)) { return null; }

            return new() {
                RSManoeuvre.Mirror(new(t, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _flipSteer, _flipGear),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Backward), _flipSteer, _flipGear),
                RSManoeuvre.Mirror(new(v, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _flipSteer, _flipGear)
            };
        }

        // Formula 8.4 (1): C|CC
        static private List<RSManoeuvre> PathCase4(float _x, float _y, float _phi, bool _flipSteer, bool _flipGear) { 
            float xi = _x - Mathf.Sin(_phi);
            float eta = _y - 1.0f + Mathf.Cos(_phi);

            PolarVector2 polar = new(new(xi, eta));
            float rho = polar.radius;
            float theta = polar.theta;

            if (rho > 4.0f) { return null; }

            float A = Mathf.Acos(rho / 4.0f);

            float t = AngleMaths.ModPI(theta + (Mathf.PI/2.0f) + A);
            float u = AngleMaths.ModPI(Mathf.PI - (2.0f*A));
            float v = AngleMaths.ModPI(t + u - _phi);

            if (float.IsNaN(t) || float.IsNaN(u) || float.IsNaN(v)) { return null; }

            return new() {
                RSManoeuvre.Mirror(new(t, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _flipSteer, _flipGear),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Backward), _flipSteer, _flipGear),
                RSManoeuvre.Mirror(new(v, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Backward), _flipSteer, _flipGear),
            };
        }
        
        // Formula 8.4 (2): CC|C
        static private List<RSManoeuvre> PathCase5(float _x, float _y, float _phi, bool _steerFlipped, bool _gearFlipped) { 

            float xi = _x - Mathf.Sin(_phi);
            float eta = _y - 1.0f + Mathf.Cos(_phi);

            PolarVector2 polar = new(new(xi, eta));
            float rho = polar.radius;
            float theta = polar.theta;

            if (rho > 4.0f) { return null; }

            float u = Mathf.Acos(1.0f - (rho*rho / 8.0f));
            float A = Mathf.Asin(2.0f * Mathf.Sin(u) / rho);
            float t = AngleMaths.ModPI(theta + (Mathf.PI/2.0f) - A);
            float v = AngleMaths.ModPI(t - u - _phi);

            return new() {
                RSManoeuvre.Mirror(new(t, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(v, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped)
            };
        }

        // Formula 8.7: CCu|CuC
        static private List<RSManoeuvre> PathCase6(float _x, float _y, float _phi, bool _steerFlipped, bool _gearFlipped) { 
            float xi = _x + Mathf.Sin(_phi);
            float eta = _y - 1.0f - Mathf.Cos(_phi);

            PolarVector2 polar = new(new(xi, eta));
            float rho = polar.radius;
            float theta = polar.theta;

            if (rho > 4.0f) { return null; }

            float t; float u; float v;

            if (rho > 2.0f) {
                float A = Mathf.Acos((rho + 2.0f) / 4.0f);
                t = AngleMaths.ModPI(theta + (Mathf.PI/2.0f) + A);
                u = AngleMaths.ModPI(A);
            }
            else {
                float A = Mathf.Acos((rho - 2.0f) / 4.0f);
                t = AngleMaths.ModPI(theta + (Mathf.PI/2.0f) - A);
                u = AngleMaths.ModPI(Mathf.PI - A);
            }

            v = AngleMaths.ModPI(_phi - t + (2.0f*u));

            if (float.IsNaN(t) || float.IsNaN(u) || float.IsNaN(v)) { return null; }

            return new() {
                RSManoeuvre.Mirror(new(t, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(v, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped)
            };
        }

        // Formula 8.8: C|CuCu|C
        static private List<RSManoeuvre> PathCase7(float _x, float _y, float _phi, bool _steerFlipped, bool _gearFlipped) {
            float xi = _x + Mathf.Sin(_phi);
            float eta = _y - 1.0f - Mathf.Cos(_phi);

            PolarVector2 polar = new(new(xi, eta));
            float rho = polar.radius;
            float theta = polar.theta;

            float u1 = (20.0f - (rho*rho)) / 16.0f;

            if (rho > 6.0f || 0.0f > u1 || u1 > 1.0f) { return null; }

            float u = Mathf.Acos(u1);
            float A = Mathf.Asin(2.0f * Mathf.Sin(u) / rho);
            float t = AngleMaths.ModPI(theta + (Mathf.PI/2.0f) + A);
            float v = AngleMaths.ModPI(t - _phi);

            if (float.IsNaN(t) || float.IsNaN(u) || float.IsNaN(v)) { return null; }

            return new() {
                RSManoeuvre.Mirror(new(t, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(v, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped)
            };
        }

        // Formula 8.9 (1): C|C[pi/2]SC    
        static private List<RSManoeuvre> PathCase8(float _x, float _y, float _phi, bool _steerFlipped, bool _gearFlipped) { 
            float xi = _x - Mathf.Sin(_phi);
            float eta = _y - 1.0f + Mathf.Cos(_phi);

            PolarVector2 polar = new(new(xi, eta));
            float rho = polar.radius;
            float theta = polar.theta;

            if (rho > 2.0f) { return null; }

            float u = Mathf.Sqrt(rho*rho - 4.0f) - 2.0f;
            float A = Mathf.Atan2(2.0f, u+2.0f);
            float t = AngleMaths.ModPI(theta + Mathf.PI/2.0f + A);
            float v = AngleMaths.ModPI(t - _phi + Mathf.PI/2.0f);

            if (float.IsNaN(t) || float.IsNaN(u) || float.IsNaN(v)) { return null; }

            return new() {
                RSManoeuvre.Mirror(new(t, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(Mathf.PI/2.0f, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Straight, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(v, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped)
            };
        }

        // Formula 8.9 (2): CSC[pi/2]|C
        static private List<RSManoeuvre> PathCase9(float _x, float _y, float _phi, bool _steerFlipped, bool _gearFlipped) { 
            float xi = _x - Mathf.Sin(_phi);
            float eta = _y - 1.0f + Mathf.Cos(_phi);

            PolarVector2 polar = new(new(xi, eta));
            float rho = polar.radius;
            float theta = polar.theta;

            if (rho < 2.0f) { return null; }

            float u = Mathf.Sqrt((rho*rho) - 4.0f) - 2.0f;
            float A = Mathf.Atan2(u+2.0f, 2.0f);
            float t = AngleMaths.ModPI(theta + (Mathf.PI/2.0f) - A);
            float v = AngleMaths.ModPI(t - _phi - (Mathf.PI/2.0f));

            if (float.IsNaN(t) || float.IsNaN(u) || float.IsNaN(v)) { return null; }

            return new() {
                RSManoeuvre.Mirror(new(t, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Straight, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(Mathf.PI/2.0f, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(v, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped)
            };
        }

        // Formula 8.10 (1): C|C[pi/2]SC
        static private List<RSManoeuvre> PathCase10(float _x, float _y, float _phi, bool _steerFlipped, bool _gearFlipped) { 
            float xi = _x + Mathf.Sin(_phi);
            float eta = _y - 1.0f - Mathf.Cos(_phi);

            PolarVector2 polar = new(new(xi, eta));
            float rho = polar.radius;
            float theta = polar.theta;

            if (rho < 2.0f) { return null; }

            float t = AngleMaths.ModPI(theta + (Mathf.PI/2.0f));
            float u = rho - 2.0f;
            float v = AngleMaths.ModPI(_phi - t - (Mathf.PI/2.0f));

            if (float.IsNaN(t) || float.IsNaN(u) || float.IsNaN(v)) { return null; }

            return new() {
                RSManoeuvre.Mirror(new(t , RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(Mathf.PI/2.0f, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Straight, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(v, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped),
            };
        }

        // Formula 8.10 (2): CSC[pi/2]|C
        static private List<RSManoeuvre> PathCase11(float _x, float _y, float _phi, bool _flipSteer, bool _flipGear) { 
            float xi = _x + Mathf.Sin(_phi);
            float eta = _y - 1.0f - Mathf.Cos(_phi);

            PolarVector2 polar = new(new(xi, eta));
            float rho = polar.radius;
            float theta = polar.theta;

            if (rho < 2.0f) { return null; }

            float t = AngleMaths.ModPI(theta);
            float u = rho - 2.0f;
            float v = AngleMaths.ModPI(_phi - t - Mathf.PI/2.0f);

            if (float.IsNaN(t) || float.IsNaN(u) || float.IsNaN(v)) { return null; }

            return new() {
                RSManoeuvre.Mirror(new(t, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _flipSteer, _flipGear),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Straight, RSManoeuvre.Gear.Forward), _flipSteer, _flipGear),
                RSManoeuvre.Mirror(new(Mathf.PI/2.0f, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _flipSteer, _flipGear),
                RSManoeuvre.Mirror(new(v, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Backward), _flipSteer, _flipGear)
            };
        }
        
        // Formula 8.11: C|C[pi/2]SC[pi/2]|C
        static private List<RSManoeuvre> PathCase12(float _x, float _y, float _phi, bool _steerFlipped, bool _gearFlipped) { 
            float xi = _x + Mathf.Sin(_phi);
            float eta = _y - 1.0f - Mathf.Cos(_phi);

            PolarVector2 polar = new(new(xi, eta));
            float rho = polar.radius;
            float theta = polar.theta;

            if (rho < 4.0f) { return null; }

            float u = Mathf.Sqrt(rho*rho - 4.0f) - 4.0f;
            float A = Mathf.Atan2(2.0f, u + 4.0f);
            float t = AngleMaths.ModPI(theta + Mathf.PI/2 + A);
            float v = AngleMaths.ModPI(t - _phi);

            if (float.IsNaN(t) || float.IsNaN(u) || float.IsNaN(v)) { return null; }
            
            return new() {
                RSManoeuvre.Mirror(new(t, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(Mathf.PI/2.0f, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(u, RSManoeuvre.Steer.Straight, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(Mathf.PI/2.0f, RSManoeuvre.Steer.Left, RSManoeuvre.Gear.Backward), _steerFlipped, _gearFlipped),
                RSManoeuvre.Mirror(new(v, RSManoeuvre.Steer.Right, RSManoeuvre.Gear.Forward), _steerFlipped, _gearFlipped)
            };

        }
        #endregion
    }
}