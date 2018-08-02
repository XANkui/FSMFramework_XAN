using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定義一個委託事件
public delegate void BaseDelegate();

/// <summary>
/// 一個繼承MonoBehaviour，和使用委託列表事件的簡單FSM
/// </summary>
public class FSMDelegate : MonoBehaviour {

	public List<BaseDelegate> states;	//委託事件狀態列表
	public int currentState ;			//當前狀態

	// Use this for initialization
	void Awake () {
		// 初始化委託事件轉台列表和當前狀態
		states = new List<BaseDelegate> ();
		currentState = 0;			//這裡借用了枚舉值和int的關係
	}
	
	// Update is called once per frame
	void Update () {

		//當前狀態在類別內，且對應的委託事件存在，則執行對應狀態事件
		if(currentState < states.Count){
			BaseDelegate state = states [currentState];
			if(state != null){
				state ();
			}
		}
	}
}
