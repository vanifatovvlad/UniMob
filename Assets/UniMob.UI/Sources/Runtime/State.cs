using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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

        protected virtual void Update(Widget widget)
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

        internal static bool CanUpdateWidget(Widget oldWidget, Widget newWidget)
        {
            return oldWidget.Key == newWidget.Key &&
                   //TODO: optimize GetType() call
                   oldWidget.GetType() == newWidget.GetType();
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
                    state = UpdateChild(context, state, newWidget);
                }

                return state;
            });
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
                    states = UpdateChildren(context, states, newWidgets);
                }

                // ReSharper disable once CoVariantArrayConversion
                return states.ToArray();
            });
        }

        private static State[] UpdateChildren(BuildContext context, State[] oldChildren, List<Widget> newWidgets)
        {
            var newChildrenTop = 0;
            var oldChildrenTop = 0;
            var newChildrenBottom = newWidgets.Count - 1;
            var oldChildrenBottom = oldChildren.Length - 1;

            var newChildren = oldChildren.Length == newWidgets.Count ? oldChildren : new State[newWidgets.Count];

            // Update the top of the list.
            while ((oldChildrenTop <= oldChildrenBottom) && (newChildrenTop <= newChildrenBottom))
            {
                var oldChild = oldChildren[oldChildrenTop];
                var newWidget = newWidgets[newChildrenTop];

                if (!CanUpdateWidget(oldChild.Widget, newWidget))
                    break;

                var newChild = UpdateChild(context, oldChild, newWidget);
                newChildren[newChildrenTop] = newChild;
                newChildrenTop += 1;
                oldChildrenTop += 1;
            }


            // Scan the bottom of the list.
            while ((oldChildrenTop <= oldChildrenBottom) && (newChildrenTop <= newChildrenBottom))
            {
                var oldChild = oldChildren[oldChildrenBottom];
                var newWidget = newWidgets[newChildrenBottom];

                if (!CanUpdateWidget(oldChild.Widget, newWidget))
                    break;

                oldChildrenBottom -= 1;
                newChildrenBottom -= 1;
            }

            // Scan the old children in the middle of the list.
            var haveOldChildren = oldChildrenTop <= oldChildrenBottom;
            Dictionary<Key, State> oldKeyedChildren = null;
            if (haveOldChildren)
            {
                oldKeyedChildren = Pools.KeyToState.Get();
                while (oldChildrenTop <= oldChildrenBottom)
                {
                    var oldChild = oldChildren[oldChildrenTop];
                    if (oldChild.Widget.Key != null)
                    {
                        oldKeyedChildren[oldChild.Widget.Key] = oldChild;
                    }
                    else
                    {
                        DeactivateChild(oldChild);
                    }

                    oldChildrenTop += 1;
                }
            }

            // Update the middle of the list.
            while (newChildrenTop <= newChildrenBottom)
            {
                State oldChild = null;
                var newWidget = newWidgets[newChildrenTop];
                if (haveOldChildren)
                {
                    var key = newWidget.Key;
                    if (key != null)
                    {
                        if (oldKeyedChildren.TryGetValue(key, out oldChild))
                        {
                            if (CanUpdateWidget(oldChild.Widget, newWidget))
                            {
                                // we found a match!
                                // remove it from oldKeyedChildren so we don't unsync it later
                                oldKeyedChildren.Remove(key);
                            }
                            else
                            {
                                // Not a match, let's pretend we didn't see it for now.
                                oldChild = null;
                            }
                        }
                    }
                }

                var newChild = UpdateChild(context, oldChild, newWidget);
                newChildren[newChildrenTop] = newChild;
                newChildrenTop += 1;
            }

            // We've scanned the whole list.
            Assert.IsTrue(oldChildrenTop == oldChildrenBottom + 1);
            Assert.IsTrue(newChildrenTop == newChildrenBottom + 1);
            Assert.IsTrue(newWidgets.Count - newChildrenTop == oldChildren.Length - oldChildrenTop);
            newChildrenBottom = newWidgets.Count - 1;
            oldChildrenBottom = oldChildren.Length - 1;

            // Update the bottom of the list.
            while ((oldChildrenTop <= oldChildrenBottom) && (newChildrenTop <= newChildrenBottom))
            {
                var oldChild = oldChildren[oldChildrenTop];
                var newWidget = newWidgets[newChildrenTop];
                var newChild = UpdateChild(context, oldChild, newWidget);
                newChildren[newChildrenTop] = newChild;
                newChildrenTop += 1;
                oldChildrenTop += 1;
            }

            // Clean up any of the remaining middle nodes from the old list.
            if (haveOldChildren && oldKeyedChildren.Count > 0)
            {
                foreach (var pair in oldKeyedChildren)
                {
                    var oldChild = pair.Value;
                    DeactivateChild(oldChild);
                }
            }

            if (oldKeyedChildren != null)
            {
                Pools.KeyToState.Recycle(oldKeyedChildren);
            }

            return newChildren;
        }

        private static void DeactivateChild([NotNull] State child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            Assert.IsNull(Atom.CurrentScope);

            child.Dispose();
        }

        private static State UpdateChild(BuildContext context, [CanBeNull] State child, [NotNull] Widget newWidget)
        {
            Assert.IsNull(Atom.CurrentScope);

            if (child != null)
            {
                if (child.Widget == newWidget)
                {
                    return child;
                }

                if (CanUpdateWidget(child.Widget, newWidget))
                {
                    child.Update(newWidget);
                    return child;
                }

                DeactivateChild(child);
            }

            return InflateWidget(context, newWidget);
        }

        private static State InflateWidget(BuildContext context, [NotNull] Widget newWidget)
        {
            if (newWidget == null) throw new ArgumentNullException(nameof(newWidget));
            Assert.IsNull(Atom.CurrentScope);

            var newChild = newWidget.CreateState();
            newChild.Mount(context);
            newChild.Update(newWidget);
            newChild.InitState();
            return newChild;
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

        protected sealed override void Update(Widget widget)
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
    }
}