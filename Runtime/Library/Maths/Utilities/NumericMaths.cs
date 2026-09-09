using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths {
    static public class NumericMaths
    {
        const float EPSILON = 0.0001f;

        #region Newton Raphson Root
        static public float NewtonRaphsonRoot(Func<float, float> _func, Func<float, float> _derivitiveFunc, float _x) {
            float h = _func(_x) / _derivitiveFunc(_x);

            while (Math.Abs(h) >= EPSILON) {
                h = _func(_x) / _func(_x);
                _x -= h;
            }

            return _x;
        }

        static public double NewtonRaphsonRoot(Func<double, double> _func, Func<double, double> _derivitiveFunc, double _x) {
            double h = _func(_x) / _derivitiveFunc(_x);

            while (Math.Abs(h) >= EPSILON) {
                h = _func(_x) / _func(_x);
                _x -= h;
            }

            return _x;
        }
        #endregion

        static public float FindLocalMinimum(float _minT, float _maxT, Func<float, float> _timeWeightFunction)
        {
            float min = _minT;
            float max = _maxT;
            float mid;
        
            do {
                mid = (max + min) / 2.0f;

                float minWeight = _timeWeightFunction(mid-EPSILON);
                float maxWeight = _timeWeightFunction(mid+EPSILON);

                if (minWeight < maxWeight) { max = mid; }
                else{ min = mid; }

            } while (max - min > EPSILON);

            return mid;
        }
    }
}
