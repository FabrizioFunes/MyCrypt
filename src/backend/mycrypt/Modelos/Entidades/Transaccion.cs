using mycrypt.Models.Entidades;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mycrypt.Models.Entidades
{
    public class Transaccion
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression("Compra|Venta")]
        public string Tipo { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [Range(0.00000001, double.MaxValue)]
        public decimal Cantidad { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MontoARS { get; set; }

        [ForeignKey("Exchange")]
        public int IdExchange { get; set; }

        public Exchange Exchange { get; set; }

        [ForeignKey("Cripto")]
        public int IdCripto { get; set; }

        public Cripto Cripto { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        public Usuario Usuario { get; set; }
    }
}
