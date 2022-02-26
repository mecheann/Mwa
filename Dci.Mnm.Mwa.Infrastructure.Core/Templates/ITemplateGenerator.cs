using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Infrastructure.Core
{
    public interface ITemplateGenerator
    {
        Task<string> GetCompiledTemplate(string templateName, object model, string newTemplatePath = null);
    }
}








