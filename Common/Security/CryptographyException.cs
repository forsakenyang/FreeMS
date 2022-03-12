namespace FreeMS.Security
{
    public class CryptographyException : ApplicationException
    {
        public CryptographyException() : base("A cryptography error occured.")
        {
        }

        public CryptographyException(string message) : base(message)
        {
        }
    }
}