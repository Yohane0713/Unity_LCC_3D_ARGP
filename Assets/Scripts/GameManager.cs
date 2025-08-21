using StarterAssets;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mtaka
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<GameManager>();
                return _instance;
            }
        }
        public static bool gameOver;
        [SerializeField] private string uiActionMap = "UI";

        private CanvasGroup groupGameOver;
        private TMP_Text textTitle;
        private WaitForSeconds waitFade = new WaitForSeconds(0.02f);
        private ThirdPersonController controller;
        private AttackSystem attackSystem;
        private PlayerInput playerInput;

        private void Awake()
        {
            gameOver = false; // 靜態變數要手動還原預設值
            groupGameOver = GameObject.Find("群組_結束畫面").GetComponent<CanvasGroup>();
            textTitle = GameObject.Find("文字_結束標題").GetComponent<TMP_Text>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            controller = FindObjectOfType<ThirdPersonController>();
            attackSystem = FindObjectOfType<AttackSystem>();
            playerInput = FindObjectOfType<PlayerInput>();
            if (playerInput && playerInput.actions && playerInput.actions.FindActionMap(uiActionMap, false) == null)
            {
                Debug.LogWarning($"[GameManager]找不到UI Map'{uiActionMap}'，可用的Maps： " +
                                 string.Join(", ", playerInput.actions.actionMaps.Select(m => m.name)));
            }
        }

        /// <summary>
        /// 顯示滑鼠並停止控制
        /// </summary>
        public void ShowCursorStopControl()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            controller.enabled = false;
            attackSystem.enabled = false;
            if (playerInput != null) playerInput.SwitchCurrentActionMap("UI");
        }

        /// <summary>
        /// 隱藏滑鼠並開始控制
        /// </summary>
        public void HideCursorStartControl()
        {
            Cursor.visible = false;
            Cursor.lockState= CursorLockMode.Locked;
            controller.enabled = true;
            attackSystem.enabled = true;
            if (playerInput != null) playerInput.SwitchCurrentActionMap("Player");
        }

        /// <summary>
        /// 開始淡入
        /// </summary>
        /// <param name="title">結束標題</param>
        public void StartFadeIn(string title)
        {
            if (gameOver) return;
            textTitle.text = title;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            StartCoroutine(FadeGameOver());
        }

        /// <summary>
        /// 淡入結束畫面
        /// </summary>
        private IEnumerator FadeGameOver()
        {
            // 迴圈累加透明度
            for (int i = 0; i < 10; i++)
            {
                groupGameOver.alpha += 0.1f;
                yield return waitFade;
            }
            // 開啟互動遮擋
            groupGameOver.interactable = true;
            groupGameOver.blocksRaycasts = true;
        }
    }
}
