using UniMob.ReView.Widgets;
using UnityEngine;

namespace UniMob.ReView.Samples.Counter
{
    public class CounterApp : MonoBehaviour
    {
        [SerializeField] private ViewPanel root;

        private void Start()
        {
            ReView.RunApp(root, context => new Counter());
        }
    }
}