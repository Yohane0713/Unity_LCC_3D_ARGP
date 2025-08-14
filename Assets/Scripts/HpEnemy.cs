using System.Collections;
using UnityEngine;

namespace Mtaka
{
    /// <summary>
    /// 敵人血量系統
    /// </summary>
    public class HpEnemy : HpSystem
    {
        [Header("渲染元件")]
        [SerializeField] private Renderer[] ren;
        private string parDissolve = "dissolve";
        private Collider col;
        private WaitForSeconds waitDissolveDelay = new WaitForSeconds(0.5f);
        private WaitForSeconds waitDissolveInterval = new WaitForSeconds(0.02f);

        protected override void Awake()
        {
            base.Awake();
            col = GetComponent<Collider>();
        }

        protected override void Dead()
        {
            base.Dead();
            GameManager.Instance.StartFadeIn("挑戰成功");
            col.enabled = false;
            StartCoroutine(Dissolve());
        }

        private IEnumerator Dissolve()
        {
            yield return waitDissolveDelay;

            float dissolve = 6;

            while (dissolve >= -1)
            {
                dissolve -= 0.1f;

                foreach (var r in ren)
                {
                    if (r == null) continue;
                    var mats = r.materials;
                    for (int i = 0; i < mats.Length; i++)
                        mats[i].SetFloat(parDissolve, dissolve);
                }
                yield return waitDissolveInterval;
            }
        }
    }
}