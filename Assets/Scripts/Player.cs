using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DaiAI;
[System.Serializable]
public class PlayerBase
{
    public virtual void OnTick() { }
}

[System.Serializable]
public class TetrPlayer : PlayerBase
{
    public BlockGroup nowCtrlGroup;

    float lastDropTime = -3f;
    public float dropCD = 1f;

    public void TurnBlock(bool isleft)
    {
        nowCtrlGroup.TryTurn(isleft);
    }

    public virtual void MoveBlock(bool isleft)
    {
        nowCtrlGroup.TryMoveLeft(isleft);
    }

    public bool isDropCD()
    {
        return !(Time.time - lastDropTime >= dropCD);
    }

    public void StartDrop()
    {
        if(!isDropCD())
        {
            nowCtrlGroup.StartDrop();
            lastDropTime = Time.time;
        }
        

    }

    public void SwitchToNext()
    {
        if(nowCtrlGroup.nowState == BlockGroupState.Prepare)
        {
            LevelManager.Instance.SwitchToNextBlock(true);
        }
    }

    public void SwitchBotType()
    {
        LevelManager.Instance.m_nowSelectType = LevelManager.Instance.m_nowSelectType == BlockBotType.Free ? BlockBotType.Push : BlockBotType.Free;
    }
}

[System.Serializable]
public class TpsPlayer : PlayerBase
{
    public PlayerCharacterBase character;

    public void OnStartFire()
    {
        character.OnStartFire();
    }

    public void OnEndFire()
    {
        character.OnEndFire();
    }

    public void OnSetWeaponDir(Vector3 dir)
    {
        character.UpdateWeaponDir(dir);
    }
}

[System.Serializable]
public class AITetrPlayer : TetrPlayer
{
    public DaiAI.DaiAIAgent m_agent;
    public int targetPos = -1;
    public bool havePos = false;

    public float lastOperateTime = -1;
    float OperateCD = .3f;
    public bool canOperate()
    {
        return Time.time - lastOperateTime >= OperateCD;
    }

    public void InitAI(DaiAI.TETRAgentBase _agent)
    {
        _agent.SetActionFunc("A_MoveBlockToRandomPos", A_MoveBlockToRandomPos);
        _agent.SetActionFunc("A_DropBlock", A_DropBlock);
        _agent.SetActionFunc("A_Idle", A_Idle);
    }
    public override void MoveBlock(bool isleft)
    {
        base.MoveBlock(isleft);
        lastOperateTime = Time.time;
    }


    public BTNodeState A_MoveBlockToRandomPos(Dictionary<string, object> infos)
    {
        if(nowCtrlGroup == null)    return BTNodeState.Fail;
        if(havePos == false)
        {
            float r = Random.Range(0f, 1f);
            targetPos = 0;
            if (r < 0.3f) targetPos = -3;
            if (r > 0.6f) targetPos = 3;
            havePos = true;
        }

        if(nowCtrlGroup.GetNowOffset() == targetPos)
        {
            havePos = false;
            Debug.Log("Reach" + nowCtrlGroup.GetNowOffset() + "to" + targetPos);
            return BTNodeState.Success;
        }

        if (canOperate())
        {
            Debug.Log("move!!!");
            MoveBlock(nowCtrlGroup.GetNowOffset() > targetPos);
        }

        return BTNodeState.Running;

    }

    public BTNodeState A_DropBlock(Dictionary<string, object> infos)
    {
        if (nowCtrlGroup == null) return BTNodeState.Fail;
        if (isDropCD()) return BTNodeState.Fail;
        StartDrop();
        return BTNodeState.Success;
    }

    public BTNodeState A_Idle(Dictionary<string, object> infos)
    {
        return BTNodeState.Success;
    }
}

public class TetrPlayer1P: TetrPlayer
{
    public override void OnTick()
    {
        base.OnTick();


        if (Input.GetKeyUp(KeyCode.W))
        {
            TurnBlock(true);
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            SwitchToNext();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            SwitchBotType();
        }


        if (Input.GetKeyUp(KeyCode.A))
        {
            MoveBlock(true);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            MoveBlock(false);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartDrop();
        }
    }
}

[System.Serializable]
public class TpsPlayer2P: TpsPlayer
{
    public override void OnTick()
    {
        base.OnTick();

        float x = 0 + (Input.GetKey(KeyCode.LeftArrow) ? -1 : 1) + (Input.GetKey(KeyCode.RightArrow) ? 1 : -1);
        float y = 0 + (Input.GetKey(KeyCode.DownArrow) ? -1 : 1) + (Input.GetKey(KeyCode.UpArrow) ? 1 : -1);

        if(character != null)
        {
            character.Move(new Vector3(x, y, 0));
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnStartFire();
        }

        if (Input.GetMouseButtonUp(0) || !(Input.GetMouseButton(0)))
        {
            OnEndFire();
        }

        
        OnSetWeaponDir(Camera.main.ScreenToWorldPoint(Input.mousePosition) - character.transform.position);

    }
}