using UnityEngine;

namespace UniMob.ReView.Widgets
{
    public class Container : SingleChildLayoutWidget
    {
        public Container(
            Widget child,
            Key key = null,
            Color? color = null,
            Alignment? alignment = null)
            : base(child, key)
        {
            Color = color ?? Color.clear;
            Alignment = alignment ?? Alignment.Center;
        }

        public Color Color { get; }
        public Alignment Alignment { get; }

        public override State CreateState() => new ContainerState();
    }

    public class ContainerState : SingleChildLayoutState<Container>, IContainerState
    {
        public ContainerState() : base("UniMob.Container")
        {
        }

        public Color Color => Widget.Color;

        public Alignment Alignment => Widget.Alignment;

        public override WidgetSize CalculateInnerSize()
        {
            return Child.OuterSize;
        }
    }
}