using Block;
using Cysharp.Threading.Tasks;
using Game;
using UnityEngine;

namespace Manager
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
        [SerializeField] private bool isStartOnAwake;

        private int _wallIndex = 0;
        
        private void Start()
        {
            if (matchManager)
            {
                matchManager.OnTriggered += StartWall;
            }
            
            if(isStartOnAwake)
            {
                StartWall();
            }
        }
        
        /// <summary>
        /// Called this function when previous wall is finished
        /// </summary>
        private void StartWall()
        {
            NextIndex();
            InitializeGroups(groups[_wallIndex]);
        }
        
        /// <summary>
        /// Initialize the wall
        /// </summary>
        /// <param name="group"></param>
        private void InitializeGroups(BlocksGroup group)
        {
            group.transform.position = spawnPoint.position; //Wall position
            group.GenerateRandomBranch().Forget(); //Reset blocks and create new branchs
        }

        /// <summary>
        /// Next Wall
        /// </summary>
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
