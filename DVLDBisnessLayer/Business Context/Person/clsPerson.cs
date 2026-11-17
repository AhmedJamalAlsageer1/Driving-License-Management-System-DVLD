using DVLD.DTOs;
using DVLDDataAccessLayer;
using DVLDDataAccessLayer.License;
using SharedLayer;
using System;
using System.Data;

namespace DVLDBisnessLayer
{
    public class clsPerson
    {
        private enum enMode { AddNew = 1, Update = 2};
        enMode _Mode;

        public int _ID { get; private set; }
        public string _NationalNo { get; private set; }
        public string _FirstName { get; private set; }
        public string _SecondName { get;  private set; }
        public string _ThirdName { get;  private set; }
        public string _LastName { get;  private set; }
        public DateTime _DateOfBirth { get;  private set; }
        public bool _Gendor { get;  private set; }
        public string _Address { get;  private set; }
        public string _Email { get;  private set; }
        public string _Phone { get;  private set; }
        public string _CountryName { get;  private set; }
        public string _ImagePath { get; private set; }


        public clsPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, bool Gendor,
            string Address, string Email, string Phone, string CountryName, string ImagePath)
        {
            _ID = -1;
            _NationalNo = NationalNo;
            _FirstName = FirstName;
            _SecondName = SecondName;
            _ThirdName = ThirdName;
            _LastName = LastName;
            _DateOfBirth = DateOfBirth;
            _Gendor = Gendor;
            _Address = Address;
            _Email = Email;
            _Phone = Phone;
            _CountryName = CountryName;
            _ImagePath = ImagePath;

            _Mode = enMode.AddNew;
        }

        private clsPerson()
        {
            _ID = -1;
            _NationalNo = "";
            _FirstName = "";
            _SecondName = "";
            _ThirdName = "";
            _LastName = "";
            _DateOfBirth = DateTime.Now.Date;
            _Gendor = false;
            _Address = "";
            _Email = "";
            _Phone = "";
            _CountryName = "";
            _ImagePath = "";

            _Mode = enMode.Update;
        }

        public static DataTable GetPersonalDataByPhone(string Phone)
        {
            return clsPersonAccessLayer.GetPersonalDataByPhone(Phone);
        }

        public void UpdateDetails(string _NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
            DateTime BirthOfDate, bool Gendor, string Address, string Phone, string Email, string Countryname, string ImagePath)
        {
            this._NationalNo = _NationalNo;
            this._FirstName = FirstName;
            this._SecondName = SecondName;
            this._ThirdName = ThirdName;
            this._LastName = LastName;
            this._DateOfBirth = BirthOfDate;
            this._Gendor = Gendor;
            this._Address = Address;
            this._Phone = Phone;
            this._Email = Email;
            this._CountryName = Countryname;
            this._ImagePath = ImagePath;
        }

        private bool AddNewPerson(clsPerson Person)
        {
            Person._ID = DVLDDataAccessLayer.clsPersonAccessLayer.AddNewPerson(Person._NationalNo, Person._FirstName, Person._SecondName, Person._ThirdName,
                Person._LastName, Person._DateOfBirth, Person._Gendor, Person._Address, Person._Phone, Person._Email, Person._CountryName, Person._ImagePath);

            _Mode = enMode.Update;

            return (Person._ID != -1);
        }

        static private clsPerson ConvertToPerson(DTOPerson person)
        {
            return new clsPerson
            {
                _ID = person.PeronsID,
                _NationalNo = person.NationalNo,
                _FirstName = person.FirstName,
                _SecondName = person.SecondName,
                _ThirdName = person.ThirdName,
                _LastName = person.LastName,
                _DateOfBirth = person.DateOfBirth,
                _Gendor = person.Gendor,
                _Address = person.Address,
                _Phone = person.Phone,
                _Email = person.Email,
                _CountryName = person.CountryName,
                _ImagePath = person.ImagePath,

                _Mode = enMode.Update
            };
        }

        static public Result <clsPerson> FindObjectByID(int ID)
        {
            if(clsPersonAccessLayer.FindByID(ID, out DTOPerson dto))
            {
                if (dto != null)
                    return Result <clsPerson>.Success(ConvertToPerson(dto), null, SharedLayer.enums.enMessageType.Info);
            }

            return Result <clsPerson>.Failure(null, "هذا الشخص غير موجود.", SharedLayer.enums.enMessageType.Error);
        }
        static public Result<DTOPerson> FindByID(int ID)
        {
            if (clsPersonAccessLayer.FindByID(ID, out DTOPerson dto))
            {
                if (dto != null)
                    return Result<DTOPerson>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
            }

            return Result<DTOPerson>.Failure(null, "هذا الشخص غير موجود.", SharedLayer.enums.enMessageType.Error);
        }

        static public Result<DTOPerson> FindByUserID(int ID)
        {
            if (clsPersonAccessLayer.FindByUserID(ID, out DTOPerson dto))
            {
                if (dto != null)
                    return Result<DTOPerson>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
            }

            return Result<DTOPerson>.Failure(null, "هذا الشخص غير موجود.", SharedLayer.enums.enMessageType.Error);
        }

        private static void DismantlingName(string FullName, out string FirstName, out string SecondName, out string ThirdName, out string LastName)
        {
            string[] Words = FullName.Split();

            FirstName = ""; SecondName = ""; ThirdName = ""; LastName = "";

            if(Words.Length >= 3)
            {
                FirstName = Words[0];
                SecondName = Words[1];
                ThirdName = Words[2];
                LastName = Words[3];
            }
            else
            {
                FirstName = Words[0];
                SecondName = Words[1];
                ThirdName = string.Empty;
                LastName = Words[2];
            }
        }

        public static Result <clsPerson> FindObjectByName(string Name)
        {
            
            DismantlingName(Name, out string FirstName, out string SecondName, out string ThirdName, out string LastName);

            if(clsPersonAccessLayer.TryFindByName(FirstName, SecondName, ThirdName, LastName, out DTOPerson dto))
            {
                if (dto != null)
                    return Result<clsPerson>.Success(ConvertToPerson(dto), null);
            }

            return Result<clsPerson>.Failure(null, "هذا الشخص غير موجود.", SharedLayer.enums.enMessageType.Error);
        }

        public static Result<DTOPerson> FindByName(string Name)
        {

            DismantlingName(Name, out string FirstName, out string SecondName, out string ThirdName, out string LastName);

            if (clsPersonAccessLayer.TryFindByName(FirstName, SecondName, ThirdName, LastName, out DTOPerson dto))
            {
                if (dto != null)
                    return Result<DTOPerson>.Success(dto, null);
            }

            return Result<DTOPerson>.Failure(null, "هذا الشخص غير موجود.", SharedLayer.enums.enMessageType.Error);
        }


        private bool UpdatePerson(clsPerson Person)
        {
            if (clsPersonAccessLayer.UpdatePerson(Person._ID, Person._NationalNo, Person._FirstName, Person._SecondName, Person._ThirdName,
               Person._LastName, Person._DateOfBirth, Person._Gendor, Person._Address, Person._Phone, Person._Email, Person._CountryName, Person._ImagePath))
                return true;
            else
                return false;
        }

        static public DataTable GetAllPoeple()
        {
            DataTable dt = clsPersonAccessLayer.GetAllPeople();
            return dt;
        }

        static public Result <clsPerson> FindObjectByNationalNo(string NationalNo)
        {
            if(clsPersonAccessLayer.FindByNationalNo(NationalNo, out DTOPerson dto))
            {
                if (dto != null)
                    return Result<clsPerson>.Success(ConvertToPerson(dto), null);
            }

            return Result<clsPerson>.Failure(null, "هذا الشخص غير موجود.", SharedLayer.enums.enMessageType.Error);
        }

        public static Result<DTOPerson> FindByNationalNo(string NationalNo)
        {
            if (clsPersonAccessLayer.FindByNationalNo(NationalNo, out DTOPerson dto))
            {
                if (dto != null)
                    return Result<DTOPerson>.Success(dto, null);
            }

            return Result<DTOPerson>.Failure(null, "هذا الشخص غير موجود.", SharedLayer.enums.enMessageType.Error);
        }

        static public DataTable GetAllByFirstName(string FirstName)
        {
            return clsPersonAccessLayer.GetAllByFirstName(FirstName);
        }

        static public DataTable GetAllBySecondName(string SecondName)
        {
            return clsPersonAccessLayer.GetAllBySecondName(SecondName);
        }

        static public DataTable GetAllByThirdName(string ThirdName)
        {
            return clsPersonAccessLayer.GetAllByThirdName(ThirdName);
        }

        static public DataTable GetAllByLastName(string LastName)
        {
            return clsPersonAccessLayer.GetAllByLastName(LastName);
        }
        static public DataTable GetAllByGendor(bool Gendor)
        {
            return clsPersonAccessLayer.GetAllByGendor(Gendor);
        }

        static public DataTable GetAllByEmail(string Email)
        {
            return clsPersonAccessLayer.GetAllByEmail(Email);
        }

        static public DataTable GetAllByNationlity(string CountryName)
        {
            return clsPersonAccessLayer.GetAllByCountry(CountryName);
        }

        static public bool DeletePerson(int PersonID)
        {
            return clsPersonAccessLayer.DeletePersonByID(PersonID);
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    return AddNewPerson(this);
                default:
                    return UpdatePerson(this);
            }

        }

        static public DataTable GetAllCountries()
        {
            DataTable dt = clsPersonAccessLayer.GetAllCountries();
            return dt;
        }

        public bool isUniqueFullName(int PersonID)
        {
            return clsPersonAccessLayer.isUniquFullName(PersonID, _FirstName, _SecondName, _ThirdName, _LastName);
        }


        public bool isUniqueNationalNo(int PersonID)
        {
            return clsPersonAccessLayer.isUniqueNationalNo(PersonID, _NationalNo);
        }

        static public string GetImagePath(int PersonID)
        {
            return clsPersonAccessLayer.GetImagePath(PersonID);
        }

        static public bool PersonExists(int Personid)
        {
            return clsPersonAccessLayer.PersonExists(Personid);
        }

        internal static Result <int?> TryGetPersonIDByNationalNo(string nationalNo)
        {
            if (!clsPersonAccessLayer.FindByNationalNo(nationalNo, out DTOPerson person))
                return Result <int?>.Failure(null, "هذا الرقم الوطني غير موجود من فضلك ادخل رقما آخر");

            return Result<int?>.Success(person.PeronsID, "");
        }

        public static Result<DTOPerson> FindByDriverID(int DriverID)
        {
            bool? result_Data = clsPersonAccessLayer.FindPersonBy_DriverID(DriverID, out DTOPerson dto);
            if (!result_Data.HasValue)
                return Result<DTOPerson>.Failure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Error);

            if (!result_Data.Value)
                return Result<DTOPerson>.Failure(null, "هذا الشخص غير موجود، من فضلك أدخل معرفا آخر!", SharedLayer.enums.enMessageType.Warning);

            return Result<DTOPerson>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
        }

        public static Result<DTOPerson> FindByLicenseID(int LicenseID)
        {
            bool? result_Data = clsPersonAccessLayer.FindPersonBy_LicenseID(LicenseID, out DTOPerson dto);
            if (!result_Data.HasValue)
                return Result<DTOPerson>.Failure(null, "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", SharedLayer.enums.enMessageType.Error);

            if (!result_Data.Value)
                return Result<DTOPerson>.Failure(null, "هذا الشخص غير موجود، من فضلك أدخل معرفا آخر!", SharedLayer.enums.enMessageType.Warning);

            return Result<DTOPerson>.Success(dto, null, SharedLayer.enums.enMessageType.Info);
        }


        public static Result<int> GetPersonIDByDriverID(int DriverID)
        {
            bool? result_GetDriverID = clsDriver_DAL.TryGetPersonIDByDriverID(DriverID, out int? _PersonID);
            if (!result_GetDriverID.HasValue)
                return Result<int>.Failure(-1, "حصل خطأ أثناء التعامل محاولة استرجاع معرف الشخص.", SharedLayer.enums.enMessageType.Error);

            if (!result_GetDriverID.Value)
                return Result<int>.Failure(-1, "هذا الشخص غير موجود من فضلك اختر شخصا آخر!", SharedLayer.enums.enMessageType.Warning);


            return Result<int>.Success(_PersonID.Value, null, SharedLayer.enums.enMessageType.Info);
        }

    }
}
