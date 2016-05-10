using CommandLine;

namespace XadesDemo.Configurations.Options
{
    [Verb("verify", HelpText = "Проверка подписи XML файла")]
    public class VerifyOptions : XadesOptionBase
    {
        [Option('f', "input", HelpText = "Подписанный XML файл", Required = true)]
        public string InputFileName { get; set; }

        [Option('e', "element", HelpText = "Id подписанного элемента(по умолчанию проверяется корневой элемент)")]
        public string Element { get; set; }
    }
}