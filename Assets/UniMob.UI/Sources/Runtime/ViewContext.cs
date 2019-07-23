using System;

namespace UniMob.UI
{
    public static class ViewContext
    {
        internal static IViewTreeElement CurrentElement;
        internal static IViewLoader Loader = new BuiltinResourcesViewLoader();

        public static bool FirstRender;
        public static int ChildIndex;

        public static IDisposable FirstRenderScope() => new FirstRenderScopeStruct(true);

        private struct FirstRenderScopeStruct : IDisposable
        {
            private readonly bool _old;

            public FirstRenderScopeStruct(bool firstRender)
            {
                _old = FirstRender;
                FirstRender = firstRender;
            }

            public void Dispose()
            {
                FirstRender = _old;
            }
        }
    }
}