using System.Collections.Generic;
using System.Linq;
using UniMob.UI.Widgets;
using UnityEngine;

namespace UniMob.UI.Samples.LayoutViewPanelSample
{
    [CreateAssetMenu(menuName = "UniMob UI Samples/LayoutViewPanelSample", fileName = "LayoutViewPanelSample Widget")]
    public class LayoutViewPanelSampleWidget : ScriptableStatefulWidget
    {
        public override Key Key => GlobalKey.Of<LayoutViewPanelSampleWidget>();
        public override State CreateState() => new LayoutViewPanelSampleState();
    }

    public class LayoutViewPanelSampleState : State<LayoutViewPanelSampleWidget>, ILayoutViewPanelSampleState
    {
        private const int ChildrenCount = 100;
        private readonly Atom<IState[]> _children;

        public override WidgetViewReference View { get; }
            = WidgetViewReference.Resource("LayoutViewPanelSample");

        public LayoutViewPanelSampleState()
        {
            _children = CreateChildren(BuildChildren);
        }

        public IState[] Children => _children.Value;

        private List<Widget> BuildChildren(BuildContext context)
        {
            return Enumerable.Range(0, ChildrenCount)
                .Select(CreateChild)
                .ToList();
        }

        private Widget CreateChild(int num)
        {
            return new Container(
                color: Color.Lerp(Color.red, Color.blue, num * 1f / ChildrenCount),
                size: new WidgetSize(200, 200),
                child: new Row(new List<Widget>())
            );
        }
    }
}