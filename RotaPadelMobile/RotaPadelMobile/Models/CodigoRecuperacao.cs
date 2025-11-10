using SQLite;

namespace RotaPadelMobile.Models
{
    [Table("codigos_recuperacao")]
    public class CodigoRecuperacao
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Email { get; set; }
        public string Codigo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataExpiracao { get; set; }
        public bool Usado { get; set; }
    }
}