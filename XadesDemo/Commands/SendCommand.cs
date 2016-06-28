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

        protected override bool IsSignatureRequired
        {
            get { return true; }
        }

        protected override void OnExecute(SendOptions option)
        {
            var valueDictionary = Enumerable.Empty<Tuple<string, string>>();
            if (!string.IsNullOrEmpty(option.ParametersFileName))
            {
                Info(string.Format("Чтение данных из {0}...", option.ParametersFileName));
                valueDictionary = Helpers.CsvHelper.ReadCsv(option.ParametersFileName);
            }

            SendRequest(option.ServiceName, option.MethodName, valueDictionary, option.OutputFileName);
        }
    }
}