
namespace UniMob.UI.Internal.ViewLoaders
{
    internal class MultiViewLoader : IViewLoader
    {
        private readonly IViewLoader[] _loaders;

        public MultiViewLoader(params IViewLoader[] loaders)
        {
            _loaders = loaders;
        }

        public (IView, WidgetViewReference) LoadViewPrefab(IState state)
        {
            foreach (var loader in _loaders)
            {
                var (view, reference) = loader.LoadViewPrefab(state);
                if (view != null)
                {
                    return (view, reference);
                }
            }

            return (null, default);
        }
    }
}