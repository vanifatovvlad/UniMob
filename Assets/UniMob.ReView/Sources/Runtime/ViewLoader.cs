using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace UniMob.ReView
{
    public interface IViewLoader
    {
        IView LoadViewPrefab([NotNull] IState model, [NotNull] string path);
    }

    internal class BuiltinResourcesViewLoader : IViewLoader
    {
        private readonly Dictionary<string, IView> _viewPrefabCache = new Dictionary<string, IView>();

        public IView LoadViewPrefab([NotNull] IState model, [NotNull] string path)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (path == null) throw new ArgumentNullException(nameof(path));

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