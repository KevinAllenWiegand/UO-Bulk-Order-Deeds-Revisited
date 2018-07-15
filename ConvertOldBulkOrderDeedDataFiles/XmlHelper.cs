using System;
using System.Xml;

namespace ConvertOldBulkOrderDeedDataFiles
{
    internal class XmlHelper
    {
        public static T GetAttributeValue<T>(XmlNode xmlNode, string attributeName)
        {
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
            var value = xmlNode.InnerText;

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
