using SQLite;

namespace RotaPadelMobile.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }

        [Unique, MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(14)]
        public string CPF { get; set; }

        [MaxLength(255)]
        public string Senha { get; set; }

        public DateTime DataCriacao { get; set; }

        public string Perfil { get; set; }
    }
}