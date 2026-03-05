using UnityEngine;

namespace MaroonSeal.Utilities.Singletons {
    public class Singleton<TComponent> : MonoBehaviour where TComponent : Component
    {
        [SerializeField] protected bool initialiseOnAwake = true;
        [SerializeField] protected bool unparentOnAwake = true;

        protected static TComponent main;
        public static bool HasMain => main != null;

        public static TComponent Main {
            get {
                if (main == null) {
                    main = FindAnyObjectByType<TComponent>();
                    if (main == null) {
                        var go = new GameObject(typeof(TComponent).Name + " Auto-Generated");
                        main = go.AddComponent<TComponent>();
                    }
                }

                return main;
            }
        }

        #region MonoBehaviour
        protected virtual void Awake() {
            if (initialiseOnAwake) { Initialise(); }
            if (unparentOnAwake) { transform.SetParent(null); }
            
        }
        #endregion

        virtual public void Initialise() {
            if (!Application.isPlaying) { return; }
            main = this as TComponent;
        }
    }
}