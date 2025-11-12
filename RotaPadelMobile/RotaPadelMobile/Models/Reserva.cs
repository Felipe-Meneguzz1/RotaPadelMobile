using SQLite;

namespace RotaPadelMobile.Models
{
    [Table("reservas")]
    public class Reserva
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Quadra { get; set; }

        public DateTime Data { get; set; }

        [MaxLength(10)]
        public string Hora { get; set; }

        public int UsuarioId { get; set; }
    }
}
