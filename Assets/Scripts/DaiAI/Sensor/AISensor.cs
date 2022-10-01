using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace DaiAI.Sensor
{
    public class AISensor : MonoBehaviour
    {
        public Dictionary<string, object> memory;
        // Use this for initialization
        void Start()
        {
            memory = new Dictionary<string, object>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}