using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace WordToPdfConvert.Consumer.Service
{
    public static class EmailService
    {
        public static bool Send(string email, MemoryStream stream, string fileName)
        {
            try
            {
                stream.Position = 0;
                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType(MediaTypeNames.Application.Pdf);
                Attachment attachment = new Attachment(stream, contentType);
                attachment.ContentDisposition.FileName = $"{fileName}.pdf";
                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient("smtp.gmail.com");
                message.From = new MailAddress("test@outlook.com");
                message.To.Add(email);
                message.Subject = "Pdf Dosyası";
                message.Body = "Yüklenmiş olduğunuz dosyanız word formatından pdf formatına dönüştürülmüştür";
                message.IsBodyHtml = true;
                message.Attachments.Add(attachment);
                
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl=true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("test@outlook.com", "**");
                client.Send(message);
                stream.Close();
                stream.Dispose();
                return true;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
