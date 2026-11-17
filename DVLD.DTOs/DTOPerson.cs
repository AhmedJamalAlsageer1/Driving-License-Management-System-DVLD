using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class DTOPerson
    {
        public int PeronsID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gendor { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CountryName { get; set; }
        public string ImagePath { get; set; }

        public DTOPerson()
        {

        }

        public DTOPerson( DTOPerson dto)
        {
            this.PeronsID = dto.PeronsID;
            this.NationalNo = dto.NationalNo;
            this.FirstName = dto.FirstName;
            this.SecondName = dto.SecondName;
            this.ThirdName = dto.ThirdName;
            this.LastName = dto.LastName;
            this.DateOfBirth = dto.DateOfBirth;
            this.Gendor = dto.Gendor;
            this.Phone = dto.Phone;
            this.Email = dto.Email;
            this.Address = dto.Address;
            this.CountryName = dto.CountryName;
            this.ImagePath = dto.ImagePath;
        }

        public void Clear()
        {
            PeronsID = -1;
            NationalNo = string.Empty;
            FirstName = string.Empty;
            SecondName = string.Empty;
            ThirdName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = new DateTime();
            Gendor = false;
            Phone = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
            CountryName = string.Empty;
            ImagePath = string.Empty;
        }
    }
}
