using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector3 centerPos;

    public Vector2Int gridPos;

    public BlockBase attachBlock;
}

public class BlockGrid : MonoBehaviour
{
    List<List<Cell>> cells;

    List<GameObject> nowAssumeGroup;

    public Vector3 m_blockSpawnRoot;

    public int m_height;
    public int m_width;
    public int m_center_x;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.Instance.EventAddListener(EventCenterType.RefreshAssumeBlock, AssumeBlockDrop);
    }

    void DebugDrawCross(Vector3 centerPos , Color c)
    {
        Debug.DrawLine(centerPos, centerPos + new Vector3(0, 0.05f, 0) , c);
        Debug.DrawLine(centerPos, centerPos + new Vector3(0, -0.05f, 0), c);
        Debug.DrawLine(centerPos, centerPos + new Vector3(0.05f, 0, 0), c);
        Debug.DrawLine(centerPos, centerPos + new Vector3(-0.05f, 0, 0), c);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var r in cells)
        {
            foreach(var c in r)
            {
                DebugDrawCross(c.centerPos, c.attachBlock == null ? Color.green : Color.red);
            }
        }
    }

    bool CheckSingleLineFilled(int lineID)
    {
        foreach(var cell in cells[lineID])
        {
            if (cell.attachBlock == null) return false;
        }
        return true;
    }

    public void CheckAllLineFilled()
    {
        for(int i =  0; i < cells.Count; i++)
        {
            if (CheckSingleLineFilled(i))
            {
                SolidLine(i);
            }
        }
    }

    void SolidLine(int lineID)
    {
        foreach (var cell in cells[lineID])
        {
            if (cell.attachBlock != null) cell.attachBlock.OnGetSolid();
        }
    }

    public Cell GetLowestCenterCell(BlockGroup blockGroup)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            foreach(var sub in blockGroup.subBlockList)
            {
                Vector3 testPos = sub.block.transform.position + new Vector3(0, i, 0) * ( - LevelManager.Instance.m_blockWidth);
                if( !CanGoingDown(testPos))
                {
                    Cell touchCell = GetNearestCell(testPos);
                    return CellOffset(touchCell, -(int)sub.offSet.x, -(int)sub.offSet.y);
                }
            }
        }

        return GetNearestCell(blockGroup.transform.position);
    }

    public int GetMaxGoDownRows(BlockGroup blockGroup)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            foreach (var sub in blockGroup.subBlockList)
            {
                Vector3 testPos = sub.block.transform.position + new Vector3(0, i, 0) * (-LevelManager.Instance.m_blockWidth);
                if (!CanGoingDown(testPos))
                {
                    return i;
                }
            }
        }

        return 0;
    }

    public void AssumeBlockDrop(params object[] data)
    {
        BlockGroup blockGroup = (BlockGroup)data[0];
        if(nowAssumeGroup!= null)
        {
            for (int i = 0; i < nowAssumeGroup.Count; i++)
            {
                Destroy(nowAssumeGroup[i]);
            }
        }
        

        int offset = GetMaxGoDownRows(blockGroup);

        SpawnAssumeBlock(GetNearestCell(blockGroup.transform.position + new Vector3(0 , -LevelManager.Instance.m_blockWidth , 0) * offset ), blockGroup);
    }

    public void SpawnAssumeBlock(Cell center , BlockGroup blockGroup)
    {
        nowAssumeGroup = new List<GameObject>();
        foreach(SubBlock sub in blockGroup.subBlockList)
        {
            nowAssumeGroup.Add( BlockUtils.SpawnSingleBlock(sub.rawInfo, sub.offSet, center.centerPos, transform, true) );
        }
    }

    public void InitGrid(Vector3 w_centerPos , int width , int height , float block_width)
    {
        cells = new List<List<Cell>>();
        int w_center = width / 2;

        m_height = height;
        m_width = width;
        m_center_x = w_center;

        for(int i = 0; i<height; i++)
        {
            List<Cell> row = new List<Cell>();
            for(int j = 0;j<width; j++)
            {
                Cell cell = new Cell();

                cell.centerPos = new Vector3((j - w_center), (0.5f + i), 0) * block_width + w_centerPos;
                cell.gridPos = new Vector2Int(j, i);

                BlockUtils.SpawnBoarderGrid(cell.centerPos, transform);


                if(i == height-1 && j == w_center)
                {
                    m_blockSpawnRoot = cell.centerPos;
                }

                row.Add(cell);
            }

            cells.Add(row);
        }
    }

    public Cell GetNearestCell(Vector3 pos)
    {
        float mindis = 9999f;
        Cell cr = null;

        foreach (var r in cells)
        {
            foreach (var c in r)
            {
                float distemp = (pos - c.centerPos).magnitude;
                if(distemp <= mindis)
                {
                    cr = c;
                    mindis = distemp;
                }
            }
        }
        return cr;
    }

    public Cell CellOffset(Cell c , int up = 0 , int right = 0)
    {
        try
        {
            return cells[c.gridPos.y + up][c.gridPos.x + right];
        }catch(System.Exception e)
        {
            return null;
        }

        
    }

    public bool CanGoingDown(Vector3 pos)
    {
        Cell nearCell = GetNearestCell(pos);
        if (nearCell.gridPos.y <= 0 && pos.y <= nearCell.centerPos.y)
        {
            return false;
        }
        if (pos.y <= nearCell.centerPos.y && CellOffset(nearCell , up:-1).attachBlock != null)
        {
            return false;
        }

        return true;
    }

    public bool CanMoveLeft(Vector3 pos , bool isLeft)
    {
        Cell nearCell = GetNearestCell(pos);
        if (isLeft)
        {
            if(nearCell.gridPos.x <= 0)
            {
                return false;
            }
        }
        else
        {
            if(nearCell.gridPos.x >= cells[0].Count-1)
            {
                return false;
            }
        }

        return true;
    }

    public void SpawnFixBlockFromNearbyNode(List<SubBlock> blocks )
    {

        foreach(SubBlock sub in blocks)
        {
            AttackBlockToCell(sub.block, GetNearestCell(sub.block.transform.position));
        }
    }

    public void AttackBlockToCell(BlockBase block , Cell cell)
    {
        cell.attachBlock = block;
        block.GetComponent<Rigidbody2D>().MovePosition(cell.centerPos);
        block.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;


        block.gameObject.layer = LayerMask.NameToLayer("BlockFixed");

        LevelManager.Instance.CheckAllGrid();
    }

}
