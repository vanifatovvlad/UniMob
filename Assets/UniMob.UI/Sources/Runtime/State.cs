using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniMob.UI.Internal;
using UnityEngine.Assertions;

namespace UniMob.UI
{
    public abstract partial class State : IState, IDisposable
    {
        private readonly Atom<WidgetSize> _size;

        public BuildContext Context { get; internal set; }

        public string ViewPath { get; }

        internal Widget Widget { get; private set; }

        public WidgetSize Size => _size.Value;

        protected State([NotNull] string view)
        {
            Assert.IsNull(Atom.CurrentScope);
            ViewPath = view ?? throw new ArgumentNullException(nameof(view));
            _size = Atom.Computed(CalculateSize);
        }

        internal virtual void Update(Widget widget)
        {
            Widget = widget;
        }

        internal void Mount(BuildContext context)
        {
            Assert.IsNull(Atom.CurrentScope);

            if (Context != null)
                throw new InvalidOperationException();

            Context = context;
        }

        public virtual void InitState()
        {
            Assert.IsNull(Atom.CurrentScope);
        }

        public virtual void Dispose()
        {
            Assert.IsNull(Atom.CurrentScope);
        }

        public virtual void DidViewMount()
        {
            Assert.IsNull(Atom.CurrentScope);
        }

        public virtual void DidViewUnmount()
        {
            Assert.IsNull(Atom.CurrentScope);
        }

        public virtual WidgetSize CalculateSize()
        {
            var prefab = ViewContext.Loader.LoadViewPrefab(this);
            var size = prefab.rectTransform.sizeDelta;
            return new WidgetSize(
                size.x > 0 ? size.x : default(float?),
                size.y > 0 ? size.y : default(float?));
        }

        internal static Atom<IState> Create(BuildContext context, WidgetBuilder builder)
        {
            Assert.IsNull(Atom.CurrentScope);

            State state = null;
            return Atom.Computed<IState>(() =>
            {
                var newWidget = builder(context);
                using (Atom.NoWatch)
                {
                    state = StateUtilities.UpdateChild(context, state, newWidget);
                }

                return state;
            }, onInactive: () => StateUtilities.DeactivateChild(state), requiresReaction: true);
        }

        internal static Atom<IState[]> CreateList(BuildContext context, Func<BuildContext, List<Widget>> builder)
        {
            Assert.IsNull(Atom.CurrentScope);

            var states = new State[0];
            return Atom.Computed<IState[]>(() =>
            {
                var newWidgets = builder.Invoke(context);
                using (Atom.NoWatch)
                {
                    states = StateUtilities.UpdateChildren(context, states, newWidgets);
                }

                // ReSharper disable once CoVariantArrayConversion
                return states.ToArray();
            }, onInactive: () =>
            {
                foreach (var state in states)
                {
                    StateUtilities.DeactivateChild(state);
                }
            }, requiresReaction: true);
        }
    }

    public abstract class State<TWidget> : State, BuildContext
        where TWidget : Widget
    {
        private readonly MutableAtom<TWidget> _widget = Atom.Value(default(TWidget));

        protected State([NotNull] string view) : base(view)
        {
        }

        BuildContext BuildContext.Parent => Context;

        protected new TWidget Widget => _widget.Value;

        internal sealed override void Update(Widget widget)
        {
            base.Update(widget);

            var oldWidget = Widget;

            if (widget is TWidget typedWidget)
            {
                _widget.Value = typedWidget;
            }
            else
            {
                throw new Exception($"Trying to pass {widget.GetType()}, but expected {typeof(TWidget)}");
            }

            if (oldWidget != null)
            {
                DidUpdateWidget(oldWidget);
            }
        }

        public virtual void DidUpdateWidget([NotNull] TWidget oldWidget)
        {
            Assert.IsNull(Atom.CurrentScope);
        }

        protected Atom<IState> CreateChild(WidgetBuilder builder) => Create(this, builder);

        protected Atom<IState[]> CreateChildren(Func<BuildContext, List<Widget>> builder)
            => CreateList(this, builder);
    }
}