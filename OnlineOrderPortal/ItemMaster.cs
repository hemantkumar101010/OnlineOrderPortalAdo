using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OrderAssignment
{
    //dotnet sonarscanner begin /k:"Ado-Orders-Project-Scanner" /d:sonar.host.url="http://localhost:9000"  /d:sonar.login="sqp_36fc4db006828a1baa3e38939f4c2b862b649753"
    //dotnet build 
    //dotnet sonarscanner end /d:sonar.login="sqp_36fc4db006828a1baa3e38939f4c2b862b649753" 
    internal class ItemMaster
    {
        readonly string sqlConnectionString = @"Data Source=LAPTOP-QM194TV4\SQLEXPRESS;Initial Catalog=OrderAssignmentDb;Integrated Security=True";
        string ItemName { get; set; }
        int? ItemRate { get; set; }
        int ItemQty { get; set; }

        public void ItemMasterMethods()
        {
        MAIN_MENU:
            Console.WriteLine("Welcome to Item Master portal.");
            Console.WriteLine();

            Console.WriteLine("Enter 1 to add item in Items Table.");
            Console.WriteLine("Enter 2 to update item in Items Table.");
            Console.WriteLine("Enter 3 to list all item in Items Table.");
            Console.WriteLine("Enter 4 to delete item in Items Table.");

            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1)
            {
                AddItems();
                Console.WriteLine("Enter 1 to go previous menu/item master.");
                Console.WriteLine("Enter 2 to go main portal.");
                int choiceAgain = Convert.ToInt32(Console.ReadLine());
                if (choiceAgain == 1)
                {
                    goto MAIN_MENU;
                }
                else if (choiceAgain == 2)
                {
                    return;
                }

            }
            else if (choice == 2)
            {
                UpdateItems();
                Console.WriteLine("Enter 1 to go previous menu/item master.");
                Console.WriteLine("Enter 2 to go main portal.");
                int choiceAgain = Convert.ToInt32(Console.ReadLine());
                if (choiceAgain == 1)
                {
                    goto MAIN_MENU;
                }
                else if (choiceAgain == 2)
                {
                    return;
                }

            }
            else if (choice == 3)
            {
                ListItems();
                Console.WriteLine("Enter 1 to go previous menu/item master.");
                Console.WriteLine("Enter 2 to go main portal.");
                int choiceAgain = Convert.ToInt32(Console.ReadLine());
                if (choiceAgain == 1)
                {
                    goto MAIN_MENU;
                }
                else if (choiceAgain == 2)
                {
                    return;
                }

            }
            else if (choice == 4)
            {
                DeleteItems();
                Console.WriteLine("Enter 1 to go previous menu/item master.");
                Console.WriteLine("Enter 2 to go main menu.");
                int choiceAgain = Convert.ToInt32(Console.ReadLine());
                if (choiceAgain == 1)
                {
                    goto MAIN_MENU;
                }
                else if (choiceAgain == 2)
                {
                    return;
                }

            }

        }

        // this function searches a item is present in the Items-table or not
        public DataTable IsItem_NameInItems(string ItemName)
        {
            #region Read-Items-Table
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            string queryString = "SELECT CASE WHEN EXISTS (SELECT * FROM Items WHERE ItemName = '" + ItemName + "') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END";
            SqlCommand cmd = new SqlCommand(queryString, sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            sqlConnection.Close();
            return dataTable;
            #endregion
        }

        //this function add unique items in the items-talbe
        public void AddItems()
        {
            #region Taking-inputs
            ItemMaster itemMaster = new ItemMaster();
        ItemName:
            Console.WriteLine("Enter item name:");
            ItemName = Console.ReadLine();
            if (string.IsNullOrEmpty(ItemName))
            {
                Console.WriteLine("Item name should not be empty!");
                goto ItemName;
            }
            if (itemMaster.IsItem_NameInItems(ItemName).Rows[0][0].ToString().Equals("True"))
            {
                Console.WriteLine($"{ItemName} is already in the Items table. Please add different item!");
                goto ItemName;
            }
        ItemRate:
            Console.WriteLine("Enter item rate:");
            string ItemRates = (Console.ReadLine());
            if (string.IsNullOrEmpty(ItemRates))
            {
                Console.WriteLine("Item rate should not be empty!");

                goto ItemRate;
            }
            ItemRate = Convert.ToInt32(ItemRates);
        ItemQty:
            Console.WriteLine("Enter quantity:");
            string ItemQtys = (Console.ReadLine());
            if (ItemQtys==null)
            {
                Console.WriteLine("Item quantity should not be empty!");
                goto ItemQty;
            }
            ItemQty = Convert.ToInt32(ItemQtys);
            #endregion

            //interacting with db
            #region connected-approach
            //creating sqlconnection object
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);

            //creating sqlcommand object: takes 2 params sp name and connection obj
            SqlCommand sqlCommand = new SqlCommand("USP_InsertData", sqlConnection)
            {
                //defining type of command
                CommandType = CommandType.StoredProcedure
            };

            //adding input parameter with sqlcommand object
            sqlCommand.Parameters.AddWithValue("@ItemName", ItemName);
            sqlCommand.Parameters.AddWithValue("@ItemRate", ItemRate);
            sqlCommand.Parameters.AddWithValue("@ItemQty", ItemQty);
            sqlConnection.Open();
            int i = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if (i > 0)
                Console.WriteLine("inserted");
            else
                Console.WriteLine("not-inserted");
            #endregion

        }
        public void ListItems()
        {
            #region Disnonnected-approach
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            string queryString = "SELECT* FROM Items";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryString, sqlConnection);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);

            Console.WriteLine("List of items:");
            //  Console.WriteLine();

            Console.WriteLine("ItemName\tItemRate\tItemQnt");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j == 1)
                        Console.Write($"{dt.Rows[i][j]}\t\t");
                    else
                        Console.Write($"{dt.Rows[i][j]}\t");
                }
                Console.WriteLine();
            }
            #endregion
        }
        public void UpdateItems()
        {
            ListItems();
            Console.WriteLine();
            Console.WriteLine("Enter name of that item which you want to update");
            ItemName = Console.ReadLine();

            Console.WriteLine("Enter 1 to update only item quantity.");
            Console.WriteLine("Enter 2 to update only item rate.");
            Console.WriteLine("Enter 3 to update item rate and quantity.");
            int i = Convert.ToInt32(Console.ReadLine());

            if (i == 1)
            {
                Console.WriteLine($"Enter number of quantity to update the quantity of {ItemName}");
                ItemQty = Convert.ToInt32(Console.ReadLine());

                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                string queryStr = "UPDATE Items SET ItemQty=" + ItemQty + " WHERE ItemName ='" + ItemName + "'";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryStr, sqlConnection);
                DataTable dataTable = new DataTable();
                int res = sqlDataAdapter.Fill(dataTable);
                if (res == 0)
                    Console.WriteLine("Quantity updated!");
                else
                    Console.WriteLine("Quantity not updated!");
            }
            else if (i == 2)
            {
                Console.WriteLine($"Enter new rate to update the rate of {ItemName}");
                ItemRate = Convert.ToInt32(Console.ReadLine());

                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                string queryStr = "UPDATE Items SET ItemRate=" + ItemRate + " WHERE ItemName='" + ItemName + "' ";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryStr, sqlConnection);
                DataTable dataTable = new DataTable();
                int res = sqlDataAdapter.Fill(dataTable);
                Console.WriteLine(res);
                if (res == 0)
                    Console.WriteLine("Rate updated!");
                else
                    Console.WriteLine("Rate not updated!");
            }
            else if (i == 3)
            {
                Console.WriteLine($"Enter number of quantity to update the quantity of {ItemName}");
                ItemQty = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine($"Enter new rate to update the rate of {ItemName}");
                ItemRate = Convert.ToInt32(Console.ReadLine());

                SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
                string queryStr = "UPDATE Items SET ItemRate= " + ItemRate + ", ItemQty = " + ItemQty + " WHERE ItemName = '" + ItemName + "' ";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryStr, sqlConnection);
                DataTable dataTable = new DataTable();
                int res = sqlDataAdapter.Fill(dataTable);
                if (res == 0)
                    Console.WriteLine("Rate updated!");
                else
                    Console.WriteLine("Rate not updated!");
            }
        }
        public void DeleteItems()
        {
            ListItems();
            Console.WriteLine();
            Console.WriteLine("Enter name of that item to delete its record/row from the items table.");
            ItemName = Console.ReadLine();

            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            string queryStr = "DELETE FROM Items WHERE ItemName ='" + ItemName + "'";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryStr, sqlConnection);
            DataTable dataTable = new DataTable();
            int res = sqlDataAdapter.Fill(dataTable);
            if (res == 0)
                Console.WriteLine($"{ItemName} row deleted from the items table.");
            else
                Console.WriteLine($"{ItemName} row not deleted from the items table.");

        }

    }
}