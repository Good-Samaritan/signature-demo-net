namespace XadesDemo.Infrastructure
{
    internal static class Constants
    {
        private const string envelopNamespaceCondition = "namespace-uri()='http://schemas.xmlsoap.org/soap/envelope/'";

        internal const string GisServicesConfigSectionName = "GisServicesConfig";
        internal const string XadesConfigSectionName = "signingConfig";

        internal const string SignElementName = "sign-element";
        internal const string SoapContentXpath = "*[local-name()='Envelope' and " + envelopNamespaceCondition + "]/*[local-name()='Body' and " + envelopNamespaceCondition + "]/*";
        internal const string SoapBodyXpath = "*[local-name()='Envelope' and " + envelopNamespaceCondition + "]/*[local-name()='Body' and " + envelopNamespaceCondition + "]";
        internal const string SoapHeaderXpath = "*[local-name()='Envelope' and " + envelopNamespaceCondition + "]/*[local-name()='Header'and " + envelopNamespaceCondition + "]";
        internal const string SignatureName = "Signature";

        internal const string GetStateMethodName = "getState";
        internal const string MessageGuidXpath = "./m:getStateRequest/m:MessageGUID";
    }
}
