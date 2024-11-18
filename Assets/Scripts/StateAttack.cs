using System.Collections;
using UnityEngine;

namespace Mtaka
{
    public class StateAttack : State
    {
        [SerializeField, Header("攻擊時間"), Range(0, 5)]
        private float attackTime = 3;

        private WaitForSeconds waitAttackTime;

        protected override void Awake()
        {
            base.Awake();
            stateName = "攻擊狀態";
            waitAttackTime = new WaitForSeconds(attackTime);
        }

        public override void StateEnter()
        {
            base.StateEnter();
            // 目標座標指定為玩家座標
            Vector3 target = transform.position;
            // 目標Y軸指定為敵人Y軸
            target.y = transform.position.y;
            // 面向目標座標
            transform.LookAt(target);
            ani.SetTrigger(parKick);
            ani.SetFloat(parMove, 0);
            StartCoroutine(ChangeToTrackState());
        }

        /// <summary>
        /// 變更到追蹤狀態
        /// </summary>
        private IEnumerator ChangeToTrackState()
        {
            yield return waitAttackTime;
            stateMachine.ChangeState(stateMachine.stateTrack);
        }
    }
}