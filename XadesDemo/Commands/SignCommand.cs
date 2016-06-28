using System.IO;
using Xades.Abstractions;
using Xades.Helpers;
using XadesDemo.Configurations.Options;
using XadesDemo.Configurations.Sections;

namespace XadesDemo.Commands
{
    public class SignCommand : XadesCommandBase<SignOptions>
    {
        public SignCommand(SignOptions option, IXadesService xadesService, SigningConfiguration signingConfig) : base(option, xadesService, signingConfig)
        {
        }

        protected override void OnExecute(SignOptions option)
        {
            Info(string.Format("Выпоняется чтение файла {0}...", option.InputFileName));
            var xmlDocument = XmlDocumentHelper.Load(option.InputFileName);
            var elementId = Option.Element;

            Info("Выполняется подпись файла...");
            var resultXmlText = Sign(xmlDocument, elementId);
            File.WriteAllText(option.OutputFileName, resultXmlText);
            Success("Файл успешно подписан");
        }
    }
}