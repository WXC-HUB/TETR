using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Empty,
    BaseBlock_A,
    BaseBlock_B
}

public enum EnemyType
{
    BasicShooter
}

[System.Serializable]
public struct RawBlock
{
    public BlockType type;
    public int blockHP;
    public int enemyCnt;
}

[System.Serializable]
public struct RawEnemy
{
    public EnemyType type;
    public int count;
}

[System.Serializable]
public struct RawBlockGroup
{
    public int ID;
    public int groupWidth;
    public int groupHeight;
    public Vector2Int centerBlock;
    public List<RawBlock> blockGroupList;
    public List<RawEnemy> enemys;
}

[System.Serializable]
public struct BlockPrefab
{
    public BlockType type;
    public GameObject prefab;
}

[System.Serializable]
public struct EnemyPrefab
{
    public EnemyType type;
    public GameObject prefab;
}

[CreateAssetMenu(menuName = "MySubMenue/Create BlockSetting")]
public class BlockSetting : ScriptableObject
{
    public List<RawBlockGroup> BlockGroupPool;
    public List<BlockPrefab> blockPrefabSetting;
    public List<EnemyPrefab> enemyPrefabSetting;

    public GameObject TPSPlayerPrefab;
}
