using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SysPool.API.ResponseModels
{
    public class ProductsResponse
    {

        [Key]
        public int IdProducto { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? ImagenProducto { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Categoria { get; set; }

    }
}