namespace BulgarianWines.Services
{
    public interface IUsersService
    {
        public T GetById<T>(string id);

        public string GetImage();
    }
}