using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal.Maths.DataStructures {

    [System.Serializable]
    public class Checklist<TData> : IEnumerable<Checklist<TData>.Step>
    {
        [System.Serializable]
        public class Step {
            [SerializeField] private TData data;
            public TData Data { get { return data; } set { data = value; }}

            [SerializeField] private bool isComplete;
            public bool IsComplete { get { return isComplete; } }
            
            [SerializeField] private bool canBeUnchecked;
            public bool CanBeUnchecked { get { return canBeUnchecked; } }

            public Step(TData _data) {
                data = _data;
                
                canBeUnchecked = false;
                isComplete = false;
            }

            public Step(TData _data, bool _canBeUnchecked) {
                data = _data;
                canBeUnchecked = _canBeUnchecked;
                isComplete = false;
            }

            public void SetComplete(bool _isComplete) {
                if (isComplete && !canBeUnchecked) { return; }
                isComplete = _isComplete; 
            }

            public void ResetCompletedState() { isComplete = false; }
        }

        [SerializeField] private List<Step> stepList;
        public int StepCount { get { return stepList.Count; } }

        public Step this[int _index] { get { return stepList[_index]; } }

        #region Constructors and Destructors
        public Checklist() {
            stepList = new(); 
        }

        ~Checklist() { stepList.Clear(); }
        #endregion
        
        public List<Step> GetStepList() {
            return new List<Step>(stepList);
        }
        
        public List<TData> GetDataList() {
            List<TData> dataList = new();
            foreach(Step checkStep in stepList) { dataList.Add(checkStep.Data); }
            return dataList;
        }

        public void AddStep(Step _newItem) {
            if (stepList.Contains(_newItem)) { return; }
            _newItem.ResetCompletedState();
            stepList.Add(_newItem);
        }

        public bool GetIsComplete() { 
            return GetIsRangeComplete(0, stepList.Count);
        }

        public bool GetIsRangeComplete(int _start, int _count) {
            List<Step> stepRange = stepList.GetRange(_start, _count);

            bool isComplete = true;

            foreach(Step checkStep in stepRange) {
                isComplete &= checkStep.IsComplete; 
            }

            return isComplete;
        }

        public List<Step> GetIncompleteSteps() {
            List<Step> incompleteItems = new();

            foreach(Step checkStep in stepList) {
                if (checkStep.IsComplete) { continue; }
                incompleteItems.Add(checkStep);
            }

            return incompleteItems;
        }

        public Step GetNextIncompleteStep() {
            List<Step> incompleteItems = GetIncompleteSteps();
            if (incompleteItems.Count <= 0) { return null; }
            Step firstStep = incompleteItems[0];
            incompleteItems.Clear();
            return firstStep;
        }

        public void ResetSteps() {
            foreach(Step condition in stepList) { condition.ResetCompletedState();  }
        }

        public void Clear() { stepList.Clear(); }

        public int GetCompletedCount() { 
            int count = 0;
            foreach(Step checkStep in stepList) {
                if (checkStep.IsComplete) { count++; }
            }
            return count;
        }

        public float GetCompletedAmount() { return GetCompletedCount() / (float)StepCount; }

        #region IEnumerable
        public IEnumerator<Step> GetEnumerator() { return stepList.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return stepList.GetEnumerator(); }
        #endregion
    }
}