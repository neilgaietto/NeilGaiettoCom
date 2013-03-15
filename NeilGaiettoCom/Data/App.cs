using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Configuration;
using System.Net.Mail;

namespace NeilGaiettoCom.Data
{
    /// <summary>
    /// The Current App's Properties.
    /// </summary>
    public class App
    {
        #region app properties
        public static String AdminEmail
        {
            get
            {

                return ConfigurationManager.AppSettings["AdminEmail"];
            }
        }
        public static String EmailUser
        {
            get
            {

                return ConfigurationManager.AppSettings["EmailUser"];
            }
        }
        public static String Emailpass
        {
            get
            {

                return ConfigurationManager.AppSettings["Emailpass"];
            }
        }
        #endregion

        #region app functions
        public static void PageVisited(string page, string visitor)
        {
            try
            {
                using (var db = new Data.NeilDBEntities())
                {
                    PageVisit pv = new PageVisit()
                    {
                        Page = page,
                        Visitor = visitor,
                        When = DateTime.Now
                    };
                    db.PageVisits.Add(pv);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendContactForm(Models.ContactForm contactForm)
        {

            var fromAddress = new MailAddress(EmailUser);
            var toAddress = new MailAddress(AdminEmail);
            string subject = "NeilGaietto.com Contact Form";
            string body = string.Format("Name:\r\n{0}\r\n\r\nEmail:\r\n{1}\r\n\r\nMessage:\r\n{2}", contactForm.FullName, contactForm.Email, contactForm.Message);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System‎‎.Net‎‎.NetworkCredential(EmailUser, Emailpass)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }


        }
        #endregion
    }
}
