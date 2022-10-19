using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CatWorx.BadgeMaker
{
  class PeopleFetcher
  {
     // method to get employees from user input in the console
    public static List<Employee> GetEmployees()
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
    
    async public static Task<List<Employee>> GetFromApi()
    {
      List<Employee> employees = new List<Employee>();using(HttpClient client = new HttpClient())
      {
        string response = await client.GetStringAsync("https://randomuser.me/api/?results=10&nat=us&inc=name,id,picture");
        
        JObject json = JObject.Parse(response);
       
        foreach (JToken person in json.SelectToken("results")!) // null-forgiveness
        {
          Employee emp = new Employee
          (
            // // convert from JObject data types to Employee constructor data types

            // get the person's name
            person.SelectToken("name.first")!.ToString(),
            person.SelectToken("name.last")!.ToString(),
            //get the person's id
            Int32.Parse(person.SelectToken("id.value")!.ToString().Replace("-", "")),
            // get the person's picture
            person.SelectToken("picture.large")!.ToString()
          );
          // add to the employees List
          employees.Add(emp);
        }
        
      }
      return employees;
    }
    
  }
}