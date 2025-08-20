using Firebase.Database;
using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        private bool isPanelOpen;

        [SerializeField]
        private DataPlayer player;
        private Transform traPlayer;
        private Button btnLoadData, btnSaveData;
        private DatabaseReference dbReference;
        private string dataName = "測試資料";
        private ThirdPersonController controller;
        private Animator ani;

        private void Awake()
        {
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            traPlayer = GameObject.Find("玩家_Unity醬").transform;
            controller = traPlayer.GetComponent<ThirdPersonController>();
            ani = traPlayer.GetComponent<Animator>();
            groupSaveLoad = GameObject.Find("群組_存讀檔畫面").GetComponent<CanvasGroup>();
            groupSaveLoad.alpha = 0f;
            groupSaveLoad.interactable = false;
            groupSaveLoad.blocksRaycasts = false;
            isPanelOpen = false;
            btnLoadData = GameObject.Find("按鈕讀取檔案").GetComponent<Button>();
            btnSaveData = GameObject.Find("按鈕儲存檔案").GetComponent<Button>();
            btnLoadData.onClick.AddListener(Load);
            btnSaveData.onClick.AddListener(Save);
        }

        private void Update()
        {
            ShowSaveLoad();
        }

        private void ShowSaveLoad()
        {
            if (GameManager.gameOver) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopAllCoroutines();
                if (!isPanelOpen)
                {
                    Time.timeScale = 0; //暫停
                    GameManager.Instance.ShowCursorStopControl();
                    StartCoroutine(Fade());
                }
                else
                {
                    StartCoroutine(Fadeout());
                }
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
            isPanelOpen = true;
        }

        private void Load()
        {
            StartCoroutine(LoadData());
        }

        private IEnumerator LoadData()
        {
            // 從資料庫獲得資料
            var serverData = dbReference.Child(dataName).GetValueAsync();
            // 等待直到伺服器下載資料完成
            yield return new WaitUntil(() => serverData.IsCompleted);
            // 儲存獲得的結果
            DataSnapshot snapshot = serverData.Result;
            // 將結果轉為JSON字串
            string jsonData = snapshot.GetRawJsonValue();
            // 將JSON字串轉為玩家資料
            player = JsonUtility.FromJson<DataPlayer>(jsonData);
            // 等待淡出
            yield return StartCoroutine(Fadeout());
            // 設定玩家
            StartCoroutine(SetPlayer());
        }

        private void Save()
        {
            player.position = traPlayer.position;
            player.rotation = traPlayer.eulerAngles;
            string json = JsonUtility.ToJson(player);
            print(json);
            // 將Json資料儲存到資料庫的dataName下方
            dbReference.Child(dataName).SetRawJsonValueAsync(json);
            StartCoroutine(Fadeout());
        }

        /// <summary>
        /// 存檔完淡出
        /// </summary>
        private IEnumerator Fadeout()
        {
            groupSaveLoad.interactable = false;
            groupSaveLoad.blocksRaycasts = false;

            for (int i = 0; i < 10; i++)
            {
                groupSaveLoad.alpha -= 0.1f;
                yield return waitFadeInterval;
            }

            Time.timeScale = 1;
            GameManager.Instance.HideCursorStartControl();
            isPanelOpen = false;
        }

        private IEnumerator SetPlayer()
        {
            controller.enabled = false;
            ani.enabled = false;
            traPlayer.position = player.position;
            traPlayer.eulerAngles = player.rotation;
            yield return new WaitForSecondsRealtime(0.5f);
            controller.enabled = true;
            ani.enabled = true;
        }
    }
}