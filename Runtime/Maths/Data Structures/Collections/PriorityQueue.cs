using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {

    public class PriorityQueue<TData>
    {
        struct QueueElement<TElementData> {
            public TElementData data;
            public float weight;
            public QueueElement(TElementData _data, float _weight) {
                data = _data;
                weight = _weight;
            }
        }

        private List<QueueElement<TData>> queueElements;
        private Dictionary<TData, QueueElement<TData>> queueElementLookup;

        public PriorityQueue() {
            queueElements = new List<QueueElement<TData>>();
            queueElementLookup = new Dictionary<TData, QueueElement<TData>>();
        }

        ~PriorityQueue() {
            queueElements.Clear();
            queueElementLookup.Clear();
        }

        public int Count() {
            return queueElements.Count;
        }

        public void Enqueue(TData _data, float _weight) {
            if (queueElementLookup.ContainsKey(_data)) {
                queueElements.Remove(queueElementLookup[_data]);
                queueElementLookup.Remove(_data);
            }

            int targetIndex = queueElements.Count;
            for (int i = 0; i < queueElements.Count; i++) {
                if (queueElements[i].weight > _weight) {
                    targetIndex = i;
                    break;
                }
            }

            QueueElement<TData> newElement = new QueueElement<TData>(_data, _weight);
            queueElements.Insert(targetIndex, newElement);
            queueElementLookup.Add(_data, newElement);
        }

        public TData Dequeue() {
            if (queueElements.Count <= 0) { return default; }
            QueueElement<TData> element = queueElements[0];

            queueElements.RemoveAt(0);
            queueElementLookup.Remove(element.data);

            return element.data;
        }

        public void Clear() {
            queueElements.Clear();
        }
    }
}