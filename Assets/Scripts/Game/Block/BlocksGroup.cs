using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Block
{
    public class BlocksGroup : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private BlockIdentity[] blocks;

        [Header("Settings")]
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        [Space]
        [SerializeField] private int minBlocks;
        [SerializeField] private int maxBlocks;
        
        private BlockIdentity _mainBlock;
        public List<BlockIdentity> _listBlocks;

        private void Start()
        {
            _listBlocks = new List<BlockIdentity>();
        }
        
        public async UniTask StartGame()
        {
            _listBlocks?.Clear();

            await ShowAllInList();
            await CreateNewBranch();
            await HideAllInList();
        }

        public void AddBlock(BlockIdentity block)
        {
            if(_listBlocks.Contains(block)) return;
            
            _listBlocks.Add(block);
        }
        
        public void RemoveBlock(BlockIdentity block)
        {
            if (!_listBlocks.Contains(block)) return;
            
            _listBlocks.Remove(block);
        }
        
        public List<BlockIdentity> GetBlocks()
        {
            return _listBlocks;
        }
        
        #region Show And Hide Blocks

        public async UniTask HidAll()
        {
            foreach (BlockIdentity block in blocks)
            {
                block.GetComponent<BlockTransform>().Hide(0.2f);
                
                await UniTask.Yield();
            }
        }
        
        private async UniTask HideAllInList()
        {
            foreach (BlockIdentity block in _listBlocks)
            {
                block.GetComponent<BlockTransform>().Hide(0.2f);
                
                await UniTask.Yield();
            }
        }

        private async UniTask ShowAllInList()
        {
            foreach (BlockIdentity block in blocks)
            {
                block.GetComponent<BlockTransform>().Show(0);
                
                await UniTask.Yield();
            }
        }

        #endregion

        #region Brach

        private BlockIdentity NewMainBlockRandom()
        {
            int column = Random.Range(0,columns);
            int row = Random.Range(0,rows);
            
            return FindBlocksByLocation(column, row);
        }
        
        private async UniTask CreateNewBranch()
        {
            int blocksNumber = Random.Range(minBlocks,maxBlocks);
            BlockIdentity lastBlock = NewMainBlockRandom();
            
            if (lastBlock == null)
            {
                Debug.LogError("No valid starting block found!");
                return;
            }
            
            for (int i = 0 ;i < blocksNumber  ;i ++)
            {
                int toColumn = Random.Range(-1,1); //-1 = back , 0 = don`t move , 1 = move to forward
                int toRow = Random.Range(-1,1);  //-1 = back , 0 = don`t move , 1 = move to forward
                
                BlockIdentity block = FindBlocksByLocation(lastBlock.column + toColumn, lastBlock.row + toRow);

                if (block == null)
                {
                    block = NewMainBlockRandom();
                }
                
                if (block != null)
                {
                    lastBlock = block;
                    AddBlock(block);
                }
                
                await UniTask.Yield();
            }
        }
        
        #endregion

        #region Get
        
        private BlockIdentity FindBlocksByLocation(int column, int row)
        {
            foreach (BlockIdentity block in blocks)
            {
                if (block.column == column && block.row == row)
                {
                    return block;
                }
            }
            
            return null;
        }

        #endregion
        
    }
}
