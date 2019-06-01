using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;

namespace Selenium_App
{
    class Program
    {
        static void Main(string[] args)
        {

            List<string> reportFile = new List<string>();

            // Read and show each line from the file.
            Dictionary<string, string> urls = new Dictionary<string, string>();
            string line = "";
            using (StreamReader sr = new StreamReader("Data.txt"))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.ToLower().Contains("url$"))
                    {
                        urls.Add(line.Split('$')[1], line.Split('$')[2]);
                    }
                    else
                    {
                        reportFile.Add(line);
                    }
                }
            }


            using (StreamWriter sw = new StreamWriter("Report.txt", false))
            {
                foreach (string s in reportFile)
                {
                    sw.WriteLine(s);
                }
            }




            foreach (var item in urls)
            {
                try
                {
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("headless");

                    IWebDriver driver = new ChromeDriver(chromeOptions);
                    string url = item.Value;
                    driver.Navigate().GoToUrl(url);


                    reportFile = new List<string>();
                    using (StreamReader sr = new StreamReader("Report.txt"))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.ToLower().Contains("$" + item.Key + ","))
                            {
                                try
                                {
                                    string newData = line;
                                    foreach (var parts in line.Split('^'))
                                    {
                                        if (parts.Contains("$"))
                                        {
                                            var data = driver.FindElement(By.XPath(parts.Split('$')[1].Split(',')[1])).Text;
                                            newData = newData.Replace("$" + item.Key + "," + parts.Split('$')[1].Split(',')[1] + "$", data); 
                                        }
                                    }

                                    reportFile.Add(newData);
                                }
                                catch (Exception ex)
                                {
                                    var newData = line.Split('$')[0] + "Error" + line.Split('$')[2];
                                    reportFile.Add(newData);
                                }
                            }
                            else
                            {
                                reportFile.Add(line);
                            }
                        }

                    }

                    driver.Close();
                    Console.Clear();

                    Console.WriteLine("SIte" + item.Key + "Processed-------------------------------------------------");

                    using (StreamWriter sw = new StreamWriter("Report.txt", false))
                    {
                        foreach (string s in reportFile)
                        {
                            sw.WriteLine(s);
                        }
                    }
                }
                catch (Exception)
                {

                }

            }


            List<string> reportFileNew = new List<string>();

            // Read and show each line from the file.
            using (StreamReader sr = new StreamReader("Report.txt"))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    reportFileNew.Add(line.Replace("^",""));
                }
            }


            using (StreamWriter sw = new StreamWriter("Report.txt", false))
            {
                foreach (string s in reportFileNew)
                {
                    sw.WriteLine(s);
                }
            }

            Console.WriteLine("Automation completed, do check your report.");

            Console.ReadKey();













        }
    }
}
