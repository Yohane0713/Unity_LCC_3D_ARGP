using System.Collections;
using UnityEngine;

namespace Mtaka
{
    public class StateAttack : State
    {
        [SerializeField, Header("攻擊時間"), Range(0, 5)]
        private float attackTime = 3;
        [SerializeField, Header("旋轉速度"), Range(0, 100)]
        private float turnSpeed = 30;
        [SerializeField, Header("旋轉間隔"), Range(0, 2)]
        private float turnInterval = 0.1f;

        private WaitForSeconds waitAttackTime;
        private WaitForSeconds waitTurnInterval;

        protected override void Awake()
        {
            base.Awake();
            stateName = "攻擊狀態";
            waitAttackTime = new WaitForSeconds(attackTime);
            waitTurnInterval = new WaitForSeconds(turnInterval);
        }

        public override void StateExit()
        {
            base.StateExit();
            StopAllCoroutines();
        }

        public override void StateEnter()
        {
            base.StateEnter();
            /*目標座標指定為玩家座標
            Vector3 target = transform.position;
            // 目標Y軸指定為敵人Y軸
            target.y = transform.position.y;
            // 面向目標座標
            transform.LookAt(target);*/  //原本的旋轉方式會瞬間面向目標，需要給等待時間
            StartCoroutine(FaceToTarget());
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

        /// <summary>
        /// 面向目標
        /// </summary>
        private IEnumerator FaceToTarget()
        {
            // 計算目標的角度 (目標座標 - 敵人座標)
            Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);

            for (int i = 0; i < 10; i++)
            {
                // 透過插值更新角度
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
                yield return waitTurnInterval;
            }
        }
    }
}