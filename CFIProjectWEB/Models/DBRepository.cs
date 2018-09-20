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
                cmd.CommandText = "select DISTINCT `Course Title` from tblSISCRNs_SR004_2016_S2 where Day_Of_Week != '0'";

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
            String sqlQueryString = String.Format("select CRN,tblSubjectCompetencies.ITSubject,`Course Title`,`Meeting Start Date`," +
            "`Meeting Finish Date`,Day_Of_Week,Time,Room,Lecturer,Campus from tblSISCRNs_SR004_2016_S2 " +
            "left join tblSubjectCompetencies on tblSISCRNs_SR004_2016_S2.`Course Code`=tblSubjectCompetencies.CourseNumber " +
            "where `Course Title` = \"{0}\" and Day_Of_Week!='0'",
            subject.Name);

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