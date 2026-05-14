using Block;
using Cysharp.Threading.Tasks;
using Effects;
using Game.Block;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Block_Buttons
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
        private ScaleEffect _scaleEffect;

        #region Initilaze

        private void Start()
        {
            _scaleEffect = blockIdentity.GetComponent<ScaleEffect>();
            
            Initialize();
        }

        private void Initialize()
        {
            UnClick();
        }

        #endregion
        
        #region Click Button

        public void ToggleBlock()
        {
            if (!blocksGroup || !_scaleEffect || !blockIdentity) return;
            
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
            _scaleEffect.ScaleIn(0.2f);
            
            imageButton.color = pressedColor;
            _isAdded = true;
        }

        public void UnClick()
        {
            blocksGroup.RemoveBlock(blockIdentity);
            _scaleEffect.ScaleOut(0.2f);
            
            imageButton.color = unpressedColor;
            _isAdded = false;
        }

        #endregion

        #region Hide And Show Button

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

        #endregion
        
        
    }
}
