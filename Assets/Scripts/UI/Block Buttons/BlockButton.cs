using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Block
{
    public class BlockButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BlocksGroup blocksGroup;
        [SerializeField] private BlockIdentity blockIdentity;
        [SerializeField] private RectTransform blockTransform;
        [SerializeField] private Image imageButton;
        
        
        [Header("Settings")]
        [SerializeField] private Color pressedColor;
        [SerializeField] private Color unpressedColor;

        private bool _isAdded = false;
        private Vector3 _startScale;
        private TranstionEffects _transtionEffects;

        private void Start()
        {
            _transtionEffects = blockIdentity.GetComponent<TranstionEffects>();
            
            Initialize();
        }

        public void ToggleBlock()
        {
            if (!blocksGroup || !_transtionEffects || !blockIdentity) return;
            
            if (_isAdded)
            {
                UnClick();
               
            }
            else
            {
                Click();

            }
        }

        public void Click()
        {
            blocksGroup.AddBlock(blockIdentity);
            _transtionEffects.ScaleIn(0.2f);
            
            imageButton.color = pressedColor;
            _isAdded = true;
        }

        public void UnClick()
        {
            blocksGroup.RemoveBlock(blockIdentity);
            _transtionEffects.ScaleOut(0.2f);
            
            imageButton.color = unpressedColor;
            _isAdded = false;
        }

        public void HideButton(float duration)
        {
            ToggleScale(Vector3.zero, duration).Forget();
        }

        public void ShowButton(float duration)
        {
            ToggleScale(Vector3.one, duration).Forget();
        }

        private async UniTask ToggleScale(Vector3 targetScale, float duration)
        {
            float value = 0;
            float timer = 0;

            Vector3 startScale = blockTransform.localScale;
            Vector3 endScale = targetScale;

            while (value < 1)
            {
                timer += Time.deltaTime;
                value = timer / duration;
                
                blockTransform.localScale = Vector3.Lerp(startScale, endScale, value);
                
                await UniTask.Yield();
            }
            
            blockTransform.localScale = endScale;
        }
        
        private void Initialize()
        {
            UnClick();
        }
        
    }
}
