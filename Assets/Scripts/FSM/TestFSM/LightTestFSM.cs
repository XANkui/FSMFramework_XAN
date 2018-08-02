using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XAN_FSM;

/// <summary>
/// Light test FSM.
/// </summary>
public class LightTestFSM : MonoBehaviour {

	public float lightIntensityMax = 1.0f;  	//Light 的最大強度
	public float lightSpeed = 1.0f;				//Light 的變化速度

	private XANStateMachine FSM;				// 狀態機
	private XANState light_Open;				//Light 的打開狀態
	private XANState light_Close;				//Light 的關閉狀態
	private XANTransition light_OpenToClose;	//Light Open 過渡到 Close
	private XANTransition light_CloseToOpen;	//light Close 過渡到 Open

	private Light _light;						//Light 參數
	private bool isToOpen = true;				//Light 是否可以打開參數
	private bool isAnimation = false;			//Light 是否可以動畫參數
	private bool isResetAnimation = false;		//Light 是否可以重置動畫參數
	private float randomValue;					//Light 隨機強度變化目標值


	// Use this for initialization
	void Start () {
		//獲得light 組件
		_light = GetComponent <Light> ();

		//初始化狀態機
		InitFSM ();

	}
	
	// Update is called once per frame
	void Update () {
		//監控狀態機 OnUpdateCallBack 
		FSM.OnUpdateCallBack (Time.deltaTime);
	}

	/// <summary>
	/// 初始化狀態機函數
	/// </summary>
	private void InitFSM(){

		//初始化 Light Open 和 Close 狀態
		light_Open = new XANState ("LightOpen");
		light_Close = new XANState ("LightClose");

		//Light Open 狀態進入狀態事件光強最大光強，并打印
		light_Open.OnEnter += (IState state) => {
			_light.intensity = lightIntensityMax;
			Debug.Log ("进入打开状态");
		};
		//Light Open 狀態，光強明暗變化
		light_Open.OnUpdate += (float f) => {
			if(isAnimation == true){
				if(isResetAnimation == false){
					if(FadeTo (randomValue)){
						isResetAnimation = true;
					}
				}else{
					if(FadeTo (lightIntensityMax)){
						isResetAnimation = false;
						isAnimation = false;
					}
				}

			}else{
				randomValue = Random.Range (0.2f, 0.4f);
				isAnimation = true;
			}
		};

		//Light Close 狀態進入時設置光強為 0，并打印
		light_Close.OnEnter += (IState state)=>{
			_light.intensity = 0.0f;
			Debug.Log ("进入关闭状态");
		};

		//Light Open 過渡到 Close 初始化，并添加過渡 isOpen 判斷
		light_OpenToClose = new XANTransition ("LightOpenToClose", light_Open, light_Close);
		light_OpenToClose.OnShouldBeginTransition += () => {
			
			return !isToOpen;
		};
		//Light Open 過渡到 Close 過渡，燈光漸漸變暗
		light_OpenToClose.OnTransitionCallBack += () => {

			return FadeTo (0.0f);
		};
		//把 Light Open 過渡到 Close 添加到 light Open狀態
		light_Open.AddTransition (light_OpenToClose);

		//Light Close 過渡到 Open 初始化，并添加過渡 isOpen 判斷
		light_CloseToOpen = new XANTransition ("LightCloseToOpen", light_Close, light_Open);
		light_CloseToOpen.OnShouldBeginTransition += () => {
			
			return isToOpen;
		};
		//Light Close 過渡到 Open 過渡，燈光漸漸變亮
		light_CloseToOpen.OnTransitionCallBack += () => {

			return FadeTo (lightIntensityMax);
		};
		//把Light Close 過渡到 Open 過渡添加到 Light Close 狀態中
		light_Close.AddTransition (light_CloseToOpen);

		//初始化狀態佳，設置默認為打開狀態，并添加兩個狀態到狀態列表中
		FSM = new XANStateMachine ("LightFSM", light_Open);
		FSM.AddState (light_Open);
		FSM.AddState (light_Close);
	}

	/// <summary>
	/// Light 變化函數
	/// </summary>
	/// <returns><c>true</c> 表示變化完成 <c>false</c> 表示變化未完成 </returns>
	/// <param name="targetValue"> 變化目標值 </param>
	bool FadeTo(float targetValue){

		//變化相差 0.01 表示變化 OK，返回 true
		if (Mathf.Abs (_light.intensity - targetValue) <= 0.01f) {
			_light.intensity = targetValue;
			return true;

			//否則繼續進行變化，返回 false
		} else {

			//根據目標值大於還是小於 Light 強度來判斷是增是減
			int flag = _light.intensity > targetValue ? -1 : 1 ;

			_light.intensity += Time.deltaTime * lightSpeed * flag;

			return false;
		}
	}

	/// <summary>
	/// Sets the is to open true.
	/// </summary>
	public void SetIsToOpenTrue(){
		isToOpen = true;
	}

	/// <summary>
	/// Sets the is to open false.
	/// </summary>
	public void SetIsToOpenFalse(){
		isToOpen = false;
	}
}
