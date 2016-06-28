using System;
using Xades.Abstractions;
using XadesDemo.Configurations.Options;
using XadesDemo.Configurations.Sections;
using XadesDemo.Infrastructure;

namespace XadesDemo.Commands
{
    public class GetStateCommand : GisCommandBase<GetStateOptions>
    {
        public GetStateCommand(GetStateOptions option, IXadesService xadesService, SigningConfiguration signingConfig, GisServiceConfiguration serviceConfig) 
            : base(option, xadesService, signingConfig, serviceConfig)
        {
        }

        protected override bool IsSignatureRequired { get { return false;}}

        protected override void OnExecute(GetStateOptions option)
        {
            var valuesDictionary = new [] { new Tuple<string, string>(Constants.MessageGuidXpath, option.MessageGuid) };
            SendRequest(option.ServiceName, Constants.GetStateMethodName, valuesDictionary, option.OutputFileName);
        }
    }
}