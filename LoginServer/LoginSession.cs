namespace FreeMS;

using Net;

class LoginSession : MapleSession<LoginSession, LoginClient>
{
    public Account Account { get; set; }

    protected override void OnStart()
    {
        Client.SendHello();
    }
}