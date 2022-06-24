using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAssignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
        MAIN_MENU:
            Console.WriteLine();
            Console.WriteLine("Welcome to main menu of 'Online Order Portal'.");
            Console.WriteLine();

            Console.WriteLine("Enter 1 to do operations in Item master.");
            Console.WriteLine("Enter 2 to do operations in Customer master.");
            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 1)
            {
                Console.WriteLine();

                //all methods of item master
                ItemMaster itemMaster = new ItemMaster();
                itemMaster.ItemMasterMethods();
                goto MAIN_MENU;
            }
            else if (choice == 2)
            {
                CustomerMaster customerMaster = new CustomerMaster();
                //all methods of customer master
                customerMaster.CustomerMasterMethods();
                goto MAIN_MENU;
            }
        }
    }
}