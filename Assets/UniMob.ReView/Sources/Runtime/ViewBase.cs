using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniMob.ReView
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class ViewBase : UIBehaviour, IViewTreeElement
    {
        [NotNull] protected readonly ViewRenderScope RenderScope = new ViewRenderScope();
        [NotNull] protected readonly List<IViewTreeElement> Children = new List<IViewTreeElement>();

        // ReSharper disable once InconsistentNaming
        public RectTransform rectTransform => (RectTransform) transform;

        protected virtual void Unmount()
        {
            foreach (var child in Children)
            {
                child.Unmount();
            }
        }

        void IViewTreeElement.AddChild(IViewTreeElement view)
        {
            Children.Add(view);
        }

        void IViewTreeElement.Unmount()
        {
            Unmount();
        }
    }
}