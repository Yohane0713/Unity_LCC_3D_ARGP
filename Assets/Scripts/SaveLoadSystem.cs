using System.Collections;
using UnityEngine;

namespace Mtaka
{
    /// <summary>
    /// 存讀檔介面和功能
    /// </summary>
    public class SaveLoadSystem : MonoBehaviour
    {
        private CanvasGroup groupSaveLoad;
        // 等待真實時間，不被時間暫停影響
        private WaitForSecondsRealtime waitFadeInterval = new WaitForSecondsRealtime(0.02f);

        private void Awake()
        {
            groupSaveLoad = GameObject.Find("").GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(Fade());
            }
        }

        private void SaveLoad()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0; //暫停
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                StartCoroutine(Fade());
            }
        }

        private IEnumerator Fade()
        {
            for (int i = 0; i < 10; i++)
            {
                groupSaveLoad.alpha += 0.1f;
                yield return waitFadeInterval;
            }

            groupSaveLoad.interactable = true;
            groupSaveLoad.blocksRaycasts = true;
        }
    }
}