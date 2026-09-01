using UnityEngine;

namespace MaroonSeal {
    static public class ColliderExtensions
    {
        static readonly Collider[] overlapCache = new Collider[32];

        static public bool ComputePenetrationInLayer(this Collider _source, LayerMask _layerMask, out Vector3 _translation, float _epsilon = 0.001f)
        {
            _translation = Vector3.zero;
            if (!_source) { return false; }

            int count = Physics.OverlapBoxNonAlloc(_source.bounds.center, _source.bounds.extents, overlapCache, _source.transform.rotation, _layerMask);

            bool collided = false;
            for (int i = 0; i < count; i++)
            {
                Collider other = overlapCache[i];
                if (_source == other) { continue; }

                if (_source.ComputeColliderPenetration(other, out Vector3 penetrationDisplacement))
                {
                    collided = true;
                    _translation += penetrationDisplacement;
                }
            }
            
            _translation += _translation.normalized * _epsilon;
            return collided;
        }

        static public bool ComputeColliderPenetration(this Collider _source, Collider _target, out Vector3 _translation)
        {
            _translation = Vector3.zero;
            if (!_source || !_target) { return false; }

            bool collided = Physics.ComputePenetration(
                    _source, _source.transform.position, _source.transform.rotation,
                    _target, _target.transform.position, _target.transform.rotation,
                    out Vector3 direction, out float distance);

            _translation = direction * distance;
            return collided; 
        }
    }
}

