using System.Collections;
using UnityEngine;

namespace DaiAI.Sensor
{
    public class BotSensor : AISensor
    {


        // Update is called once per frame
        void Update()
        {
            Debug.Log("DIS:" + Time.frameCount + "F" + (transform.position - LevelManager.Instance.GetTPSPlayerPos()).magnitude);
            memory["n_playerDis"] = (transform.position - LevelManager.Instance.GetTPSPlayerPos()).magnitude;
            memory["b_haveGun"] = (this.GetComponent<GameCharacterBase>().HaveWeapon());
        }
    }
}