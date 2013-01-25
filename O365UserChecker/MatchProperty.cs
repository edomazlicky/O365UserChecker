/*
 * Copyright 2013 Eric Domazlicky
   This file is part of O365 User Checker
   O365 User Checker is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
   O365 User Checker is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
   You should have received a copy of the GNU General Public License. If not, see http://www.gnu.org/licenses/. 
 */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;

namespace O365UserChecker
{
    /// <remarks>
    /// Class to represent a Match rule
    /// </remarks>
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

        /// <summary>
        /// Compares an O365 User property collection to an on-premise user collection
        ///  or a fixed value depending on how the MatchProperty was initialized
        /// </summary>
        public bool IsMatch(NameValueCollection O365User, NameValueCollection LocalUser,ref string values)
        {
            if ((O365User == null) || (LocalUser == null))
                return false;
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

    /// <remarks>
    /// Creates a MatchProperty from an XmlReader object (assumes reader "cursor" is in correct place)
    /// </remarks>
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
