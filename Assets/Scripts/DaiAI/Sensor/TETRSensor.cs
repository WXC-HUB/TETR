using System.Collections;
using UnityEngine;

namespace DaiAI.Sensor
{
    public class TETRSensor : AISensor
    {


        // Update is called once per frame
        void Update()
        {
            if(LevelManager.Instance.tetrPlayer.nowCtrlGroup == null)
            {
                memory["n_nowBlockOffset"] = 0;
                memory["b_haveBlock"] = false;
                memory["b_canDropBlock"] = false;
            }
            else
            {
                memory["n_nowBlockOffset"] = LevelManager.Instance.tetrPlayer.nowCtrlGroup.GetNowOffset();
                memory["b_haveBlock"] = LevelManager.Instance.tetrPlayer.nowCtrlGroup != null;
                memory["b_canDropBlock"] = !LevelManager.Instance.tetrPlayer.isDropCD();
            }
            
        }
    }
}