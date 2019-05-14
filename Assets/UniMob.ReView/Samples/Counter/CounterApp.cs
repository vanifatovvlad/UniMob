using UnityEngine;

namespace UniMob.ReView.Samples.Counter
{
    public class CounterApp : MonoBehaviour
    {
        [SerializeField] private ContainerView root;

        private void Start()
        {
            ReView.RunApp(root, context => new Counter());
        }
    }
}