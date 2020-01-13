using UniMob.UI.Internal;
using UniMob.UI.Widgets;
using UnityEngine;

[assembly: RegisterComponentViewFactory("$$_Empty",
    typeof(RectTransform), typeof(EmptyView))]

namespace UniMob.UI.Widgets
{
    public class EmptyView : View<IEmptyState>
    {
        protected override void Render()
        {
        }
    }

    public interface IEmptyState : IViewState
    {
    }
}