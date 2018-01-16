using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TalentAcquisition.Core.Domain
{
    public class Person
    {
        /// <summary>
        /// Defines a base Class for individual
        /// </summary>
        public Person()
        {

        }
        public int ID { get; set; }
        public string UserId { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        [Display(Name = "Primary Address")]
        public string Address { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
        public int Age
        {
            get
            {
                return DateTime.Now.Year - DateOfBirth.Year;
            }
        }
    }
}