using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DaiAI
{
    public class AISensor
    {

    }

    public class AIExecutor
    {
        
    }

    public class BehaviorTree 
    {
        public BTNode startNode;
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
    }

    public abstract class BTNode
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

        public abstract bool RunNode();
    }
}