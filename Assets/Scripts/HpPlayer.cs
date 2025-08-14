using StarterAssets;
using UnityEngine;

namespace Mtaka
{
    /// <summary>
    /// 血量系統：玩家
    /// </summary>
    public class HpPlayer : HpSystem
    {
        private ThirdPersonController controller;
        private AttackSystem attackSystem;

        protected override void Awake()
        {
            base.Awake();
            controller = GetComponent<ThirdPersonController>();
            attackSystem = GetComponent<AttackSystem>();
        }

        protected override void Dead()
        {
            base.Dead();
            GameManager.Instance.StartFadeIn("挑戰失敗");
            controller.enabled = false;
            attackSystem.enabled = false;
        }
    }
}