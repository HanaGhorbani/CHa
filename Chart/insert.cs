using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Chart.industry;


namespace Chart
{
    public abstract class Insert<T>
    {
        protected async Task InsertDataAsync(SqlConnection con, string query, object data, int siteID)
        {
            try
            {
                con.OpenAsync().Wait();
                SqlCommand cmd = new SqlCommand(query, con);
                foreach (PropertyInfo property in data.GetType().GetProperties())
                {
                    cmd.Parameters.AddWithValue("@" + property.Name, property.GetValue(data));
                }
                cmd.Parameters.AddWithValue("@SiteID", siteID);
                cmd.ExecuteNonQueryAsync().Wait();

            }
            catch (SqlException e)
            {
                Console.WriteLine("ERROR" + e.ToString());
            }
            con.Close();
        }

        public abstract Task<T> GetData(string take);
        public abstract string GetInsertQuery();
        public abstract void InsertSql(string take);

    }

    public class insertInd : Insert<List<indusrtyChart>>
    {
        public override async Task<List<indusrtyChart>> GetData(string take)
        {
            var industryReceiver = new DataReceiver<List<indusrtyChart>>(take + "/Data/Industries");
            List<indusrtyChart> industryData = await industryReceiver.GetData();
            return industryData;
        }

        public override string GetInsertQuery()
        {
            return "INSERT INTO Industries (name, y, SiteID) VALUES (@name, @y, @SiteID)";
        }

        public override async void InsertSql(string take)
        {
            SqlConnection con = new SqlConnection(@"Data Source=HANA;Initial Catalog=ParseSQL;Integrated Security=True");
            string siteAttrebuteQuery = "SELECT ID FROM siteAttrebute WHERE Href = @Href";
            string query = GetInsertQuery();
            insertInd insertInd = new insertInd();           
            try
            {
                using (SqlCommand siteAttrebuteCmd = new SqlCommand(siteAttrebuteQuery, con))
                {
                    siteAttrebuteCmd.Parameters.AddWithValue("@Href", take);
                    con.Open();
                    int siteID = Convert.ToInt32(siteAttrebuteCmd.ExecuteScalar());
                    con.Close();
                    List<indusrtyChart> data = insertInd.GetData(take).Result;
                    foreach (var item in data)
                    {
                        await InsertDataAsync(con, query, item, siteID);
                    }
                }                                           
            }
            catch (Exception)
            {
                Console.Write("");
            }
            
        }
    }


    public class insertMix : Insert<MixChart>
    {
        public override async Task<MixChart> GetData(string take)
        {
            var MixReceiver = new DataReceiver<MixChart>(take + "/Data/MixAsset");
            MixChart MixData = await MixReceiver.GetData();
            return MixData;
        }

        public override string GetInsertQuery()
        {
            return "INSERT INTO NTDS (DepositTodayPercent, TopFiveStockTodayPercent, CashTodayPercent, OtherAssetTodayPercent, BondTodayPercent, OtherStock, JalaliDate, SiteID) " +
                "VALUES (@DepositTodayPercent, @TopFiveStockTodayPercent, @CashTodayPercent, @OtherAssetTodayPercent, @BondTodayPercent, @OtherStock, @JalaliDate, @SiteID)";
        }

        public override async void InsertSql(string take)
        {
            SqlConnection con = new SqlConnection(@"Data Source=HANA;Initial Catalog=ParseSQL;Integrated Security=True");
            string siteAttrebuteQuery = "SELECT ID FROM siteAttrebute WHERE Href = @Href";
            string query = GetInsertQuery();
            insertMix insertMix = new insertMix();
            try
            {
                using (SqlCommand siteAttrebuteCmd = new SqlCommand(siteAttrebuteQuery, con))
                {                  
                    siteAttrebuteCmd.Parameters.AddWithValue("@Href", take);
                    con.Open();
                    int siteID = Convert.ToInt32(siteAttrebuteCmd.ExecuteScalar());
                    con.Close();           
                    MixChart data = insertMix.GetData(take).Result;
                    await InsertDataAsync(con, query, data, siteID);
                }
            }
            catch (Exception)
            {
                Console.Write("");
            }

        }
    }

    public class insertNAV : Insert<List<NAVchart>>
    {
        public override async Task<List<NAVchart>> GetData(string take)
        {
            var NAVReceiver = new DataReceiver<List<NAVchart>>(take + "/Data/NAVPerShare");
            List<NAVchart> NAVData = await NAVReceiver.GetData();
            return NAVData;
        }


        public override string GetInsertQuery()
        {
            return "INSERT INTO NAVPerShare (JalaliDate, PurchaseNAVPerShare, SellNAVPerShare, StatisticalNAVPerShare, SiteID) " +
                "VALUES (@JalaliDate, @PurchaseNAVPerShare, @SellNAVPerShare, @StatisticalNAVPerShare, @SiteID)";
        }

        public override async void InsertSql(string take)
        {
            SqlConnection con = new SqlConnection(@"Data Source=HANA;Initial Catalog=ParseSQL;Integrated Security=True");
            string siteAttrebuteQuery = "SELECT ID FROM siteAttrebute WHERE Href = @Href";
            string query = GetInsertQuery();
            insertNAV insertNAV = new insertNAV();
            try
            {
                using (SqlCommand siteAttrebuteCmd = new SqlCommand(siteAttrebuteQuery, con))
                {
                    siteAttrebuteCmd.Parameters.AddWithValue("@Href", take);
                    con.Open();
                    int siteID = Convert.ToInt32(siteAttrebuteCmd.ExecuteScalar());
                    con.Close();
                    List<NAVchart> data = insertNAV.GetData(take).Result;
                    foreach (var item in data)
                    {
                        await InsertDataAsync(con, query, item, siteID);
                    }
                }
            }
            catch (Exception)
            {
                Console.Write("");
            }

        }
    }

    public class insertPure : Insert<List<PureChart>>
    {
        public override async Task<List<PureChart>> GetData(string take)
        {
            var PureReceiver = new DataReceiver<List<PureChart>>(take + "/Data/PureAsset");
            List<PureChart> PureData = await PureReceiver.GetData();
            return PureData;
        }


        public override string GetInsertQuery()
        {
            return "INSERT INTO TSOI (NAV, JalaliDate, PurchaseNAVPerShare,SiteID ) VALUES (@NAV, @JalaliDate, @PurchaseNAVPerShare,@SiteID)";
        }

        public override async void InsertSql(string take)
        {
            SqlConnection con = new SqlConnection(@"Data Source=HANA;Initial Catalog=ParseSQL;Integrated Security=True");
            string siteAttrebuteQuery = "SELECT ID FROM siteAttrebute WHERE Href = @Href";
            string query = GetInsertQuery();
            insertPure insertPure = new insertPure();
            try
            {
                using (SqlCommand siteAttrebuteCmd = new SqlCommand(siteAttrebuteQuery, con))
                {
                    siteAttrebuteCmd.Parameters.AddWithValue("@Href", take);
                    con.Open();
                    int siteID = Convert.ToInt32(siteAttrebuteCmd.ExecuteScalar());
                    con.Close();
                    List<PureChart> data = insertPure.GetData(take).Result;
                    foreach (var item in data)
                    {
                        await InsertDataAsync(con, query, item, siteID);
                    }
                }
            }
            catch (Exception)
            {
                Console.Write("");
            }

        }
    }

}