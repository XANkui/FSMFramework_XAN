using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XAN_FSM;

/// <summary>
/// 使用 new 的方式，使用狀態
/// </summary>
public class LightTest : MonoBehaviour {

    XANState lightOpenState ;
    XANState lightCloseState;

	// Use this for initialization
	void Awake () {
        lightOpenState = new XANState("LightOpenState");
        lightCloseState = new XANState("LightClosrState");

        lightOpenState.OnEnter += (IState state) =>
        {
            Debug.Log("LightOpenState _OnEnter");
        };

        lightOpenState.OnUpdate += (float f) =>{

        };

        lightOpenState.OnExit += OpenExit;
        lightCloseState.OnEnter += (IState state) =>
        {
            
        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OpenExit(IState state)
    {

    }
}
