using UnityEngine;

namespace Mtaka
{
    /// <summary>
    /// 狀態機：管理所有狀態
    /// </summary>
    public class StateMachine : MonoBehaviour
    {
        [SerializeField, Header("預設狀態")]
        private State stateDefault;
        [Header("追蹤狀態")]
        public State stateTrack;
        [Header("攻擊狀態1")]
        public State stateAttack1;

        private State stateCurrent;

        private void Start()
        {
            ChangeState(stateDefault);
        }

        private void Update()
        {
            stateCurrent?.StateUpdate();
        }

        /// <summary>
        /// 變更狀態
        /// </summary>
        /// <param name="newState">要變更的新狀態</param>
        public void ChangeState(State newState)
        {
            // 退出 當前狀態
            stateCurrent?.StateExit();
            // 指定當前狀態為 新的狀態
            stateCurrent = newState;
            // 進入 當前狀態
            stateCurrent?.StateEnter();
        }
    }
}