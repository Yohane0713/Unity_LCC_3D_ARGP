using UnityEngine;

namespace Mtaka
{
    [CreateAssetMenu(menuName = "Mtaka/NPC")]
    public class DataNPC : ScriptableObject
    {
        [Header("動畫參數")]
        public string[] animatorParameters;
        [Header("Hugging Face要分析的句子")]
        public string[] sentences; 
    }
}