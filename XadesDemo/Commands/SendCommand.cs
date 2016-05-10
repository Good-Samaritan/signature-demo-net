using System;
using System.Linq;
using Xades.Abstractions;
using XadesDemo.Configurations.Options;
using XadesDemo.Configurations.Sections;

namespace XadesDemo.Commands
{
    public class SendCommand : GisCommandBase<SendOptions>
    {
        public SendCommand(SendOptions option, IXadesService xadesService,  SigningConfiguration signingConfig, GisServiceConfiguration serviceConfig) 
            : base(option, xadesService, signingConfig, serviceConfig)
        {
        }

        protected override bool IsSignatureRequired => true;

        protected override void OnExecute(SendOptions option)
        {
            var valueDictionary = Enumerable.Empty<Tuple<string, string>>();
            if (!string.IsNullOrEmpty(option.ParametersFileName))
            {
                Info($"Чтение данных из {option.ParametersFileName}...");
                valueDictionary = Helpers.CsvHelper.ReadCsv(option.ParametersFileName);
            }

            SendRequest(option.ServiceName, option.MethodName, valueDictionary, option.OutputFileName);
        }
    }
}