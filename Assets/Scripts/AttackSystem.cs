using UnityEngine;

namespace Mtaka
{
    /// <summary>
    /// 攻擊連段系統：處理攻擊連段行為
    /// </summary>
    public class AttackSystem : MonoBehaviour
    {
        private Animator ani;
        private string parAttack = "拳頭連段攻擊";
        private int attackCountMax = 4;
        private int attackCountCurrent;

        private void Awake()
        {
            ani = GetComponent<Animator>();
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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                attackCountCurrent++;
                Debug.Log(attackCountCurrent);
            }
        }
    }
}