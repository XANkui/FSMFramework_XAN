using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XAN_FSM {

	//状态接口基类
    public interface IState
    {
        string Name { get; }					//状态名称
        string Tag { get; set; }				//状态标签
        IStateMachine Parent { get; set; }		//状态附属的状态机
        float Timer { get; }					//计时器
		List<ITransition> Transitions{ get;}	//状态过度的列表

        void OnEnterCallBack(IState prev);		//状态进入的回调
        void OnExitCallBack(IState next);		//状态退出的回调

        void OnUpdateCallBack(float deltaTime);			//状态 Update 回调
        void OnFixedUpdateCallBack(float deltaTime);	//状态 FixedUpdate 回调
        void OnLateUpdateCallBack(float deltaTime);		//状态 lateUpdate 回调

		void AddTransition (ITransition t);				//添加过度的函数

    }
}


