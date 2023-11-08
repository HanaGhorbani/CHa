using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Chart.industry;


namespace Chart
{
    public abstract class Insert<T>
    {
        protected async Task InsertDataAsync(SqlConnection con, string query, object data)
        {
                try
                {
                    con.OpenAsync().Wait();
                    SqlCommand cmd = new SqlCommand(query, con);
                    foreach (PropertyInfo property in data.GetType().GetProperties())
                    {
                        cmd.Parameters.AddWithValue("@" + property.Name, property.GetValue(data));
                    }
                    cmd.ExecuteNonQueryAsync().Wait();

                }
                catch (SqlException e)
                {
                    Console.WriteLine("ERROR" + e.ToString());
                }
                finally
                {

                    con.Close();
                }
            
        }

        public abstract Task<T> GetData(string take);
        public abstract string GetInsertQuery();
        public abstract void InsertSql(string take);
        
    }

    public class insertInd : Insert<List<indusrtyChart>>
    {
        public override async Task<List<indusrtyChart>> GetData(string take)
        {
            var industryReceiver = new DataReceiver<List<indusrtyChart>>(take+"/Data/Industries");
            List<indusrtyChart> industryData = await industryReceiver.GetData();
            return industryData;
        }


        public override string GetInsertQuery()
        {
            return "INSERT INTO Industries (name, y) VALUES (@name, @y)";
        }

        public override async void InsertSql(string take)
        {
            SqlConnection con = new SqlConnection(@"Data Source=HANA;Initial Catalog=ParseSQL;Integrated Security=True");
            string query = GetInsertQuery();
            insertInd insertInd = new insertInd();
            try
            {
                List<indusrtyChart> data = insertInd.GetData(take).Result;
                foreach (var item in data)
                {
                    await InsertDataAsync(con, query, item);
                }
            }
            catch(Exception)
            {
                Console.WriteLine("");
            }
            finally
            {
                Console.WriteLine("Data Inserted!");
            }
            
            
            
            /*
               string insertNameQuery = "INSERT INTO Industries (name) SELECT [Name] FROM siteAttrebute";
               using (SqlCommand cmd = new SqlCommand(insertNameQuery, con))
               {
                 await con.OpenAsync();
                 await cmd.ExecuteNonQueryAsync();
                 await con.CloseAsync();
               }
            */
        }
    }
    

    public class insertMix : Insert<MixChart>
    {
        public override async Task<MixChart> GetData(string take)
        {
            var MixReceiver = new DataReceiver<MixChart>(take+"/Data/MixAsset");
            MixChart MixData = await MixReceiver.GetData();
            return MixData;
        }


        public override string GetInsertQuery()
        {
            return "INSERT INTO NTDS (DepositTodayPercent, TopFiveStockTodayPercent, CashTodayPercent, OtherAssetTodayPercent, BondTodayPercent, OtherStock, JalaliDate) " +
                "VALUES (@DepositTodayPercent, @TopFiveStockTodayPercent, @CashTodayPercent, @OtherAssetTodayPercent, @BondTodayPercent, @OtherStock, @JalaliDate)";
        }

        public override async void InsertSql(string take)
        {
            SqlConnection con = new SqlConnection(@"Data Source=HANA;Initial Catalog=ParseSQL;Integrated Security=True");
            string query = GetInsertQuery();
            insertMix insertMix = new insertMix();
            try
            {
                MixChart data = insertMix.GetData(take).Result;
                await InsertDataAsync(con, query, data);
            }
            catch(Exception)
            {
                Console.WriteLine("");
            }
            finally
            {
                Console.WriteLine("Data Inserted!");
            }

        }
    }

    public class insertNAV : Insert<List<NAVchart>>
    {
        public override async Task<List<NAVchart>> GetData(string take)
        {
            var NAVReceiver = new DataReceiver<List<NAVchart>>(take +"/Data/NAVPerShare");
            List<NAVchart> NAVData = await NAVReceiver.GetData();
            return NAVData;
        }


        public override string GetInsertQuery()
        {
            return "INSERT INTO NAVPerShare (JalaliDate, PurchaseNAVPerShare, SellNAVPerShare, StatisticalNAVPerShare) " +
                "VALUES (@JalaliDate, @PurchaseNAVPerShare, @SellNAVPerShare, @StatisticalNAVPerShare)";
        }

        public override async void InsertSql(string take)
        {
            SqlConnection con = new SqlConnection(@"Data Source=HANA;Initial Catalog=ParseSQL;Integrated Security=True");
            string query = GetInsertQuery();
            insertNAV insertNAV = new insertNAV();
            try
            {
                List<NAVchart> data = insertNAV.GetData(take).Result;

                foreach (var item in data)
                {
                    await InsertDataAsync(con, query, item);
                }
            }
            catch(Exception)
            {
                Console.WriteLine("");
            }
            finally
            {
                Console.WriteLine("Data Inserted!");
            }


        }
    }

    public class insertPure : Insert<List<PureChart>>
    {       
        public override async Task<List<PureChart>> GetData(string take)
        {
            var PureReceiver = new DataReceiver<List<PureChart>>(take +"/Data/PureAsset");
            List<PureChart> PureData = await PureReceiver.GetData();
            return PureData;
        }


        public override string GetInsertQuery()
        {
            return "INSERT INTO TSOI (NAV, JalaliDate, PurchaseNAVPerShare ) VALUES (@NAV, @JalaliDate, @PurchaseNAVPerShare)";
        }

        public override async void InsertSql(string take)
        {
            SqlConnection con = new SqlConnection(@"Data Source=HANA;Initial Catalog=ParseSQL;Integrated Security=True");
            string query = GetInsertQuery();
            insertPure insertPure = new insertPure();
            try
            {
                List<PureChart> data = insertPure.GetData(take).Result;

                foreach (var item in data)
                {
                    await InsertDataAsync(con, query, item);
                }
            }
            catch(Exception)
            {
                Console.WriteLine("");
            }
            finally
            {
                Console.WriteLine("Data Inserted!");
            }


        }
    }

}