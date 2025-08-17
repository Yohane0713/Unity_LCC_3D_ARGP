using System;
using TMPro;
using UnityEngine;

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
        private bool playerIn;
        private bool isInteractableStart;
        private Animator ani;

        public DataNPC data => dataNPC;
        public string inputMessage;

        private void Awake()
        {
            ani = GetComponent<Animator>();
            inputFieldDialogue.onEndEdit.AddListener(GetPlayerInput);
        }

        public void PlayAnimation(int index)
        {
            ani.SetTrigger(data.animatorParameters[index]);
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
            CloseInteractable();
        }

        private void PlayerInputAndShowUI()
        {
            if (!playerIn) return;
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

        private void CloseInteractable()
        {
            isInteractableStart = false;
            goInteractbale.SetActive(false);
            inputFieldDialogue.text = string.Empty;
            inputFieldDialogue.DeactivateInputField();
            GameManager.Instance.HideCursorStartControl();
        }
    }
}