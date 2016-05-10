using System;
using Xades.Helpers;
using XadesDemo.Configurations.Options;

namespace XadesDemo.Commands
{
    public class CertificateCommand : CommandBase<CertificateOptions>
    {
        public CertificateCommand(CertificateOptions option) : base(option)
        {
        }

        protected override void OnExecute(CertificateOptions option)
        {
            Option.Verbose = true;
            var certificates = CertificateHelper.GetCertificates();
            Info("Информация о сертификатах");
            foreach (var cert in certificates)
            {
                Info($"Субъект: {cert.Subject}");
                Info($"Отпечаток: {cert.Thumbprint}");
                Console.WriteLine();
            }
        }

    }
}