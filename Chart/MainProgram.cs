using HtmlAgilityPack;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Chart
{
    internal class MainProgram
    {
        public static void Main(string[] args)
        {          
            var task = new Scheduler();
            task.StartCronJob();
           
        }
    }
}
