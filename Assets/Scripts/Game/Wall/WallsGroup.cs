using System;
using Cysharp.Threading.Tasks;
using Game.Block;
using UnityEngine;

namespace Game.Wall
{
    public class WallsGroup : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private BlocksGroup[] groups;

        [Header("Settings")]
        [SerializeField] private float durationBetweenWalls;

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            LoopGame().Forget();
        }

        private void StopGame()
        {
            
        }

        private void InitializeGroups(BlocksGroup group)
        {
            group.transform.position = spawnPoint.position;
            group.StartGame().Forget();
        }
        
        private async UniTask LoopGame()
        {
            while (true)
            {
                foreach (BlocksGroup block in groups)
                {
                    InitializeGroups(block);
                    
                    await UniTask.WaitForSeconds(durationBetweenWalls);
                }
            }
        }
    }
}
