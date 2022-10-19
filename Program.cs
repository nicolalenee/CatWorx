using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatWorx.BadgeMaker 
{
  class Program
  {
    // method to get employees from user input in the console
    static List<Employee> GetEmployees()
    {
      List<Employee> employees = new List<Employee>(); 
      // collect user values until the value is an empty string
      while (true) 
      {
        Console.WriteLine("Enter a first name (leave empty to exit): ");
        string firstName = Console.ReadLine() ?? "";
        // Break if the user hits ENTER without typing a name
        if (firstName == "")
        {
          break;
        }
        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine() ?? "";
        Console.Write("Enter ID: ");
        int id = Int32.Parse(Console.ReadLine() ?? "");
        Console.Write("Enter Photo URL: ");
        string photoUrl = Console.ReadLine() ?? "";
        // create a new Employee instance
        Employee currentEmployee = new Employee(firstName, lastName, id, photoUrl);
        employees.Add(currentEmployee);
      }
      // important to return the list to the user's call!
      return employees;
    }
    
    
  // Main method that runs on application initiation
    async static Task Main(string[] args)
    {
      List<Employee> employees = GetEmployees();
      Util.PrintEmployees(employees);
      Util.MakeCSV(employees);
      await Util.MakeBadges(employees);
    }
  }
}