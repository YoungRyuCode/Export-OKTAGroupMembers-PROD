using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Okta.Core;
using Okta.Core.Clients;
using Okta.Core.Models;

namespace Export_OKTAGroupMembers_PROD
{
    class Program
    {
        static void Main(string[] args)
        {
            var appToken = ConfigurationManager.AppSettings["AppToken"].ToString();
            var subDomain = ConfigurationManager.AppSettings["Subdomain"].ToString();
            var groupName = ConfigurationManager.AppSettings["GroupName"].ToString();


            // Set OKTAClient using AppToken and Subdomain
            var oktaClient = new OktaClient(appToken, subDomain);

            // Set GroupClient
            var groupClient = oktaClient.GetGroupsClient();

            // Get OKTA group by Name
            //var groupName = groupClient.GetByName("app-workplace");
            var appGroupName = groupClient.GetByName(groupName);

            // Get OKTA Group client
            var groupUsersClient = new GroupUsersClient(appGroupName, appToken, subDomain);

            // Get all the users of OKTA App Group
            var users = oktaClient.GetGroupUsersClient(appGroupName);

            // Define export object
            var export = new CsvExport();

            foreach (User user in users)
            {
                export.AddRow();
                export["UPN"] = user.Profile.Email;
                export["First Name"] = user.Profile.FirstName;
                export["Last Name"] = user.Profile.LastName;
                export["Status"] = user.Status;
                export["Date Activated"] = user.Activated;
                export["Date Created"] = user.Created;
                export["LastLogin"] = user.LastLogin;
                export["LastUpdated"] = user.LastUpdated;
                //export["Credentials"] = user.Credentials;
                //export["recoveryQuestion"] = user.recoveryQuestion;
                //export["StatusChanged"] = user.StatusChanged;
                //export["TransitioningToStatus"] = user.TransitioningToStatus;
            }
            export.ExportToFile(@"C:\\" + appGroupName.Profile.Name + "-OMG-Members.csv");
        }

    }
}
