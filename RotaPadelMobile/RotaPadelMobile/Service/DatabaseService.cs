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
            _database.CreateTableAsync<Reserva>().Wait();

        }
        public async Task<int> CadastrarUsuario(Usuario usuario)
        {
            return await _database.InsertAsync(usuario);
        }
        public async Task<Usuario> Login(string email, string senha)
        {
            return await _database.Table<Usuario>()
                .Where(u => u.Email == email && u.Senha == senha)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> EmailExiste(string email)
        {
            var usuario = await _database.Table<Usuario>()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
            return usuario != null;
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
        public async Task<int> CadastrarReserva(Reserva reserva)
        {
            return await _database.InsertAsync(reserva);
        }

        public async Task<List<Reserva>> ObterAgendamentosPorDataEQuadra(DateTime data, string quadra)
        {
            return await _database.Table<Reserva>()
                .Where(r => r.Data == data && r.Quadra == quadra)
                .ToListAsync();
        }

        public async Task<bool> HorarioOcupado(string quadra, DateTime data, string hora)
        {
            var reserva = await _database.Table<Reserva>()
                .Where(r => r.Quadra == quadra && r.Data == data && r.Hora == hora)
                .FirstOrDefaultAsync();

            return reserva != null;
        }

        public Task<List<Reserva>> ObterReservasPorUsuarioAsync(int usuarioId)
        {
            return _database.Table<Reserva>()
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public Task<int> DeletarReservaAsync(int id)
        {
            return _database.DeleteAsync<Reserva>(id);
        }
        public async Task<List<Reserva>> ObterReservasPorUsuario(int usuarioId)
        {
            return await _database.Table<Reserva>()
                .Where(r => r.UsuarioId == usuarioId)
                .OrderBy(r => r.Data)
                .ThenBy(r => r.Hora)
                .ToListAsync();
        }

        public async Task<int> CancelarReserva(int reservaId)
        {
            return await _database.DeleteAsync<Reserva>(reservaId);
        }

        public bool PodeCancelar(Reserva reserva)
        {
            var horaReserva = TimeSpan.Parse(reserva.Hora);
            var dataHoraReserva = reserva.Data.Date + horaReserva;
            return dataHoraReserva > DateTime.Now &&
                   dataHoraReserva > DateTime.Now.AddHours(2);
        }

        public bool JaPassou(Reserva reserva)
        {
            var horaReserva = TimeSpan.Parse(reserva.Hora);
            var dataHoraReserva = reserva.Data.Date + horaReserva;
            return dataHoraReserva < DateTime.Now;
        }
        public async Task<int> AtualizarUsuario(Usuario usuario)
        {
            return await _database.UpdateAsync(usuario);
        }

        public async Task<Usuario> ObterUsuarioPorId(int id)
        {
            return await _database.Table<Usuario>()
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> EmailJaExiste(string email, int usuarioIdAtual)
        {
            var usuario = await _database.Table<Usuario>()
                .Where(u => u.Email == email && u.Id != usuarioIdAtual)
                .FirstOrDefaultAsync();
            return usuario != null;
        }
    }
}