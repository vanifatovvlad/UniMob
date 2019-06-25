using System;
using JetBrains.Annotations;
using UniMob.Async;
using UniMob.ReView.Widgets;

namespace UniMob.ReView
{
    public static class ReView
    {
        public static void RunApp([NotNull] ContainerView container, [NotNull] WidgetBuilder builder)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var context = new AppContext();
            var state = State.Create(context, builder);
            var render = Atom.CreateReaction(() => container.SetState(state.Value));

            render.Get();
        }

        private class AppContext : BuildContext
        {
        }
    }
}