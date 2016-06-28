using CommandLine;

namespace XadesDemo.Configurations.Options
{
    [Verb("send", HelpText = "Отправка запроса в ГИС ЖКХ")]
    public class 
        SendOptions : XadesOptionBase
    {
        [Option('s', "service", HelpText = "Имя сервиса", Required = true)]
        public string ServiceName { get; set; }

        [Option('m', "method", HelpText = "Метод сервиса", Required = true)]
        public string MethodName { get; set; }

        [Option('o', "output", HelpText = "Файл для сохранения результатов выполнения запроса", Required = true)]
        public string OutputFileName { get; set; }

        [Option('c', "csv", HelpText = "Файл с входными параметрами для запроса")]
        public string ParametersFileName { get; set; }

        [Option('p', "password", HelpText = "Пароль от выбраного сертификата")]
        public override string Password { get; set; }

        [Option('a', "auth", HelpText = "Логин:пароль для basic авторизации", Required = true)]
        public override string BasicAuthorization { get; set; }
    }
}