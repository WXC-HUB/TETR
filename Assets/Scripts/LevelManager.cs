using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    PVETPS,
}

public enum GameState
{
    Pause,
    Running
}

public class LevelManager : MonoSingleton<LevelManager>
{
    public TetrPlayer tetrPlayer;
    public TpsPlayer tpsPlayer;

    public GameMode gameMode = GameMode.PVETPS;
    public GameState gameState = GameState.Pause;

    public Transform m_blockSpawnRoot;
    public Transform m_gridRoot;
    public float m_blockWidth;

    public BlockSetting blockSetting;

    public BlockGrid m_blockGrid;

    int _nowSelect = 0;

    public GameObject m_blockGroupPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _LoadPlayer(gameMode);

        m_blockGrid = GetComponent<BlockGrid>();
        m_blockGrid.InitGrid(m_gridRoot.position , 9 ,17 , m_blockWidth);

        _InitLevelBasicObject();

        gameState = GameState.Running;
    }

    void _LoadPlayer(GameMode gameMode)
    {
        if(gameMode == GameMode.PVETPS)
        {
            tetrPlayer = new TetrPlayer1P();
            tpsPlayer = new TpsPlayer2P();
        }
    }

    void _InitLevelBasicObject()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(gameState == GameState.Running)
        {
            tetrPlayer.OnTick();
            tpsPlayer.OnTick();
        }

        CheckNowBlock();

    }

    void CheckNowBlock()
    {
        if(tetrPlayer.nowCtrlGroup == null || tetrPlayer.nowCtrlGroup.nowState != BlockGroupState.Prepare)
        {
            TrySpawnNextBlock();
        }
    }

    public void TrySpawnNextBlock(bool goNext = false )
    {
        
        
        RawBlockGroup nextBlock = getNextBlockGroup(goNext);

        GameObject blockObj =  Instantiate(m_blockGroupPrefab, m_blockGrid.m_blockSpawnRoot, new Quaternion() , m_blockSpawnRoot);

        BlockGroup newGroup = blockObj.GetComponent<BlockGroup>();

        newGroup.InitBlockGroup(nextBlock);

        tetrPlayer.nowCtrlGroup = newGroup;

    }

    public void SwitchToNextBlock(bool goNext = false)
    {
        if (tetrPlayer.nowCtrlGroup != null)
        {
            Destroy(tetrPlayer.nowCtrlGroup.gameObject);
        }
        TrySpawnNextBlock(goNext);
    }


    RawBlockGroup getNextBlockGroup(bool goNext = false)
    {
        if(blockSetting == null || blockSetting.BlockGroupPool.Count <= 0)
        {
            return new RawBlockGroup();
        }
        else
        {
            if (goNext)
            {
                _nowSelect++;
                if(_nowSelect >= blockSetting.BlockGroupPool.Count)
                {
                    _nowSelect = 0;
                }
            }
            return blockSetting.BlockGroupPool[_nowSelect];
        }
    }
}
