using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace DoAnNhom_GAMERAN_
{
    internal class SendOTP
    {
        public void SendOTPEmail(string email, int otp)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("lenguyenminhkhoi2006bl@gmail.com");
            mail.To.Add(email);
            mail.Subject = "OTP From game Snake ";
            mail.Body = "Your OTP code is: " + otp;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(
                "lenguyenminhkhoi2006bl@gmail.com",
                "mvtl wdgx yzlv mmme"
            );
            smtp.EnableSsl = true;

            smtp.Send(mail);   
        }
    }
}
