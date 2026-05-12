using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Block;
using UnityEngine;

namespace Game
{
    public class MatchManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BlocksGroup blocksGroup;

        [Header("Settings")] [SerializeField] private string matchBlockUseTag;
        
        [Tooltip("Events")]
        public event Action<bool> OnMatch;

        private BlocksGroup _groupTriggered;
        
        private async UniTask Distance()
        {
            if (!_groupTriggered)
            {
                return;
            }
            
            await UniTask.WaitUntil(() => Vector3.Distance(blocksGroup.transform.position, _groupTriggered.transform.position) < 1f);

            if (IsMatch(_groupTriggered))
            {
                OnMatch?.Invoke(true);
            }
            else
            {
                OnMatch?.Invoke(false);
            }

            Reset();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(matchBlockUseTag))
            {
                _groupTriggered = other.gameObject.GetComponent<BlocksGroup>();
                Debug.Log("triggered");
                
                Distance().Forget();
            }
            else
            {
                Debug.Log($"tag {other.tag} isn't found");
            }
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

        private void Reset()
        {
            _groupTriggered.HidAll().Forget();
        }
        
    }
}
