using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttorneyNetwork.Database;

namespace AttorneyNetwork.Services
{
    public class WorkModel
    {
        public long ATTORNEYID { get; set; }
        public long LAWFIRMNIT { get; set; }
        public DateTime STARTDATE { get; set; }
        public DateTime? ENDDATE { get; set; } = null;
    }
    public class WorkService
    {
        Database.Context context;

        public WorkService()
        {
            context = new Context();
        }

        public void RegisterWork(WorkModel work)
        {
            string helper = work.ENDDATE == null ? "" : $", TO_DATE('{work.ENDDATE:yyyy-MM-dd}', 'YYYY-MM-DD')";
            Dictionary<string,object> dictionary = new Dictionary<string, object>
            {
                { ":attorney_id", work.ATTORNEYID },
                { ":lawfirm_nit", work.LAWFIRMNIT },
                { ":start_date", work.STARTDATE.ToString("yyyy-MM-dd") },
            };
            context.ExecuteNonQuery($"INSERT INTO WORKSAT(ATTORNEYID,LAWFIRMNIT,STARTDATE{(helper == "" ? "": ", ENDDATE")})" +
                                    " VALUES(:attorney_id, :lawfirm_nit, " +
                                    "TO_DATE(:start_date, 'YYYY-MM-DD')" +
                                    $"{helper})", dictionary);
        }
        public DataSet GetworksByDate(DateTime date)
        {
            DataSet data = context.executeSelect($"SELECT WORKSAT.LAWFIRMNIT, LAWFIRMNAME, WORKSAT.ATTORNEYID, ATTORNEYNAME, ATTORNEYTYPE, TO_CHAR(ENDDATE, 'MM/DD/YYYY')  FROM WORKSAT INNER JOIN ATTORNEY ON WORKSAT.ATTORNEYID = ATTORNEY.ATTORNEYID INNER JOIN LAWFIRMCONSORTIUM ON LAWFIRMCONSORTIUM.LAWFIRMNIT = WORKSAT.LAWFIRMNIT WHERE STARTDATE = TO_DATE('{date.ToString("yyyy-MM-dd")}', 'YYYY/MM/DD')");

            return data;


        }
        public DataSet GetworksByAttorneyId(long id)
        {
            DataSet data = context.executeSelect($"SELECT WORKSAT.LAWFIRMNIT, LAWFIRMNAME, WORKSAT.ATTORNEYID, ATTORNEYNAME, ATTORNEYTYPE, TO_CHAR(ENDDATE, 'MM/DD/YYYY')  FROM WORKSAT INNER JOIN ATTORNEY ON WORKSAT.ATTORNEYID = ATTORNEY.ATTORNEYID INNER JOIN LAWFIRMCONSORTIUM ON LAWFIRMCONSORTIUM.LAWFIRMNIT = WORKSAT.LAWFIRMNIT WHERE ATTORNEY.ATTORNEYID = {id}");

            return data;
        }
        public DataSet GetworksByLawFirmNit(long nit)
        {
            DataSet data = context.executeSelect($"SELECT WORKSAT.LAWFIRMNIT, LAWFIRMNAME, WORKSAT.ATTORNEYID, ATTORNEYNAME, ATTORNEYTYPE, TO_CHAR(ENDDATE, 'MM/DD/YYYY')  FROM WORKSAT INNER JOIN ATTORNEY ON WORKSAT.ATTORNEYID = ATTORNEY.ATTORNEYID INNER JOIN LAWFIRMCONSORTIUM ON LAWFIRMCONSORTIUM.LAWFIRMNIT = WORKSAT.LAWFIRMNIT WHERE LAWFIRMCONSORTIUM.LAWFIRMNIT = {nit}");

            return data;
        }
    }
}
