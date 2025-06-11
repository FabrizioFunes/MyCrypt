namespace mycrypt.Models.DTOs
{
    public class TransaccionResumenDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Cripto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal MontoARS { get; set; }
        public DateTime Fecha { get; set; }
    }
}
