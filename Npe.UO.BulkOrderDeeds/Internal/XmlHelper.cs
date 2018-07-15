using System;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds.Internal
{
    internal class XmlHelper
    {
        public static T GetAttributeValue<T>(XmlNode xmlNode, string attributeName)
        {
            Guard.ArgumentNotNull(nameof(xmlNode), xmlNode);
            Guard.ArgumentNotNullOrEmpty(nameof(attributeName), attributeName);

            try
            {
                var value = xmlNode.Attributes[attributeName].InnerText;

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public static T GetNodeValue<T>(XmlNode xmlNode)
        {
            Guard.ArgumentNotNull(nameof(xmlNode), xmlNode);

            var value = xmlNode.InnerText;

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
