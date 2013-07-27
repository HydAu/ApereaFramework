using System.Security.Cryptography.X509Certificates;

namespace Aperea.Identity.Settings
{
    public class EncryptionSettings : IEncryptionSettings
    {
        readonly CertificateStore certificateStore;

        public EncryptionSettings(ICertificateSettings settings)
        {
            certificateStore = new CertificateStore(settings, settings.EncryptingCertificateName);
        }

        public X509Certificate2 Certificate
        {
            get { return certificateStore.Certificate; }
        }

        public bool Encrypt
        {
            get { return Certificate != null; }
        }
    }
}