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

        public List<MatchProperty> Matches
        {
            get { return matches; }
        }

        public VerifyRule(string commandName,string argumentName,string argumentValue)
        {
            this.commandName = commandName;
            this.argumentName = argumentName;
            this.argumentValue = argumentValue;
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
                    VerifyRule rule = new VerifyRule(commandName, argumentName, argumentValue);
                    while (reader.ReadToFollowing("match"))
                        rule.Matches.Add(MatchPropertyFactory.CreateFromXmlReader(reader));
                    rules.Add(rule);                    
                }
            }
            return rules;
        }
    }
}
