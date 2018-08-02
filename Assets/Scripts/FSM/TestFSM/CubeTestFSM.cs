using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XAN_FSM;

/// <summary>
/// Cube test FSM.
/// </summary>
public class CubeTestFSM : MonoBehaviour {

	public float moveSpeed = 1.0f;				// Cube 移動速度

	private XANStateMachine FSM;				// 狀態機參數
	private XANState cube_Idle;					// Cube 的 Idle 狀態
	private XANState cube_Move;					// Cube 的 Move 狀態
	private XANTransition cube_IdleToMove;		// Cube 的 Idle 過渡到 Move
	private XANTransition cube_MoveToIdle;		// Cube 的 Move 過渡到 Idle

	private bool isToMove = false;				// Cube 是否可以移動

	// Use this for initialization
	void Start () {

		//初始化 Cube Idle 狀態，並且設置進入狀態打印
		cube_Idle = new XANState ("CubeIdle");
		cube_Idle.OnEnter += (IState state) => {
			Debug.Log ("进入Idle状态");
		};

		//初始化 Cube Move 狀態，並且設置進入狀態打印
		cube_Move = new XANState ("CubeMove");
		cube_Move.OnEnter += (IState state) => {
			Debug.Log ("进入Move状态");
		};
		// cube_Move 狀態添加 OnUpdate 事件，向前移動 Cube
		cube_Move.OnUpdate += (float f) => {
			transform.position += Vector3.forward * f * moveSpeed;
		};

		//初始化 Cube 的 Idle 過渡到 Move，并根據 isToMove，判斷是否可以移動
		cube_IdleToMove = new XANTransition ("IdleToMove", cube_Idle, cube_Move);
		cube_IdleToMove.OnShouldBeginTransition += () => {
			return isToMove;
		};

		//初始化 Cube 的 Move 過渡到 Idle，并根據 isToMove，判斷是否可以移動
		cube_MoveToIdle = new XANTransition ("MoveToIdle", cube_Move, cube_Idle);
		cube_MoveToIdle.OnShouldBeginTransition += () => {
			return !isToMove;
		};

		//把 Cube 的 Idle 過渡到 Move 添加到 Cube Idle 狀態
		cube_Idle.AddTransition (cube_IdleToMove);
		//把 Cube 的 Move 過渡到 Idle 添加到 Cube Move 狀態
		cube_Move.AddTransition (cube_MoveToIdle);

		//初始化狀態機，設置 Cube Idle狀態為默認狀態
		FSM = new XANStateMachine ("CubeFSM", cube_Idle);
		//並把 Cube Idle 和 Cube Move 狀態添加到狀態列表中
		FSM.AddState (cube_Idle);
		FSM.AddState (cube_Move);
	}
	
	// Update is called once per frame
	void Update () {
		
		//在Update函數中，監聽狀態機 OnUpdateCallBack 回調
		FSM.OnUpdateCallBack (Time.deltaTime);
	}

	/// <summary>
	/// Sets the is to move true.
	/// </summary>
	public void SetIsToMoveTrue(){
		isToMove = true;
	}

	/// <summary>
	/// Sets the is to move false.
	/// </summary>
	public void SetIsToMoveFalse(){
		isToMove = false;
	}
}
