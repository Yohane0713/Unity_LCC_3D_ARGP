using UnityEngine;

namespace Mtaka
{
    /// <summary>
    /// �����ϰ�G�I�����W��a�������O
    /// </summary>
    public class AttackArea : MonoBehaviour
    {
        [SerializeField, Header("�����O"), Range(0, 500)]
        private float attack = 100;
        [SerializeField, Header("�����B��"), Range(0, 1)]
        private float attackfloat = 0.3f;

        /// <summary>
        /// �����ƭ�
        /// </summary>
        public float attackValue => attack + attack * Random.Range(0, attackfloat);
    }
}