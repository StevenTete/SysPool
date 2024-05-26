namespace SysPool.API.ResponseModels
{
    public class BillsResponse
    {
        public DateTime fecha { get; set; }
        public decimal total { get; set; }
        public decimal totalConImpuestos => total * impuestos;
        public decimal impuestos { get; set; }
        public int cantidad { get; set; }
        public string nombreProducto { get; set; }
        public decimal precioVenta { get; set; }
        public string tiempoInicio { get; set; }
        public string tiempoFinal { get; set; }
        public int tiempoJugado { get; set; }
        public int valorTiempo { get; set; }
    }
}
