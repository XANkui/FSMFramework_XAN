using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 通過IF語句來控制燈的開關，最簡單的狀態中控制
/// </summary>
public class LightController_IF : MonoBehaviour {
	//設置燈變量
	private Light light;

	// Use this for initialization
	void Start () {

		//獲取燈組件
		light = GetComponent <Light> ();
	}
	
	// Update is called once per frame
	void Update () {

		//按下O鍵，打開燈，按下C鍵關閉燈
		if(Input.GetKeyDown (KeyCode.O)){
			light.intensity = 1.0f;
		}

		if(Input.GetKeyDown (KeyCode.C)){
			light.intensity = 0.0f;
		}
	}
}
