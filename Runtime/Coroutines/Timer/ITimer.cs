using System.Collections;
using UnityEngine;

namespace MaroonSeal
{
    public interface ITimer : IEnumerator
    {
        public void Step(float _deltaTime);
    }
}
