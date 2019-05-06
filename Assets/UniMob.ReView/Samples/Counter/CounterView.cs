using UniMob.Async;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UniMob.ReView.Samples.Counter
{
    public class CounterView : View<ICounterState>
    {
        [Header("Texts")]
        [SerializeField] private TMP_Text counterText;
        
        [Header("Buttons")]
        [SerializeField] private Button incrementButton;
        [SerializeField] private Button decrementButton;

        protected override void Awake()
        {
            incrementButton.Click(() => State.Increment);
            decrementButton.Click(() => State.Decrement);
        }

        protected override void Render()
        {
            counterText.text = State.Counter.Value;
        }
    }

    public interface ICounterState : IState
    {
        Atom<string> Counter { get; }

        void Increment();
        void Decrement();
    }
}