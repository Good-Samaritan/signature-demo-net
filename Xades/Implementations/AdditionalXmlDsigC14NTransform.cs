using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Xades.Implementations
{
    /// <summary>
    /// См. https://www.cryptopro.ru/forum2/default.aspx?g=posts&m=57702&find=lastpost  
    /// </summary>
    public class AdditionalXmlDsigC14NTransformOperation : IDisposable
    {
        public AdditionalXmlDsigC14NTransformOperation(XmlDocument document)
        {
            AdditionalXmlDsigC14NTransform.document = document;
            CryptoConfig.AddAlgorithm(typeof(AdditionalXmlDsigC14NTransform), "http://www.w3.org/TR/2001/REC-xml-c14n-20010315");
        }

        public void Dispose()
        {
            CryptoConfig.AddAlgorithm(typeof(XmlDsigC14NTransform), "http://www.w3.org/TR/2001/REC-xml-c14n-20010315");
            AdditionalXmlDsigC14NTransform.document = null;
        }

        public class AdditionalXmlDsigC14NTransform : XmlDsigC14NTransform
        {
            static XmlDocument _document;
            public static XmlDocument document
            {
                set
                {
                    _document = value;
                }
            }

            public override Object GetOutput()
            {
                return base.GetOutput();
            }

            public override void LoadInnerXml(XmlNodeList nodeList)
            {
                base.LoadInnerXml(nodeList);
            }

            protected override XmlNodeList GetInnerXml()
            {
                XmlNodeList nodeList = base.GetInnerXml();
                return nodeList;
            }

            public XmlElement GetXml()
            {
                return base.GetXml();
            }

            public override void LoadInput(Object obj)
            {
                int n;
                bool fDefaultNS = true;

                XmlElement element = ((XmlDocument)obj).DocumentElement;

                if (element.Name.Contains("SignedInfo"))
                {
                    XmlNodeList DigestValue = element.GetElementsByTagName("DigestValue", element.NamespaceURI);
                    string strHash = DigestValue[0].InnerText;
                    XmlNodeList nodeList = _document.GetElementsByTagName(element.Name);

                    for (n = 0; n < nodeList.Count; n++)
                    {
                        XmlNodeList DigestValue2 = ((XmlElement)nodeList[n]).GetElementsByTagName("DigestValue", ((XmlElement)nodeList[n]).NamespaceURI);
                        string strHash2 = DigestValue2[0].InnerText;
                        if (strHash == strHash2) break;
                    }

                    XmlNode node = nodeList[n];

                    while (node.ParentNode != null)
                    {
                        XmlAttributeCollection attrColl = node.ParentNode.Attributes;
                        if (attrColl != null)
                        {
                            for (n = 0; n < attrColl.Count; n++)
                            {
                                XmlAttribute attr = attrColl[n];
                                if (attr.Prefix == "xmlns")
                                {
                                    element.SetAttribute(attr.Name, attr.Value);
                                }
                                else if (attr.Name == "xmlns")
                                {
                                    if (fDefaultNS)
                                    {
                                        element.SetAttribute(attr.Name, attr.Value);
                                        fDefaultNS = false;
                                    }
                                }
                            }
                        }

                        node = node.ParentNode;
                    }
                }

                base.LoadInput(obj);
            }
        }
    }
}