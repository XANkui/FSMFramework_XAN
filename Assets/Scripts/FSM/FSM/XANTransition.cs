using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XAN_FSM
{
	/// <summary>
	/// 過渡委託事件
	/// </summary>
	public delegate bool XANTransitionDelegate();

	public class XANTransition : ITransition {

		/// <summary>
		/// 從南哥狀態過渡屬性
		/// </summary>
		/// <value>The state from.</value>
		public IState StateFrom {
			get {
				return stateFrom;
			}
			set {
				stateFrom = value;
			}
		}

		/// <summary>
		/// 過渡到哪個狀態屬性
		/// </summary>
		/// <value>The state to.</value>
		public IState StateTo {
			get {
				return stateTo;
			}
			set {
				stateTo = value;
			}
		}

		/// <summary>
		/// 過渡名稱屬性
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return name;
			}
		}

		// 過渡事件 和 是否可以過渡事件
		public event XANTransitionDelegate OnTransitionCallBack;
		public event XANTransitionDelegate OnShouldBeginTransition;

		/// <summary>
		/// 過渡類的構造函數
		/// </summary>
		/// <param name="name">過渡名稱</param>
		/// <param name="stateFrom">從哪個狀態過渡</param>
		/// <param name="stateTo">過渡到哪個狀態</param>
		public XANTransition(string name, IState stateFrom, IState stateTo){

			//初始化
			this.name = name;
			this.stateFrom = stateFrom;
			this.stateTo = stateTo;
		}

		/// <summary>
		/// 過渡回調函數
		/// </summary>
		/// <returns><c>true</c>表示過渡完成 <c>false</c> 過渡未完成</returns>
		public bool TransitionCallBack ()
		{
			//過渡事件不為空，則執行事件
			if(OnTransitionCallBack != null){
				return OnTransitionCallBack ();
			}

			return true;
		}

		/// <summary>
		/// 判斷是否可以過渡
		/// </summary>
		/// <returns><c>true</c>表示可以進行過渡 <c>false</c> 表示還不可以進行過渡</returns>
		public bool ShouldBegineTransitiom ()
		{
			//判斷是否可以過渡事件不為空，則執行事件
			if(OnShouldBeginTransition != null){
				return OnShouldBeginTransition ();
			}

			return false;
		}


		private IState stateFrom; 		//從哪個狀態過渡
		private IState stateTo;			//過渡到哪個狀態
		private string name;			//過渡名稱

	}
}
