namespace FreeMS.Constants
{
    /// <summary>
    /// 游戏区状态。
    /// </summary>
    public enum ServerStatus : ushort
    {
        /// <summary>
        /// 正常。
        /// </summary>
        Normal,

        /// <summary>
        /// 人数较多。
        /// </summary>
        HighlyPopulated,

        /// <summary>
        /// 人数已满。
        /// </summary>
        Full
    }
}