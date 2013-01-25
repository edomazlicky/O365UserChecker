O365UserChecker
===============
Copyright 2013 Eric Domazlicky
This file is part of O365 User Checker
O365 User Checker is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
O365 User Checker is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
You should have received a copy of the GNU General Public License. If not, see http://www.gnu.org/licenses/. 

Office 365 User Checker is a utility for quickly checking Federated user properties in Office 365. User properties are checked using rules defined in an XML file called verifyrules.xml. Verify rules XML defines Powershell commands to run and a list of properties to check. Properties can be either fixed (i.e. IsLicensed=true) or compared against on-premise user proeperties (ImmutableId=$objectGUID,FirstName=$GivenName etc..). Make sure to edit the verifyrules.xml file and change the line:
<match property="Licenses" value="{myo365domain:EXCHANGESTANDARD_STUDENT}"/>

to match the Licenses property returned from the powershell command on a known good user:
get-msoluser -userprincipalname gooduser@yourdomain.edu | fl

After editing the verifyrules.xml you will be able to click "Verify User" in the program to verify a user and return the results of all property matches. For a normal user the result should be ideally true for all properties. If some properties do not match the output should reflect which ones did not match.

Another function of the program is the "Reset UPN" button. This executes a powershell command sequence to change a user's UPN back to the onmicrosoft.com default domain and then back to the user's orignial UPN. This sequence is sometimes recommended by Microsoft support to solves strange issues (i.e. user being redirected to Windows Live login screen). If you use this function make sure your admin username ends in onmicrosoft.com so the program knows what the default onmicrosoft.com is.
