using SQLite;

namespace SysPool.Local
{
    public class LocalDbService
    {
        private const string DB_NAME = "Adicion.db";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<ProductToBuy>();
        }

        public async Task<List<ProductToBuy>> GetProductsToBuy()
        {
            return await _connection.Table<ProductToBuy>().Where(p => p.idUsuario == App.UserID).ToListAsync();
        }

        public async Task AddProduct(ProductToBuy product)
        {
            await _connection.InsertAsync(product);
        }

        public async Task DeleteProduct(int productId)
        {
            await _connection.DeleteAsync<ProductToBuy>(productId);
        }
    }
}
