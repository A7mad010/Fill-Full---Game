using System.Collections.Generic;
using System.Linq;
using Core.Attributes;
using Cysharp.Threading.Tasks;
using Effects;
using Game.Block;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Block
{
    /// <summary>
    /// Use this class to manage blocks
    /// </summary>
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
        
        [Header("Debug")]
        [SerializeField , ReadOnly] private List<BlockIdentity> listBlocks;
        
        private BlockIdentity _mainBlock; 
        
        private void Start()
        {
            listBlocks = new List<BlockIdentity>();
        }
        
        public async UniTask GenerateRandomBranch()
        {
            listBlocks?.Clear();

            await ShowAllInList();
            await CreateNewBranch();
            await HideAllInList();
        }

        #region Add and Remove from a list

        public void AddBlock(BlockIdentity block)
        {
            if(listBlocks.Contains(block)) return;
            
            Debug.Log($"add , {block.name}");
            listBlocks.Add(block);
        }
        
        public void RemoveBlock(BlockIdentity block)
        {
            if (!listBlocks.Contains(block)) return;
            
            Debug.Log($"Removing , {block.name}");
            listBlocks.Remove(block);
        }

        #endregion
        
        #region Show And Hide Blocks

        public async UniTask HidAll()
        {
            foreach (BlockIdentity block in blocks)
            {
                block.GetComponent<ScaleEffect>().ScaleOut(0.2f);
                
                await UniTask.Yield();
            }
        }
        
        private async UniTask HideAllInList()
        {
            Debug.Log($"Hides all 1 : {listBlocks.Count} blocks!");
            
            foreach (BlockIdentity block in listBlocks)
            {
                if(!listBlocks.Contains(block)) continue;
                
                block.GetComponent<ScaleEffect>().ScaleOut(0.2f);
                
                await UniTask.Yield();
            }
            
            Debug.Log($"Hides all 2 : {listBlocks.Count} blocks!");
        }

        private async UniTask ShowAllInList()
        {
            foreach (BlockIdentity block in blocks)
            {
                block.GetComponent<ScaleEffect>().ScaleIn(0.2f);
                
                await UniTask.Yield();
            }
        }

        #endregion

        #region Brach

        private BlockIdentity NewMainBlockRandom()
        {
            int column = Random.Range(0,columns);
            int row = Random.Range(0,rows);
            
            return GetBlocksByLocation(column, row);
        }
        
        private async UniTask CreateNewBranch()
        {
            int blocksNumber = Random.Range(minBlocks, maxBlocks + 1);
            BlockIdentity lastBlock = NewMainBlockRandom();

            if (lastBlock == null)
            {
                Debug.LogError("No valid starting block found!");
                return;
            }

            AddBlock(lastBlock); // add starting block

            for (int i = 0; i < blocksNumber; i++)
            {
                int toColumn = Random.Range(-1, 2);
                int toRow = Random.Range(-1, 2);

                int newColumn = lastBlock.column + toColumn;
                int newRow = lastBlock.row + toRow;

                // prevent out of bounds
                if (newColumn < 0 || newColumn >= columns || newRow < 0 || newRow >= rows)
                {
                    await UniTask.Yield();
                    continue;
                }

                BlockIdentity block = GetBlocksByLocation(newColumn, newRow);

                if (block != null && !IsBlockInList(block))
                {
                    AddBlock(block);
                    lastBlock = block;
                }

                await UniTask.Yield();
            }
            
            Debug.Log($"Created new branch! {listBlocks.Count} blocks created!");
        }
        
        #endregion

        #region Get
        
        public List<BlockIdentity> GetBlocksList()
        {
            return listBlocks;
        }
        
        private BlockIdentity GetBlocksByLocation(int column, int row)
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

        private bool IsBlockInList(BlockIdentity block)
        {
            return listBlocks.Any(b => b.column == block.column && b.row == block.row);
        }

        #endregion
    }
}
