namespace mycrypt.Models.DTOs
{
    public class TransaccionUpdateDto
    {
        public decimal? Cantidad { get; set; }
        public decimal? MontoARS { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdExchange { get; set; }
        public int? IdCripto { get; set; }
    }
}
