using System.Collections;
using UnityEngine;

namespace DaiAI
{
    public class TETRAgentBase : DaiAIAgent
    {

        public override void Start()
        {
            base.Start();

            TextAsset itemText = Resources.Load<TextAsset>("TETRAI/TETRTestAI");
            RawBehaviorTree rawTree = LitJson.JsonMapper.ToObject<RawBehaviorTree>(itemText.text);
            behaviorTree.InitTreeFromRawInfo(rawTree);
        }
    }
}