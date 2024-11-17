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
        protected string playerName = "unitychan";
        protected string stateName;

        protected void Awake()
        {
            ani = GetComponent<Animator>();
            player = GameObject.Find(playerName).transform;
        }

        public void StateEnter()
        {
            LogSystem.Log($"進入{stateName}", "#3f3");
        }

        public void StateExit()
        {
            LogSystem.Log($"離開{stateName}", "#f33");
        }

        public void StateUpdate()
        {
            LogSystem.Log($"更新{stateName}", "#77f");
        }
    }
}