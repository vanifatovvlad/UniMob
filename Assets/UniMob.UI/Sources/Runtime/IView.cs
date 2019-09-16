using UnityEngine;

namespace UniMob.UI
{
    public interface IView
    {
        // ReSharper disable once InconsistentNaming
        GameObject gameObject { get; }

        // ReSharper disable once InconsistentNaming
        RectTransform rectTransform { get; }

        WidgetViewReference ViewReference { get; set; }

        void SetSource(object source);
        void ResetSource();
    }
}