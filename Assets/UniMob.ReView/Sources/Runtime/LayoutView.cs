using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniMob.Async;
using UnityEngine;

namespace UniMob.ReView
{
    public abstract class LayoutView<TState> : View<TState>
        where TState : IState
    {
        protected ViewMapperBase Mapper { get; private set; }

        protected override void Activate()
        {
            base.Activate();

            if (Mapper == null)
                Mapper = new PooledViewMapper(transform);
        }
/*
        protected static void ApplySizeAndPosition(IState viewState, IView view, Alignment alignment, Vector2 position)
        {
            ApplySize(viewState, view, alignment.ToAnchor());
            
            var pivot = view.rectTransform.pivot;
            view.rectTransform.anchoredPosition =
                RectTools.PositionToAnchored(position, pivot, viewState.Size, alignment);
        }
        
        protected static void ApplySize(IState viewState, IView view, Vector2 anchor)
        {
            var size = viewState.Size;

            var layoutState = viewState as ILayoutState;
            var widthStretch = layoutState?.StretchHorizontal ?? false;
            var heightStretch = layoutState?.StretchVertical ?? false;
            
            SetSize(view.rectTransform, size, anchor, widthStretch, heightStretch);
        }*/

        protected static void SetPosition(RectTransform rt, Vector2 size, Vector2 position, Alignment corner)
        {
            var pivot = rt.pivot;
            rt.anchoredPosition = RectTools.PositionToAnchored(position, pivot, size, corner);
        }

        protected static void SetSize(RectTransform rt, Vector2 size, Vector2 anchor,
            bool widthStretch, bool heightStretch)
        {
            var sizeDelta = size;
            var anchorMin = anchor;
            var anchorMax = anchor;

            if (widthStretch)
            {
                sizeDelta.x = 0;
                anchorMin.x = 0;
                anchorMax.x = 1;
            }

            if (heightStretch)
            {
                sizeDelta.y = 0;
                anchorMin.y = 0;
                anchorMax.y = 1;
            }

            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.sizeDelta = sizeDelta;
        }
    }

    public abstract class LayoutWidget : Widget
    {
        public bool StretchHorizontal { get; set; }
        public bool StretchVertical { get; set; }
    }

    public abstract class SingleChildLayoutWidget : Widget
    {
        protected SingleChildLayoutWidget([NotNull] Widget child)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        public Widget Child { get; }
    }

    public abstract class MultiChildLayoutWidget : LayoutWidget
    {
        protected MultiChildLayoutWidget([NotNull] WidgetList children)
        {
            Children = children ?? throw new ArgumentNullException(nameof(children));
        }

        public WidgetList Children { get; }
    }

    public interface ILayoutState : IState
    {
        bool StretchVertical { get; }
        bool StretchHorizontal { get; }
    }

    public interface ISingleChildLayoutState : ILayoutState
    {
        IState Child { get; }
    }

    public interface IMultiChildLayoutState : ILayoutState
    {
        IState[] Children { get; }
    }


    public class WidgetList : List<Widget>
    {
    }

    public abstract class SingleChildLayoutState<TWidget> : State<TWidget>, ISingleChildLayoutState
        where TWidget : SingleChildLayoutWidget
    {
        private readonly Atom<IState> _child;

        protected SingleChildLayoutState([NotNull] string view) : base(view)
        {
            _child = Create(this, context => Widget.Child);
        }

        public IState Child => _child.Value;
        public bool StretchVertical => false;
        public bool StretchHorizontal => false;
    }

    public abstract class MultiChildLayoutState<TWidget> : State<TWidget>, IMultiChildLayoutState
        where TWidget : MultiChildLayoutWidget
    {
        private readonly Atom<IState[]> _children;

        protected MultiChildLayoutState(string view) : base(view)
        {
            _children = CreateList(this, context => Widget.Children);
        }

        public IState[] Children => _children.Value;
        public bool StretchVertical => Widget.StretchVertical;
        public bool StretchHorizontal => Widget.StretchHorizontal;
    }
}