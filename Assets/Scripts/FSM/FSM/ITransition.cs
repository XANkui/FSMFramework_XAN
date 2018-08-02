using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace XAN_FSM
{
	//过渡接口基类
	public interface ITransition  {

		IState StateFrom{ get; set;}		//从哪个状态过渡
		IState StateTo{ get; set;}  		//过渡到哪个状态
		string Name{ get;}					//过渡名称

		bool TransitionCallBack ();			//过渡需要的回调
		bool ShouldBegineTransitiom ();		//判断是否可以过渡
	}
}
