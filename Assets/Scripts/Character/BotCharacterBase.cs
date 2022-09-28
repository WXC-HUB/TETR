using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class BotCharacterBase : GameCharacterBase
{
    public DaiAI.DaiAIAgent m_agent;

    public bool m_isActive;

    public void SetCharacterActive(bool active)
    {
        m_isActive = active;

        SetGameObjectLayer();

        SetAgentActive();
    }

    public DaiAI.BTNodeState A_MoveToPlayer(Dictionary<string , object> infos)
    {
        if ((float)m_agent.sensor.memory["n_playerDis"] >= 2)
        {
            Debug.Log("going for dis" + Time.frameCount + "F" + m_agent.sensor.memory["n_playerDis"]);
            
            float speedscale = System.Convert.ToSingle(infos["speedscale"]);
            Move(LevelManager.Instance.GetTPSPlayerPos() - transform.position,  speedscale );

            return DaiAI.BTNodeState.Running;
        }
        else
        {
            Debug.Log("HAVE SUCCESS");
            return DaiAI.BTNodeState.Success;
        }

        
    }

    public DaiAI.BTNodeState A_Idle(Dictionary<string, object> infos)
    {
        return DaiAI.BTNodeState.Success;
    }

    public void SetGameObjectLayer()
    {
        if (m_isActive)
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyActive");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyPreparing");
        }
    }

    void SetAgentActive()
    {
        m_agent.isActive = m_isActive;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Start()
    {
        base.Start();
        m_agent = this.GetComponent<DaiAI.DaiAIAgent>();

        m_agent.SetActionFunc("A_MoveToPlayer", A_MoveToPlayer);
        m_agent.SetActionFunc("A_Idle", A_Idle);
    }
}
