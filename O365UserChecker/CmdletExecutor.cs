/*
 * Copyright 2013 Eric Domazlicky
   This file is part of O365 User Checker
   O365 User Checker is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
   O365 User Checker is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
   You should have received a copy of the GNU General Public License. If not, see http://www.gnu.org/licenses/. 
 */ 
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
        private PowerShell MSOLPowershell = null;
        private PowerShell LivePowershell = null;
        private NameValueCollection ToStringHints;

        public CmdletExecutor(string adminUsername, SecureString adminPassword, NameValueCollection ToStringHints)
        {
            this.adminUsername = adminUsername;
            this.adminPassword = adminPassword;
            this.ToStringHints = ToStringHints;            
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
            MSOLPowershell = PowerShell.Create();
            MSOLPowershell.Runspace = rs;
            Command connectCommand = new Command("Connect-MsolService");
            connectCommand.Parameters.Add("Credential", cred);
            MSOLPowershell.Commands.AddCommand(connectCommand);
            try
            {
                MSOLPowershell.Invoke();
                int errtotal = 0;
                foreach (ErrorRecord err in MSOLPowershell.Streams.Error.ReadAll())                
                    errtotal++;
                MSOLPowershell.Commands.Clear();
                // now connect to MS Live Powershell for other cmdlet support
                WSManConnectionInfo connectionInfo = new WSManConnectionInfo(new Uri("https://ps.outlook.com/PowerShell-LiveID?PSVersion=2.0"), "http://schemas.microsoft.com/powershell/Microsoft.Exchange", cred);
                connectionInfo.AuthenticationMechanism = AuthenticationMechanism.Basic;
                connectionInfo.SkipCACheck = true;
                connectionInfo.SkipCNCheck = true;
                connectionInfo.MaximumConnectionRedirectionCount = 30;
                Runspace LRS = RunspaceFactory.CreateRunspace(connectionInfo);
                LRS.Open();
                LivePowershell = PowerShell.Create();
                LivePowershell.Runspace = LRS;                
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
                PropertyInfo prop = obj.GetType().GetProperty(ToStringHints[obj.GetType().ToString()]);
                MethodInfo prop_method = prop.GetGetMethod();
                return (string)prop_method.Invoke(obj, null);
            }
            else
                return obj.ToString();                
        }


        /// <summary>
        /// Executes arbitary commands with Parameters
        /// </summary>
        public NameValueCollection ExecuteCommand(string CommandName, NameValueCollection Parameters,PowershellType pstype, ref string ErrorOutput)
        {
            PowerShell powershell = this.MSOLPowershell;
            if (pstype == PowershellType.Live)
                powershell = this.LivePowershell;
            if (powershell == null)
                return null;
            PSCommand command = new PSCommand();
            ErrorOutput = "";
            NameValueCollection output = new NameValueCollection();
            command.AddCommand(CommandName);
            foreach(string key in Parameters)
                command.AddParameter(key,Parameters[key]);
            powershell.Commands = command;
            try
            {
                foreach (PSObject psobject in powershell.Invoke())
                {
                    if (psobject != null)
                    {
                        foreach (PSPropertyInfo propinfo in psobject.Properties)
                        {
                            if (propinfo.Value != null)
                            {
                                // turn strings into formatted {lists}
                                if (propinfo.Value.GetType().IsGenericType)
                                {
                                    if (propinfo.Value.GetType().GetGenericTypeDefinition() == typeof(List<>))
                                    {
                                        String list_output = "{";
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
                }
                foreach (ErrorRecord err in powershell.Streams.Error.ReadAll())
                {
                    ErrorOutput += "Error executing command:" + CommandName + " error:" + err.Exception.Message;
                }
                return output;
            }
            catch (Exception ex)
            {
                ErrorOutput += "Error executing command:" + CommandName + " error:" + ex.Message;
                return null;
            }
        }

        public void Dispose()
        {
            if (MSOLPowershell != null)
                MSOLPowershell.Dispose();
            if (LivePowershell != null)
                LivePowershell.Dispose();
        }

    }
   
}
