using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace OrderAssignment
{
    public class SendMail
    {
        public static void SendMailMethod(string CustomerName, string recieverMail)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Online Orders Portal", "heman5502@gmail.com"));
            message.To.Add(MailboxAddress.Parse(recieverMail));

            message.Subject = "!!!Welcome!!!";
            message.Body = new TextPart("plain")
            {
                Text = $"Dear {CustomerName}, Thanks for registering with us."
            };

            #region private data
            string email = "heman5502@gmail.com";
            string password = "ytrnamdknanthdth";
            #endregion

            SmtpClient smtpClient = new SmtpClient();
            try
            {
                smtpClient.Connect("smtp.gmail.com", 465, true);
                smtpClient.Authenticate(email, password);
                smtpClient.Send(message);
                Console.WriteLine($"!!Thanks dear {CustomerName} for registration with us!!");
                Console.WriteLine($"A 'Welcome Message' is just sent to your registered mail id from '{email}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                smtpClient.Disconnect(true);
                smtpClient.Dispose();
            }
        }
        //public static void SendEmail(string fromAddress, string password)
        //{
        //    CustomerMaster customerMaster = new CustomerMaster();

        //    #region smtp
        //    SmtpClient email = new SmtpClient
        //    {
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        EnableSsl = true,
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        Credentials = new NetworkCredential(fromAddress, password)
        //    };
        //    string subject = "Welcome";
        //    string body = $"Dear , Thanks for registering with us";

        //    try
        //    {
        //        Console.WriteLine("Sending email********");
        //        email.EnableSsl = true;
        //        email.Send(fromAddress, ToAddress(), subject, body);
        //        Console.WriteLine("Email sent********");

        //    }
        //    catch (SmtpException e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //    #endregion
        //}

        //public static string GetUserName()
        //{
        //    return "iamteencoder@gmail.com";
        //}
        //public static string GetPassword()
        //{
        //    return "";
        //}
        //public static string ToAddress()
        //{
        //    return "heman5502@gmail.com";
        //}
    }

}
