namespace FreeMS.Constants;

/// <summary>
/// 服务端发送给客户端的命令。
/// </summary>
public enum ClientOpCode : ushort
{
    /// <summary>
    /// 登录状态。
    /// </summary>
    LoginStatus = 0x00,

    /// <summary>
    ///  服务器状态。
    /// </summary>
    ServerStatus = 0x06,

    /// <summary>
    /// 服务器列表。
    /// </summary>
    ServerList = 0x09,
}