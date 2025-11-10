using SQLite;
using RotaPadelMobile.Models;

namespace RotaPadelMobile.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "rotapadel.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Usuario>().Wait();
        }

        // Cadastrar usuário
        public async Task<int> CadastrarUsuario(Usuario usuario)
        {
            return await _database.InsertAsync(usuario);
        }
        // Login
        public async Task<Usuario> Login(string email, string senha)
        {
            return await _database.Table<Usuario>()
                .Where(u => u.Email == email && u.Senha == senha)
                .FirstOrDefaultAsync();
        }

        // Verificar email
        public async Task<bool> EmailExiste(string email)
        {
            var usuario = await _database.Table<Usuario>()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
            return usuario != null;
        }
    }
}