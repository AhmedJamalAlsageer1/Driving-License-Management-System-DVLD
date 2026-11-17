using DVLD.DTOs;
using SharedLayer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public static class clsPersonAccessLayer
    {
        static private object ToDbNullIfEmpty(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value;
        }

        private static bool Fail(out DTOPerson dto)
        {
            dto = null;
            return false;
        }

        private static bool TryBuild_Person(SqlDataReader reader, out DTOPerson dto)
        {
            dto = null;

            if (reader == null)
                return Fail(out dto);


            if (!clsMapReader.TryGetIntFromReaderImmediately(reader, "PersonID", out int personId))
                return Fail(out dto);

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "FirstName", out string firstName))
                return Fail(out dto);

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "SecondName", out string secondName))
                return Fail(out dto);

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "LastName", out string lastName))
                return Fail(out dto);

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "NationalNo", out string NationalNo))
                return Fail(out dto);

            if (!clsMapReader.TryGetDatetimeFromReaderImediately(reader, "DateOfBirth", out DateTime dateOfBirth))
                return Fail(out dto);

            if (!clsMapReader.TryGetByteFromReaderImediately(reader, "Gendor", out byte gendor))
                return Fail(out dto);

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "Address", out string address))
                return Fail(out dto);

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "Phone", out string phone))
                return Fail(out dto);

            if (!clsMapReader.TryGetStringFromReaderImediately(reader, "CountryName", out string countryName))
                return Fail(out dto);

            dto = new DTOPerson
            {
                PeronsID = personId,
                FirstName = firstName,
                SecondName = secondName,
                ThirdName = clsMapReader.TryGetString_NullFromReaderImediately(reader, "ThirdName"),
                LastName = lastName,
                NationalNo = NationalNo,
                DateOfBirth = dateOfBirth,
                Gendor = gendor == 1,
                Address = address,
                Phone = phone,
                Email = clsMapReader.TryGetString_NullFromReaderImediately(reader, "Email"),
                CountryName = countryName,
                ImagePath = clsMapReader.TryGetString_NullFromReaderImediately(reader, "ImagePath")
            };

            return true;
        }

        static public bool TryFindByName(string FirstName, string SecondName, string ThirdName, string LastName, out DTOPerson dto)
        {
            bool isFound = false;
            dto = null;

            string Query = @"select PersonID,
            NationalNo, 
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            DateOfBirth,
            Gendor,
            Address,
            Phone,
            Email,
            Countries.CountryName,
            ImagePath
            from People
            join Countries on People.NationalityCountryID = Countries.CountryID
            where FirstName = @FirstName And SecondName = @SecondName And 
            (ThirdName = @ThirdName OR (ThirdName is NULL AND @ThirdName is NULL)) And LastName = @LastName";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(Query, connection))
                {
                    cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = FirstName;
                    cmd.Parameters.Add("@SecondName", SqlDbType.NVarChar, 20).Value = SecondName;
                    cmd.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 20).Value = (object)ThirdName ?? DBNull.Value;
                    cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = LastName;


                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        if (TryBuild_Person(reader, out dto))
                        {
                            if (dto != null)
                                isFound = true;
                        }

                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                isFound = false;
                dto = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "خطأ في هذه الدالة [TryFindByName]", "Error",
                    ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        static public bool FindByID(int PersonID, out DTOPerson dto)
        {
            dto = null;
            bool IsFound = false;

            string Query = @"select PersonID,
            NationalNo, 
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            DateOfBirth,
            Gendor,
            Address,
            Phone,
            Email,
            Countries.CountryName,
            ImagePath
            from People
            join Countries on People.NationalityCountryID = Countries.CountryID
            where PersonID = @PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(Query, connection))
                {
                    cmd.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;


                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        if (TryBuild_Person(reader, out dto))
                        { 
                            if (dto != null)
                                IsFound = true;
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                IsFound = false;
                dto = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [FindByID]", "Error",
                    ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return IsFound;
        }

        static public bool FindByUserID(int UserID, out DTOPerson dto)
        {
            dto = null;
            bool IsFound = false;

            string Query = @"select P.PersonID,
            NationalNo, 
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            DateOfBirth,
            Gendor,
            Address,
            Phone,
            Email,
            Countries.CountryName,
            ImagePath
            from People P Inner Join Users U On P.PersonID = U.PersonID Inner Join
            Countries on P.NationalityCountryID = Countries.CountryID
			Where U.UserID = @UserID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(Query, connection))
                {
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;


                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        if (TryBuild_Person(reader, out dto))
                        {
                            if (dto != null)
                                IsFound = true;
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                IsFound = false;
                dto = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [FindByID]", "Error",
                    ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return IsFound;
        }


        static public bool FindByNationalNo(string NationalNo, out DTOPerson dto)
        {
            bool isFound = false;
            dto = null;

            string Query = @"select PersonID,
            NationalNo, 
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            DateOfBirth,
            Gendor,
            Address,
            Phone,
            Email,
            Countries.CountryName,
            ImagePath
            from People
            join Countries on People.NationalityCountryID = Countries.CountryID
            where NationalNo = @NationalNo";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(Query, connection))
                {
                    cmd.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = NationalNo;

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if(reader.Read())
                    {
                        if(TryBuild_Person(reader, out dto))
                        {
                            if (dto != null)
                                isFound = true;
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                dto = null;
                isFound = false;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [FindByID]", "Error",
                 ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        static public DataTable GetAllByFirstName(string FirstName)
        {
            DataTable dt = new DataTable();
            string Query = @"select PersonID,
            NationalNo, 
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            DateOfBirth,
            CASE
                 WHEN Gendor = 0 THEN 'Male'
                 WHEN Gendor = 1 THEN 'Female'
                 ELSE 'Unknown'
            END AS Gendor,
            Address,
            Phone,
            Email,
            C.CountryName,
            ImagePath
            from People P
			join Countries C On P.NationalityCountryID = C.CountryID
            Where FirstName = @FirstName";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand cmd = new SqlCommand(Query, connection);

            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = FirstName;


            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public DataTable GetAllBySecondName(string SecondName)
        {
            DataTable dt = new DataTable();

            string Query = @"select PersonID,
            NationalNo, 
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            DateOfBirth,
            CASE
                 WHEN Gendor = 0 THEN 'Male'
                 WHEN Gendor = 1 THEN 'Female'
                 ELSE 'Unknown'
            END AS Gendor,
            Address,
            Phone,
            Email,
            Countries.CountryName,
            ImagePath
            from People
            join Countries on People.NationalityCountryID = Countries.CountryID
            where SecondName = @SecondName";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand cmd = new SqlCommand(Query, connection);

            cmd.Parameters.Add("@SecondName", SqlDbType.NVarChar, 20).Value = SecondName;


            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public DataTable GetAllByThirdName(string ThirdName)
        {
            DataTable dt = new DataTable();
            string Query = @"select PersonID,
            NationalNo, 
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            DateOfBirth,
            CASE
                 WHEN Gendor = 0 THEN 'Male'
                 WHEN Gendor = 1 THEN 'Female'
                 ELSE 'Unknown'
            END AS Gendor,
            Address,
            Phone,
            Email,
            Countries.CountryName,
            ImagePath
            from People
            join Countries on People.NationalityCountryID = Countries.CountryID
            where ThirdName = @ThirdName";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand cmd = new SqlCommand(Query, connection);

            cmd.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 20).Value = ThirdName;


            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public DataTable GetAllByLastName(string LastName)
        {
            DataTable dt = new DataTable();

            string Query = @"select PersonID,
            NationalNo, 
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            DateOfBirth,
            CASE
                 WHEN Gendor = 0 THEN 'Male'
                 WHEN Gendor = 1 THEN 'Female'
                 ELSE 'Unknown'
            END AS Gendor,
            Address,
            Phone,
            Email,
            Countries.CountryName,
            ImagePath
            from People
            join Countries on People.NationalityCountryID = Countries.CountryID
            where LastName = @LastName";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand cmd = new SqlCommand(Query, connection);

            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = LastName;

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public DataTable GetAllByGendor(bool Gendor)
        {
            DataTable dt = new DataTable();

            string Query = @"select PersonID,
            NationalNo, 
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            DateOfBirth,
            CASE
                 WHEN Gendor = 0 THEN 'Male'
                 WHEN Gendor = 1 THEN 'Female'
                 ELSE 'Unknown'
            END AS Gendor,
            Address,
            Phone,
            Email,
            Countries.CountryName,
            ImagePath
            from People
            join Countries on People.NationalityCountryID = Countries.CountryID
            where Gendor = @Gendor";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand cmd = new SqlCommand(Query, connection);

            cmd.Parameters.Add("@Gendor", SqlDbType.TinyInt).Value = Gendor ? 1 : 0;


            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                dt.Clear();
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public DataTable GetPersonalDataByPhone(string Phone)
        {
            DataTable dt = new DataTable();

            string Query = @"select PersonID,
            NationalNo, 
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            DateOfBirth,
            CASE
                 WHEN Gendor = 0 THEN 'Male'
                 WHEN Gendor = 1 THEN 'Female'
                 ELSE 'Unknown'
            END AS Gendor,
            Address,
            Phone,
            Email,
            Countries.CountryName,
            ImagePath
            from People
            join Countries on People.NationalityCountryID = Countries.CountryID
            where Phone = @Phone";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand cmd = new SqlCommand(Query, connection);

            cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = Phone;


            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                dt.Clear();
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }


        static public DataTable GetAllByCountry(string CountryName)
        {
            DataTable dt = new DataTable();
            string Query = @"select PersonID,
            NationalNo, 
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            DateOfBirth,
            CASE
                 WHEN Gendor = 0 THEN 'Male'
                 WHEN Gendor = 1 THEN 'Female'
                 ELSE 'Unknown'
            END AS Gendor,
            Address,
            Phone,
            Email,
            Countries.CountryName,
            ImagePath
            from People
            join Countries on People.NationalityCountryID = Countries.CountryID
            where NationalityCountryID = @NationalityCountryID";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand cmd = new SqlCommand(Query, connection);

            cmd.Parameters.Add("@NationalityCountryID", SqlDbType.Int).Value = GetCountryIDByName(CountryName);


            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        static public DataTable GetAllByEmail(string Email)
        {
            DataTable dt = new DataTable();
            string Query = @"select PersonID,
            NationalNo, 
            FirstName,
            SecondName,
            ThirdName,
            LastName,
            DateOfBirth,
            CASE
                 WHEN Gendor = 0 THEN 'Male'
                 WHEN Gendor = 1 THEN 'Female'
                 ELSE 'Unknown'
            END AS Gendor,
            Address,
            Phone,
            Email,
            Countries.CountryName,
            ImagePath
            from People
            join Countries on People.NationalityCountryID = Countries.CountryID
            where Email = @Email";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand cmd = new SqlCommand(Query, connection);

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = Email;

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }


        static public int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, String LastName, DateTime DateOfBirth, bool Gendor, string Address,
            string Phone, string Email, string CountryName, string ImagePath)
        {
            int PersonID = -1;

            string Query = @"Insert into People (NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address,
            Phone, Email, NationalityCountryID, ImagePath) Values (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName,
            @DateOfBirth, @Gendor,  @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);
            Select SCOPE_IDENTITY();";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand cmd = new SqlCommand(Query, connection);
            cmd.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = NationalNo;
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = FirstName;
            cmd.Parameters.Add("@SecondName", SqlDbType.NVarChar, 20).Value = SecondName;
            cmd.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 20).Value = ToDbNullIfEmpty(ThirdName);
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = LastName;
            cmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = DateOfBirth;
            cmd.Parameters.Add("@Gendor", SqlDbType.TinyInt).Value = Gendor;
            cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = Address;
            cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = Phone;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = ToDbNullIfEmpty(Email);
            cmd.Parameters.Add("@NationalityCountryID", SqlDbType.Int).Value = GetCountryIDByName(CountryName);
            cmd.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 250).Value = ToDbNullIfEmpty(ImagePath);

            try
            {
                connection.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    PersonID = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                PersonID = -1;
            }
            finally
            {
                connection.Close();
            }
            return PersonID;
        }

        static private int GetCountryIDByName(string CountryName)
        {
            int CountryID = -1;

            string Query = "Select CountryID from Countries Where CountryName = @CountryName;";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@CountryName", SqlDbType.NVarChar, 50).Value = CountryName;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    CountryID = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return CountryID;
        }

        static public bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, String LastName, DateTime DateOfBirth, bool Gendor, string Address,
            string Phone, string Email, string CountryName, string ImagePath)
        {
            int rowsAffected = 0;

            string Query = @"Update People
                    SET  NationalNo = @NationalNo,
                         FirstName = @FirstName,
                         SecondName = @SecondName,
                         ThirdName = @ThirdName,
                         LastName = @LastName,
                         DateOfBirth = @DateOfBirth,
                         Gendor = @Gendor,
                         Address = @Address,
                         Phone = @Phone,
                         Email = @Email,
                         NationalityCountryID = @NationalityCountryID,
                         ImagePath = @ImagePath
                     WHERE PersonID = @PersonID;";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = NationalNo;
            command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = FirstName;
            command.Parameters.Add("@SecondName", SqlDbType.NVarChar, 20).Value = SecondName;
            command.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 20).Value = ToDbNullIfEmpty(ThirdName);
            command.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = LastName;
            command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = DateOfBirth;
            command.Parameters.Add("@Gendor", SqlDbType.TinyInt).Value = Gendor;
            command.Parameters.Add("@Address", SqlDbType.NVarChar, 500).Value = Address;
            command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = Phone;
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = ToDbNullIfEmpty(Email);
            command.Parameters.Add("@NationalityCountryID", SqlDbType.Int).Value = GetCountryIDByName(CountryName);
            command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 250).Value = ToDbNullIfEmpty(ImagePath);
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                rowsAffected = 0;
            }
            finally
            {
                connection.Close();
            }
            return rowsAffected > 0;
        }

        static public DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT 
                   PersonID, 
                   NationalNo, 
                   FirstName, 
                   SecondName, 
                   ThirdName, 
                   LastName, 
                   DateOfBirth,
                   CASE
                       WHEN Gendor = 0 THEN 'Male'
                       WHEN Gendor = 1 THEN 'Female'
                       ELSE 'Unknown'
                   END AS Gendor,
                   Address, 
                   Phone, 
                   Email, 
                   CountryName, 
                   ImagePath
                   FROM People 
                   INNER JOIN Countries ON NationalityCountryID = CountryID";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();
            string Query = "Select * From Countries";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        static public bool DeletePersonByID(int ID)
        {
            bool isDeleted = false;
            string Query = "Delete From People Where PersonID = @PersonID";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = ID;

            try
            {
                connection.Open();
                int rowAffected = command.ExecuteNonQuery();

                isDeleted = (rowAffected > 0);
            }
            catch (Exception ex)
            {
                isDeleted = false;
            }
            finally
            {
                connection.Close();
            }
            return isDeleted;
        }

        static public bool isUniquFullName(int PersonID, string FirstName, string SecondName, string ThirdName, string LastName)
        {
            bool isUnique = false;
            string Query = @" Select
                                    CAST(
                                      CASE
                                          WHEN EXISTS (
                                              SELECT 1 FROM People
                                              WHERE FirstName = @FirstName
                                                AND SecondName = @SecondName
                                                AND ThirdName = @ThirdName
                                                AND LastName = @LastName
                                                AND PersonID != @PersonID
                                          )
                                          THEN 0
                                          ELSE 1
                                      END AS BIT
                                    ) AS IsUnique";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = FirstName;
            command.Parameters.Add("@SecondName", SqlDbType.NVarChar, 20).Value = SecondName;
            command.Parameters.Add("@ThirdName", SqlDbType.NVarChar, 20).Value = ThirdName;
            command.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = LastName;
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                isUnique = (result != null && result != DBNull.Value) && Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                isUnique = false;
            }
            finally
            {
                connection.Close();
            }

            return isUnique;
        }

        static public bool isUniqueNationalNo(int PersonID, string NationalNo)
        {
            bool isUnique = false;
            string query = @"Select
                                cast(
                	                case	
                	                	WHEN EXISTS
                	                	(
                	                	Select 1 From People
                	                	Where NationalNo = @NationalNo AND PersonID != @PersonID
                	                	)
                	                	Then 0
                	                	else 1
                                        end As bit) AS IsUnique";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@NationalNo", SqlDbType.NVarChar, 20).Value = NationalNo;
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                isUnique = (result != null && result != DBNull.Value) && Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                isUnique = false;
            }
            finally
            {
                connection.Close();
            }

            return isUnique;
        }

        static public string GetImagePath(int PersonID)
        {
            string ImagePath = "";

            string Query = @"Select ImagePath From People
                            Where PersonID = @PersonID";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query, connection);
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    ImagePath = Convert.ToString(result);
            }
            catch (Exception ex)
            {
                ImagePath = "";
            }
            finally
            {
                connection.Close();
            }
            return ImagePath;
        }

        static public bool PersonExists(int PersonID)
        {
            bool isFound = false;
            string Query = @"Select
                            Case
                                When Exists(
                                    Select 1 From People Where PersonID = @PersonID
                                )
                                Then 1
                                Else 0
                                End As PersonExists";
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            SqlCommand command = new SqlCommand(Query,connection);
            command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int value))
                    isFound = value == 1 ? true : false;
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool? FindPersonBy_DriverID(int DriverID, out DTOPerson dto)
        {
            dto = null;
            bool? isFound = false;

            string Query = @"SELECT P.PersonID, P.FirstName, P.SecondName, P.ThirdName, P.LastName,
                            P.NationalNo, P.DateOfBirth, P.Gendor, P.Address, P.Email, P.Phone,
                            CTR.CountryName, P.ImagePath
                            FROM Drivers DR
                            INNER JOIN People P ON DR.PersonID = P.PersonID
							Inner Join Countries CTR On P.NationalityCountryID = CTR.CountryID	
                                WHERE DR.DriverID = @DriverID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (TryBuild_Person(reader, out dto))
                        {
                            if (dto != null)
                            {
                                return true;
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [FindPersonBy_DriverID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

        static public bool? FindPersonBy_LicenseID(int LicenseID, out DTOPerson dto)
        {
            dto = null;
            bool? isFound = false;

            string Query = @"SELECT P.PersonID, P.FirstName, P.SecondName, P.ThirdName, P.LastName,
                            P.NationalNo, P.DateOfBirth, P.Gendor, P.Address, P.Email, P.Phone,
                            CTR.CountryName, P.ImagePath
                            FROM Licenses L Inner Join Drivers DR
								On L.DriverID = DR.DriverID
                            INNER JOIN People P ON DR.PersonID = P.PersonID
							Inner Join Countries CTR On P.NationalityCountryID = CTR.CountryID	
                                WHERE L.LicenseID = @LicenseID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString))
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (TryBuild_Person(reader, out dto))
                        {
                            if (dto != null)
                            {
                                return true;
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                isFound = null;
                Logger.LogError(clsDataAccessSetting.ConnectionString, "حصل خطأ في دالة [FindPersonBy_DriverID]",
                    "Error", ex, "DAL", SharedLayer.Context.AppContext.CurrentUser.UserID);
            }

            return isFound;
        }

    }
}
