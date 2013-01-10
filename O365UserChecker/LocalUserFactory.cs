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

    }
}
