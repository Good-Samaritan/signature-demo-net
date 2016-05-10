using System;
using System.Linq;
using System.Xml;
using Xades.Abstractions;
using Xades.Helpers;
using XadesDemo.Configurations.Options;
using XadesDemo.Configurations.Sections;

namespace XadesDemo.Commands
{
    public abstract class XadesCommandBase<TOption> : CommandBase<TOption> where TOption : XadesOptionBase
    {
        private readonly IXadesService _xadesService;
        private readonly SigningConfiguration _signingConfig;

        private string Password => Option.Password ?? _signingConfig.CertificatePassword;

        protected XadesCommandBase(TOption option, IXadesService xadesService, SigningConfiguration signingConfig) : base(option)
        {
            _xadesService = xadesService;
            _signingConfig = signingConfig;
        }

        protected string Sign(XmlDocument xml, string elementId)
        {
            if (string.IsNullOrEmpty(elementId))
            {
                var rootNode = xml.DocumentElement;
                var rootNodeId = GetRootId(rootNode);
                if (!string.IsNullOrEmpty(rootNodeId))
                {
                    Warning($"Не задан элемент для подписи. Используется корневой элемент {rootNode.Name} с Id {rootNodeId}");
                    elementId = rootNodeId;
                }
                else
                {
                    elementId = Guid.NewGuid().ToString("N");
                    var attribulte = xml.CreateAttribute("Id");
                    attribulte.Value = elementId;
                    rootNode.Attributes.Append(attribulte);
                    Warning($"Не задан элемент для подписи. Используется корневой элемент {rootNode.Name} с Id {elementId} (атрибут сгенерирован)");
                }
            }

            return _xadesService.Sign(xml.OuterXml, elementId, _signingConfig.CertificateThumbprint, Password);
        }

        protected string SignNode(XmlDocument xml, string xpath)
        {
            var manager = xml.CreateNamespaceManager();
            var node = xml.SelectSingleNode(xpath, manager);
            if (node == null)
            {
                throw new InvalidOperationException($"Не удалось найти узел{xpath}");
            }
            var nodeId = node.Attributes["id"];

            if (nodeId == null)
            {
                nodeId = xml.CreateAttribute("Id");
                node.Attributes.Append(nodeId);
            }

            if (string.IsNullOrEmpty(nodeId.Value))
            {
                nodeId.Value = Guid.NewGuid().ToString("N");
            }

            return  Sign(xml, nodeId.Value);
        }

        protected void Validate(XmlDocument xml, string elementId)
        {
            if (string.IsNullOrEmpty(elementId))
            {
                var rootNode = xml.DocumentElement;
                var rootNodeId = GetRootId(rootNode);
                if (!string.IsNullOrEmpty(rootNodeId))
                {
                    Warning($"Не задан элемент для проверки подписи. Используется элемент {rootNode.Name} с Id {rootNodeId}");
                    elementId = rootNodeId;
                }
                else
                {
                    throw new ArgumentException("Не задан Id элемента для проверки подписи и корневой элемент не имеет Id");
                }
            }
            _xadesService.ValidateSignature(xml.OuterXml, elementId);
        }

        private string GetRootId(XmlNode rootId)
        {
            var idName = _idAttributeNames.SingleOrDefault(x => rootId.Attributes[x] != null);
            return !string.IsNullOrEmpty(idName) ? rootId.Attributes[idName]?.Value : null;
        }

        private readonly string[] _idAttributeNames = {  "Id", "id", "ID", "iD", "_Id", "_id", "_ID", "_iD" };
    }
}