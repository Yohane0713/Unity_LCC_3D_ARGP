using UnityEngine;

namespace Mtaka
{
    /// <summary>
    /// 攻擊區域：碰撞器上攜帶的攻擊力
    /// </summary>
    public class AttackArea : MonoBehaviour
    {
        [SerializeField, Header("攻擊力"), Range(0, 500)]
        private float attack = 100;
        [SerializeField, Header("攻擊浮動"), Range(0, 1)]
        private float attackfloat = 0.3f;

        /// <summary>
        /// 攻擊數值
        /// </summary>
        public float attackValue => attack + attack * Random.Range(0, attackfloat);
    }
}