using System;
using System.Collections.Generic;
using System.Linq;
using Core.Attributes;
using Cysharp.Threading.Tasks;
using Game.Block;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// This Class used to read the match between two walls
    /// </summary>
    public class MatchManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BlocksGroup blocksGroup;
        
        [Header("Settings")] 
        [SerializeField ,Tag] private string matchBlockUseTag;
        
        [Tooltip("Events")]
        public event Action OnTriggered;
        public event Action OnMatch;
        public event Action OnUnMatch;

        private BlocksGroup _groupTriggered;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(matchBlockUseTag))
            {
                _groupTriggered = other.gameObject.GetComponent<BlocksGroup>();
                Debug.Log("triggered");
                
                Match().Forget();
            }
            else
            {
                Debug.Log($"tag {other.tag} isn't found");
            }
        }

        private async UniTask Match()
        {
            if (!_groupTriggered)
            {
                return;
            }
            
            await UniTask.WaitUntil(() => Vector3.Distance(blocksGroup.transform.position, _groupTriggered.transform.position) < 1f);

            if (IsMatch(_groupTriggered))
            {
                OnMatch?.Invoke();
            }
            else
            {
                OnUnMatch?.Invoke();
            }

            OnTriggered?.Invoke();
            ResetTargetWll();
        }
        
        private bool IsMatch(BlocksGroup matchWith)
        {
            List<BlockIdentity> targetBlocks = matchWith.GetBlocks();
            List<BlockIdentity> myBlocks = blocksGroup.GetBlocks();
            
            if(targetBlocks.Count != myBlocks.Count)
            {
                return false;
            }

            foreach (BlockIdentity block in targetBlocks)
            {
                bool match = myBlocks.Any(b => b.column == block.column && b.row == block.row);

                if (!match)
                {
                    return false;
                }
            }

            foreach (BlockIdentity block in myBlocks)
            {
                bool match = targetBlocks.Any(b => b.column == block.column && b.row == block.row);
                
                if (!match)
                {
                    return false;
                }
            }
            
            return true;
        }

        private void ResetTargetWll()
        {
            _groupTriggered.HidAll().Forget();
        }
    }
}
