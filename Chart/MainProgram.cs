using HtmlAgilityPack;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Chart
{
    internal class MainProgram
    {
        static async Task Main(string[] args)
        {
            string linktext = "";
            HtmlWeb htmlWeb = new HtmlWeb();
            Console.WriteLine("Insert URL:");
            string take = Console.ReadLine();
            HtmlDocument doc = htmlWeb.Load(take);
            List<string> links = new List<string>();
            string classname = "btn global-border-btn";
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes($"//a[contains(@class, '{classname}')]"))
            {
                string href = link.GetAttributeValue("href", null);
                if (!string.IsNullOrEmpty(href))
                {
                    string content = link.InnerText.Trim();
                    linktext = $"{content} - {href}";
                    links.Add(linktext);
                }
            }
            SqlConnection con = new SqlConnection(@"Data Source=HANA;Initial Catalog=ParseSQL;Integrated Security=True");
            string query = "INSERT INTO siteAttrebute (Name, Href) VALUES (@Name, @Href)";

            try
            {
                await con.OpenAsync();

                foreach (var link in linktext)
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Name", link.Name);
                    cmd.Parameters.AddWithValue("@Href", link.Href);
                    await cmd.ExecuteNonQueryAsync();
                }


            }
            catch (SqlException e)
            {
                Console.WriteLine("ERROR" + e.ToString());

            }
            finally
            {
                con.Close();

            }

            var task = new Scheduler();
            task.StartCronJob();
           
        }
    }
}
