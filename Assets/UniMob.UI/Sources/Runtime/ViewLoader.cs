using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace UniMob.UI
{
    public interface IViewLoader
    {
        IView LoadViewPrefab([NotNull] IState model);
    }

    internal class BuiltinResourcesViewLoader : IViewLoader
    {
        private readonly Dictionary<string, IView> _viewPrefabCache = new Dictionary<string, IView>();

        public IView LoadViewPrefab(IState model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var path = model.ViewPath;

            if (_viewPrefabCache.TryGetValue(path, out var view))
                return view;

            var prefab = Resources.Load(path) as GameObject;
            if (prefab == null)
            {
                Debug.LogError($"Failed to load prefab '{path}' for '{model.GetType().Name}'. " +
                               "Invalid path?");
                return null;
            }

            view = prefab.GetComponent<IView>();
            if (view == null)
            {
                Debug.LogError($"Failed to get IView from prefab '{path}' for '{model.GetType().Name}'. " +
                               "Missing view component?");
                return null;
            }

            _viewPrefabCache.Add(path, view);

            return view;
        }
    }
}