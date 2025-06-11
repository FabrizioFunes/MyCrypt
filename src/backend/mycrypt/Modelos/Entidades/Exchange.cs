using System.ComponentModel.DataAnnotations;

namespace mycrypt.Models.Entidades
{
    public class Exchange
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string? URL { get; set; }

        public ICollection<Transaccion> Transacciones { get; set; }
    }
}
