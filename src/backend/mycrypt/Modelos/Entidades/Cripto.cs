using System.ComponentModel.DataAnnotations;

namespace mycrypt.Models.Entidades
{
    public class Cripto
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Codigo { get; set; }

        public ICollection<Transaccion> Transacciones { get; set; }
    }
}
