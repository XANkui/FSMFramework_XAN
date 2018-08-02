using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cube的狀態枚舉
public enum CubeState {
    Idle,
    Move,
}

/// <summary>
/// 使用可以new的簡單的泛型FSM狀態機控制Cube的狀態
/// </summary>
public class CubeController_FSMDelegateNew : MonoBehaviour {

    public float moveSpeed = 3.0f;				//Cube的移動速度

    private FSMDelegateNew<CubeState> FSM;		//泛型Cube FSM狀態機
    private Material matCube;					//Cube的材質參數

	// Use this for initialization
	void Start () {
		
		//獲取Cube的材質
		//建立Cube狀態機並且設置當前默認狀態
		//給狀態機添加對應狀態下的對應狀態事件
        matCube = GetComponent<Renderer>().material;
        FSM = new FSMDelegateNew<CubeState>(CubeState.Idle);
        FSM.AddCallback(CubeState.Idle, new BaseDelegateNew(OnCuebIdle));
        FSM.AddCallback(CubeState.Move, new BaseDelegateNew(OnCubeMove));
	}
	
	// Update is called once per frame
	//執行FSM狀態機需要每幀執行的函數
	void Update () {
        FSM.Update();
	}

	/// <summary>
	/// Cube在Idle 狀態下執行的函數
	/// </summary>
    private void OnCuebIdle() {

		//獲取水平和垂直的輸入，不為 0 ，則設置 Cube 到移動狀態
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h != 0 || v != 0) {
            FSM.currentState = CubeState.Move;
        }
    }

	/// <summary>
	/// Cube 在 Move 狀態下執行的函數
	/// </summary>
    private void OnCubeMove() {

		//獲取水平和垂直的輸入，都為 0 ，則設置 Cube 到 Idle 狀態，并設置為白色
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h == 0 && v == 0)
        {
            matCube.color = Color.white;
            FSM.currentState = CubeState.Idle;
        }

		//移動 Cube，移動越快 Cube 顏色就逐漸為紅色
        Vector3 target = new Vector3(h, 0 ,v).normalized;
        float f = Mathf.Abs(h) >= Mathf.Abs(v) ? Mathf.Abs(h) : Mathf.Abs(v);
        matCube.color = new Color(1, 1 - f, 1 - f);
        transform.position += Time.deltaTime * moveSpeed * target;
    }
}
