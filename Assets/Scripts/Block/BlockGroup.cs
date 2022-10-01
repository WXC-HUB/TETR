using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockGroupState
{
    Prepare,
    GoingDown,
    ReachBottom,
}

[System.Serializable]
public class SubBlock
{
    public Vector2 offSet;
    public BlockBase block;
    public RawBlock rawInfo;
}

[System.Serializable]
public enum BlockBotType
{
    Free,
    Push
}

public class BlockGroup : MonoBehaviour
{

    public BlockGroupState nowState = BlockGroupState.Prepare;

    public List<SubBlock> subBlockList;
    public List<List<BotCharacterBase>> subEnemys;


    public Transform SubEnemyRoot;

    float goingDownSpeed = 3f;

    public int GetNowOffset()
    {
        Cell c_center = LevelManager.Instance.m_blockGrid.GetNearestCell(transform.position);

        return c_center.gridPos.x - LevelManager.Instance.m_blockGrid.m_center_x;
    }

    public void InitBlockGroup(RawBlockGroup rawGroup)
    {
        subBlockList = new List<SubBlock>();
        for (int i = 0; i < rawGroup.groupHeight; i++)
        {
            for (int j = 0; j < rawGroup.groupWidth; j++)
            {
                int blockID = i * rawGroup.groupWidth + j;
                RawBlock block = rawGroup.blockGroupList[blockID];
                Vector2 v = new Vector2(j + 1, i + 1) - rawGroup.centerBlock;
                v.y = -v.y;
                GameObject blockObj = BlockUtils.SpawnSingleBlock(block, v , transform.position, transform);

                if(blockObj != null)
                {
                    SubBlock sub = new SubBlock();
                    sub.offSet = v;
                    sub.block = blockObj.GetComponent<BlockBase>();
                    sub.rawInfo = block;

                    subBlockList.Add(sub);

                }
            }
        }


        subEnemys = new List<List<BotCharacterBase>>();
        for (int i = 0; i < rawGroup.enemys.Count; i++)
        {
            subEnemys.Add(  SpawnSingelEnemy(rawGroup.enemys[i], rawGroup.groupHeight + i) );
        }
    }

    public void StartDrop()
    {
        nowState = BlockGroupState.GoingDown;

        foreach(var enemygroup in subEnemys)
        {
            foreach(var enemy in enemygroup)
            {
                enemy.SetCharacterActive(true);
            }
        }

        SubEnemyRoot.DetachChildren();
    }

    public List<BotCharacterBase> SpawnSingelEnemy(RawEnemy rawEnemy , int y_offset)
    {

        GameObject enemyObj = LevelManager.Instance.blockSetting.enemyPrefabSetting.Find((EnemyPrefab e) => e.type == rawEnemy.type).prefab;

        List<BotCharacterBase> objs = new List<BotCharacterBase>();

        for(int i = 0; i < rawEnemy.count; i+=1)
        {
            Vector3 blockOffset = new Vector3(0, y_offset, 0) * LevelManager.Instance.m_blockWidth + new Vector3(Mathf.CeilToInt((float)i / 2), 0, 0) * (i % 2 == 0 ? 1 : -1) * LevelManager.Instance.m_enemyGap;
            GameObject newObj = Instantiate(enemyObj, transform.position + blockOffset, new Quaternion() , SubEnemyRoot);
            objs.Add(newObj.GetComponent<BotCharacterBase>());

            newObj.GetComponent<BotCharacterBase>().EquipWeaponByID(rawEnemy.weaponID);
        }

        return objs;
    }

    public void TryMoveLeft(bool isleft)
    {
        if (CanMoveLeft(isleft) == false) return;

        transform.position += new Vector3(isleft?-LevelManager.Instance.m_blockWidth : LevelManager.Instance.m_blockWidth , 0 , 0);
        EventCenter.Instance.EventTrigger(EventCenterType.RefreshAssumeBlock, this);
    }

    bool CanMoveLeft(bool isleft)
    {
        foreach (SubBlock sub in subBlockList)
        {
            /*
            bool havewall = Physics2D.Raycast(sub.block.transform.position , new Vector3(0,-1,0) , LevelManager.Instance.m_blockWidth * 0.5f , layerMask: LayerMask.GetMask("Wall")  );
            bool haveblock = Physics2D.Raycast(sub.block.transform.position, new Vector3(0, -1, 0), LevelManager.Instance.m_blockWidth * 0.5f, layerMask: LayerMask.GetMask("BlockFixed"));

            //Debug.DrawLine(sub.block.transform.position, sub.block.transform.position + new Vector3(0, -1, 0) * (LevelManager.Instance.m_blockWidth * 0.5f + .2f));

            if(haveblock || havewall)
            {
                return false;
            }*/

            if (LevelManager.Instance.m_blockGrid.CanMoveLeft(sub.block.transform.position , isleft) == false)
            {
                return false;
            }
        }
        return true;
    }

    public bool TryTurn(bool isleft)
    {
        if (CanTurn(isleft))
        {
            transform.Rotate(new Vector3(0, 0 , isleft ? 90 : -90) );


            for(int i = 0; i<subBlockList.Count; i++)
            {
                subBlockList[i].offSet = isleft ? new Vector2(-subBlockList[i].offSet.y, subBlockList[i].offSet.x) : new Vector2(subBlockList[i].offSet.y, -subBlockList[i].offSet.x);
            }

            EventCenter.Instance.EventTrigger(EventCenterType.RefreshAssumeBlock, this);
        }

        return false;
    }

    bool CanTurn(bool isleft)
    {
        foreach(var sub in subBlockList)
        {
            Vector3 targetdir = isleft ? new Vector3( -sub.offSet.y , sub.offSet.x, 0) : new Vector3(sub.offSet.y, -sub.offSet.x, 0);
            bool havewall = Physics2D.BoxCast(transform.position, new Vector2(LevelManager.Instance.m_blockWidth, LevelManager.Instance.m_blockWidth), 0, targetdir, LevelManager.Instance.m_blockWidth * targetdir.magnitude , layerMask: LayerMask.GetMask("Wall"));
            bool haveblock = Physics2D.BoxCast(transform.position, new Vector2(LevelManager.Instance.m_blockWidth, LevelManager.Instance.m_blockWidth), 0, targetdir, LevelManager.Instance.m_blockWidth * targetdir.magnitude, layerMask: LayerMask.GetMask("BlockFixed"));

            Debug.DrawLine(transform.position, transform.position + targetdir * LevelManager.Instance.m_blockWidth * targetdir.magnitude ,Color.red , duration:100f);

            if (haveblock || havewall)
            {
                return false;
            }
        }

        return true;
    }

    GameObject SpawnSingleBlock(RawBlock block , Vector3 offset)
    {
        if(block.type == BlockType.BaseBlock_A || block.type == BlockType.BaseBlock_B)
        {
            GameObject blockObj = LevelManager.Instance.blockSetting.blockPrefabSetting.Find((BlockPrefab n) => n.type == block.type).prefab;
            Vector3 blockOffset = new Vector3(offset.x, offset.y, 0) * LevelManager.Instance.m_blockWidth;
            return Instantiate(blockObj, transform.position + blockOffset, transform.rotation, transform);
        }

        return null;
        
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nowState == BlockGroupState.GoingDown)
        {
            TryGoingDown(1);
        }
    }

    void TryGoingDown(float speedScale)
    {
        if (CanGoingDown())
        {
            transform.position += new Vector3(0, -speedScale * goingDownSpeed * Time.deltaTime, 0);
            foreach(var block in subBlockList)
            {
                block.block.gameObject.layer = LayerMask.NameToLayer("BlockGoingDown");
            }
        }
        else
        {
            ReleaseSubBlock();
        }
        
    }

    void ReleaseSubBlock()
    {
        LevelManager.Instance.m_blockGrid.SpawnFixBlockFromNearbyNode(subBlockList);
        nowState = BlockGroupState.ReachBottom;
        transform.DetachChildren();

        EventCenter.Instance.EventTrigger(EventCenterType.RefreshAssumeBlock, this);

        Destroy(gameObject);
    }

    bool CanGoingDown()
    {
        foreach(SubBlock sub in subBlockList)
        {
            /*
            bool havewall = Physics2D.Raycast(sub.block.transform.position , new Vector3(0,-1,0) , LevelManager.Instance.m_blockWidth * 0.5f , layerMask: LayerMask.GetMask("Wall")  );
            bool haveblock = Physics2D.Raycast(sub.block.transform.position, new Vector3(0, -1, 0), LevelManager.Instance.m_blockWidth * 0.5f, layerMask: LayerMask.GetMask("BlockFixed"));

            //Debug.DrawLine(sub.block.transform.position, sub.block.transform.position + new Vector3(0, -1, 0) * (LevelManager.Instance.m_blockWidth * 0.5f + .2f));

            if(haveblock || havewall)
            {
                return false;
            }*/

            if (LevelManager.Instance.m_blockGrid.CanGoingDown(sub.block.transform.position) == false)
            {
                return false;
            }
        }
        return true;
    }
}
