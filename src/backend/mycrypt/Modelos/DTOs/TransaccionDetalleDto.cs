namespace mycrypt.Models.DTOs
{
    public class TransaccionDetalleDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Cantidad { get; set; }
        public decimal MontoARS { get; set; }

        public string CriptoNombre { get; set; }
        public string ExchangeNombre { get; set; }
        public string UsuarioNombre { get; set; }
    }
}
