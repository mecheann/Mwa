using System.Threading.Tasks;

namespace Dci.Mnm.Mwa.Core.Database
{
    public interface DatabaseWithSeeding
    {
        Task Seed(AppConfig appConfig);
    }
}









