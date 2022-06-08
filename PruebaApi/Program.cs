using System;
using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Chrome;
using System.Threading;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace PruebaApi
{
    class Program
    {
        static Screenshot screenshot;

        static void Main(string[] args)
        {
            var options = new ChromeOptions();
            options.DebuggerAddress = "127.0.0.1:9222";
            var driver = new ChromeDriver(options);
            int contador = 0;
            
            do
            {
                if (contador >= 10)
                {
                    Console.Clear();
                    contador = 0;
                }

                SacarScreenShot(driver);
                ValidarScreenShot();
                contador++;
            }
            while (true);
        }


        public static void SacarScreenShot(ChromeDriver driver)
        {
            Console.WriteLine("Redirigiendo...");
            driver.Navigate().GoToUrl("https://onlineservices.immigration.govt.nz/WorkingHoliday/Application/Create.aspx?CountryId=13&OffShore=1&STZ=0");
            Thread.Sleep(15000);
            Console.WriteLine("Sacando screenshot");
            screenshot = (driver as ITakesScreenshot).GetScreenshot();
            screenshot.SaveAsFile("screenshot.png", ScreenshotImageFormat.Png);
            Console.WriteLine("Listo.");
            Console.WriteLine("------------------------------------");
            //driver.Manage().Cookies.DeleteAllCookies();

        }

        public static bool ValidarScreenShot()
        {
            if(LeerImagen("original.png") == LeerImagen("screenshot.png"))
            {
                return true;
            }

            EnviarAviso();
            screenshot.SaveAsFile("original.png", ScreenshotImageFormat.Png);
            return false;
        }

        public static string LeerImagen(string fileName)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(fileName))
                {
                    return Convert.ToBase64String(md5.ComputeHash(stream));
                }
            }
        }
        public static void EnviarAviso()
        {

            Console.WriteLine("-----------------ALERTA DE CAMBIO-----------------");

            String EmailOrigen = "botdecorreo@gmail.com";
            String EmailDestino = "botdecorreo@gmail.com";
            String Password = "erejjtonqwgbnwgt";


            MailMessage mail = new MailMessage(EmailOrigen, EmailDestino, "Cambio la pagina", "Cambio la pagina");
            mail.IsBodyHtml = true;


            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(EmailOrigen, Password);
           
            smtpClient.Send(mail);
            smtpClient.Dispose();

        }
    }
}
