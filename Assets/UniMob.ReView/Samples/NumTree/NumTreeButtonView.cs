using UnityEngine;
using UnityEngine.UI;

namespace UniMob.ReView.Samples.NumTree
{
    public class NumTreeButtonView : View<INumTreeButtonState>
    {
        [SerializeField] private Button button;

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