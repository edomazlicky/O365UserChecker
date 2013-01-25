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
using System.DirectoryServices.AccountManagement;
using System.Collections.Specialized;
using System.Reflection;
using System.DirectoryServices;

namespace O365UserChecker
{
    /// <remarks>
    /// Class to represents a Local User in the On-premise domain
    /// </remarks>
    class LocalUserFactory
    {
        private string DomainServer;

        public LocalUserFactory(string DomainServer)
        {
            this.DomainServer = DomainServer;            
        }

        /// <summary>
        /// Opens the specified user and translates all properties into a NameValueCollection for easy comparsions
        /// </summary>
        public NameValueCollection GetUserProperties(string userPrincipalName)
        {
            try
            {
                NameValueCollection retvalue = new NameValueCollection();
                PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, DomainServer);
                using (UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, IdentityType.UserPrincipalName, userPrincipalName))
                {
                    // special handling to get the local ImmutableId
                    if (user.GetUnderlyingObject() != null)
                    {
                        DirectoryEntry userDE = (DirectoryEntry)user.GetUnderlyingObject();
                        byte[] guid_bytes = (byte[])userDE.Properties["objectGuid"].Value;
                        retvalue["objectGuid"] = System.Convert.ToBase64String(guid_bytes);
                    }

                    foreach (PropertyInfo propinfo in user.GetType().GetProperties())
                    {
                        try
                        {
                            MethodInfo prop_method = propinfo.GetGetMethod();
                            object prop_value = prop_method.Invoke(user, null);
                            if (prop_value != null)
                                retvalue[propinfo.Name] = prop_value.ToString();
                            else
                                retvalue[propinfo.Name] = null;
                        }
                        catch (Exception ex)
                        {
                            retvalue[propinfo.Name] = ex.Message;
                        }

                    }

                }
                return retvalue;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
