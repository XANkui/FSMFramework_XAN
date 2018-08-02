using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使用繼承MonoBehaviour，和使用委託列表事件的簡單FSM實現控制燈狀態
/// </summary>
public class LightController_FSMDelegate : MonoBehaviour {

	//燈狀態枚舉值，以0開始，對應FSM狀態列表下標值
	public enum LightState{
		Open = 0,
		Close,
	}

	public float lightIntensityMax = 1.0f; 		//設置燈最大強度
	public float lightSpeed = 1.0f;				//設置燈的變化速度

	private Light light;						//燈參數
	private float lightIntensityMin;			//燈的最小強度
	private bool isAnimation = false;			//是否動畫的參數
	private bool isResetAnimation = false;		//是否重置動畫的參數
	private bool isReadyClose = false;			//是否準備關燈的參數
	private FSMDelegate FSM;					//FSM狀態機參數

	// Use this for initialization
	void Start () {

		//獲取燈組件。獲取FSM狀態機組件
		//設置FSM狀態機的當前狀態
		//添加燈打開/關閉狀態的委託事件，對應好下標
		light = GetComponent <Light> ();
		FSM = gameObject.AddComponent <FSMDelegate> ();
		FSM.currentState = (int)LightState.Open;
		FSM.states.Add (new BaseDelegate(OnOpenState));
		FSM.states.Add (new BaseDelegate(OnCloseState));
	}

	/// <summary>
	/// 燈打開狀態時的操作函數
	/// </summary>
	private void OnOpenState(){

		//開始動畫
		if(isAnimation == true){

			//未重置動畫，燈強度從強到弱變化，當到達目標時，重置動畫
			if (isResetAnimation == false) {
				if((light.intensity - lightIntensityMin ) <= 0.01f){

					light.intensity = lightIntensityMin;
					isResetAnimation = true;

					return;
				}

				//未到達目標值時燈光逐漸變暗
				light.intensity -= Time.deltaTime * lightSpeed;
			} else {

				//到達目標時，停止動畫和停止重置動畫，并返回
				if((lightIntensityMax - light.intensity ) <= 0.01f){

					light.intensity = lightIntensityMax;
					isResetAnimation = false;
					isAnimation = false;

					return;
				}

				//未到達目標值時燈光逐漸變亮
				light.intensity += Time.deltaTime * lightSpeed;
			}

			//一段動畫完成，停止動畫，並且重新獲取變化最小值，并設置開始動畫
		}else{
			lightIntensityMin = Random.Range (0.2f, 0.4f);
			isAnimation = true;
		}

		//按下C鍵，進行關閉燈操作，設置最小強度為0
		//開始動畫，不重置動畫
		//準備關閉燈
		if(Input.GetKeyDown (KeyCode.C)){

			lightIntensityMin = 0.0f;
			isAnimation = true;
			isResetAnimation = false;

			isReadyClose = true;
		}

		//當在準備關閉燈狀態時，到達目標值時，燈滅，設置燈狀態為關閉狀態
		//停止動畫，準備關閉結束
		if(isReadyClose == true){
			if((light.intensity - lightIntensityMin ) <= 0.01f){

				light.intensity = lightIntensityMin;
				FSM.currentState = (int)LightState.Close;
				isAnimation = false;
				isReadyClose = false;
			}
		}
	}

	/// <summary>
	/// 燈關閉狀態下執行的函數
	/// </summary>
	private void OnCloseState(){

		//在關閉狀態下按下O鍵，開始和重置動畫，並且設置燈的狀態為打開狀態
		if(Input.GetKeyDown (KeyCode.O)){

			isAnimation = true;
			isResetAnimation = true;

			FSM.currentState = (int)LightState.Open ;
		}
	}
}
