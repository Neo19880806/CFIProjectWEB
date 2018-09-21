using CFIProjectWEB.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CFIProjectWEB.Controllers
{
    public class DBRepository
    {
        public static DBRepository DefaultDBRepository = new DBRepository();
        private bool connection_open;
        private MySqlConnection connection;

        private DBRepository()
        {

        }


        public List<CFIValidSubject> GetValidSubjects()
        {
            List<CFIValidSubject> list = new List<CFIValidSubject>();

            Get_Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "select DISTINCT `Course Title` from tblSISCRNs_SR004_2018_S2";

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    String name = reader.GetString("Course Title");
                    CFIValidSubject subject = new CFIValidSubject { Name = name };
                    list.Add(subject);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return list;
        }

        public List<CFIDetail> GetDetails(CFIValidSubject subject)
        {
            List<CFIDetail> list = new List<CFIDetail>();
            string TB_SR2018 = "tblSISCRNs_SR004_2018_S2";
            string TB_CP = "tblSubjectCompetencies";
            string TB_CS = "tblCRNSessions";
            string TB_SD = "tblStaffDetails";
            //Basic Query
            String BQuery = String.Format("SELECT {0}.CRN,{1}.ITSubject as ITSubject,{2}.Time as Time,{2}.`Day_of_Week` as Day_Of_Week," +
            "{3}.`unique_name` as Lecturer,`Course Title`,`Meeting Start Date`," +
            "`Meeting Finish Date`, Room,`Lecturer ID`, Campus from {0}", TB_SR2018, TB_CP, TB_CS, TB_SD);
            //Left join Compentency
            String LJCP = String.Format("left join {1} on {0}.`Course Code`= {1}.CourseNumber", TB_SR2018, TB_CP);
            //Left join CRNSession
            String LJCS = String.Format("left join {1} on {1}.CRN = {0}.CRN", TB_SR2018, TB_CS);
            //Left join StaffDetails
            String LJSD = String.Format("left join {1} on {1}.`InstructorID` = {0}.`Lecturer ID`", TB_SR2018, TB_SD);
            //Where
            String WHERE = String.Format("where `Course Title` = \"{0}\" and Day_Of_Week!=\"\" and Room!=\"\"", subject.Name);
            //Final QueryString
            String sqlQueryString = String.Format("{0} {1} {2} {3} {4}", BQuery, LJCP, LJCS, LJSD, WHERE);

            Get_Connection();

            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sqlQueryString;
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CFIDetail detail = new CFIDetail();
                    detail.CRN = reader.GetString("CRN");
                    detail.SubjectCode = reader.GetString("ITSubject");
                    detail.CompetencyName = reader.GetString("Course Title");
                    detail.StartDate = reader.GetString("Meeting Start Date");
                    detail.EndDate = reader.GetString("Meeting Finish Date");
                    detail.DayOfWeek = reader.GetString("Day_Of_Week");
                    detail.Time = reader.GetString("Time");
                    detail.Room = reader.GetString("Room");
                    detail.Lecturer = reader.GetString("Lecturer");
                    detail.Campus = reader.GetString("Campus");
                    list.Add(detail);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return list;
        }
        private void init()
        {

        }
        private void Get_Connection()
        {
            connection_open = false;

            connection = new MySqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultMySQLConnection"].ConnectionString;

            if (Open_Local_Connection())
            {
                connection_open = true;
            }
            else
            {

            }

        }

        private bool Open_Local_Connection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}