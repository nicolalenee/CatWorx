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
      //LAYOUT VARIABLES
      int BADGE_WIDTH = 669;
      int BADGE_HEIGHT = 1044;

      int PHOTO_LEFT_X = 184;
      int PHOTO_TOP_Y = 215;
      int PHOTO_RIGHT_X = 486;
      int PHOTO_BOTTOM_Y = 517;

      int COMPANY_NAME_Y = 150;

      int EMPLOYEE_NAME_Y = 600;

      int EMPLOYEE_ID_Y = 730;

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

          //Arguments for SKPaint object constructor
          SKPaint paint = new SKPaint();
          paint.TextSize = 42.0f;
          paint.IsAntialias = true;
          paint.Color = SKColors.White;
          paint.IsStroke = false;
          paint.TextAlign = SKTextAlign.Center;
          paint.Typeface = SKTypeface.FromFamilyName("Arial");
          //draw the text for the company name
          canvas.DrawText(i.GetCompanyName(), BADGE_WIDTH /2f, COMPANY_NAME_Y, paint);
          // make the employee name black and draw text
          paint.Color = SKColors.Black;
          canvas.DrawText(i.GetFullName(), BADGE_WIDTH / 2f, EMPLOYEE_NAME_Y, paint);
          // make the employee id font Courier New and draw text
          paint.Typeface = SKTypeface.FromFamilyName("Courier New");
          canvas.DrawText(i.GetId().ToString(), BADGE_WIDTH / 2f, EMPLOYEE_ID_Y, paint);
          
          //final image
          SKImage finalImage = SKImage.FromBitmap(badge);
          SKData data = finalImage.Encode();
          string template = "data/{0}_badge.png";
          data.SaveTo(File.OpenWrite(string.Format(template, i.GetId())));
        }
      }
      
    }
  }
}