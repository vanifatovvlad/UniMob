using System;
using JetBrains.Annotations;
using UniMob.Async;
using UniMob.ReView.Widgets;

namespace UniMob.ReView
{
    public static class ReView
    {
        public static void RunApp([NotNull] ViewPanel root, [NotNull] WidgetBuilder builder)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var context = new AppContext();
            var state = State.Create(context, builder);
            var render = Atom.CreateReaction(() => root.SetState(state.Value));

            render.Get();
        }

        private class AppContext : BuildContext
        {
        }
    }
}