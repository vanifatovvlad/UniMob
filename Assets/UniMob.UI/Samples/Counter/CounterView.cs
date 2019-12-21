using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UniMob.UI.Samples.Counter
{
    public class CounterView : View<ICounterState>
    {
        [Header("Texts")]
        [SerializeField]
        private TMP_Text counterText = default;

        [Header("Buttons")]
        [SerializeField]
        private Button incrementButton = default;

        [SerializeField] private Button decrementButton = default;

        protected override void Awake()
        {
            incrementButton.Click(() => State.Increment);
            decrementButton.Click(() => State.Decrement);
        }

        protected override void Render()
        {
            counterText.text = State.CounterText;
        }
    }

    public interface ICounterState : IViewState
    {
        string CounterText { get; }

        void Increment();
        void Decrement();
    }
}