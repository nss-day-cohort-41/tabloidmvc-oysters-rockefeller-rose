﻿using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserTypeRepository : BaseRepository, IUserTypeRepository
    {
        public UserTypeRepository(IConfiguration config) : base(config) { }


        //GetAllProcess being Initiated below
        public List<UserType> GetAllUserTypes()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT Id, Name
                            FROM UserType
                        ";
                    // cmd.Parameters.AddWithValue("@email", email);

                    //UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();



                    var userType = new List<UserType>();

                    while (reader.Read())
                    {
                        userType.Add(new UserType()

                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))


                        });
                    }

                    reader.Close();

                    return userType;
                }
            }
        }


     


            }
        }

