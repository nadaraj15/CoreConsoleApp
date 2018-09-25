using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Threading;

namespace CoreConsoleApp
{
    class Program
    {
        private static IWebDriver driver;
        private static IJavaScriptExecutor jsExecutor;

        static void Main(string[] args)
        {
            try
            {

                #region RemoteWebDriver logic for Selenium Grid
                //RemoteWebDriver remoteDriver;
                //DesiredCapabilities capability = DesiredCapabilities.Chrome();
                //capability.SetCapability("platform", "VISTA");
                //capability.SetCapability("platformName", "windows");
                //capability.SetCapability("version", "latest");
                //capability.SetCapability("gridlasticUser", USERNAME);
                //capability.SetCapability("gridlasticKey", ACCESS_KEY);
                //capability.SetCapability("video", "True");
                //driver = new RemoteWebDriver(
                //  new Uri("http://YOUR_GRIDLASTIC_SUBDOMAIN.gridlastic.com:80/wd/hub/"), capability, TimeSpan.FromSeconds(600));// NOTE: connection timeout of 600 seconds or more required for time to launch grid nodes if non are available.
                //driver.Manage().Window.Maximize(); // If Linux set window size, max 1920x1080, like driver.Manage().Window.Size = new Size(1920, 1020);
                //driver.Navigate().GoToUrl("https://www.google.com");
                //IWebElement query = driver.FindElement(By.Name("q"));
                //query.SendKeys("webdriver");
                //query.Submit();


                //Console.WriteLine("Video: " + VIDEO_URL + driver.SessionId);

                //driver.Quit();

                #endregion

                #region Send test mail

                Console.WriteLine("Sending test email. ");
                using (MailMessage mailMessage = new MailMessage())
                {
                    using (SmtpClient SmtpServer = new SmtpClient("smtp.office365.com"))
                    {
                        mailMessage.From = new MailAddress("swapnilm@maqsoftware.com");
                        mailMessage.To.Add("swapnilm@maqsoftware.com");
                        mailMessage.Body = "Sample test email.";
                        SmtpServer.Port = 587;
                        string subject = "Test results from container";
                        mailMessage.Subject = string.Concat(subject, CultureInfo.InvariantCulture);
                        SmtpServer.Credentials = new System.Net.NetworkCredential("swapnilm@maqsoftware.com", "cyxnblwdfjsggsfl");
                        SmtpServer.EnableSsl = true;
                        SmtpServer.Send(mailMessage);
                    }
                }
                #endregion
                Console.WriteLine("Test Mail sent.");


                Console.WriteLine("Program started...");
                //driver = new ChromeDriver(@"C:\Users\MAQUser\source\repos\CoreConsoleApp\CoreConsoleApp\bin\Debug");
                driver = new ChromeDriver();

                jsExecutor = (IJavaScriptExecutor)driver;
                driver.Manage().Window.Maximize();

                driver.Navigate().GoToUrl("https://www.google.co.in");
                Thread.Sleep(2000);

                // Take Screenshot
                Bitmap bitmap = new Bitmap(800, 600);
                Graphics graphics = Graphics.FromImage(bitmap as Image);
                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                bitmap.Save(Directory.GetCurrentDirectory() + "\\screenshot.jpeg", ImageFormat.Jpeg);
                string screenshotPath = Directory.GetCurrentDirectory() + "\\screenshot.jpeg";
                Console.WriteLine("Image Saved successfully to: " + screenshotPath);

                // Send attachment email

                using (MailMessage mailMessage = new MailMessage())
                {
                    using (SmtpClient SmtpServer = new SmtpClient("smtp.office365.com"))
                    {
                        mailMessage.From = new MailAddress("swapnilm@maqsoftware.com");
                        mailMessage.To.Add("swapnilm@maqsoftware.com");
                        mailMessage.Body = "Sample result";
                        SmtpServer.Port = 587;
                        string subject = "Test results from container";
                        mailMessage.Subject = string.Concat(subject, CultureInfo.InvariantCulture);
                        SmtpServer.Credentials = new System.Net.NetworkCredential("swapnilm@maqsoftware.com", "cyxnblwdfjsggsfl");
                        SmtpServer.EnableSsl = true;
                        Attachment attachment = new Attachment(screenshotPath);
                        mailMessage.Attachments.Add(attachment);
                        SmtpServer.Send(mailMessage);
                    }
                }

                driver.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
}
