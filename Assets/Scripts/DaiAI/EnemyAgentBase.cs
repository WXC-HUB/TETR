using System.Collections;
using UnityEngine;

namespace DaiAI
{
    public class EnemyAgentBase : DaiAIAgent
    {
        BotCharacterBase botcharacterBase;
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            TextAsset itemText = Resources.Load<TextAsset>("BotAI/BotTest");
            RawBehaviorTree rawTree =  LitJson.JsonMapper.ToObject<RawBehaviorTree>(itemText.text);
            behaviorTree.InitTreeFromRawInfo(rawTree);

            botcharacterBase = this.GetComponent<BotCharacterBase>();
        }

    }
}