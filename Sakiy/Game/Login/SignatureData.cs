namespace Sakiy.Game.Login
{
    public sealed class SignatureData
    {
        public readonly long Expiration;
        public readonly byte[] PublicKey;
        public readonly byte[] Signature;
        internal SignatureData(long expiration, byte[] publicKey, byte[] signature)
        {
            Expiration = expiration;
            PublicKey = publicKey;
            Signature = signature;
        }
    }
}
