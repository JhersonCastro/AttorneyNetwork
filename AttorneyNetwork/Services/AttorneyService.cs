using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttorneyNetwork.Services
{
    public class AttorneyModel
    {
        public long  AttorneyId { get; set; }
        public string AttorneyName { get; set; }
        public string AttorneyType { get; set; }
        public string CasesWon { get; set; }
    }
    public class AttorneyService
    {
        private Database.Context context;
        public AttorneyService()
        {
            context = new Database.Context();
        }
        public void RegisterAttorney(AttorneyModel attorney)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { ":id", attorney.AttorneyId },
                { ":name", attorney.AttorneyName },
                { ":type", attorney.AttorneyType },
                { ":cases_won", attorney.CasesWon }
            };
            context.ExecuteNonQuery("INSERT INTO ATTORNEY (attorneyId, attorneyName, attorneyType, casesWon) VALUES (:id, :name, :type, :cases_won)", dictionary);
        }

        public List<AttorneyModel> GetAttorneys()
        {
            DataSet data = context.executeSelect("SELECT * FROM ATTORNEY");
            List<AttorneyModel> attorneys = new List<AttorneyModel>();


            if (data.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    AttorneyModel attorney = new AttorneyModel
                    {
                        AttorneyId = long.Parse(row["attorneyId"].ToString()),
                        AttorneyName = row["attorneyName"].ToString(),
                        AttorneyType = row["attorneyType"].ToString(),
                        CasesWon = row["casesWon"].ToString()
                    };
                    attorneys.Add(attorney);
                }
            }

            return attorneys;
        }

        public AttorneyModel GetAttorneyById(long attorneyId)
        {
            string query = "SELECT * FROM ATTORNEY WHERE attorneyId = :id";
            Dictionary<string, string> param = new Dictionary<string, string>
            {
                {":id", attorneyId.ToString()}
            };
            DataSet data =  context.executeSelect(query, param);
            if (data.Tables[0].Rows.Count > 0)
            {
                return new AttorneyModel()
                {
                    AttorneyId = long.Parse(data.Tables[0].Rows[0]["attorneyId"].ToString()),
                    AttorneyName = data.Tables[0].Rows[0]["attorneyName"].ToString(),
                    AttorneyType = data.Tables[0].Rows[0]["attorneyType"].ToString(),
                    CasesWon = data.Tables[0].Rows[0]["casesWon"].ToString()
                };
            }

            return null;
        }

        public void UpdateAttorney(AttorneyModel attorney)
        {
            if (attorney.AttorneyId <= 0)
            {
                throw new ArgumentException("Invalid AttorneyId");
            }

            string query =
                $"UPDATE ATTORNEY SET attorneyName = '{attorney.AttorneyName}'," +
                $" attorneyType = '{attorney.AttorneyType}', casesWon = '{attorney.CasesWon}' " +
                $"WHERE attorneyId = {attorney.AttorneyId}";

            context.ExecuteNonQuery(query);
        }
        public void DeleteAttorneyById(long attorneyId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { ":id", attorneyId }
            };
            context.ExecuteNonQuery("DELETE FROM ATTORNEY WHERE attorneyId = :id", dictionary);
        }
    }
}
