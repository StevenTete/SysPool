namespace SysPool.API.ResponseModels
{
    public class BookingsResponse
    {
        public int ReservaId { get; set; }
        public int UsuarioId { get; set; }
        public int MesaId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Estado { get; set; }
        public string ColorEstado { get; set; }
        public string ImagenMesa { get; set; }
        public string TipoMesa { get; set; }
        public string FechaFormateada => Fecha.ToString("dd/MM/yyyy");
    }
}
