using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Net;
using System.Security;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("Hello World");
            using (ClientContext clientContext = new ClientContext("https://sanjeevkumarp.sharepoint.com/sites/TestSiteCollection"))
            {
                SecureString pwd= GetPassword();
                clientContext.Credentials = new SharePointOnlineCredentials("singh@sanjeevkumarp.onmicrosoft.com", pwd);
                clientContext.ExecuteQuery();
                
                clientContext.Load(clientContext.Web, web => web.Title);
                
                    
                clientContext.ExecuteQuery();

                Console.WriteLine("My Resource Details:");
                List oList = clientContext.Web.Lists.GetByTitle("HR Data");
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = "<View><RowLimit>100</RowLimit></View>";
                ListItemCollection collListItem = oList.GetItems(camlQuery);
                clientContext.Load(collListItem,
                        items => items.Include(
                                item => item["Title"],
                                item => item["Training"],
                                item => item["Status"]));
                                clientContext.ExecuteQuery();
                foreach (ListItem oListItem in collListItem)
                {

                    Console.WriteLine(string.Format("Name:  {0}     Training:  {1}  Status: {2}", oListItem["Title"], oListItem["Training"], oListItem["Status"]));

                }
                Console.Read();
            }
        }

        private static SecureString GetPassword()
        {
            Console.WriteLine("Enter Password");
            ConsoleKeyInfo info;
            //Get the user's password as a SecureString  
            SecureString securePassword = new SecureString();
            do
            {
                info = Console.ReadKey(true);
                if (info.Key != ConsoleKey.Enter)
                {
                    securePassword.AppendChar(info.KeyChar);
                }
            }
            while (info.Key != ConsoleKey.Enter);
            return securePassword;
        }  
    }
}
