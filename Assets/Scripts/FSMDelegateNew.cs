using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定位一個委託
public delegate void BaseDelegateNew();

/// <summary>
/// 一個可以new的簡單的泛型FSM狀態機
/// </summary>
public class FSMDelegateNew <T> {

	//定義一個字典存儲狀態和對應狀態事件
	private Dictionary<T, BaseDelegateNew> states;
	public T currentState ;				//泛型的當前狀態

	// Use this for initialization
	/// <summary>
	/// 構造函數，初始化字典和設置默認當前狀態
	/// </summary>
	/// <param name="defaultState">默認狀態</param>
	public FSMDelegateNew (T defaultState) {
		states = new Dictionary<T, BaseDelegateNew> ();
		currentState = defaultState;
	}

	/// <summary>
	/// 添加狀態和對應狀態事件到字典中
	/// </summary>
	/// <param name="state">狀態</param>
	/// <param name="delegateEvent">對應狀態時間</param>
	public void AddCallback(T state, BaseDelegateNew delegateEvent){
		//判斷是否為空或是否存在該狀態，沒有則添加
		if(state != null && delegateEvent != null && !states.ContainsKey (state)){
			states.Add (state, delegateEvent);
		}
	}

	// Update is called once per frame
	/// <summary>
	/// 需要每幀調用的函數
	/// </summary>
	public void Update () {
		//如果當前字典存在當前狀態，且對應委託時間不為空，則執行委託事件
		if(states.ContainsKey (currentState)){
			BaseDelegateNew state = states [currentState];
			if(state != null){
				state ();
			}
		}
	}
}
