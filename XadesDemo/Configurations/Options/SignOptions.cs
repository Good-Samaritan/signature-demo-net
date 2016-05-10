using CommandLine;

namespace XadesDemo.Configurations.Options
{
    [Verb("sign", HelpText = "Подписать XML файл")]
    public class SignOptions : XadesOptionBase
    {
        [Option('f', "input", HelpText = "Файл, который необходимо подписать", Required = true)]
        public string InputFileName { get; set; }

        [Option('o', "output", HelpText = "Файл для сохранения подписанного документа", Required = true)]
        public string OutputFileName { get; set; }

        [Option('e', "element", HelpText = "Id элемента, который необходимо подписать(по умолчанию подписывается корневой элемент)")]
        public string Element { get; set; }

        [Option('p', "password", HelpText = "Пароль от выбраного сертификата")]
        public override string Password { get; set; }
    }
}