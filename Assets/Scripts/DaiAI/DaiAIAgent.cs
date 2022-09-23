using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaiAI
{
    public class DaiAIAgent : MonoBehaviour
    {
        AISensor sensor;
        AIExecutor executor;
        BehaviorTree behaviorTree;
        
        // Start is called before the first frame update
        void Start()
        {

        }

        void TickBehaviorTree()
        {
            foreach(BTNode node in behaviorTree.walkAllNodes)
            {
                if (node.RunNode())
                {
                    break;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
