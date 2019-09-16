using UnityEngine;

namespace UniMob.UI
{
    public class AddressableLoadingView : MonoBehaviour, IView
    {
        public RectTransform rectTransform => (RectTransform) transform;

        public WidgetViewReference ViewReference { get; set; }

        public void SetSource(object source)
        {
        }

        public void ResetSource()
        {
        }
    }
}