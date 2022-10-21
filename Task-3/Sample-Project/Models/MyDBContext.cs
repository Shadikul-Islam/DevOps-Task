using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace K8STestApp.Models
{
    public class MyDBContext
    {
        SqlConnection con = new SqlConnection("Data Source=db;Initial Catalog=mytestdb;Integrated Security=false; User Id=sa; Password=rootpa@sw0rdmysql");
        public bool LoginCheck(LoginModels ad)
        {
            bool isAuthenticated = false;
            SqlCommand com = new SqlCommand("usp_GetAdminUsersByLogInID", con);
            com.CommandType = CommandType.StoredProcedure;
            //com.Parameters.AddWithValue("@LoginID", ad.LogInID);
            //com.Parameters.AddWithValue("@Password", ad.Password);
            SqlParameter oblogin = new SqlParameter();
            oblogin.ParameterName = "LoginID";
            oblogin.SqlDbType = SqlDbType.NVarChar;
            oblogin.Direction = ParameterDirection.Input;
            oblogin.Value = ad.LogInID;
            com.Parameters.Add(oblogin);
            con.Open();
            SqlDataReader dr = com.ExecuteReader();
            //int res = Convert.ToInt32(oblogin.Value);
            while (dr.Read())
            {
                if (dr["Password"] != System.DBNull.Value)
                {
                    if(dr["Password"].ToString() == ad.Password)
                    {
                        isAuthenticated = true;
                    }
                }
            }
            con.Close();
            return isAuthenticated;
        }
    }
}
