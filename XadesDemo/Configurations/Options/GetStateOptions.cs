using CommandLine;

namespace XadesDemo.Configurations.Options
{
    [Verb("get-state", HelpText = "Запрос ответа на асинхронный метод ГИС ЖКХ")]
    public class GetStateOptions : XadesOptionBase
    {
        [Option('s', "service", HelpText = "Имя сервиса", Required = true)]
        public string ServiceName { get; set; }

        [Option('g', "guid", HelpText = "Уникальный идентификатор запроса в ГИС ЖКХ (MessageGUID)", Required = true)]
        public string MessageGuid { get; set; }

        [Option('o', "output", HelpText = "Файл для сохранения результатов выполнения запроса", Required = true)]
        public string OutputFileName { get; set; }

        [Option('a', "auth", HelpText = "Логин:пароль для basic авторизации", Required = true)]
        public override string BasicAuthorization { get; set; }
    }
}