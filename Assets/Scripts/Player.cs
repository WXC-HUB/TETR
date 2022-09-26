using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void MoveBlock(bool isleft)
    {
        nowCtrlGroup.TryMoveLeft(isleft);
    }

    public void StartDrop()
    {
        if(Time.time - lastDropTime >= dropCD)
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