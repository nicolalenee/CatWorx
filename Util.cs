using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using SkiaSharp;
using System.Threading.Tasks;

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
      using (StreamWriter file = new StreamWriter("data/employees.csv"))
      {
        file.WriteLine("ID,Name,PhotoUrl");
        // loop over the employees
        foreach(var i in employees)
        {
          // write each employee to the file
          string template = "{0}, {1}, {2}";
          file.WriteLine(String.Format(template, i.GetId(), i.GetFullName(), i.GetPhotoUrl()));
        }
      }
    }
    async public static Task MakeBadges(List<Employee> employees)
    {
      // create the canvas that we'll use to make the badge
      //LAYOUT VARIABLES
      int BADGE_WIDTH = 669;
      int BADGE_HEIGHT = 1044;

      int PHOTO_LEFT_X = 184;
      int PHOTO_TOP_Y = 215;
      int PHOTO_RIGHT_X = 486;
      int PHOTO_BOTTOM_Y = 517;


      // create image. instance of HttpClient is disposed after code in black has run
      using(HttpClient client = new HttpClient())
      {
        foreach(var i in employees)
        {
          SKImage photo = SKImage.FromEncodedData(await client.GetStreamAsync(i.GetPhotoUrl()));
          SKImage background = SKImage.FromEncodedData(File.OpenRead("badge.png"));

          SKBitmap badge = new SKBitmap(BADGE_WIDTH, BADGE_HEIGHT);
          SKCanvas canvas = new SKCanvas(badge);

          canvas.DrawImage(background, new SKRect(0, 0, BADGE_WIDTH, BADGE_HEIGHT));
          canvas.DrawImage(photo, new SKRect(PHOTO_LEFT_X, PHOTO_TOP_Y, PHOTO_RIGHT_X, PHOTO_BOTTOM_Y));

          SKImage finalImage = SKImage.FromBitmap(badge);
          SKData data = finalImage.Encode();
          data.SaveTo(File.OpenWrite("data/employeeBadge.png"));

          // how we'll see if the file was actually written
          //SKData data = background.Encode();
          //data.SaveTo(File.OpenWrite("data/employeeBadge.png"));
        }
      }
      
    }
  }
}