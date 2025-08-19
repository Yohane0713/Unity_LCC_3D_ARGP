using UnityEngine;

namespace Mtaka
{
    /// <summary>
    /// 攻擊區域：碰撞器相關設定
    /// </summary>
    public class AttackArea : MonoBehaviour
    {
        [SerializeField, Header("攻擊力"), Range(0, 500)]
        private float attack = 100;
        [SerializeField, Header("攻擊浮動"), Range(0, 1)]
        private float attackfloat = 0.3f;
        [Header("擊中音效")]
        public AudioClip hitSound;

        private Collider hitbox;

        /// <summary>
        /// 攻擊數值
        /// </summary>
        public float attackValue => attack + attack * Random.Range(0, attackfloat);

        private void Awake()
        {
            hitbox = GetComponent<Collider>();
            if (hitbox == null)
            {
                Debug.LogError($"{name} 缺少 Collider 作為攻擊判定！");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!hitbox.enabled) return;

            if (hitSound)
            {
                SoundManager.instance.PlaySound(hitSound);
            }
        }
    }
}