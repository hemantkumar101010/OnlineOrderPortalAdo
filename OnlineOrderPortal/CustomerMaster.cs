using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderAssignment
{
    internal class CustomerMaster
    {
        #region customer-props
        string CustomerFName { get; set; }
        string CustomerLName { get; set; }
        int CustomerPhone { get; set; }
        string CustomerEmail { get; set; }
        string Password { get; set; }
        #endregion

        readonly string sqlConnectionString = @"Data Source=LAPTOP-QM194TV4\SQLEXPRESS;Initial Catalog=OrderAssignmentDb;Integrated Security=True";
        public void CustomerMasterMethods()
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to Customer portal.");
            Console.WriteLine();
        //MAIN_MENU:
            Console.WriteLine("Enter 1 for registration in 'online order portal'.");
            Console.WriteLine("Enter 2 to login.");

            CustomerMaster customerMaster = new CustomerMaster();

            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                #region customer-registration
                customerMaster.InsertCustomerRegistrationData();
                Console.WriteLine("Enter 1 to go back.");
                Console.WriteLine("Enter 2 to logout.");
                int i = Convert.ToInt32(Console.ReadLine());
                if (i == 1)
                {
                    CustomerMasterMethods();
                }
                else if (i == 2)
                {
                    return;
                }
                #endregion
            }
            else if (choice == 2)
            {
            #region customer-login
            logIn:
                Console.WriteLine("Enter your registered email to login.");
                CustomerEmail = Console.ReadLine();
               
                if (customerMaster.IsDuplicateInCustomers(CustomerEmail).Rows[0][0].ToString().Equals("True"))//varifying email
                {
                Password:
                    Console.WriteLine("Enter your password.");
                    Password = Console.ReadLine();

                    #region validating-password
                    SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                    string queryString = "SELECT CASE WHEN EXISTS (SELECT * FROM Customers WHERE Password = '" + Password + "' AND CustomerEmail='" + CustomerEmail + "') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END";
                    SqlCommand cmd = new SqlCommand(queryString, sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    sqlConnection.Close();
                    #endregion

                    #region customer-features
                    if (dataTable.Rows[0][0].ToString().Equals("True"))  //varifying customer password
                    {
                        Console.WriteLine();
                        Console.WriteLine("Welcome to your 'online order customer portal'.");
                    CustomerFeaturs:
                        Console.WriteLine("Choose one of the following options.");
                        Console.WriteLine();
                        Console.WriteLine("Enter 1 to update your registration details.");
                        Console.WriteLine("Enter 2 to see your registration details.");
                        Console.WriteLine("Enter 3 to delete your account/registration details.");
                        int choiceInCustomer = Convert.ToInt32(Console.ReadLine());

                        if (choiceInCustomer == 1)
                        {
                            Console.WriteLine();
                            customerMaster.updateRegistrationdetails(CustomerEmail);
                            Console.WriteLine("Enter 1 to go back.");
                            Console.WriteLine("Enter 2 to logout.");
                            int i = Convert.ToInt32(Console.ReadLine());
                            if (i == 1)
                            {
                                goto CustomerFeaturs;
                            }
                            else if (i == 2)
                            {
                                return;
                            }
                        }
                        else if (choiceInCustomer == 2)
                        {
                            Console.WriteLine();
                            customerMaster.ShowMyRegistrationDetails(CustomerEmail);
                            Console.WriteLine("Enter 1 to go back.");
                            Console.WriteLine("Enter 2 to logout.");
                            int i = Convert.ToInt32(Console.ReadLine());
                            if (i == 1)
                            {
                                Console.WriteLine();
                                goto CustomerFeaturs;
                            }
                            else if (i == 2)
                            {
                                return;
                            }
                        }
                        else if (choiceInCustomer == 3)
                        {
                            Console.WriteLine();
                            customerMaster.DeleteAccount(CustomerEmail);
                            Console.WriteLine("Enter 1 to go back.");
                            Console.WriteLine("Enter 2 to logout.");
                            int i = Convert.ToInt32(Console.ReadLine());
                            if (i == 1)
                            {
                                Console.WriteLine();
                                goto CustomerFeaturs;
                            }
                            else if (i == 2)
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Password is invalid, try again!");
                        goto Password;
                    }
                    #endregion
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"{CustomerEmail} is not in the server, please a register email address only.");
                    goto logIn;
                }
                #endregion
            }
        }

        public void CustomerFeatures()
        {
          
        }
        #region all-functions
        public DataTable IsDuplicateInCustomers(string email)
        {
            #region Read-Items-Table
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            string queryString = "SELECT CASE WHEN EXISTS (SELECT * FROM Customers WHERE CustomerEmail = '" + email + "') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END";
            SqlCommand cmd = new SqlCommand(queryString, sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            sqlConnection.Close();
            return dataTable;
            #endregion
        }
        public void InsertCustomerRegistrationData()
        {
            #region Taking-inputs
            CustomerMaster customerMaster = new CustomerMaster();
        CustomerFName:
            Console.WriteLine("Enter your first name:");
            CustomerFName = Console.ReadLine();
            if (string.IsNullOrEmpty(CustomerFName))
            {
                Console.WriteLine("Customer first name should not be empty!");
                goto CustomerFName;
            }

        CustomerLName:
            Console.WriteLine("Enter your last name:");
            CustomerLName = Console.ReadLine();
            if (string.IsNullOrEmpty(CustomerLName))
            {
                Console.WriteLine("Customer last name should not be empty!");
                goto CustomerLName;
            }

        CustomerPhone:
            Console.WriteLine("Enter your phone number:");
            string customerPhone = (Console.ReadLine());
            if (string.IsNullOrEmpty(customerPhone))
            {
                Console.WriteLine("Phone number should not be empty!");
                goto CustomerPhone;
            }
            customerMaster.CustomerPhone = Convert.ToInt32(customerPhone);
        CustomerEmail:
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            Regex regex = new Regex(pattern);

            Console.WriteLine("Enter your mail id:");
            CustomerEmail = (Console.ReadLine());
            if (string.IsNullOrEmpty(CustomerEmail))
            {
                Console.WriteLine("Customer email should not be empty!");
                goto CustomerEmail;
            }
            if (IsDuplicateInCustomers(CustomerEmail).Rows[0][0].ToString().Equals("True"))
            {
                Console.WriteLine($"{CustomerEmail} mail-id is already exist in the server, please try with a different mail!");
                goto CustomerEmail;
            }
            #endregion
            Match match = regex.Match(CustomerEmail);
            if (!match.Success)
            {
                Console.WriteLine($"{CustomerEmail} is not Valid Email Address");
                goto CustomerEmail;
            }

            Console.WriteLine("Set a password.");
            Password = Console.ReadLine();

            //interacting with db
            #region connected-approach

            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            string queryStr = "INSERT INTO Customers VALUES('" + CustomerFName + "','" + CustomerLName + "'," + customerMaster.CustomerPhone + ",'" + CustomerEmail + "','" + Password + "')";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryStr, sqlConnection);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            Console.WriteLine();
            #endregion

            SendMail.SendMailMethod($"{CustomerFName} {CustomerLName}", CustomerEmail);
            Console.WriteLine();
            return;
        }
        public void ShowMyRegistrationDetails(string customerEmail)
        {

            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            string queryStr = "SELECT* FROM Customers WHERE CustomerEmail='" + customerEmail + "'";

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryStr, sqlConnection);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            Console.WriteLine();
            if (dataTable.Rows.Count > 0)
            {
                Console.WriteLine($"Hey '{dataTable.Rows[0][0]} {dataTable.Rows[0][1]}' your registration details are below:");
                Console.WriteLine("FName\tLName\tPhone\tEmail\t\t\tYour password");
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    Console.Write($"{dataTable.Rows[0][i]}\t");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"There is no account exist with '{customerEmail}' mail name.");
            }
        }
        public void updateRegistrationdetails(string customerEmail)
        {

            ShowMyRegistrationDetails(customerEmail);
            Console.WriteLine();
            Console.WriteLine("Enter 1 to update phone.");
            Console.WriteLine("Enter 2 to reset your password.");
            Console.WriteLine("enter 3 to update your mail.");

            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                Console.WriteLine($"Enter new value of phone.");
                int phone = Convert.ToInt32(Console.ReadLine());

                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                string queryStr = $"UPDATE Customers SET CustomerPhone =" + phone + " WHERE CustomerEmail ='" + customerEmail + "'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryStr, sqlConnection);
                DataTable dataTable = new DataTable();
                int res = sqlDataAdapter.Fill(dataTable);
                if (res == 0)
                    Console.WriteLine($"updated!");
                else
                    Console.WriteLine($"not updated!");
            }
            else if (choice == 2)
            {
                Console.WriteLine($"Enter new password.");
                string password = (Console.ReadLine());

                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                string queryStr = $"UPDATE Customers SET Password ='" + password + "' WHERE CustomerEmail ='" + customerEmail + "'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryStr, sqlConnection);
                DataTable dataTable = new DataTable();
                int res = sqlDataAdapter.Fill(dataTable);
                if (res == 0)
                    Console.WriteLine($"updated!");
                else
                    Console.WriteLine($"not updated!");
            }
            else if (choice == 3)
            {
                Console.WriteLine($"Enter new mail.");
                string email = (Console.ReadLine());

                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                string queryStr = $"UPDATE Customers SET CustomerEmail ='" + email + "' WHERE CustomerEmail ='" + customerEmail + "'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryStr, sqlConnection);
                DataTable dataTable = new DataTable();
                int res = sqlDataAdapter.Fill(dataTable);
                if (res == 0)
                    Console.WriteLine($"updated!");
                else
                    Console.WriteLine($"not updated!");
            }



        }
        public void DeleteAccount(string customerEmail)
        {
            Console.WriteLine($"Press 1 to conferm, after entering 1 your account will be deleted permanentily.");
            int Check = Convert.ToInt32(Console.ReadLine());
            if (Check == 1)
            {
                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                string queryStr = $"DELETE FROM Customers WHERE CustomerEmail ='" + customerEmail + "'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryStr, sqlConnection);
                DataTable dataTable = new DataTable();
                int res = sqlDataAdapter.Fill(dataTable);
                if (res == 0)
                    Console.WriteLine($"Your account deleted permanently!!");
                else
                    Console.WriteLine($"Not deleted due to some reason.");
            }

        }
        #endregion
    }
}

//#region Accesing-db
//string query = "SELECT* FROM Customers WHERE CustomerEmail='"+CustomerEmail+"'";
//SqlCommand cmd1 = new SqlCommand(query, sqlConnection);

//sqlConnection.Open();
//SqlDataReader reader1 = cmd.ExecuteReader();
//DataTable dataTable1 = new DataTable();
//dataTable.Load(reader1);
//sqlConnection.Close();
//#endregion