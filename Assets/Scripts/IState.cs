namespace Mtaka
{
    /// <summary>
    /// 狀態：進入、離開與更新
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 狀態機：進入
        /// </summary>
        public void StateEnter();
        /// <summary>
        /// 狀態機：離開
        /// </summary>
        public void StateExit();
        /// <summary>
        /// 狀態機：更新
        /// </summary>
        public void StateUpdate();
    }
}
