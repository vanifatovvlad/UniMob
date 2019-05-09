using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniMob.Async;
using UnityEngine;

namespace UniMob.ReView
{
    public class LayoutView : View<ILayoutState>
    {
        private ViewMapperBase _mapper;

        protected override void Activate()
        {
            base.Activate();

            if (_mapper == null)
                _mapper = new PooledViewMapper(transform);
        }

        protected override void Render()
        {
            switch (State)
            {
                case IMultiChildLayoutState multiChildLayoutState:
                    var children = multiChildLayoutState.Children;
                    var crossAxis = multiChildLayoutState.CrossAxisAlignment;

                    var anchorX = crossAxis == CrossAxisAlignment.Start ? 0f
                        : crossAxis == CrossAxisAlignment.End ? 1f
                        : 0.5f;

                    var corner = crossAxis == CrossAxisAlignment.Start ? RectTools.Corner.TopLeft
                        : crossAxis == CrossAxisAlignment.End ? RectTools.Corner.TopRight
                        : RectTools.Corner.TopCenter;

                    float y = 0;

                    try
                    {
                        _mapper.BeginRender();
                        foreach (var child in children)
                        {
                            var childSize = CalcChildSize(child);
                            var childView = _mapper.RenderItem(child);

                            var position = new Vector2(0, -y);
                            var pivot = childView.rectTransform.pivot;

                            if (crossAxis == CrossAxisAlignment.Stretch)
                            {
                                childView.rectTransform.anchorMin = new Vector2(0, 1f);
                                childView.rectTransform.anchorMax = new Vector2(1, 1f);

                                childView.rectTransform.anchoredPosition =
                                    RectTools.CornerPositionToAnchored(position, pivot, childSize, corner);
                            }
                            else
                            {
                                childView.rectTransform.anchorMin =
                                    childView.rectTransform.anchorMax = new Vector2(anchorX, 1f);
                                childView.rectTransform.anchoredPosition =
                                    RectTools.CornerPositionToAnchored(position, pivot, childSize, corner);
                            }

                            y += childSize.y;
                        }
                    }
                    finally
                    {
                        _mapper.EndRender();
                    }

                    break;
            }
        }

        private static Vector2 CalcChildSize(IState state)
        {
            var view = ResolveChildViewPrefab(state);
            return view.CalcSize(state);
        }

        private static IView ResolveChildViewPrefab(IState state)
        {
            return ViewContext.Loader.LoadViewPrefab(state);
        }
    }

    public abstract class SingleChildLayoutWidget : Widget
    {
        protected SingleChildLayoutWidget([NotNull] Widget child, Key key = null) : base(key)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        public Widget Child { get; }
    }

    public abstract class MultiChildLayoutWidget : Widget
    {
        protected MultiChildLayoutWidget(
            [NotNull] WidgetList children,
            CrossAxisAlignment crossAxisAlignment = CrossAxisAlignment.Center,
            MainAxisAlignment mainAxisAlignment = MainAxisAlignment.Start,
            MainAxisSize mainAxisSize = MainAxisSize.Min,
            [CanBeNull] Key key = null)
            : base(key)
        {
            Children = children ?? throw new ArgumentNullException(nameof(children));
            CrossAxisAlignment = crossAxisAlignment;
            MainAxisAlignment = mainAxisAlignment;
            MainAxisSize = mainAxisSize;
        }

        public WidgetList Children { get; }
        public CrossAxisAlignment CrossAxisAlignment { get; }
        public MainAxisAlignment MainAxisAlignment { get; }
        public MainAxisSize MainAxisSize { get; }
    }

    public interface ILayoutState : IState
    {
    }

    public interface ISingleChildLayoutState : ILayoutState
    {
        IState Child { get; }
    }

    public interface IMultiChildLayoutState : ILayoutState
    {
        IState[] Children { get; }

        CrossAxisAlignment CrossAxisAlignment { get; }
    }


    public class WidgetList : List<Widget>
    {
    }

    public class MultiChildLayoutState<TWidget> : State<TWidget>, IMultiChildLayoutState
        where TWidget : MultiChildLayoutWidget
    {
        private readonly Atom<IState[]> _children;
        
        public MultiChildLayoutState() : base("UniMob.ReView.Layout")
        {
            _children = CreateList(this, context => Widget.Children);
        }

        public IState[] Children => _children.Value;
        public CrossAxisAlignment CrossAxisAlignment => Widget.CrossAxisAlignment;
    }

    public class Column : MultiChildLayoutWidget
    {
        public Column(
            [NotNull] WidgetList children,
            CrossAxisAlignment crossAxisAlignment = CrossAxisAlignment.Center,
            MainAxisAlignment mainAxisAlignment = MainAxisAlignment.Start,
            MainAxisSize mainAxisSize = MainAxisSize.Max,
            [CanBeNull] Key key = null)
            : base(children, crossAxisAlignment, mainAxisAlignment, mainAxisSize, key)
        {
        }

        public override State CreateState() => new MultiChildLayoutState<Column>();
    }
}