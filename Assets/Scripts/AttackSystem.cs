using System.Collections;
using UnityEngine;

namespace Mtaka
{
    /// <summary>
    /// 攻擊連段系統：處理攻擊連段行為
    /// </summary>
    public class AttackSystem : MonoBehaviour
    {
        [SerializeField, Header("中斷連段時間"), Range(0, 2)]
        private float breakComboTime = 1;
        [SerializeField, Header("攻擊動畫長度"), Range(0, 10)]
        private float[] attackAnimationLength;

        private WaitForSeconds waitBreakComboTime;
        private WaitForSeconds[] waitAttackAnimationTime;
        private Animator ani;
        private string parAttack = "拳頭連段攻擊";
        private string parTriggerAttack = "觸發攻擊";
        private int attackCountMax = 4;
        private int attackCountCurrent;
        private bool isAttacking;

        private void Awake()
        {
            ani = GetComponent<Animator>();
            waitBreakComboTime = new WaitForSeconds(breakComboTime);
            // 新增與攻擊動畫相同數量的等待時間
            waitAttackAnimationTime = new WaitForSeconds[attackAnimationLength.Length];
            // 初始化等待時間
            for (int i = 0; i < waitAttackAnimationTime.Length; i++)
            {
                waitAttackAnimationTime[i] = new WaitForSeconds(attackAnimationLength[i]);
            }
        }

        private void Update()
        {
            Combo();
        }

        /// <summary>
        /// 連段攻擊：連段數字處理
        /// </summary>
        private void Combo()
        {
            // 如果正在攻擊中就跳出
            if (isAttacking) return;
            // 如果按下滑鼠左鍵 當前攻擊段數+1
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                attackCountCurrent++;
                // 如果段數大於最大值 段數 = 1
                if (attackCountCurrent >= attackCountMax) attackCountCurrent = 1;
                // 停止所有的協同程序
                StopAllCoroutines();
                StartCoroutine(AttackHandle());
                Debug.Log(attackCountCurrent);
            }
        }

        private IEnumerator AttackHandle()
        {
            // 正在攻擊中
            isAttacking = true;
            // 更新連段攻擊參數 = 段數
            ani.SetFloat(parAttack, attackCountCurrent);
            // 觸發攻擊參數
            ani.SetTrigger(parTriggerAttack);
            // 啟動套用動畫動作
            ani.applyRootMotion = true;
            // 等待攻擊動畫的長度
            yield return waitAttackAnimationTime[attackCountCurrent - 1];
            // 沒有攻擊
            isAttacking = false;
            // 取消套用動畫動作
            ani.applyRootMotion = false;
            // 等待 中斷連段時間後 將連段數歸零
            yield return waitBreakComboTime;
            attackCountCurrent = 0;
            Debug.Log($"<color=#f33>中斷攻擊，連段數：{attackCountCurrent}</color>");
        }
    }
}