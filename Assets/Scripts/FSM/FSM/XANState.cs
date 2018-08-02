using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XAN_FSM
{
	/// <summary>
	/// 一个没有输入参数，没有返回值的委托
	/// </summary>
    public delegate void XANDelegate();

	/// <summary>
	/// 有一个输入状态有返回值的委托
	/// </summary>
    public delegate void XANDelegateState(IState state);

	/// <summary>
	/// 带一个float参数有返回值的委托
	/// </summary>
    public delegate void XANDelegateFloat(float f);

	/// <summary>
	/// 继承接口状态的状态类
	/// </summary>
    public class XANState : IState
    {
		/// <summary>
		/// 状态名称属性
		/// </summary>
		/// <value>The name.</value>
        public string Name
        {
            get
            {
                return name;
            }
        }

		/// <summary>
		/// 状态标签属性
		/// </summary>
		/// <value>The tag.</value>
        public string Tag {
            get {
                return tag;
            }
            set {
                tag = value;
            }
        }	

		/// <summary>
		/// 状态附属的状态机属性
		/// </summary>
		/// <value>The parent.</value>
        public IStateMachine Parent {
            get {
                return parent;
            }
            set {
                parent = value;
            }
        }

		/// <summary>
		/// 状态计时器
		/// </summary>
		/// <value>The timer.</value>
        public float Timer {
            get {
                return timer;
            }
        }

		/// <summary>
		/// 状态过渡列表属性
		/// </summary>
		/// <value>The transitions.</value>
		public List<ITransition> Transitions{ 
			get{ 
				return transitions;
			}
		}

		/// <summary>
		/// 状态构造函数
		/// </summary>
		/// <param name="_name">状态名称</param>
        public XANState(string _name) {
            name = _name;

			//初始化状态过渡列表
			transitions = new List<ITransition> ();
        }

		//定义状态各个行为函数的事件
        public event XANDelegateState OnEnter;
        public event XANDelegateState OnExit;
        public event XANDelegateFloat OnUpdate;
        public event XANDelegateFloat OnFixedUpdate;
        public event XANDelegateFloat OnLateUpdate;

		/// <summary>
		/// 状态类的进入回调
		/// </summary>
		/// <param name="prev">上一个状态</param>
        public virtual void OnEnterCallBack(IState prev)
        {
			//进入之后重置计时器
            timer = 0.0f;

			//进入事件不为空时，触发进入事件
            if (OnEnter != null) {
                OnEnter(prev);
            }
        }

		/// <summary>
		/// 状态退出的回调
		/// </summary>
		/// <param name="next">下一个状态</param>
        public virtual void OnExitCallBack(IState next)
        {
			//推出事件不为空，执行退出事件
            if (OnExit != null)
            {
                OnExit(next);
            }

			//重置计时器 
            timer = 0.0f;
        }

		/// <summary>
		/// 状态固定频率帧的回調函數
		/// </summary>
		/// <param name="deltaTime">固定的時間頻率</param>
        public virtual void OnFixedUpdateCallBack(float deltaTime)
        {
			//固定頻率事件不為空，則執行事件
            if (OnFixedUpdate != null)
            {
                OnFixedUpdate(deltaTime);
            }
        }

		/// <summary>
		/// late 頻率事件回調函數
		/// </summary>
		/// <param name="deltaTime">時間頻率</param>
        public virtual void OnLateUpdateCallBack(float deltaTime)
        {
			//late 頻率事件不為空，則執行事件
            if (OnLateUpdate != null)
            {
                OnLateUpdate(deltaTime);
            }
        }

		/// <summary>
		/// 頻率事件回調函數
		/// </summary>
		/// <param name="deltaTime">時間頻率</param>
        public virtual void OnUpdateCallBack(float deltaTime)
        {
			//計時器開始計時，記錄裝運行時間
            timer += deltaTime;

			//頻率事件不為空，則執行事件
            if (OnUpdate != null)
            {
                OnUpdate(deltaTime);
            }
        }

		/// <summary>
		/// 添加過渡到過渡列表
		/// </summary>
		/// <param name="t">過渡</param>
		public void AddTransition (ITransition t){

			//過渡不為空，且沒有添加過，則添加過渡到列表中
			if(t != null && !transitions.Contains (t)){

				transitions.Add (t);
			}
		}

        private string name;					// 狀態名
        private string tag;						// 狀態標籤
        private IStateMachine parent;			// 附屬狀態機
        private float timer;					// 狀態執行計時器
		private List<ITransition> transitions;	// 狀態過渡列表
    }
}
