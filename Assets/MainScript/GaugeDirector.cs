using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GaugeDirector : MonoBehaviour
{
    GameObject HpGauge;

    // Start is called before the first frame update
    void Start()
    {
        this.HpGauge = GameObject.Find("HpGauge");
    }


    // HPを減らす処理
    public void DecreaseHp()
    {
        this.HpGauge.GetComponent<Image>().fillAmount -= 0.01f;
    }

    // HPを増やす処理
    public void RiseHp()
    {
        this.HpGauge.GetComponent<Image>().fillAmount += Time.deltaTime;
    }
}
