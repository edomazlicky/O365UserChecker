using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;

namespace O365UserChecker
{
    class MatchProperty
    {
        private string propertyName, propertyValue;

        public string PropertyName
        {
            get{ return propertyName; }
        }

        public string PropertyValue
        {
            get { return propertyValue; }
        }

        public MatchProperty(string propertyName, string propertyValue)
        {
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
        }       

        public bool IsMatch(NameValueCollection O365User, NameValueCollection LocalUser,ref string values)
        {
            if (propertyValue.StartsWith("$"))
            {
                values = O365User[propertyName] + " " + LocalUser[propertyValue.Substring(1)];
                return String.Compare(O365User[propertyName], LocalUser[propertyValue.Substring(1)])==0;
            }
            else
            {
                values = O365User[propertyName] + " " + propertyValue;
                return String.Compare(O365User[propertyName], propertyValue)==0;
            }
        }

    }

    class MatchPropertyFactory
    {
        public static MatchProperty CreateFromXmlReader(XmlReader reader)
        {            
            reader.MoveToAttribute("property");
            string propertyName = reader.Value;
            reader.MoveToAttribute("value");
            string propertyValue = reader.Value;
            return new MatchProperty(propertyName, propertyValue);
        }

    }
}
