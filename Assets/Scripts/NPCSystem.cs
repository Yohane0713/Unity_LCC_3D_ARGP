using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mtaka
{
    public class NPCSystem : MonoBehaviour
    {
        [SerializeField, Header("群組_NPC_互動介面")]
        private GameObject goInteractbale;
        [SerializeField, Header("NPC_互動提示")]
        private GameObject goTip;
        [SerializeField, Header("輸入欄位_對話")]
        private TMP_InputField inputFieldDialogue;
        [SerializeField, Header("NPC 資料")]
        private DataNPC dataNPC;
        [SerializeField, Header("群組_對話框")]
        private GameObject goDialogue;
        [SerializeField, Header("文字_對話內容")]
        private TMP_Text textContent;
        [SerializeField, Header("按鈕_關閉")]
        private Button btnClose;
        [SerializeField, Header("敵人_牛頭人")]
        private GameObject enemy;
        [SerializeField, Header("敵人介面")]
        private GameObject enemyUI;
        [SerializeField, Header("敵人_BGM")]
        private AudioClip soundEnemy;
        [SerializeField, Header("BGM來源")]
        private AudioSource bgmSource;
        private bool playerIn;
        private bool isInteractableStart;
        private bool handlingSubmit;
        private Animator ani;
        private string dialogue = "這座森林有一隻瘋狂的牛頭人，害我沒辦法安心住在這裡，你可以幫我處理牠嗎？";

        public DataNPC data => dataNPC;
        public string inputMessage;

        private void Awake()
        {
            ani = GetComponent<Animator>();
            inputFieldDialogue.onSubmit.AddListener(GetPlayerInput);
            btnClose.onClick.AddListener(() =>
            {
                if (goDialogue) goDialogue.SetActive(false);
                StartBattle();
            });
        }

        public void PlayAnimation(int index)
        {
            ani.SetTrigger(data.animatorParameters[index]);

            if (index == 3) StartCoroutine(ShowDialogue());
        }

        private IEnumerator ShowDialogue()
        {
            var aniState = ani.GetCurrentAnimatorStateInfo(0);
            float waitAnimation = aniState.length / Mathf.Max(0.0001f, Mathf.Abs(ani.speed));
            yield return new WaitForSeconds(waitAnimation);

            goDialogue.SetActive(true);
            textContent.text = dialogue;

            var canvasGroup = goDialogue.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }

            GameManager.Instance.ShowCursorStopControl();
        }

        private void Update()
        {
            PlayerInputAndShowUI();
        }

        private void OnTriggerEnter(Collider other)
        {
            goTip.SetActive(true);
            playerIn = true;
        }

        private void OnTriggerExit(Collider other)
        {
            goTip.SetActive(false);
            playerIn = false;
            if (isInteractableStart) CloseInteractable();
        }

        /// <summary>
        /// 獲得玩家輸入訊息
        /// </summary>
        /// <param name="message">玩家輸入的訊息</param>
        private void GetPlayerInput(string message)
        {
            LogSystem.Log(message, "#f36");
            inputMessage = message;
            HuggingFaceManager.instance.StartGetSimilarity();
            CloseInteractable(keepControlBlocked: false);
        }

        private void PlayerInputAndShowUI()
        {
            if (!playerIn) return;
            if (goDialogue != null && goDialogue.activeSelf) return;

            if (Input.GetKeyDown(KeyCode.F))
            {
                isInteractableStart = !isInteractableStart;
                goInteractbale.SetActive(isInteractableStart);

                if (isInteractableStart)
                {
                    GameManager.Instance.ShowCursorStopControl();
                    inputFieldDialogue.text = string.Empty;
                    inputFieldDialogue.ActivateInputField();
                    inputFieldDialogue.Select();
                }
                else
                {
                    CloseInteractable();
                }
            }
        }

        private void CloseInteractable(bool keepControlBlocked = false)
        {
            isInteractableStart = false;
            if (goInteractbale) goInteractbale.SetActive(false);
            inputFieldDialogue.text = string.Empty;

            if (!keepControlBlocked)
                GameManager.Instance.HideCursorStartControl();
        }

        public void StartBattle()
        {
            if (bgmSource != null && soundEnemy != null)
            {
                bgmSource.loop = true;
                bgmSource.clip = soundEnemy;
                bgmSource.Play();
            }

            if (enemy != null)
            {
                enemy.SetActive(true);
            }

            if (enemyUI != null)
            {
                enemyUI.SetActive(true);
            }
            isInteractableStart = false;
            if (goInteractbale) goInteractbale.SetActive(false);
            GameManager.Instance.HideCursorStartControl();
        }
    }
}