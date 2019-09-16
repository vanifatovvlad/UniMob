using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace UniMob.UI.Internal.ViewLoaders
{
    internal class AddressableViewLoader : IViewLoader
    {
        private readonly Dictionary<string, IView> _viewPrefabCache = new Dictionary<string, IView>();

        private readonly Dictionary<string, Data> _loaders = new Dictionary<string, Data>();

        private class Data
        {
            public AsyncOperationHandle<GameObject> Handle;
            public MutableAtom<WidgetViewReference> ViewRef;
        }

        private IView _loadingViewPrefab;

        public (IView, WidgetViewReference) LoadViewPrefab(IState state)
        {
            var viewReference = state.View;

            if (viewReference.Type != WidgetViewReferenceType.Addressable)
            {
                return (null, default);
            }

            var path = viewReference.Path;

            if (_viewPrefabCache.TryGetValue(path, out var cachedView))
            {
                return (cachedView, viewReference);
            }

            if (!_loaders.TryGetValue(path, out var data))
            {
                var op = Addressables.LoadAssetAsync<GameObject>(path);
                var tempReference = Atom.Value(WidgetViewReference.Resource("ADDR__loading__"));

                data = new Data
                {
                    Handle = op,
                    ViewRef = tempReference,
                };

                _loaders.Add(path, data);

                op.Completed += handle =>
                {
                    if (handle.Result == null)
                    {
                        Debug.LogError($"Failed to load addressable '{path}' for '{state.GetType().Name}'. " +
                                       "Invalid path?");
                        return;
                    }

                    var prefab = handle.Result;
                    var view = prefab.GetComponent<IView>();
                    if (view == null)
                    {
                        Debug.LogError($"Failed to get IView from addressable '{path}' for '{state.GetType().Name}'. " +
                                       "Missing view component?");
                        return;
                    }

                    _viewPrefabCache.Add(path, view);
                    tempReference.Value = WidgetViewReference.Resource("ADDR__loaded__");
                };
            }

            if (_loadingViewPrefab == null)
            {
                var go = new GameObject(nameof(AddressableLoadingView),
                    typeof(RectTransform), typeof(AddressableLoadingView));
                Object.DontDestroyOnLoad(go);

                _loadingViewPrefab = go.GetComponent<IView>();
                _loadingViewPrefab.rectTransform.sizeDelta = Vector2.one;
            }

            return (_loadingViewPrefab, new WidgetViewReference(data.ViewRef));
        }
    }
}