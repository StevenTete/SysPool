using SQLite;

namespace SysPool.Local
{
    [Table("productToBuy")]
    public class ProductToBuy
    {

        [PrimaryKey]
        [Column("idProducto")]
        public int idProducto { get; set; }

        [Column("idUsuario")]
        public int idUsuario { get; set; }

        [Column("nombreProducto")]
        public string? nombreProducto { get; set; }

        [Column("cantidadSeleccionada")]
        public int cantidadSeleccionada { get; set; }

        [Column("imagenProducto")]
        public string? imagenProducto { get; set; }

        [Column("precio")]
        public double precio { get; set; }

    }
}
