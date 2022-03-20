namespace FreeMS.Constants
{
    /// <summary>
    /// 客户端发送给服务端的命令。
    /// </summary>
    public enum OpCode : ushort
    {
        /// <summary>
        /// 使用账号密码登录。
        /// </summary>
        Login = 0x01,
    }
}