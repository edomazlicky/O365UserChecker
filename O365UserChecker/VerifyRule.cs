/*
 * Copyright 2013 Eric Domazlicky
   This file is part of O365 User Checker
   O365 User Checker is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
   O365 User Checker is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
   You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/. 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace O365UserChecker
{
    class VerifyRule
    {
        private string commandName, argumentName, argumentValue;
        private List<MatchProperty> matches;
        private PowershellType pstype;

        public string CommandName
        {
            get { return commandName; }
        }

        public string ArgumentName
        {
            get { return argumentName; }
        }

        public string ArgumentValue
        {
            get { return argumentValue; }
        }

        public PowershellType PSType
        {
            get { return pstype; }
        }

        public List<MatchProperty> Matches
        {
            get { return matches; }
        }

        public VerifyRule(string commandName,string argumentName,string argumentValue,PowershellType pstype)
        {
            this.commandName = commandName;
            this.argumentName = argumentName;
            this.argumentValue = argumentValue;
            this.pstype = pstype;
            matches = new List<MatchProperty>();            
        }
    }

    class VerifyRuleFactory
    {
        public static List<VerifyRule> LoadFromFile(string xmlfilename)
        {
            List<VerifyRule> rules = new List<VerifyRule>();
            using (XmlReader reader = XmlReader.Create(xmlfilename))
            {
                while (reader.ReadToFollowing("rule"))
                {

                    reader.MoveToAttribute("command");
                    string commandName = reader.Value;
                    reader.MoveToAttribute("argumentname");
                    string argumentName = reader.Value;
                    reader.MoveToAttribute("argumentvalue");
                    string argumentValue = reader.Value;
                    PowershellType pstype = PowershellType.MSOL;
                    if (reader.MoveToAttribute("powershell"))
                    {
                        string powershell = reader.Value;
                        if (String.Compare(powershell, "Live", true) == 0)
                            pstype = PowershellType.Live;
                    }
                    VerifyRule rule = new VerifyRule(commandName, argumentName, argumentValue, pstype);
                    if (reader.ReadToFollowing("match"))
                    {                        
                        rule.Matches.Add(MatchPropertyFactory.CreateFromXmlReader(reader));
                        while(reader.ReadToNextSibling("match"))
                            rule.Matches.Add(MatchPropertyFactory.CreateFromXmlReader(reader));                        
                        rules.Add(rule);
                    }
                }
            }
            return rules;
        }
    }

    public enum PowershellType { MSOL, Live };
}
