using System.Collections;
using UnityEngine;
using System.Collections.Generic;



namespace DaiAI
{
    public static class DaiAIUtils
    {
        public static bool JudgeCondition(Dictionary<string , object> memory , string condition_key , object condition_content)
        {
            string condition_type = condition_key.Substring(0, 2);

            if (!memory.ContainsKey(condition_key)) return false;
            object memory_value = memory[condition_key];
            if(condition_type == "n_")
            {
                float value;
                bool result = false;
                if(((string)condition_content).Substring(0, 2) == ">=")
                {
                    value = float.Parse(((string)condition_content).Substring(2) );

                    Debug.Log( "condition_key:" + memory_value.ToString()  + ">=" + value.ToString()) ;

                    result = ((float)memory_value) >= value;

                }else if (((string)condition_content).Substring(0, 2) == "<=")
                {
                    value = float.Parse(((string)condition_content).Substring(2));

                    Debug.Log("condition_key:" + memory_value.ToString() + "<=" + value.ToString());
                    result = ((float)memory_value) <= value;
                }
                else if (((string)condition_content).Substring(0, 1) == ">")
                {
                    value = float.Parse(((string)condition_content).Substring(1));

                    Debug.Log("condition_key:" + memory_value.ToString() + ">" + value.ToString());
                    result = ((float)memory_value) > value;
                }
                else if (((string)condition_content).Substring(0, 1) == "<")
                {
                    value = float.Parse(((string)condition_content).Substring(1));

                    Debug.Log("condition_key:" + memory_value.ToString() + "<" + value.ToString());
                    result = ((float)memory_value) < value;
                }
                else
                {
                    value = (float)condition_content;
                    result = ((float)memory_value) == value;
                }

                Debug.Log("result:" + result.ToString());
                return result;

            }else if(condition_type == "b_")
            {
                bool value = bool.Parse(((string)condition_content));
                return ((bool)memory_value) == value;
            }else if(condition_type == "s_")
            {
                return ((string)memory_value) == ((string)condition_content);
            }

            return false;
        }
    }
}