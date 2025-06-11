namespace mycrypt.Servicios
{
    public interface ICriptoYaService
    {
        Task<decimal?> ObtenerPrecioAsync(string codigoCripto, string exchange);
    }
}
