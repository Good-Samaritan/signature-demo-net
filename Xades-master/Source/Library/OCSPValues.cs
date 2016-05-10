// OCSPValues.cs
//
// XAdES Starter Kit for Microsoft .NET 3.5 (and above)
// 2010 Microsoft France
// Published under the CECILL-B Free Software license agreement.
// (http://www.cecill.info/licences/Licence_CeCILL-B_V1-en.txt)
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// WHETHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE. 
// THE ENTIRE RISK OF USE OR RESULTS IN CONNECTION WITH THE USE OF THIS CODE 
// AND INFORMATION REMAINS WITH THE USER. 
//

using System;
using System.Xml;
using System.Collections;

namespace Microsoft.Xades
{
	/// <summary>
	/// This class contains a collection of OCSPValues
	/// </summary>
	public class OCSPValues
	{
		#region Private variables
		private OCSPValueCollection ocspValueCollection;
		#endregion

		#region Public properties
		/// <summary>
		/// Collection of OCSP values
		/// </summary>
		public OCSPValueCollection OCSPValueCollection
		{
			get
			{
				return this.ocspValueCollection;
			}
			set
			{
				this.ocspValueCollection = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public OCSPValues()
		{
			this.ocspValueCollection = new OCSPValueCollection();
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Check to see if something has changed in this instance and needs to be serialized
		/// </summary>
		/// <returns>Flag indicating if a member needs serialization</returns>
		public bool HasChanged()
		{
			bool retVal = false;

			if (this.ocspValueCollection.Count > 0)
			{
				retVal = true;
			}

			return retVal;
		}

		/// <summary>
		/// Load state from an XML element
		/// </summary>
		/// <param name="xmlElement">XML element containing new state</param>
		public void LoadXml(System.Xml.XmlElement xmlElement)
		{
			XmlNamespaceManager xmlNamespaceManager;
			XmlNodeList xmlNodeList;
			OCSPValue newOCSPValue;
			IEnumerator enumerator;
			XmlElement iterationXmlElement;
			
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			this.ocspValueCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:OCSPValue", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						newOCSPValue = new OCSPValue();
						newOCSPValue.LoadXml(iterationXmlElement);
						this.ocspValueCollection.Add(newOCSPValue);
					}
				}
			}
			finally 
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		/// <summary>
		/// Returns the XML representation of the this object
		/// </summary>
		/// <returns>XML element containing the state of this object</returns>
		public XmlElement GetXml()
		{
			XmlDocument creationXmlDocument;
			XmlElement retVal;

			creationXmlDocument = new XmlDocument();
            retVal = creationXmlDocument.CreateElement("xades", "OCSPValues", XadesSignedXml.XadesNamespaceUri);

			if (this.ocspValueCollection.Count > 0)
			{
				foreach (OCSPValue ocspValue in this.ocspValueCollection)
				{
					if (ocspValue.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(ocspValue.GetXml(), true));
					}
				}
			}

			return retVal;
		}
		#endregion
	}
}
