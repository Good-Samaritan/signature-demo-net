namespace XadesDemo.Infrastructure
{
    internal static class Constants
    {
        internal const string GisServicesConfigSectionName = "GisServicesConfig";
        internal const string XadesConfigSectionName = "signingConfig";

        internal const string SignElementName = "sign-element";
        internal const string SoapContentXpath = "soap:Envelope/soap:Body/*";
        internal const string SoapBodyXpath = "soap:Envelope/soap:Body";
        internal const string SoapHeaderXpath = "soap:Envelope/soap:Header";
        internal const string SignatureName = "Signature";

        internal const string GetStateMethodName = "getState";
        internal const string MessageGuidXpath = "./m:getStateRequest/m:MessageGUID";
    }
}
