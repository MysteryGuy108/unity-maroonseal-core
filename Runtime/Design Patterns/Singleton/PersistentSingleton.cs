using UnityEngine;

namespace MaroonSeal.Utilities.Singletons {
    public class PersistentSingleton<TComponent> : Singleton<TComponent> where TComponent : Component
    {
        override public void Initialise() {
            if (!Application.isPlaying) { return; }

            if (main == null) {
                main = this as TComponent;
                DontDestroyOnLoad(gameObject);
            } 
            else {
                if (main != this) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
