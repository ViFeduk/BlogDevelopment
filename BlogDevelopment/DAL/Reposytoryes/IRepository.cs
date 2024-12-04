namespace BlogDevelopment.DAL.Reposytoryes
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(); // Используем IQueryable для гибкости
        Task<T?> GetByIdAsync(int id); // Асинхронный метод получения по ID
        Task CreateAsync(T item); // Асинхронное добавление
        void Update(T item); // Обновление (обычно синхронно)
        void Delete(T item); // Удаление (обычно синхронно)
        Task SaveChangesAsync(); // Асинхронное сохранение изменений
    }
}
