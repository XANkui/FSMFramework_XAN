using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XAN_FSM;

/// <summary>
/// Light test FSM sub FSM. 帶子狀態機的狀態機使用
/// </summary>
public class LightTestFSMSubFSM : MonoBehaviour {

	public float lightIntensityMax = 1.0f;				//Light 的最大強度
	public float lightSpeed = 1.0f;						//Light 的變化速度

	private XANStateMachine FSM;						// 狀態機
	private XANStateMachine light_Open;					// Light Open 狀態機（狀態機繼承狀態）
	private XANState light_OpenChangeIntensity;			// Light Open 狀態機 光強變化狀態
	private XANState light_OpenChangeColor;				// Light Open 狀態機 顏色變化狀態
	private XANTransition light_OpenIntensityToColor;	// Light Open 狀態機 光強變化過度到顏色變化
	private XANTransition Light_OpenColorToIntensity;	// Light Open 狀態機 顏色變化過度到光強變化
	private XANState light_Close;						//Light 的關閉狀態
	private XANTransition light_OpenToClose;			//Light Open 過渡到 Close
	private XANTransition light_CloseToOpen;			//light Close 過渡到 Open

	private Light _light;								//Light 參數
	private bool isToOpen = true;						//Light 是否可以打開參數
	private bool isAnimation = false;					//Light 是否可以動畫參數
	private bool isResetAnimation = false;				//Light 是否可以重置動畫參數
	private float randomValue;							//Light 隨機強度變化目標值
	private Color randomColor;							//Light 隨機顏色變化目標值
	private Color lastColor;							//Light 當前顏色
	private float colorTimer;							//顏色變化時間

	[SerializeField]									//可在Inspect顯示私有變量的標籤
	private bool isIntensityToColor = false;			//是否可以光強到顏色過度


	// Use this for initialization
	void Start () {

		//獲得 Light 組件，并初始化FSM
		_light = GetComponent <Light> ();
		InitFSM ();

	}

	// Update is called once per frame
	void Update () {  
		//監控狀態機的 OnUpdateCallBack 回調
		FSM.OnUpdateCallBack (Time.deltaTime);
	}

	/// <summary>
	/// 初始化狀態機
	/// </summary>
	private void InitFSM(){

		//初始化 Light 光強變化的狀態，並在進入時設置可以 Light 動畫和重置動畫為 true
		light_OpenChangeIntensity = new XANState ("LightOpen_ChangeIntensity");
		light_OpenChangeIntensity.OnEnter += (IState state) => {
			isAnimation = true;
			isResetAnimation = true;
		};
		//Light 光強變化狀態的 OnUpdate，光強明暗變化
		light_OpenChangeIntensity.OnUpdate += (float f) => {
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

		//初始化 Light 顏色變化的狀態，並在進入時設置可以 Light 動畫為 true
		light_OpenChangeColor = new XANState ("LightOpen_ChangeColor");
		light_OpenChangeColor.OnEnter += (IState state) => {
			//設置為false，是讓他首先生成一個隨機顏色在進入動畫
			//不然,每次都是默認第一個隨機色為黑色 color(0,0,0)
			isAnimation = false;
		};
		//Light 顏色變化的狀態 的 OnUpdate，顏色隨機漸漸變化
		light_OpenChangeColor.OnUpdate += (float f) => {

			//進入動畫
			if(isAnimation == true){
				//根據 colorTime 變化時長來判定是否變化OK，變化完重置時間
				if(colorTimer >= 1f){
					isAnimation = false;
					colorTimer = 0.0f;
				}else{
					//這樣設置能保證Lerp變為勻速和時間，後面 * X 就為幾秒
					colorTimer += Time.deltaTime;
					_light.color = Color.Lerp (lastColor, randomColor, colorTimer);
				}

				//未進入動畫前隨機生成一個顏色，并記錄當前顏色，然後開始動畫true
			}else{
				float r = Random.Range (0.0f, 1.0f);
				float g = Random.Range (0.0f, 1.0f);
				float b = Random.Range (0.0f, 1.0f);
				randomColor = new Color(r, g, b);
				lastColor = _light.color;
				isAnimation = true;
			}
		};

		//Light Open 狀態機 光強變化過度到顏色變化初始化，并添加過渡 isIntensityToColor 判斷
		light_OpenIntensityToColor = new XANTransition ("LightOpenIntensityToColor", light_OpenChangeIntensity, light_OpenChangeColor);
		light_OpenIntensityToColor.OnShouldBeginTransition += () => {
			return isIntensityToColor;
		};
		//Light Open 狀態機 光強變化過度到顏色變化 添加到 Light Open 狀態機 光強變化狀態
		light_OpenChangeIntensity.AddTransition (light_OpenIntensityToColor);

		//Light Open 狀態機 顏色變化過度到光強變化初始化，并添加過渡 isIntensityToColor 判斷
		Light_OpenColorToIntensity = new XANTransition ("LightOpenColorToIntensity", light_OpenChangeColor, light_OpenChangeIntensity);
		Light_OpenColorToIntensity.OnShouldBeginTransition += () => {
			return !isIntensityToColor;
		};
		//Light Open 狀態機 顏色變化過度到光強變化 添加到 Light Open 狀態機 顏色變化狀態
		light_OpenChangeColor.AddTransition (Light_OpenColorToIntensity);

		//初始化 light Open 狀態機，并設置光強變化為默認狀態
		//添加 光強變化和顏色變化狀態到 Light Open 狀態列表中
		light_Open = new XANStateMachine ("LightOpen", light_OpenChangeIntensity);
		light_Open.AddState (light_OpenChangeIntensity);
		light_Open.AddState (light_OpenChangeColor);

		//初始化 Light Close狀態
		light_Close = new XANState ("LightClose");

		//Light Open 狀態進入狀態事件光強最大光強，并打印
		light_Open.OnEnter += (IState state) => {
			_light.intensity = lightIntensityMax;
			Debug.Log ("进入打开状态");
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

		//把 Light Open 過渡到 Close 添加到 light Open狀態機
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
