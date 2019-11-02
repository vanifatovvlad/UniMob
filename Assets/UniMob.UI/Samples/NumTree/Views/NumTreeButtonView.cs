using UnityEngine;
using UnityEngine.UI;

namespace UniMob.UI.Samples.NumTree.Views
{
    public class NumTreeButtonView : View<INumTreeButtonState>
    {
        [SerializeField] private Button button = default;

        protected override void Awake()
        {
            base.Awake();

            button.Click(() => State.Tap);
        }

        protected override void Render()
        {
        }
    }

    public interface INumTreeButtonState : IState
    {
        void Tap();
    }
}