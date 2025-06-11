using System.ComponentModel.DataAnnotations;

namespace mycrypt.Models.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Contrasenia { get; set; }

        public decimal TotalPesos { get; set; } = 0;

        public ICollection<Transaccion> Transacciones { get; set; }
    }
}
