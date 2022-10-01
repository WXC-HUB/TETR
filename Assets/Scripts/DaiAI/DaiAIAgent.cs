using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DaiAI
{
    public class DaiAIAgent : MonoBehaviour
    {
        public Sensor.AISensor sensor;
        public BehaviorTree behaviorTree;

        public bool isActive = false;

        public Dictionary<string, BTNodeActionFunc> eventsDictonary = new Dictionary<string, BTNodeActionFunc>();

        public delegate BTNodeState BTNodeActionFunc(Dictionary<string, object> infos);

        // Start is called before the first frame update
        public virtual void Start()
        {
            behaviorTree = new BehaviorTree();
            sensor = GetComponent<Sensor.AISensor>();
        }

        public void SetActionFunc(string eventName, BTNodeActionFunc action)
        {
            // 存在存在要监听的事件
            if (eventsDictonary.ContainsKey(eventName))
            {
                eventsDictonary[eventName] = action;
            }
            // 要监听的事件不存在,添加对应事件和订阅后执行的方法
            else
            {
                eventsDictonary.Add(eventName, action);
            }
        }
        public BTNodeState RunActionFunc(string eventName, Dictionary<string , object> data)
        {
            // 存在存在要监听的事件,依次执行订阅了的委托方法
            if (eventsDictonary.ContainsKey(eventName))
            {
                // eventsDictonary[eventName]();
                return eventsDictonary[eventName](data);
            }

            return BTNodeState.Fail;
        }

        void TickBehaviorTree()
        {
            behaviorTree.startNode.RunNode(sensor.memory , this);
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive)
            {
                TickBehaviorTree();
            }
        }
    }
}
