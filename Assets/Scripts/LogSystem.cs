using UnityEngine;

namespace Mtaka
{
    /// <summary>
    /// 輸出系統
    /// </summary>
    public class LogSystem : MonoBehaviour
    {
        public static void Log(string message, string color = "#fff")
        {
            Debug.Log($"<color={color}>{message}</color>");
        }
    }
}