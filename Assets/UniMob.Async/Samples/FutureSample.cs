using System;
using UnityEngine;

namespace UniMob.Async.Samples
{
    public class FutureSample : MonoBehaviour
    {
        private void Start()
        {
            Future.Value()
                .Then(() => Debug.Log("Start - step 1"))
                .Then(() => Return34())
                .Then(result => Debug.Log($"Start - step 2: result={result}"))
                .Catch<OperationCanceledException>(ex => Debug.Log("Operation cancelled"))
                .Catch(Debug.LogException);
        }

        private Future<int> Return34()
        {
            return Future.Run(() =>
            {
                Debug.Log("Do Smth - step  1");

                return Future.Delayed(TimeSpan.FromSeconds(2))
                    .Then(() => 34)
                    .Then(() => Debug.Log("Do Smth - step  2"));
            });
        }
    }
}