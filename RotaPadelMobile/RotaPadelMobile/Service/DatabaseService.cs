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

        public async Task<int> SalvarCodigoRecuperacao(CodigoRecuperacao codigo)
        {
            return await _database.InsertAsync(codigo);
        }

        public async Task<CodigoRecuperacao> ValidarCodigo(string email, string codigo)
        {
            return await _database.Table<CodigoRecuperacao>()
                .Where(c => c.Email == email &&
                            c.Codigo == codigo &&
                            !c.Usado &&
                            c.DataExpiracao > DateTime.Now)
                .FirstOrDefaultAsync();
        }

        public async Task<int> MarcarCodigoComoUsado(int codigoId)
        {
            var codigo = await _database.Table<CodigoRecuperacao>()
                .Where(c => c.Id == codigoId)
                .FirstOrDefaultAsync();

            if (codigo != null)
            {
                codigo.Usado = true;
                return await _database.UpdateAsync(codigo);
            }
            return 0;
        }

        public async Task<Usuario> ObterUsuarioPorEmail(string email)
        {
            return await _database.Table<Usuario>()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<int> AtualizarSenha(int usuarioId, string novaSenha)
        {
            var usuario = await _database.Table<Usuario>()
                .Where(u => u.Id == usuarioId)
                .FirstOrDefaultAsync();

            if (usuario != null)
            {
                usuario.Senha = novaSenha;
                return await _database.UpdateAsync(usuario);
            }
            return 0;
        }
    }
}