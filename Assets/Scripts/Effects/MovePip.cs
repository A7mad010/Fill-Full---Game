using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Effects
{
    /// <summary>
    /// Use this class to looping movement
    /// </summary>
    public class MovePip : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform target;

        [Header("Settings")] 
        [SerializeField] private float duration = 1f;
        [SerializeField] private Vector3 offset;

        [Header("Events")]
        [SerializeField] private UnityEvent onArrive;

        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private CancellationTokenSource _cts;

        private void Start()
        {
            SavePoints();
            Play();
        }

        private void Play()
        {
            Stop();
            Loop().Forget();
        }

        private void Stop()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private void SavePoints()
        {
            _startPosition = target.position;
            _endPosition = _startPosition + offset;
        }
        
        private async UniTaskVoid Loop()
        {
            _cts = new CancellationTokenSource();

            while (!_cts.Token.IsCancellationRequested)
            {
                //Go
                await Move(_endPosition, _cts.Token);

                //Back
                await Move(_startPosition, _cts.Token);
            }
        }

        private async UniTask Move(Vector3 targetPosition, CancellationToken token)
        {
            float value = 0f;
            float timer = 0f;
            Vector3 startPos = target.position;

            while (timer < duration)
            {
                token.ThrowIfCancellationRequested();

                timer += Time.deltaTime;
                value = timer / duration;
                target.position = Vector3.Lerp(startPos, targetPosition, value);

                await UniTask.Yield(token);
            }

            target.position = targetPosition;
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}