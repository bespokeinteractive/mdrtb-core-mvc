using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using EtbSomalia.Models;
using EtbSomalia.Extensions;
          
namespace EtbSomalia.Services
{
    public class UserService
    {
        public Users GetUser(String username)
        {
            Users user = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT usr_idnt, usr_name, usr_email, log_enabled, log_tochange, log_admin_lvl, log_access_lvl, log_password FROM Users INNER JOIN [Login] ON usr_idnt=log_user WHERE log_username='" + username +"'");
            if (dr.Read()) {
                user = new Users {
                    Id = Convert.ToInt64(dr[0]),
                    Name = dr[1].ToString(),
                    Email = dr[2].ToString(),
                    Enabled = Convert.ToBoolean(dr[3]),
                    ToChange = Convert.ToBoolean(dr[4]),

                    AdminLevel = Convert.ToInt64(dr[5]),
                    AccessLevel = dr[6].ToString(),

                    Username = username,
                    Password = dr[7].ToString()
                };
            }

            return user;
        }
    }
}
