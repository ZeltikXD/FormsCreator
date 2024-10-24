using Microsoft.AspNetCore.Http;

namespace FormsCreator.Application.Abstractions
{
    public interface ICommentNotifier
    {
        void RegisterClient(Guid templateId, Guid userId, HttpResponse writer);

        void UnregisterClient(Guid templateId, Guid userId);

        Task SendCommentPostedNotificationAsync(Guid templateId);

        void UnregisterTemplate(Guid templateId);
    }
}