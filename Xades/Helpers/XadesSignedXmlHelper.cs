using System.Xml;
using Microsoft.Xades;

namespace Xades.Helpers
{
    public static class XadesSignedXmlHelper
    {
        public static void InjectSignatureTo(this XadesSignedXml signedXml, XmlDocument originalDoc)
        {
            var signatureElement = signedXml.GetXml();
            var importSignatureElement = originalDoc.ImportNode(signatureElement, true);
            var signedDataContainer = signedXml.GetIdElement(originalDoc, signedXml.SignedElementId);
            signedDataContainer.InsertBefore(importSignatureElement, signedDataContainer.FirstChild);
        }
    }
}
