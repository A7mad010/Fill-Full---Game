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
            matchManager.OnMatch += OnUpdateMatch;

            HideButtons();
        }

        private void OnUpdateMatch(bool isMatch)
        {
            ResetButtons();
        }
    
        private void ResetButtons()
        {
            foreach (BlockButton button in buttons)
            {
                button.UnClick();   
            }
        }

        public async UniTask ShowButtons()
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
