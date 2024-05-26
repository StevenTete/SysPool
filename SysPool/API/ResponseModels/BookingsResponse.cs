namespace SysPool.API.ResponseModels
{
    public class BookingsResponse
    {
        public int ReservaId { get; set; }
        public int UsuarioId { get; set; }
        public int MesaId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public required string Estado { get; set; }
        public required string ColorEstado { get; set; }
        public required string ImagenMesa { get; set; }
        public required string TipoMesa { get; set; }
        public string FechaFormateada => Fecha.ToString("dd/MM/yyyy");
        public bool CancelButtonVisibility { get; set; } = true;
    }
}
