using System;
using JetBrains.Annotations;
using UniMob.Async;

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
            var render = Atom.Reaction(() => container.SetState(state.Value));

            render.Get();
        }

        private class AppContext : BuildContext
        {
            public BuildContext Context { get; } = null;
        }
    }
}