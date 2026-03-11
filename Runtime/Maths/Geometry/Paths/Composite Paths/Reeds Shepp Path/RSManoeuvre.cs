using UnityEngine;

namespace MaroonSeal.Maths.Geometry.Paths.ReedsShepp {
    [System.Serializable]
    public struct RSManoeuvre {
        public float travel;

        public enum Steer { Left = -1, Straight = 0, Right = 1}
        public Steer steer;

        public enum Gear { Forward = 1, Backward = -1 }
        public Gear gear;

        public RSManoeuvre(float _travel, Steer _steer, Gear _gear) {
            travel = _travel;
            steer = _steer;
            gear = (Gear)(((int)_gear) * (_travel < 0.0f ? -1 : 1));
            travel = Mathf.Abs(_travel);
        }

        static public RSManoeuvre Mirror(RSManoeuvre _manoeuvre, bool _flipSteer, bool _flipGear) {
            int steerFlip = _flipSteer ? -1 : 1;
            int gearFlip = _flipGear ? -1 : 1;

            return new(
                _manoeuvre.travel,
                (Steer)(((int)_manoeuvre.steer) * steerFlip),
                (Gear)(((int)_manoeuvre.gear) * gearFlip)
            );
        }
    }
}