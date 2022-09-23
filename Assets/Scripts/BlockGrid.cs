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

    public Vector3 m_blockSpawnRoot;
    // Start is called before the first frame update
    void Start()
    {
        
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

    public void InitGrid(Vector3 w_centerPos , int width , int height , float block_width)
    {
        cells = new List<List<Cell>>();
        int w_center = width / 2;

        for(int i = 0; i<height; i++)
        {
            List<Cell> row = new List<Cell>();
            for(int j = 0;j<width; j++)
            {
                Cell cell = new Cell();

                cell.centerPos = new Vector3((j - w_center), (0.5f + i), 0) * block_width + w_centerPos;
                cell.gridPos = new Vector2Int(j, i);

                Debug.Log(cell.centerPos);

                if(i == height-1 && j == w_center)
                {
                    m_blockSpawnRoot = cell.centerPos;
                }

                row.Add(cell);
            }

            cells.Add(row);
        }
    }

    Cell GetNearestCell(Vector3 pos)
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
            Debug.Log("type1");
            return false;
        }
        if (pos.y <= nearCell.centerPos.y && CellOffset(nearCell , up:-1).attachBlock != null)
        {
            Debug.Log("type2");
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
        block.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        block.gameObject.layer = LayerMask.NameToLayer("BlockFixed");

        
    }

}
