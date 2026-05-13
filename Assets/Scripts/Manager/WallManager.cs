using System;
using Cysharp.Threading.Tasks;
using Game.Block;
using UnityEngine;

namespace Game.Wall
{
    /// <summary>
    /// Use this class to manage walls spawn
    /// </summary>
    public class WallManager : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private MatchManager matchManager;
        [SerializeField] private BlocksGroup[] groups;

        [Header("Settings")]
        [SerializeField] private bool startOnAwake;

        private int _wallIndex = 0;
        
        private void Start()
        {
            if (matchManager)
            {
                matchManager.OnTriggered += StartWall;
            }
            
            if(startOnAwake)
            {
                StartWall();
            }
        }

        private void StartWall()
        {
            NextIndex();
            InitializeGroups(groups[_wallIndex]);
        }
        
        private void InitializeGroups(BlocksGroup group)
        {
            group.transform.position = spawnPoint.position;
            group.StartGame().Forget();
        }

        private void NextIndex()
        {
            if (_wallIndex >= groups.Length - 1)
            {
                _wallIndex = 0;
                return;
            }
            
            _wallIndex++;
        }
    }
}
