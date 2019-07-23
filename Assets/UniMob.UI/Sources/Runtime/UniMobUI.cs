using System;
using JetBrains.Annotations;
using UniMob.Async;
using UniMob.UI.Widgets;

namespace UniMob.UI
{
    public static class UniMobUI
    {
        public static IDisposable RunApp([NotNull] ViewPanel root, [NotNull] WidgetBuilder builder)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            IView view = root;
            var context = new AppContext();
            var state = State.Create(context, builder);
            var render = Atom.AutoRun(() => view.SetSource(state.Value));

            // ReSharper disable once ImplicitlyCapturedClosure
            return new ActionDisposable(() =>
            {
                render.Dispose();

                if (!Engine.IsApplicationQuiting)
                {
                    view.ResetSource();
                }
            });
        }

        private class AppContext : BuildContext
        {
            public BuildContext Parent => null;
            public TState AncestorStateOfType<TState>() where TState : IState => default;
            public TState RootAncestorStateOfType<TState>() where TState : IState => default;
        }

        private class ActionDisposable : IDisposable
        {
            private readonly Action _action;
            public ActionDisposable(Action action) => _action = action;
            public void Dispose() => _action?.Invoke();
        }
    }
}