using System;
using JetBrains.Annotations;

namespace UniMob.ReView
{
    public static class ReView
    {
        public static void RunApp([NotNull] ViewContainer container, [NotNull] WidgetBuilder builder)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var context = new AppContext();
            container.SetSource(State.Create(context, builder));
        }

        private class AppContext : BuildContext
        {
            public BuildContext Context { get; } = null;
        }
    }
}