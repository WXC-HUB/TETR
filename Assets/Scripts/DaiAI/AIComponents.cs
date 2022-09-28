using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DaiAI
{
    public enum BTNodeState
    {
        Success,
        Fail,
        Running
    }
    public class AIExecutor
    {
        
    }

    [System.Serializable]
    public struct RawBehaviorNode
    {
        public string id;
        public string name;
        public string title;
        public Dictionary<string, object> properties;
        public List<string> children;
    }

    [System.Serializable]
    public struct RawBehaviorTree
    {
        public string root;
        public Dictionary<string, RawBehaviorNode> nodes;
    }

    public class BehaviorTree 
    {
        public BTNode startNode;

        RawBehaviorTree rawTreeInfo;
        public IEnumerable walkAllNodes
        {
            get
            {
                Stack<BTNode> searchStack = new Stack<BTNode>();

                searchStack.Push(startNode);

                while(searchStack.Count > 0)
                {
                    BTNode testNode = searchStack.Pop();
                    foreach(var subNode in testNode.nextNodes)
                    {
                        searchStack.Push(subNode);
                    }
                    yield return testNode;
                }
            }
        }

        public void InitTreeFromRawInfo(RawBehaviorTree rawTree)
        {
            rawTreeInfo = rawTree;
            startNode = _SpawnNodeFromRawInfo(rawTree.nodes[rawTree.root]);
            Debug.Log("startNode");
            Debug.Log(startNode);
        }

        BTNode _SpawnNodeFromRawInfo(RawBehaviorNode rawNode)
        {
            BTNode newNode;
            switch (rawNode.name)
            {
                case "Condition":
                    
                    newNode = new BTNode_Condition();

                    ((BTNode_Condition)newNode).conditions = rawNode.properties;

                    break;
                case "Sequence":
                    newNode = new BTNode_Sequence();
                    break;

                case "Selector":
                    newNode = new BTNode_Selector();
                    break;

                case "Runner":
                    newNode = new BTNode_Runner();
                    ((BTNode_Runner)newNode)._actionName = rawNode.title;
                    ((BTNode_Runner)newNode)._actionParam = rawNode.properties;
                    break;
                case "Succeeder":
                    newNode = new BTNode_Succeeder();
                    ((BTNode_Succeeder)newNode)._actionName = rawNode.title;
                    ((BTNode_Succeeder)newNode)._actionParam = rawNode.properties;
                    break;
                default:
                    newNode = new BTNode();
                    break;
            }


            newNode.delAllNode();

            if(rawNode.children != null)
            {
                foreach (var subnode in rawNode.children)
                {
                    newNode.addSubNode(_SpawnNodeFromRawInfo(rawTreeInfo.nodes[subnode]));
                }
            }

            return newNode;
        }
    }

    public class BTNode
    {
        public List<BTNode> nextNodes;
        public BTNode()
        {
            nextNodes = new List<BTNode>();
        }

        public void addSubNode(BTNode node)
        {
            nextNodes.Add(node);
        }

        public void delAllNode()
        {
            nextNodes = new List<BTNode>();
        }

        public virtual BTNodeState RunNode(Dictionary<string , object> memory , DaiAIAgent agent) { return BTNodeState.Fail; }
    }

    public class BTNode_Sequence : BTNode
    {
        int lastRunIndex = -1;

        public override BTNodeState RunNode(Dictionary<string , object> memory, DaiAIAgent agent)
        {
            for(int i = lastRunIndex < 0 ? 0 : lastRunIndex; i<nextNodes.Count; i++)
            {
                var node = nextNodes[i];

                BTNodeState result = node.RunNode(memory, agent);
                if (result == BTNodeState.Fail)
                {
                    if (lastRunIndex >= 0) lastRunIndex = -1;
                    return BTNodeState.Fail;
                }
                if(result == BTNodeState.Success)
                {
                    if (lastRunIndex >= 0) lastRunIndex = -1;
                    continue;
                }
                if (result == BTNodeState.Running)
                {
                    lastRunIndex = i;
                    return BTNodeState.Running;
                }
            }

            return BTNodeState.Success;
        }
    }

    public class BTNode_Selector : BTNode
    {
        int lastRunIndex = -1;

        public override BTNodeState RunNode(Dictionary<string, object> memory, DaiAIAgent agent)
        {
            for (int i = lastRunIndex < 0 ? 0 : lastRunIndex; i < nextNodes.Count; i++)
            {
                var node = nextNodes[i];

                BTNodeState result = node.RunNode(memory, agent);
                if (result == BTNodeState.Success)
                {
                    if (lastRunIndex >= 0) lastRunIndex = -1;
                    return BTNodeState.Success;
                }
                if (result == BTNodeState.Fail)
                {
                    if (lastRunIndex >= 0) lastRunIndex = -1;
                    continue;
                }
                if (result == BTNodeState.Running)
                {
                    lastRunIndex = i;
                    return BTNodeState.Running;
                }
            }

            return BTNodeState.Fail;
        }
    }
    public class BTNode_Runner : BTNode
    {
        public string _actionName;

        public Dictionary<string, object> _actionParam;

        public override BTNodeState RunNode(Dictionary<string, object> memory, DaiAIAgent agent)
        {
            Debug.Log("DoAction" + (string)_actionParam["action"]);
            return agent.RunActionFunc((string)_actionParam["action"], _actionParam);
        }
    }

    public class BTNode_Succeeder : BTNode
    {
        public string _actionName;

        public Dictionary<string , object> _actionParam;

        public override BTNodeState RunNode(Dictionary<string, object> memory, DaiAIAgent agent)
        {
            Debug.Log("DoSuccess" + (string)_actionParam["action"]);
            agent.RunActionFunc((string)_actionParam["action"], _actionParam);
            return BTNodeState.Success;
        }
    }

    public class BTNode_Condition : BTNode
    {
        public Dictionary<string, object> conditions;

        public override BTNodeState RunNode(Dictionary<string, object> memory, DaiAIAgent agent)
        {
            foreach(var key in conditions.Keys)
            {
                if (DaiAIUtils.JudgeCondition(memory, key, conditions[key]) == false)
                {
                    Debug.Log("deny:" + key);
                    return BTNodeState.Fail;
                }
            }

            return BTNodeState.Success;
        }

    }
}