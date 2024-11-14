using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public class Pool<T> where T : MonoBehaviour
    {
        private readonly Queue<T> _pool = new();
        private readonly Func<T> _onCreate;
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;

        public Pool(int initialCount, Func<T> onCreate, Action<T> onGet, Action<T> onRelease)
        {
            _onGet = onGet;
            _onRelease = onRelease;
            _onCreate = onCreate;

            for (var i = 0; i < initialCount; i++)
            {
                _pool.Enqueue(onCreate());
            }
        }

        public T Get()
        {
            if (_pool.TryDequeue(out var item))
            {
                _onGet(item);
                return item;
            }

            return _onCreate();
        }

        public void Release(T item)
        {
            _pool.Enqueue(item);
        }
    }
}