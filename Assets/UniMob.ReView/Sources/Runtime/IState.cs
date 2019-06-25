using UnityEngine;

namespace UniMob.ReView
{
    public interface IState
    {
        Vector2 Size { get; }

        string ViewPath { get; }

        void DidViewMount();
        void DidViewUnmount();
    }
}