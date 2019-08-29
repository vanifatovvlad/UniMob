using UniMob.UI.Widgets;
using UnityEngine;

namespace UniMob.UI.Samples.LayoutViewPanelSample
{
    public class LayoutViewPanelSampleView : View<ILayoutViewPanelSampleState>
    {
        [SerializeField] private LayoutViewPanel layoutViewPanel = default;

        protected override void Render()
        {
            layoutViewPanel.Render(State.Children);
        }
    }

    public interface ILayoutViewPanelSampleState : IState
    {
        IState[] Children { get; }
    }
}