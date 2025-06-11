namespace mycrypt.Models.DTOs
{
    public class TransaccionCreateDto
    {
        public string Tipo { get; set; }          // "Compra" o "Venta"
        public DateTime Fecha { get; set; }
        public decimal Cantidad { get; set; }
        public int IdExchange { get; set; }
        public int IdCripto { get; set; }
        public int IdUsuario { get; set; }
    }
}
