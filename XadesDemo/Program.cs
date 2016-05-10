using System;
using System.Configuration;
using CommandLine;
using CommandLine.Text;
using Xades.Implementations;
using XadesDemo.Commands;
using XadesDemo.Configurations.Options;
using XadesDemo.Configurations.Sections;
using XadesDemo.Infrastructure;

namespace XadesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var gisConfig = (GisServiceConfiguration)ConfigurationManager.GetSection(Constants.GisServicesConfigSectionName);
                var xadesConfig = (SigningConfiguration)ConfigurationManager.GetSection(Constants.XadesConfigSectionName);
                var xadesService = new GostXadesBesService();

                SentenceBuilder.UseSentenceBuilder(new RussianSentenceBuilder());

                ICommand command = Parser.Default.ParseArguments<SignOptions, VerifyOptions, CertificateOptions, GetStateOptions, SendOptions>(args)
                    .MapResult(
                        (SignOptions o) => new SignCommand(o, xadesService, xadesConfig),
                        (VerifyOptions o) => new VerifyCommand(o, xadesService, xadesConfig),
                        (SendOptions o) => new SendCommand(o, xadesService, xadesConfig, gisConfig),
                        (GetStateOptions o) => new GetStateCommand(o, xadesService, xadesConfig, gisConfig),
                        (CertificateOptions o) => new CertificateCommand(o),
                        errors => (ICommand)null);
                command?.Execute();
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Произошла ошибка:");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
