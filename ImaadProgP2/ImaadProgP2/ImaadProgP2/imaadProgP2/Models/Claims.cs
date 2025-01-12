using System;
using System.ComponentModel.DataAnnotations;

namespace ImaadProgP2.Models
{
    // A lecturer's claim for the number of hours worked during a given claims period is represented by the Claims class.
    public class Claims
    {
        // A distinct identity for every claim (primary key)
        public int Id { get; set; }

        // The mandatory field is the ID of the lecturer who is making the claim.
        [Required]
        public string LecturerID { get; set; }

        //lecturers first name
        [Required]
        public string FirstName { get; set; }

        //lecturers surname
        [Required]
        public string LastName { get; set; }

        //dateandtime of the claims period
        //start date
        [Required]
        public DateTime ClaimsPeriodStart { get; set; }

        //end date
        [Required]
        public DateTime ClaimsPeriodEnd { get; set; }

        //total hours worked throughout the claims period
        [Required]
        public double HoursWorked { get; set; }

        //payment per hour
        [Required]
        public double RatePerHour { get; set; }
        //what service did they provide
        public string? DescriptionOfWork { get; set; }
        //document upload
        public string? DocumentPath { get; set; }

        //is the claim approved
        public string ApprovalStatus { get; set; } = "Pending";
        //who approved the claim
        public string? ApprovedBy { get; set; }
        public double TotalAmount { get; internal set; }


        public double GetTotalAmount()
        {
            return HoursWorked * RatePerHour;
        }
    }
}