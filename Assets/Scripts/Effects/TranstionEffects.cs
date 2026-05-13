using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Block
{
    public class TranstionEffects : MonoBehaviour
    {
        private Vector3 _startPosition;

        private void Start()
        {
            SavePoints();
        }

        public void ScaleIn(float duration)
        {
            ToggleScaleSize(_startPosition, duration).Forget();
        }

        public void ScaleOut(float duration)
        {
            ToggleScaleSize(Vector3.zero, duration).Forget();
        }

        private void SavePoints()
        {
            _startPosition = transform.localScale;
        }
        
        private async UniTaskVoid ToggleScaleSize(Vector3 targetScale, float duration)
        {
            float value = 0;
            float timer = 0;

            Vector3 startScale = transform.localScale;
            
            while (value < 1)
            {
                timer += Time.deltaTime;
                value = timer / duration;
                
                transform.localScale = Vector3.Lerp(startScale, targetScale, value);
                
                await UniTask.Yield();
            }
            
            transform.localScale = targetScale;
        }
    }
}
