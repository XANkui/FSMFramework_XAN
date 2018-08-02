using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XAN_FSM;

/// <summary>
/// 繼承狀態的方式引用狀態
/// </summary>
public class CubeMoveStateTest : XANState {

    public CubeMoveStateTest(string name) : base(name) {

    }

    public override void OnEnterCallBack(IState prev)
    {
        base.OnEnterCallBack(prev);
    }
}
