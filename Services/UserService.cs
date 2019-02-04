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
        public UserService() { }
        public UserService(HttpContext context) { }

        public Users GetUser(String username) {
            Users user = null;

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT log_user, log_username, usr_name, usr_email, log_tochange, log_enabled, log_admin_lvl, log_access_lvl, log_password, rl_idnt, rl_name FROM Login INNER JOIN Users ON log_user=usr_idnt INNER JOIN Roles ON log_admin_role=rl_idnt WHERE log_username='" + username + "'");
            if (dr.Read()) {
                user = new Users {
                    Id = Convert.ToInt64(dr[0]),
                    Username = dr[1].ToString(),
                    Name = dr[2].ToString(),
                    Email = dr[3].ToString(),
                    ToChange = Convert.ToBoolean(dr[4]),
                    Enabled = Convert.ToBoolean(dr[5]),
                    AdminLevel = Convert.ToInt32(dr[6]),
                    AccessLevel = dr[7].ToString(),
                    Password = dr[8].ToString(),
                    Role = new Roles(Convert.ToInt64(dr[9]), dr[10].ToString())
                };
            }

            return user;
        }

        public List<Users> GetUsers() {
            List<Users> users = new List<Users>();

            SqlServerConnection conn = new SqlServerConnection();
            SqlDataReader dr = conn.SqlServerConnect("SELECT log_user, log_username, usr_name, usr_email, log_tochange, log_enabled, log_admin_lvl, log_access_lvl, log_password, rl_idnt, rl_name FROM Login INNER JOIN Users ON log_user=usr_idnt INNER JOIN Roles ON log_admin_role=rl_idnt ORDER BY usr_name");
            if (dr.HasRows) {
                while (dr.Read()) {
                    users.Add(new Users {
                        Id = Convert.ToInt64(dr[0]),
                        Username = dr[1].ToString(),
                        Name = dr[2].ToString(),
                        Email = dr[3].ToString(),
                        ToChange = Convert.ToBoolean(dr[4]),
                        Enabled = Convert.ToBoolean(dr[5]),
                        AdminLevel = Convert.ToInt32(dr[6]),
                        AccessLevel = dr[7].ToString(),
                        Password = dr[8].ToString(),
                        Role = new Roles(Convert.ToInt64(dr[9]), dr[10].ToString())
                    });
                }
            }

            return users;
        }


        /*Data Writers*/
        public Users SaveUser(Users user) {
            return user;
        }

        public Users UpdateUserPassword(Users user) {
            SqlServerConnection conn = new SqlServerConnection();
            user.Id = conn.SqlServerUpdate("UPDATE Login SET log_tochange=0, log_password='" + user.Password + "' output INSERTED.log_user WHERE log_user=" + user.Id);

            return user;
        }


    }
}
