using Quartz;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using static Chart.industry;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Chart
{
    public class MyJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {          
            getLinks links = new getLinks();
            links.InsertData();            
            List<string> hrefs = links.hrefValue();
            Console.WriteLine("To Save pure=> 1 Mix=>2 NAV=>3 Industry=>4");
            int cho = int.Parse(Console.ReadLine());
            foreach (string take in hrefs)
            {
                if (cho == 1)
                {
                    insertPure insertPure = new insertPure();
                    insertPure.InsertSql(take);
                }
                else if (cho == 2)
                {

                    insertMix insertMix = new insertMix();
                    insertMix.InsertSql(take);
                }
                else if (cho == 3)
                {
                    insertNAV insertNAV = new insertNAV();
                    insertNAV.InsertSql(take);
                }
                else if (cho == 4)
                {
                    insertInd insertInd = new insertInd();
                    insertInd.InsertSql(take);
                }             
            }
            Console.WriteLine("Data Inserted!");
        }
    }
}


