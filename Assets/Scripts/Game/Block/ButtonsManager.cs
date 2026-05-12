using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Block
{
    public class ButtonsManager : MonoBehaviour
    {
        [Header("References ")] 
        [SerializeField] private MatchManager matchManager;
        [SerializeField] private BlockButton[] buttons;

        private void Start()
        {
            if(matchManager == null) return;
            matchManager.OnTriggered += ResetButtons;

            HideButtons();
        }

        public void ShowButton()
        {
            TaskShowButtons().Forget();
        }
    
        private void ResetButtons()
        {
            foreach (BlockButton button in buttons)
            {
                button.UnClick();   
            }
        }

        private async UniTask TaskShowButtons()
        {
            foreach (BlockButton button in buttons)
            {
                button.ShowButton(0.2f);
                await UniTask.Yield();
            }
        }
        
        private void HideButtons()
        {
            foreach (BlockButton button in buttons)
            {
                button.HideButton(0);
            }
        }
    }
}
