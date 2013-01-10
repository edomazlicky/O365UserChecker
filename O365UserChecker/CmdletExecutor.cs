using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Remoting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Collections.Specialized;
using System.Reflection;
using System.Xml;

namespace O365UserChecker
{
    /// <remarks>
    /// Pipeline executes necessary O365 Powershell commands
    /// </remarks>
    class CmdletExecutor : IDisposable 
    {

        private string adminUsername;
        private SecureString adminPassword;
        private PowerShell powershell = null;
        private NameValueCollection ToStringHints;

        public CmdletExecutor(string adminUsername, SecureString adminPassword, string xmlfilename)
        {
            this.adminUsername = adminUsername;
            this.adminPassword = adminPassword;
            LoadToStringHints(xmlfilename);

        }

        private void LoadToStringHints(string xmlfilename)
        {
            ToStringHints = new NameValueCollection();
            using (XmlReader reader = XmlReader.Create(xmlfilename))
            {
                while (reader.ReadToFollowing("typehint"))
                {
                    reader.MoveToAttribute("name");
                    string name = reader.Value;
                    reader.MoveToAttribute("property");
                    string propvalue = reader.Value;
                    ToStringHints[name] = propvalue;                    
                }
            }

        }

        /// <summary>
        /// Logins onto Office 365, must be called before executing commands
        /// </summary>
        public bool LoginToOffice365()
        {
            InitialSessionState iss = InitialSessionState.CreateDefault();
            string[] modules = {"MSOnline"};
            iss.ImportPSModule(modules);
            PSCredential cred = new PSCredential(adminUsername, adminPassword);
            Runspace rs = RunspaceFactory.CreateRunspace(iss);
            rs.Open();
            powershell = PowerShell.Create();
            powershell.Runspace = rs;
            Command connectCommand = new Command("Connect-MsolService");
            connectCommand.Parameters.Add("Credential", cred);
            powershell.Commands.AddCommand(connectCommand);
            try
            {
                powershell.Invoke();
                int errtotal = 0;
                foreach (ErrorRecord err in powershell.Streams.Error.ReadAll())                
                    errtotal++;                
                powershell.Commands.Clear();
                return errtotal <= 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Utility function to turn a powershell object to string using ToStringHints if necessary
        /// </summary>
        private string PowerShellObjToString(object obj)
        {
            if (ToStringHints[obj.GetType().ToString()] != null)
            {
                PropertyInfo prop = obj.GetType().GetProperty("AccountSkuId");
                MethodInfo prop_method = prop.GetGetMethod();
                return (string)prop_method.Invoke(obj, null);
            }
            else
                return obj.ToString();                
        }


        /// <summary>
        /// Executes arbitary commands with Parameters
        /// </summary>
        public NameValueCollection ExecuteCommand(string CommandName, NameValueCollection Parameters, ref string ErrorOutput)
        {
            PSCommand command = new PSCommand();
            ErrorOutput = "";
            NameValueCollection output = new NameValueCollection();
            command.AddCommand(CommandName);
            foreach(string key in Parameters)
                command.AddParameter(key,Parameters[key]);
            powershell.Commands = command;
            foreach (PSObject psobject in powershell.Invoke())
            {
                foreach (PSPropertyInfo propinfo in psobject.Properties)
                {
                    if (propinfo.Value != null)
                    {
                        if (propinfo.Value.GetType().IsGenericType)
                        {
                            if (propinfo.Value.GetType().GetGenericTypeDefinition() == typeof(List<>))
                            {
                                String list_output="{";
                                System.Collections.IList proplist = (System.Collections.IList)propinfo.Value;
                                foreach (object obj in proplist)
                                {
                                    list_output += PowerShellObjToString(obj) + ",";
                                }
                                if (list_output.EndsWith(","))
                                    list_output = list_output.Remove(list_output.Length - 1);
                                list_output += "}";
                                output.Add(propinfo.Name, list_output);
                            }
                            else
                                output.Add(propinfo.Name, PowerShellObjToString(propinfo.Value));
                        }
                        else
                            output.Add(propinfo.Name, PowerShellObjToString(propinfo.Value));
                    }
                    else
                        output.Add(propinfo.Name, null);
                }
            }
            foreach (ErrorRecord err in powershell.Streams.Error.ReadAll())
            {
                ErrorOutput += "Error executing command:"+CommandName+" error:"+err.Exception.Message;
            }
            return output;
        }

        public void Dispose()
        {
            if (powershell != null)
                powershell.Dispose();
        }

    }
   
}
