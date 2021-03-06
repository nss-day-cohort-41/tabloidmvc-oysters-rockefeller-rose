﻿using Microsoft.Data.SqlClient.Server;
using System;
using System.Runtime.Intrinsics.X86;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TabloidMVC.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreateDateTime { get; set; }
        public bool IsActive { get; set; }
        public string ImageLocation { get; set; }
        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}