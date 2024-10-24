using FormsCreator.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Text.Json;

namespace FormsCreator.Application.Services
{
    internal class CommentNotifier : ICommentNotifier
    {
        private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, HttpResponse>> _clients = new();

        public void RegisterClient(Guid templateId, Guid userId, HttpResponse writer)
        {
            if (_clients.TryGetValue(templateId, out var list))
            {
                if(!list.TryAdd(userId, writer))
                {
                    UnregisterClient(templateId, userId);
                    RegisterClient(templateId, userId, writer);
                }
            }
            else
            {
                list = new();
                list.TryAdd(userId, writer);
                if(!_clients.TryAdd(templateId, list))
                {
                    UnregisterClient(templateId, userId);
                    RegisterClient(templateId, userId, writer);
                }
            }
        }

        public async Task SendCommentPostedNotificationAsync(Guid templateId)
        {
            if (!_clients.TryGetValue(templateId, out var list)) return;

            var tasks = list.Values.Select(async writer =>
            {
                await writer.WriteAsync("data: ");
                await JsonSerializer.SerializeAsync(writer.Body, new { IsNew = true });
                await writer.WriteAsync("\n\n");
                await writer.Body.FlushAsync();
            });

            await Task.WhenAll(tasks);
        }

        public void UnregisterClient(Guid templateId, Guid userId)
        {
            if (_clients.TryGetValue(templateId, out var list))
            {
                list.TryRemove(userId, out _);
                if (list.IsEmpty)
                    UnregisterTemplate(templateId);
            }
        }

        public void UnregisterTemplate(Guid templateId)
        {
            _clients.TryRemove(templateId, out _);
        }
    }
}
