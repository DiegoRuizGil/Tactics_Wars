using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        private TreeNode _root;

        private readonly Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        private void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }

        protected abstract TreeNode SetupTree();

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            if (_dataContext.TryGetValue(key, out object value))
                return value;
            else
                return null;
        }

        public bool ClearData(string key)
        {
            return _dataContext.Remove(key);
        }

        public void ClearAllData()
        {
            _dataContext.Clear();
        }

        public void RemoveNonMatchingKeys(string matchingKey)
        {
            List<string> keysToRemove = new List<string>();

            foreach (string key in _dataContext.Keys)
            {
                if (matchingKey != key)
                {
                    keysToRemove.Add(key);
                }
            }

            foreach (string key in keysToRemove)
            {
                _dataContext.Remove(key);
            }
        }

        public bool ContainsKeyInData(string key)
        {
            return _dataContext.ContainsKey(key);
        }
    }
}
