using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Xades.Helpers
{
    public static class CertificateHelper
    {
        /// <summary>
        /// Получение сертификата из личного локального хранилища по отпечатку
        /// </summary>
        /// <param name="thumbprint">Отпечаток требуемого сертификата</param>
        /// <returns>Сертификат с нужным отпечатком</returns>
        public static X509Certificate2 GetCertificateByThumbprint(string thumbprint)
        {
            var certificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            try
            {
                certificateStore.Open(OpenFlags.ReadOnly);
                var certificateCollection = certificateStore.Certificates
                                                            .Cast<X509Certificate2>()
                                                            .Where(i => string.Equals(i.Thumbprint, thumbprint, StringComparison.InvariantCultureIgnoreCase))
                                                            .ToArray();

                if (!certificateCollection.Any())
                {
                    throw new ArgumentException("Некорректный отпечаток сертификата");
                }

                var activeCertificates = certificateCollection.Where(i => DateTime.Parse(i.GetEffectiveDateString()) <= DateTime.Now &&
                                                                          DateTime.Now <= DateTime.Parse(i.GetExpirationDateString()))
                                                              .ToArray();
                if (activeCertificates.Any())
                {
                    return activeCertificates[0];
                }

                throw new ArgumentException(string.Format("Сертификат с указанным отпечатком {0} недействителен", thumbprint));
            }
            finally
            {
                certificateStore.Close();
            }
        }

        /// <summary>
        /// Получение сертификатов из локального хранилища текущего пользователя
        /// </summary>
        /// <returns>Сертификаты из локального хранилища текущего пользователя</returns>
        public static IEnumerable<X509Certificate2> GetCertificates()
        {
            var certificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            try
            {
                certificateStore.Open(OpenFlags.ReadOnly);
                return certificateStore.Certificates.OfType<X509Certificate2>();
            }
            finally
            {
                certificateStore.Close();
            }
        }
    }
}