# CAT WORX
A badge-making program made easy.

## Description
This program allows users to create employee badges from the command line.
Users are given the option between auto-generating user information from the Random User Generator API or manually entering the information in for each employee.

## Requirements
- C#
- .NET framework

## Technologies
**Language**: This program was written with C#.
**Tools**: To assist with generating random employee information, the Random User Generator API was used to fetch employees first and last names, along with an ID, and photo.
**Packages**: HttpClient, Newtonsoft.Json, SkiaSharp

## Installation 
After download, this program can be run using the simple `dotnet run` command.

## Image Previews
If a user wants the employee information automatically generated, said information will be returned to the user and the data will be saved in a CSV file and badges will also be created for each employee.
<img width="648" alt="image" src="https://user-images.githubusercontent.com/86696492/196764255-574ecd74-4b42-43bc-ae30-90c571dc3029.png">


<img width="275" alt="image" src="https://user-images.githubusercontent.com/86696492/196764450-e320311b-4642-45c0-8191-f2d598786837.png">

If a user wants to manually enter in employee information they will be prompted to enter each relevant piece of data. Similar to the following steps above, the data will be saved in the CSV file and the badge will be created.

<img width="661" alt="image" src="https://user-images.githubusercontent.com/86696492/196765857-66ef7eb1-8027-4177-8585-d8f3fa06fe87.png">

<img width="275" alt="image" src="https://user-images.githubusercontent.com/86696492/196765944-e98e1d69-98b7-4c7e-99ab-e31be26d6658.png">
*purrrr*fect.

## CSV 
For this project, using a CSV file was selected as the easiest method of storing data. This is due to all of the data being text that required no alterations or modifications after entry. 

The `System.IO.StreamWriter` class facilitates creating and writing to files and was used to create our CSV file that contains all of the employee information:
```C#
using (StreamWriter file = new StreamWriter("data/employees.csv"))
{
  file.WriteLine("ID,Name,PhotoUrl");
  // Loop over employees
  for (int i = 0; i < employees.Count; i++)
  {
    // Write each employee to the file
    string template = "{0},{1},{2}";
    file.WriteLine(String.Format(template, employees[i].GetId(), employees[i].GetFullName(), employees[i].GetPhotoUrl()));
  }
}
```
## SkiaSharp
SkiaSharp was a huge player in creating the badges. For this project we wielded it to use its graphic tools to create the badge template alongside HttpClient to import the employee data. 

The most important features that we used were SKBitmap, SKData, SKCanvas, SKImage, and SKPaint.

For brevity's sake, the import steps are outlined below in general terms followed by the code snippeet.

* use HttpClient class to import the employee data from the employee list asynchronously,
* Convert the employee image for the badge into an SKImage,
* Convert the backgroundd image forr the badge into an SKImage,
* Use SKIBitmap into an SKCanvas object so that we can call its methods to draw the images on the canvas
* Call the `DrawImage()` method on the canvas object while allocating the background and the employee's photo position and size on the badge
* Using the SKPaint object we can set a number of properties for the text that we want to draw the text onto the badge. 
  
```C#
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
```
## Questions?
Email: marblenicola@gmail.com  
Repository: https://github.com/nicolalenee/CatWorx
