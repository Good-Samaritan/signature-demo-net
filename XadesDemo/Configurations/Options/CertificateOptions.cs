using CommandLine;

namespace XadesDemo.Configurations.Options
{
    [Verb("list-certs", HelpText = "Отобразить список сертификатов, установленных в локальное хранилище пользователя")]
    public class CertificateOptions : OptionBase
    {
    }
}
