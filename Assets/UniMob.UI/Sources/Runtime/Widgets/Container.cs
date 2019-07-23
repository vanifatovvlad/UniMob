using JetBrains.Annotations;
using UnityEngine;

namespace UniMob.UI.Widgets
{
    public sealed class Container : SingleChildLayoutWidget
    {
        public Container(
            [NotNull] Widget child,
            [CanBeNull] Key key = null,
            [CanBeNull] Color? color = null,
            [CanBeNull] Alignment? alignment = null)
            : base(child, key)
        {
            Color = color ?? Color.clear;
            Alignment = alignment ?? Alignment.Center;
        }

        public Color Color { get; }
        public Alignment Alignment { get; }

        public override State CreateState() => new ContainerState();
    }

    internal sealed class ContainerState : SingleChildLayoutState<Container>, IContainerState
    {
        public ContainerState() : base("UniMob.Container")
        {
        }

        public Color Color => Widget.Color;

        public Alignment Alignment => Widget.Alignment;

        public override WidgetSize CalculateSize() => Child.Size;
    }
}