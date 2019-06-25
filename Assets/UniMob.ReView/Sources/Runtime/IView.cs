using UnityEngine;

namespace UniMob.ReView
{
    public interface IView
    {
        // ReSharper disable once InconsistentNaming
        GameObject gameObject { get; }

        // ReSharper disable once InconsistentNaming
        RectTransform rectTransform { get; }

        void SetSource(IState source);
        void ResetSource();
    }
}