using System;
using System.Collections.Generic;
using UniMob.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UniMob.UI.Internal.ViewLoaders
{
    public class InternalViewLoader : IViewLoader
    {
        private readonly Dictionary<string, Func<GameObject>> _builders;
        private readonly Dictionary<string, IView> _cache;

        public InternalViewLoader()
        {
            _cache = new Dictionary<string, IView>();
            _builders = new Dictionary<string, Func<GameObject>>
            {
                ["$$_Column"] = ColumnViewBuilder,
                ["$$_Row"] = RowViewBuilder,
                ["$$_ZStack"] = ZStackViewBuilder,
                ["$$_Container"] = ContainerBuilder,
                ["$$_Navigator"] = NavigatorBuilder,
                ["$$_FadeTransition"] = FadeTransitionBuilder,
            };
        }

        public (IView, WidgetViewReference) LoadViewPrefab(IViewState state)
        {
            var viewReference = state.View;

            if (viewReference.Type != WidgetViewReferenceType.Resource ||
                !viewReference.Path.StartsWith("$$_"))
            {
                return (null, default);
            }

            var name = viewReference.Path;

            if (_cache.TryGetValue(name, out var view))
            {
                return (view, viewReference);
            }

            if (!_builders.TryGetValue(name, out var builder))
            {
                throw new InvalidOperationException("No builder");
            }

            var template = builder();
            Object.DontDestroyOnLoad(template);
            template.hideFlags = HideFlags.HideInHierarchy;

            view = template.GetComponent<IView>();

            _cache.Add(name, view);

            return (view, viewReference);
        }

        private static GameObject NavigatorBuilder()
        {
            return ResetRectSize(new GameObject("Navigator",
                typeof(RectTransform),
                typeof(NavigatorView)));
        }

        private static GameObject FadeTransitionBuilder()
        {
            return ResetRectSize(new GameObject("FadeTransition",
                typeof(RectTransform),
                typeof(CanvasGroup),
                typeof(FadeTransitionView)));
        }

        private static GameObject ColumnViewBuilder()
        {
            return ResetRectSize(new GameObject("Column",
                typeof(RectTransform),
                typeof(ColumnView)));
        }

        private static GameObject RowViewBuilder()
        {
            return ResetRectSize(new GameObject("Row",
                typeof(RectTransform),
                typeof(RowView)));
        }
        
        private static GameObject ZStackViewBuilder()
        {
            return ResetRectSize(new GameObject("ZStack",
                typeof(RectTransform),
                typeof(ZStackView)));
        }

        private static GameObject ContainerBuilder()
        {
            return ResetRectSize(new GameObject("Container",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image),
                typeof(ContainerView)));
        }

        private static GameObject ResetRectSize(GameObject go)
        {
            var rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = Vector2.zero;
            return go;
        }
    }
}