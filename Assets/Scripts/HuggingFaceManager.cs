using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mtaka
{
    [Serializable]
    public class SentenceSimInputs
    {
        [JsonProperty("source_sentence")]
        public string source_sentence;

        [JsonProperty("sentences")]
        public List<string> sentences;
    }

    [Serializable]
    public class SentenceSimRequest
    {
        [JsonProperty("inputs")]
        public SentenceSimInputs inputs;
    }

    public class HuggingFaceManager : MonoBehaviour
    {
        #region 金鑰
        private string key = "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";
        #endregion

        private static HuggingFaceManager _instance;
        public static HuggingFaceManager instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<HuggingFaceManager>();
                return _instance;
            }
        }
        private string model = "https://router.huggingface.co/hf-inference/models/sentence-transformers/paraphrase-multilingual-MiniLM-L12-v2/pipeline/sentence-similarity";

        [SerializeField, Header("NPC")]
        private NPCSystem npcSystem;

        public void StartGetSimilarity()
        {
            StartCoroutine(GetSimilarity());
        }

        private IEnumerator GetSimilarity()
        {
            // 輸入Model需要的資料
            // 玩家輸入的文字
            // NPC語句
            var requestData = new SentenceSimRequest
            {
                inputs = new SentenceSimInputs
                {
                    source_sentence = npcSystem?.inputMessage ?? string.Empty,
                    sentences = npcSystem?.data?.sentences?.ToList() ?? new List<string>()
                }
            };

            if (string.IsNullOrWhiteSpace(requestData.inputs.source_sentence) || requestData.inputs.sentences.Count == 0)
            {
                LogSystem.Log("來源句子或候選句子為空，已略過呼叫。", "#f93");
                yield break;
            }

            string jsonBody = JsonConvert.SerializeObject(requestData);
            byte[] postData = Encoding.UTF8.GetBytes(jsonBody);

            var request = new UnityWebRequest(model, UnityWebRequest.kHttpVerbPOST);
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + key);

            yield return request.SendWebRequest();

            string body = request.downloadHandler != null ? request.downloadHandler.text : "";
            if (request.result != UnityWebRequest.Result.Success)
            {
                LogSystem.Log($"HTTP Error: {request.responseCode} - {request.error}\n{body}", "#f33");
                yield break;
            }
            try
            {
                var response = JsonConvert.DeserializeObject<List<float>>(body);
                LogSystem.Log($"Response: {body}", "#3f3");

                if (response != null && response.Count > 0)
                {
                    int best = response
                        .Select((value, index) => new { value, index })
                        .OrderByDescending(x => x.value)
                        .First().index;

                    LogSystem.Log($"最佳結果：{npcSystem.data.sentences[best]}", "#f93");
                    npcSystem.PlayAnimation(best);
                }
                else
                {
                    LogSystem.Log("API 回傳非預期格式或空結果。", "#f33");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Log($"解析回應失敗：{ex.Message}\n原始回應：{body}", "#f33");
            }
        }
    }
}