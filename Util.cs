using System;
using System.Collections.Generic;

namespace CatWorx.BadgeMaker
{
  class Util 
  {
    // method declared as "static" = it belongs to the class itself instead of individual instances or objects.  we can access this method directly from the class name
    public static void PrintEmployees(List<Employee> employees)
    {
      foreach(var i in employees)
      {
        string template = "{0, -10}\t{1, -20}\t{2}";
        Console.WriteLine(String.Format(template, i.GetId(), i.GetFullName(), i.GetPhotoUrl()));
      }
    }
    public static void MakeCSV(List<Employee> employees)
    {
      // check to see if data folder exists
      if (!Directory.Exists("data"))
      {
        // If not, create it
        Directory.CreateDirectory("data");
      }
    }
  }
}