using Mtaka;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;

namespace Mtaka
{
    /// <summary>
    /// 血量系統
    /// </summary>
    public class HpSystem : MonoBehaviour
    {
        [SerializeField, Header("血量最大值"), Range(0, 5000)]
        private float hpMax = 1000;
        [SerializeField, Header("血條紅色")]
        private Image imgRed;
        [SerializeField, Header("血條白色")]
        private Image imgWhite;
        [SerializeField, Header("血條紅色縮減時間"), Range(0, 1)]
        private float redHpShortTime = 0.05f;
        [SerializeField, Header("血條紅色縮減延遲"), Range(0, 1)]
        private float redHpShortDelay = 0.5f;

        private float hp;
        private WaitForSeconds waitRedHpShortTime;
        private WaitForSeconds waitRedHpShortDelay;
        private Animator ani;
        private string parDead = "觸發死亡";
        private bool isDead;

        public event EventHandler onDead;

        protected virtual void Awake()
        {
            hp = hpMax;
            waitRedHpShortTime = new WaitForSeconds(redHpShortTime);
            waitRedHpShortDelay = new WaitForSeconds(redHpShortDelay);
            ani = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isDead) return;
            // 碰撞後嘗試抓取碰撞物的AttackArea元件
            if(other.TryGetComponent(out AttackArea attackArea))
            {
                float attack = attackArea.attackValue;
                Damage(attack);
            }
        }

        private void Damage(float damage)
        {
            if (GameManager.gameOver) return;
            float hpOriginal = hp;
            hp -= damage;
            StopAllCoroutines();
            StartCoroutine(RedHpShortEffect(hpOriginal));
            imgWhite.fillAmount = hp / hpMax;
            LogSystem.Log($"{name}受傷，血量:{hp}", "#7f9");
            if (hp <= 0) Dead();
        }

        protected virtual void Dead()
        {
            isDead = true;
            ani.SetTrigger(parDead);
            onDead?.Invoke(this, null);
            LogSystem.Log($"{name}受傷，死亡", "#f12");
        }

        /// <summary>
        /// 扣血後紅血縮減效果協程
        /// </summary>
        private IEnumerator RedHpShortEffect(float hpOriginal)
        {
            yield return waitRedHpShortDelay;
            float hpPerShort = (hpOriginal - hp) / 10;
            for (int i = 0; i < 10; i++)
            {
                hpOriginal -= hpPerShort;
                imgRed.fillAmount = hpOriginal / hpMax;
                yield return waitRedHpShortTime;
            }
        }
    }
}
