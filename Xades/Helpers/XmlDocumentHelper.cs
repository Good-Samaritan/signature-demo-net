using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Xades.Helpers
{
    public static class XmlDocumentHelper
    {
        /// <summary>
        /// �������� ������� ���� XmlDocument �� ������
        /// </summary>
        public static XmlDocument Create(string xmlData)
        {
            var xmlDocument = new XmlDocument { PreserveWhitespace = true };
            try
            {
                xmlDocument.LoadXml(xmlData);
            }
            catch (XmlException xmlEx)
            {
                throw new InvalidOperationException(string.Format("������������ xml ��������: {0}", xmlEx.Message), xmlEx);
            }
            return xmlDocument;
        }

        /// <summary>
        /// �������� ������� ���� XmlDocument �� �����
        /// </summary>
        public static XmlDocument Load(string pathName)
        {
            var xmlDocument = new XmlDocument { PreserveWhitespace = true };
            try
            {
                xmlDocument.Load(pathName);
            }
            catch (XmlException xmlEx)
            {
                throw new InvalidOperationException(string.Format("������������ xml ��������: {0}",xmlEx.Message), xmlEx);
            }
            return xmlDocument;
        }

        /// <summary>
        /// ������� �������� ����������� ���� �� ������� XmlDocument
        /// </summary>
        public static XmlNamespaceManager CreateNamespaceManager(this XmlDocument xml)
        {
            var manager = new XmlNamespaceManager(xml.NameTable);
            foreach (var name in GetNamespaceDictionary(xml))
            {
                manager.AddNamespace(name.Key, name.Value);
            }

            return manager;
        }

        private static IDictionary<string, string> GetNamespaceDictionary(this XmlDocument xml)
        {
            var nameSpaceList = xml.SelectNodes(@"//namespace::*[not(. = ../../namespace::*)]").OfType<XmlNode>();
            return nameSpaceList.Distinct(new LocalNameComparer()).ToDictionary(xmlNode => xmlNode.LocalName, xmlNode => xmlNode.Value);
        }

        private class LocalNameComparer : IEqualityComparer<XmlNode>
        {
            public bool Equals(XmlNode x, XmlNode y)
            {
                return x.LocalName == y.LocalName;
            }

            public int GetHashCode(XmlNode obj)
            {
                return obj.LocalName.GetHashCode();
            }
        }
    }
}