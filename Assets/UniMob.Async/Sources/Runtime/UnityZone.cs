using System;
using System.Threading;
using UnityEngine;

namespace UniMob.Async
{
    internal class UnityZone : MonoBehaviour, IZone
    {
        private static TimerDispatcher _dispatcher;

        public Action<Exception> HandleUncaughtException => Debug.LogException;
        public Action<Action> Invoke => _dispatcher.Invoke;
        public Action<float, Action> InvokeDelayed => _dispatcher.InvokeDelayed;

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
    }
}