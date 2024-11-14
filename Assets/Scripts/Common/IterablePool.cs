using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public class IterablePool<T> where T : MonoBehaviour
    {
        private readonly Pool<T> _pool;
        private readonly List<T> _activeList = new();

        public IterablePool(Pool<T> pool)
        {
            _pool = pool;
        }

        public T Get()
        {
            var item = _pool.Get();
            _activeList.Add(item);
            return item;
        }

        public void Release(T item)
        {
            _activeList.Remove(item);
            _pool.Release(item);
        }

        public void Iterate(Action<T> onIterate)
        {
            for (int i = 0, count = _activeList.Count; i < count; i++)
            {
                if (_activeList.Count <= i) continue;
                onIterate(_activeList[i]);
            }
        }
    }
}