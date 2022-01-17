using System.Collections.Generic;
using System.Xml;

namespace NUnit.VisualStudio.TestAdapter.NUnitEngine
{
    public interface INUnitTestNode
    {
        string Id { get; }
        string FullName { get; }
        string Name { get; }
        List<NUnitProperty> Properties { get; }
    }

    public abstract class NUnitTestNode : INUnitTestNode
    {
        protected XmlNode Node { get; set; }  // Need to be protected, but still the outputnodes are XmlNode
        public virtual string Id
        {
            get
            {
                var rawId = Node.GetAttribute("id");
                if (rawId == null)
                    return null;

                if (!rawId.StartsWith("0-"))
                    return "0" + rawId.Substring(1);

                return rawId;
            }
        }

        public string FullName => Node.GetAttribute("fullname");
        public string Name => Node.GetAttribute("name");
        public bool IsNull => Node == null;
        public List<NUnitProperty> Properties { get; } = new List<NUnitProperty>();

        protected NUnitTestNode(XmlNode node)
        {
            Node = node;
            var propertyNodes = Node.SelectNodes("properties/property");
            if (propertyNodes != null)
            {
                foreach (XmlNode prop in propertyNodes)
                {
                    Properties.Add(new NUnitProperty(prop.GetAttribute("name"), prop.GetAttribute("value")));
                }
            }
        }
    }
}