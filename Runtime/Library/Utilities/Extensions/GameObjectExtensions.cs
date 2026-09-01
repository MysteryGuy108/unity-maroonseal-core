using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal {
    static public class GameObjectExtensions
    {
        static public TInterface FindInterfaceInComponents<TInterface>(this GameObject _gameObject) where TInterface : class
        {
            Component[] componentList = _gameObject.GetComponents<Component>();
            foreach (Component component in componentList)
            {
                if (component is not TInterface) { continue; }
                return component as TInterface;
            }
            return null;
        }

        static public List<TInterface> FindInterfacesInComponents<TInterface>(this GameObject _gameObject) where TInterface : class
        {
            Component[] componentList = _gameObject.GetComponents<Component>();
            List<TInterface> interfaceList = new();

            foreach (Component component in componentList)
            {
                if (component is not TInterface) { continue; }
                interfaceList.Add(component as TInterface);
            }
            
            return interfaceList;
        }
    }
}
