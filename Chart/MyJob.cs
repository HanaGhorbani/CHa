using Quartz;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;


namespace Chart
{
    public class MyJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //Console.WriteLine("Insert URL:");
            getLinks links = new getLinks();
            links.InsertData();
            List<string> hrefs = links.gethrefValues();
            Console.WriteLine("to save pure => 1 Mix=>2 NAV=>3 Industry=>4");
            int cho = int.Parse(Console.ReadLine());
            foreach (string take in hrefs)
            { 
                if (cho == 1)
                {
                    insertPure insertPure = new insertPure();
                    insertPure.InsertSql(take);
                    Console.WriteLine("Data Inserted!");
                }
                else if (cho == 2)
                {
                    insertMix insertMix = new insertMix();
                    insertMix.InsertSql(take);
                    Console.WriteLine("Data Inserted!");
                }
                else if (cho == 3)
                {
                    insertNAV insertNAV = new insertNAV();
                    insertNAV.InsertSql(take);
                    Console.WriteLine("Data Inserted!");
                }
                else if (cho == 4)
                {
                    insertInd insertInd = new insertInd();
                    insertInd.InsertSql(take);
                    Console.WriteLine("Data Inserted!");
                }

            }

        }
        
    }
}

