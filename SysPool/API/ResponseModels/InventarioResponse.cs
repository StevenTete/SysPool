using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SysPool.API.ResponseModels
{
    public class InventarioResponse
    {

        [Key]
        public int InventarioId { get; set; }
        public int ProductoId { get; set; }
        public int Stock { get; set; }


    }
}
