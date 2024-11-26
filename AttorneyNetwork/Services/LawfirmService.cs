using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttorneyNetwork.Services
{
    public class LawfirmModel
    {
        public long LawFirmNit { get; set; }
        public string LawFirmName { get; set; }
        public DateTime FundationDate { get; set; }
    }
    internal class LawfirmService
    {
        private Database.Context context;
        public LawfirmService()
        {
            context = new Database.Context();
        }
        public void RegisterLawfirm(LawfirmModel lawfirm)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { ":nit", lawfirm.LawFirmNit },
                { ":name", lawfirm.LawFirmName },
                { ":fundation_date", lawfirm.FundationDate.ToString("yyyy-MM-dd") }
            };
            context.ExecuteNonQuery("INSERT INTO LAWFIRMCONSORTIUM " +
                                    "(LAWFIRMNIT, LAWFIRMNAME, LAWFIRMFOUNDATIONDATE) " +
                                    "VALUES (:nit, :name, TO_DATE(:fundation_date, 'YYYY-MM-DD'))", dictionary);
        }

        public void UpdateLawFirm(LawfirmModel lawfirm)
        {
            string query = $"UPDATE LAWFIRMCONSORTIUM SET LAWFIRMNAME = '{lawfirm.LawFirmName}'," +
                           $"LAWFIRMFOUNDATIONDATE = TO_DATE('{lawfirm.FundationDate.ToString("yyyy-MM-dd")}', 'YYYY-MM-DD') " +
                           $"WHERE LAWFIRMNIT = {lawfirm.LawFirmNit}";
            context.ExecuteNonQuery(query);
        }
        public void DeleteLawFirm(long lawFirmNit)
        {
            string query = $"DELETE FROM LAWFIRMCONSORTIUM WHERE LAWFIRMNIT = {lawFirmNit}";
            context.ExecuteNonQuery(query);
        }

        public List<LawfirmModel> GetLawFirms()
        {
            string query = "SELECT * FROM LAWFIRMCONSORTIUM";
            DataSet data = context.executeSelect(query);
            List<LawfirmModel> lawFirms = new List<LawfirmModel>();
            if (data.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    lawFirms.Add(new LawfirmModel()
                    {
                        LawFirmNit = long.Parse(row["LAWFIRMNIT"].ToString()),
                        LawFirmName = row["LAWFIRMNAME"].ToString(),
                        FundationDate = row.Field<DateTime>("LAWFIRMFOUNDATIONDATE")
                    });
                }
            }
            return lawFirms;
        }
        public LawfirmModel GetLawfirmModelById(long lawFirmNit)
        {
            string query = "SELECT * FROM LAWFIRMCONSORTIUM WHERE LAWFIRMNIT = :nit";
            Dictionary<string, string> param = new Dictionary<string, string>
            {
                {":nit", lawFirmNit.ToString()}
            };
            DataSet data = context.executeSelect(query, param);
            if (data.Tables[0].Rows.Count > 0)
            {
                return new LawfirmModel()
                {
                    LawFirmNit = long.Parse(data.Tables[0].Rows[0]["LAWFIRMNIT"].ToString()),
                    LawFirmName = data.Tables[0].Rows[0]["LAWFIRMNAME"].ToString(),
                    FundationDate = data.Tables[0].Rows[0].Field<DateTime>("LAWFIRMFOUNDATIONDATE")
                };
            }

            return null;
        }
       
    }
}
