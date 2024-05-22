using System.ComponentModel.DataAnnotations;

namespace SysPool.API.ResponseModels
{
    public class TablesResponse
    {
        public int MesaId { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
        public string ColorEstado { get; set; }
        public string ImagenMesa { get; set; }
    }
}