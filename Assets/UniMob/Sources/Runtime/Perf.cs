using System;

namespace UniMob
{
    public struct Perf : IDisposable
    {
        public Perf(string name)
        {
#if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample(name);
#endif
        }

        public void Dispose()
        {
#if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
#endif
        }
    }
}