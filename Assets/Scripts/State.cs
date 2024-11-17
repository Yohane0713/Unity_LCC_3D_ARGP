using UnityEngine;

namespace Mtaka
{
    public class State : MonoBehaviour, IState
    {
        protected Animator ani;
        protected string parMove = "浮點數_移動";
        protected string parAttack = "觸發攻擊";
        protected string parKick = "觸發踢擊";
        protected Transform player;
        protected string playerName = "玩家_Unity醬";
        protected string stateName;
        protected StateMachine stateMachine;

        protected virtual void Awake()
        {
            ani = GetComponent<Animator>();
            stateMachine = GetComponent<StateMachine>();
            player = GameObject.Find(playerName).transform;
        }

        public virtual void StateEnter()
        {
            LogSystem.Log($"進入{stateName}", "#3f3");
        }

        public virtual void StateExit()
        {
            LogSystem.Log($"離開{stateName}", "#f33");
        }

        public virtual void StateUpdate()
        {
            LogSystem.Log($"更新{stateName}", "#77f");
        }
    }
}