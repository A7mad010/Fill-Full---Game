using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace Effects
{
    /// <summary>
    /// Use This class to change cube scale
    /// </summary>
    public class ScaleEffect : MonoBehaviour
    {
        private Vector3 _startScale;
        private bool _initialized;
        private CancellationTokenSource _cts;

        private void Awake()
        {
            SaveScale();
        }

        #region Scale In And Out

        /// <summary>
        /// Change scale size from 0 to start scale
        /// </summary>
        /// <param name="duration"></param>
        public void ScaleIn(float duration)
        {
            EnsureInitialized(); // Make sure we have the original scale
            CancelCurrentTween();
            ToggleScaleSize(_startScale, duration, _cts.Token).Forget();
        }

        /// <summary>
        /// Change scale size from start scale to zero
        /// </summary>
        /// <param name="duration"></param>
        public void ScaleOut(float duration)
        {
            EnsureInitialized();
            CancelCurrentTween();
            ToggleScaleSize(Vector3.zero, duration, _cts.Token).Forget();
        }
        
        /// <summary>
        /// Token Cancel
        /// </summary>
        private void CancelCurrentTween()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
        }
        
        /// <summary>
        /// Change current size to targetScale
        /// </summary>
        /// <param name="targetScale"></param>
        /// <param name="duration"></param>
        /// <param name="token"></param>
        private async UniTaskVoid ToggleScaleSize(Vector3 targetScale, float duration, CancellationToken token)
        {
            float timer = 0;
            Vector3 startScale = transform.localScale;
            
            while (timer < duration)
            {
                if (token.IsCancellationRequested) return;
                
                timer += Time.deltaTime;
                float t = timer / duration;
                
                transform.localScale = Vector3.Lerp(startScale, targetScale, t);
                await UniTask.Yield();
            }
            
            if (!token.IsCancellationRequested)
                transform.localScale = targetScale;
        }

        #endregion

        #region Save Scale

        /// <summary>
        /// To make sure use the original scale
        /// </summary>
        private void EnsureInitialized()
        {
            if (!_initialized)
            {
                SaveScale();
            }
        }

        /// <summary>
        /// Save Scale
        /// </summary>
        private void SaveScale()
        {
            _startScale = transform.localScale;
            _initialized = true;
        }

        #endregion

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}