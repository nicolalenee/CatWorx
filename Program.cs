using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatWorx.BadgeMaker 
{
  class Program
  {
    async static Task Main(string[] args)
    {
      Console.WriteLine("Do you want to fetch employee data from the API? If so type 'yes'. Otherwise, enter any key ");
      string answer = Console.ReadLine() ?? "";
      if (answer == "yes")
      {
        List<Employee> employees = await PeopleFetcher.GetFromApi();
        Util.PrintEmployees(employees);
        Util.MakeCSV(employees);
        await Util.MakeBadges(employees);
      } else {
        List<Employee> employees = PeopleFetcher.GetEmployees();
        Util.PrintEmployees(employees);
        Util.MakeCSV(employees);
        await Util.MakeBadges(employees);
      }
      
      
    }
  }
}