using System.Collections;
using UnityEngine;

public static class BlockUtils
{
    public static GameObject SpawnSingleBlock(RawBlock block, Vector3 offset , Vector3 pos, Transform parent , bool isAssume = false)
    {

        if (block.type == BlockType.BaseBlock_A || block.type == BlockType.BaseBlock_B)
        {
            GameObject blockObj = LevelManager.Instance.blockSetting.blockPrefabSetting.Find((BlockPrefab n) => n.type == block.type).prefab;
            Vector3 blockOffset = new Vector3(offset.x, offset.y, 0) * LevelManager.Instance.m_blockWidth;
            GameObject n =  GameObject.Instantiate(blockObj, pos + blockOffset, parent.rotation, parent);

            if (isAssume)
            {
                n.GetComponent<BlockBase>().OnSetAssumed();
            }

            return n;
        }

        return null;
    }

    public static GameObject SpawnBoarderGrid(Vector3 pos , Transform parent)
    {
        GameObject blockObj = LevelManager.Instance.blockSetting.blockPrefabSetting.Find((BlockPrefab n) => n.type == BlockType.Boarder).prefab;

        GameObject n = GameObject.Instantiate(blockObj, pos, parent.rotation, parent);

        return n;
    }
}