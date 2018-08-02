using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XAN_FSM
{
	//状态机接口基类
    public interface IStateMachine 
    {
        IState CurrentState { get; }		//当前的状态
        IState DefaultState { get; set; }	//默认状态

        void AddState(IState state);		//添加状态
        void RemoveState(IState state);		//移除状态
        IState GetStateWithTag(string tag);	//使用标签获得状态

    }
}
