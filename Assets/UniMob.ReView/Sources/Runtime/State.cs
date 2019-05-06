using System;
using UniMob.Async;
using JetBrains.Annotations;
using UnityEngine.Assertions;

namespace UniMob.ReView
{
    public abstract class State : IState, IDisposable
    {
        public string ViewPath { get; }

        protected State([NotNull] string view)
        {
            ViewPath = view ?? throw new ArgumentNullException(nameof(view));
        }

        internal abstract void SetWidget(Widget widget);
        internal abstract void SetContext(BuildContext context);

        public virtual void InitState()
        {
        }

        public virtual void Dispose()
        {
        }

        public virtual void DidViewMount()
        {
        }

        public virtual void DidViewUnmount()
        {
        }
        
        internal static Atom<IState> Create(BuildContext context, WidgetBuilder builder)
        {
            Widget oldWidget = null;
            State state = null;
            return Atom.Func<IState>(() =>
            {
                var newWidget = builder(context);

                bool changed = false;
                if (oldWidget == null || (changed = oldWidget.GetType() != newWidget.GetType() || oldWidget.Key != newWidget.Key))
                {
                    if (changed)
                    {
                        Assert.IsNotNull(state);
                        state.Dispose();
                    }

                    oldWidget = newWidget;

                    state = newWidget.CreateState();
                    state.SetContext(context);
                    state.SetWidget(newWidget);
                    state.InitState();
                }
                else
                {
                    Assert.IsNotNull(state);
                    state.SetWidget(newWidget);
                }

                return state;
            });
        }
    }

    public abstract class State<TWidget> : State, BuildContext
        where TWidget : Widget
    {
        protected State([NotNull] string view) : base(view)
        {
        }

        public TWidget Widget { get; private set; }
        public BuildContext Context { get; internal set; }

        internal override void SetContext(BuildContext context)
        {
            if (Context != null)
                throw new InvalidOperationException();

            Context = context;
        }

        internal sealed override void SetWidget(Widget widget)
        {
            var oldWidget = Widget;
            Widget = (TWidget) widget;

            if (oldWidget != null)
            {
                DidUpdateWidget(oldWidget);
            }
        }

        public virtual void DidUpdateWidget([NotNull] TWidget oldWidget)
        {
        }

        protected Atom<IState> CreateChild(WidgetBuilder builder) => Create(this, builder);
    }
}