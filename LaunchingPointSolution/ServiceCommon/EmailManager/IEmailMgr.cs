using LP2.Service.Common;

namespace EmailManager
{
    public interface IEmailMgr
    {
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        bool SendEmail(SendEmailRequest req);

        /// <summary>
        /// Previews the email.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        EmailPreviewResponse PreviewEmail(EmailPreviewRequest req);

        /// <summary>
        /// Processes the email que.
        /// </summary>
        /// <returns></returns>
        bool ProcessEmailQue();
    }
}