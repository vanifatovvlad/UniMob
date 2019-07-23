using UnityEngine;
using UnityEngine.UI;

namespace UniMob.UI.Samples.NumTree
{
    public class NumTreeElementView : View<INumTreeElementViewState>
    {
        [SerializeField] private Text valueText = default;

        protected override void Render()
        {
            valueText.text = State.Value.ToString();
        }
    }

    public interface INumTreeElementViewState : IState
    {
        int Value { get; }
    }
}