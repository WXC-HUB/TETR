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
    public float dropCD = 3f;

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
            nowCtrlGroup.nowState = BlockGroupState.GoingDown;
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

    }
}

[System.Serializable]
public class TpsPlayer : PlayerBase
{

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

}