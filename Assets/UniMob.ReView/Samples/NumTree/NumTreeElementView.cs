using UnityEngine;
using UnityEngine.UI;

namespace UniMob.ReView.Samples.NumTree
{
    public class NumTreeElementView : View<INumTreeElementViewState>
    {
        [SerializeField] private Text valueText;
        
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