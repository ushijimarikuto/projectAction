using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDirector : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
	{
        if(other.gameObject.tag == "Player")
        {
            //当たるとダメージを受ける処理
            GameObject director = GameObject.Find("GaugeDirector");
            director.GetComponent<GaugeDirector>().DecreaseHp();
        }
	}
}
