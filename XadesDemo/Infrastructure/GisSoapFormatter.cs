using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using Xades.Helpers;
using XadesDemo.Configurations.Sections;
using XadesDemo.Helpers;

namespace XadesDemo.Infrastructure
{
    public class GisSoapFormatter
    {

        public string SchemaVersion { get; set; }
        public string Template { get; set; }
        public string SenderId { get; set; }
        public IEnumerable<Tuple<string, string>> ValuesDictionary { get; set; }
        public bool AddSenderId { get; set; }
        public SoapConfiguration Config { get; set; }

        public string GetSoapRequest()
        {
            var header = GetXmlHeader();
            var body = GetXmlBody(Template, ValuesDictionary);
            return GetSoapText(header, body);
        }

        private XmlNode GetXmlHeader()
        {
            var headerXml = new XmlDocument();
            if (AddSenderId)
            {
                headerXml.Load(PathHelper.ToAppAbsolutePath(Config.RequestHeaderTemplatePath));

                var senderIdNode = headerXml.CreateNode(XmlNodeType.Element, headerXml.DocumentElement.Prefix, "SenderID", headerXml.DocumentElement.NamespaceURI);
                senderIdNode.InnerXml = SenderId;
                headerXml.DocumentElement.AppendChild(senderIdNode);

                var isOperatorSighnarure = headerXml.CreateNode(XmlNodeType.Element, headerXml.DocumentElement.Prefix, "IsOperatorSighnature", headerXml.DocumentElement.NamespaceURI);
                isOperatorSighnarure.InnerXml = "true";
                headerXml.DocumentElement.AppendChild(isOperatorSighnarure);
            }
            else
            {
                headerXml.Load(PathHelper.ToAppAbsolutePath(Config.ISRequestHeaderTemplatePath));
            }

            var guidNode = headerXml.CreateNode(XmlNodeType.Element, headerXml.DocumentElement.Prefix, "MessageGUID", headerXml.DocumentElement.NamespaceURI);
            guidNode.InnerXml = Guid.NewGuid().ToString();
            headerXml.DocumentElement.PrependChild(guidNode);

            var dataNode = headerXml.CreateNode(XmlNodeType.Element, headerXml.DocumentElement.Prefix, "Date", headerXml.DocumentElement.NamespaceURI);
            dataNode.InnerXml = DateTime.Now.ToString("o");
            headerXml.DocumentElement.PrependChild(dataNode);

            return headerXml.DocumentElement;
        }

        private string GetSoapText(XmlNode header, XmlNode body)
        {
            var soapXml = new XmlDocument();
            soapXml.Load(PathHelper.ToAppAbsolutePath(Config.SoapTemplatePath));

            var manager = soapXml.CreateNamespaceManager();

            var soapHeader = soapXml.SelectSingleNode(Constants.SoapHeaderXpath, manager);
            var importHeaderNode = soapHeader.OwnerDocument.ImportNode(header, true);
            soapHeader.AppendChild(importHeaderNode);

            var soapBody = soapXml.SelectSingleNode(Constants.SoapBodyXpath, manager);
            var importBodyNode = soapBody.OwnerDocument.ImportNode(body, true);
            soapBody.AppendChild(importBodyNode);

            var soapText = soapXml.OuterXml;
            return Regex.Replace(soapText, SchemeVersionPattern, SchemeVersionReplacement);
        }

        private XmlNode GetXmlBody(string templatePath, IEnumerable<Tuple<string, string>> xpathToValues)
        {
            var bodyXml = new XmlDocument();
            bodyXml.Load(templatePath);

            var manager = bodyXml.CreateNamespaceManager();

            foreach (var xpathToValuePair in xpathToValues)
            {
                var node = bodyXml.SelectSingleNode(xpathToValuePair.Item1, manager);
                if (node == null)
                {
                    throw new Exception(string.Format("Не найден элемент с путем: {0}",xpathToValuePair.Item1));
                }
                var value = ParseValue(xpathToValuePair.Item2);
                node.InnerXml = value;
            }

            return bodyXml.DocumentElement;
        }

        private string ParseValue(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return value;
            }
            Func<string> modifier;
            return ParseModifiers.TryGetValue(value.ToLower(),out modifier) ? modifier() : value;
        }

        private static readonly Dictionary<string, Func<string>> ParseModifiers = new Dictionary<string, Func<string>>()
        {
             {"{util:randomguid}", () => Guid.NewGuid().ToString("D") }
        };

        private static string SchemeVersionPattern
        {
            get { return "(?<scheme>http://dom.gosuslugi.ru/schema/integration/)\\d\\.\\d\\.\\d\\.\\d"; }
        }

        private string SchemeVersionReplacement
        {
            get { return string.Format("${{scheme}}{0}", SchemaVersion); }
        } 
    }
}