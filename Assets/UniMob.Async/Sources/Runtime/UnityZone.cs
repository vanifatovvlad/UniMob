using System;
using System.Threading;
using UnityEngine;

namespace UniMob.Async
{
    internal class UnityZone : MonoBehaviour, IZone
    {
        private static TimerDispatcher _dispatcher;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void Init()
        {
            if (_dispatcher != null) return;

            var go = new GameObject(nameof(UnityZone));
            DontDestroyOnLoad(go);
            go.AddComponent<UnityZone>();
        }

        private void Awake()
        {
            if (_dispatcher != null)
            {
                Destroy(gameObject);
                return;
            }

            var unityThreadId = Thread.CurrentThread.ManagedThreadId;
            _dispatcher = new TimerDispatcher(unityThreadId, HandleUncaughtException);

            Zone.Current = this;
        }

        private void Update()
        {
            _dispatcher.Tick(Time.unscaledTime);
        }

        public void HandleUncaughtException(Exception exception)
        {
            Debug.Log(exception);
        }

        public void Invoke(Action action)
        {
            _dispatcher.Invoke(action);
        }

        public void InvokeDelayed(float delay, Action action)
        {
            _dispatcher.InvokeDelayed(delay, action);
        }
    }
}