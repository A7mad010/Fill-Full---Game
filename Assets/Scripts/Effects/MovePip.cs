using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Effects
{
    public class MovePip : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform pipTransform;

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

        public void Play()
        {
            Stop();
            Loop().Forget();
        }

        public void Stop()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
        
        private async UniTaskVoid Loop()
        {
            _cts = new CancellationTokenSource();

            while (!_cts.Token.IsCancellationRequested)
            {
                await Move(_endPosition, _cts.Token);
                onArrive?.Invoke();

                await Move(_startPosition, _cts.Token);
                onArrive?.Invoke();
            }
        }

        private void SavePoints()
        {
            _startPosition = pipTransform.position;
            _endPosition = _startPosition + offset;
        }

        private async UniTask Move(Vector3 targetPosition, CancellationToken token)
        {
            float value = 0f;
            float timer = 0f;
            Vector3 startPos = pipTransform.position;

            while (timer < duration)
            {
                token.ThrowIfCancellationRequested();

                timer += Time.deltaTime;
                value = timer / duration;
                pipTransform.position = Vector3.Lerp(startPos, targetPosition, value);

                await UniTask.Yield(token);
            }

            pipTransform.position = targetPosition;
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}