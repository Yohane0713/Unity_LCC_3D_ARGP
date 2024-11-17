using UnityEngine;
using UnityEngine.AI;

namespace Mtaka
{
    public class StateTrack : State
    {
        [SerializeField, Header("追蹤速度"), Range(0, 10)]
        private float trackSpeed = 3;
        [SerializeField, Header("停止距離"), Range(0, 5)]
        private float stoppingDistance = 3;

        private NavMeshAgent agent;

        protected override void Awake()
        {
            base.Awake();
            stateName = "追蹤狀態";
            agent = GetComponent<NavMeshAgent>();
        }

        public override void StateEnter()
        {
            base.StateEnter();
            agent.speed = trackSpeed;
            agent.stoppingDistance = stoppingDistance;
        }

        public override void StateUpdate()
        {
            base.StateUpdate();
            // 代理器 設定目的地 (玩家的座標)
            agent.SetDestination(player.position);
            // 將代理器加速度的值 更新到移動動畫參數內
            ani.SetFloat(parMove, agent.velocity.magnitude / trackSpeed);
            // 切換狀態的條件
            ChangeToAttackState();
        }

        /// <summary>
        /// 切換到攻擊狀態
        /// </summary>
        private void ChangeToAttackState()
        {
            if (Vector3.Distance(transform.position, player.position) <= stoppingDistance)
            {
                LogSystem.Log("從追蹤狀態切換到攻擊狀態", "#37f");
                stateMachine.ChangeState(stateMachine.stateAttack1);
            }
        }
    }
}