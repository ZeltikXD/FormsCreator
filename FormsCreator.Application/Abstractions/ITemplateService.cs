using FormsCreator.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace FormsCreator.Application.Abstractions
{
    public interface ITemplateService : ITemplateService<IFormFile>;
}
