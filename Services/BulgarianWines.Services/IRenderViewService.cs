namespace BulgarianWines.Services
{
    using System.Threading.Tasks;

    public interface IRenderViewService
    {
        public Task<string> RenderToStringAsync(string viewName, object model);
    }
}
