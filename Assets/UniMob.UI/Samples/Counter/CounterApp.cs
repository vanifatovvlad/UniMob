using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UniMob.UI.Samples.Counter
{
    public class CounterApp : UniMobUIApp
    {
        [SerializeField] private AssetReferenceGameObject counterView = default;

        protected override void Initialize()
        {
            StateProvider.Register<Counter>(() => new CounterState(counterView));
        }

        protected override Widget Build(BuildContext context)
        {
            return new Counter(
                min: 10,
                max: 100
            );
        }
    }
}