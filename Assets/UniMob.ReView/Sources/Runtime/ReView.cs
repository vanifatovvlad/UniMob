using System;
using JetBrains.Annotations;
using UniMob.Async;
using UniMob.ReView.Widgets;
using UnityEngine;

namespace UniMob.ReView
{
    public static class ReView
    {
        public static IDisposable RunApp([NotNull] ViewPanel root, [NotNull] WidgetBuilder builder)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            IView view = root;
            var context = new AppContext();
            var state = State.Create(context, builder);
            var render = Atom.RunReaction(() => view.SetSource(state.Value));

            // ReSharper disable once ImplicitlyCapturedClosure
            return new ActionDisposable(() =>
            {
                render.Deactivate();

                if (!Engine.IsApplicationQuiting)
                {
                    view.ResetSource();
                }
            });
        }

        private class AppContext : BuildContext
        {
        }
        
        private class ActionDisposable : IDisposable
        {
            private readonly Action _action;
            public ActionDisposable(Action action) => _action = action;
            public void Dispose() => _action?.Invoke();
        }
    }
}