using FormsCreator.Core.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Services
{
    /// <summary>
    /// Interface for sending emails.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email to a single recipient.
        /// </summary>
        /// <param name="to">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendEmailAsync(string to, string subject, string body);

        /// <summary>
        /// Sends an email to a single recipient with an option for HTML content.
        /// </summary>
        /// <param name="to">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <param name="isHtml">A boolean indicating whether the body is HTML.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendEmailAsync(string to, string subject, string body, bool isHtml);

        /// <summary>
        /// Sends an email to multiple recipients.
        /// </summary>
        /// <param name="to">An array of recipient email addresses.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendEmailAsync(string[] to, string subject, string body);

        /// <summary>
        /// Sends an email to multiple recipients with an option for HTML content.
        /// </summary>
        /// <param name="to">An array of recipient email addresses.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <param name="isHtml">A boolean indicating whether the body is HTML.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendEmailAsync(string[] to, string subject, string body, bool isHtml);

        /// <summary>
        /// Sends an email to a single recipient with attachments.
        /// </summary>
        /// <param name="to">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <param name="attachments">A collection of attachments to include in the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendEmailWithAttachmentsAsync(string to, string subject, string body, IEnumerable<EmailAttachment> attachments);

        /// <summary>
        /// Sends an email to multiple recipients with attachments.
        /// </summary>
        /// <param name="to">An array of recipient email addresses.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <param name="attachments">A collection of attachments to include in the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendEmailWithAttachmentsAsync(string[] to, string subject, string body, IEnumerable<EmailAttachment> attachments);
    }
}