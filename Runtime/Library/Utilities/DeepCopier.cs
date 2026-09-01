using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

using UnityEngine;

namespace MaroonSeal.Utilities {
    public static class DeepCopier
    {
        public static TObject ReflectionDeepCopy<TObject>(TObject _target)
        {
            Type type = _target.GetType();
            PropertyInfo[] propertyArray = type.GetProperties();

            TObject clonedObj = (TObject)Activator.CreateInstance(type);

            foreach (PropertyInfo property in propertyArray)
            {
                if (property.CanWrite)
                {
                    object value = property.GetValue(_target);
                    if (value != null && value.GetType().IsClass && !value.GetType().FullName.StartsWith("System."))
                    {
                        property.SetValue(clonedObj, ReflectionDeepCopy(value));
                    }
                    else
                    {
                        property.SetValue(clonedObj, value);
                    }
                }
            }
            return clonedObj;
        }

        public static TObject XMLDeepCopy<TObject>(TObject input)
        {
            using var stream = new MemoryStream();
                    
            var serializer = new XmlSerializer(typeof(TObject));
            serializer.Serialize(stream, input);
            stream.Position = 0;
            return (TObject)serializer.Deserialize(stream);
        }
    }
}
