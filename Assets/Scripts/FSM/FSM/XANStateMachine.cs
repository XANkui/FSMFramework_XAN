using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace XAN_FSM
{
	/// <summary>
	/// 繼承狀態類和狀態機類的狀態機類
	/// </summary>
    public class XANStateMachine : XANState, IStateMachine
    {
		/// <summary>
		/// 狀態機類的當前狀態屬性
		/// </summary>
		/// <value>The state of the current.</value>
        public IState CurrentState
        {
            get
            {
                return currentState;
            }
			set{ 
				currentState = value;
			}
        }

		/// <summary>
		/// 狀態機的默認狀態屬性
		/// </summary>
		/// <value>The default state.</value>
        public IState DefaultState {
            get {
                return defaultState;
            }
            set {
                defaultState = value;
            }
        }

		/// <summary>
		/// 狀態機的構造函數
		/// </summary>
		/// <param name="_name">狀態名稱</param>
		/// <param name="defaultState">狀態機默認狀態</param>
        public XANStateMachine(string _name, IState defaultState):base(_name) {
			//默認狀態和狀態列表初始化
            DefaultState = defaultState;
			states = new List<IState> ();
        }

		/// <summary>
		/// 添加狀態到狀態列表
		/// </summary>
		/// <param name="state">狀態</param>
        public void AddState(IState state)
        {
			//如果狀態沒有添加過，把狀態添加到狀態列表
			//設置狀態附屬為當前狀態機
			//如果默認狀態為空，則把第一個膠乳狀態列表的狀態設置為默認狀態
            if (!states.Contains(state)) {
                states.Add(state);
                state.Parent = this;
                if(defaultState == null){
                    defaultState = state;
                }
            }
        }

		/// <summary>
		/// 使用標籤獲得狀態
		/// </summary>
		/// <returns>The state with tag.</returns>
		/// <param name="tag">狀態標籤名</param>
        public IState GetStateWithTag(string tag)
        {
            return null;
        }

		/// <summary>
		/// 從狀態列表中移除某個狀態
		/// </summary>
		/// <param name="state">某個狀態</param>
        public void RemoveState(IState state)
        {
			//如果移除的是當前狀態，則暫時不移除
            if (currentState == state) {
                return;
            }

			//判斷狀態是否在類表中，在就移除，且把狀態附屬的狀態機置為空
			//如果默認狀態是刪除狀態，若列表不為空，則把列表第一個狀態設置為默認狀態
            if (states.Contains(state)) {
                states.Remove(state);
                state.Parent = null;
                if (defaultState == state) {
                    defaultState = states.Count >= 1 ? states[0] : null;
                }
            }
        }

		/// <summary>
		/// 頻率事件回調函數
		/// </summary>
		/// <param name="deltaTime">時間頻率</param>
		public override void OnUpdateCallBack (float deltaTime)
		{
			//如果正在過渡，則進行過渡
			if(isTransition == true){

				//當前過渡完成，進入下一個狀態切換，并置過渡為false
				if(currentTransition.TransitionCallBack ()){
					DoTransition (currentTransition);
					isTransition = false;
				}

				//正在過渡返回
				return;
			}

			//執行基類的函數
			base.OnUpdateCallBack (deltaTime);	

			//若當前狀態為空，則把默認狀態置為當前狀態
			if(currentState == null ){
				currentState = defaultState;
			}

			//獲得當前狀態的過渡列表，計算列表長度，依次檢測過渡是否可以進行
			List<ITransition> ts = currentState.Transitions;
			int count = ts.Count;
			for(int i = 0; i < count; i++){
				ITransition t = ts [i];

				//若有過渡可執行，則置過渡為true，并置為當前過渡，然後跳出
				if(t.ShouldBegineTransitiom ()){
					isTransition = true;
					currentTransition = t;
					break;
				}
			}

			//回調狀態的每幀回調
			currentState.OnUpdateCallBack (deltaTime);
		}

		/// <summary>
		/// 状态固定频率帧的回調函數
		/// </summary>
		/// <param name="deltaTime">固定的時間頻率</param>
		public override void OnFixedUpdateCallBack (float deltaTime)
		{
			base.OnFixedUpdateCallBack (deltaTime);
			currentState.OnFixedUpdateCallBack (deltaTime);
		}

		/// <summary>
		/// late 頻率事件回調函數
		/// </summary>
		/// <param name="deltaTime">時間頻率</param>
		public override void OnLateUpdateCallBack (float deltaTime)
		{
			base.OnLateUpdateCallBack (deltaTime);
			currentState.OnLateUpdateCallBack (deltaTime);
		}

		/// <summary>
		/// 執行過渡函數
		/// </summary>
		/// <param name="t">哪個過渡</param>
		void DoTransition(ITransition t){

			//退出當前狀態，之當前過渡為目標狀態，進入目標狀態
			currentState.OnExitCallBack (t.StateTo);
			currentState = t.StateTo;
			currentState.OnEnterCallBack (t.StateFrom);
		}

        private IState currentState;				//當前狀態
        private IState defaultState;				//默認狀態
        private List<IState> states;				//狀態列表
		private bool isTransition = false;			//是否驚醒過渡參數
		private ITransition currentTransition;		//當前過渡
    }
}
