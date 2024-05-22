namespace SysPool.API.ResponseModels
{
    public class UsersResponse
    {
        public int usuarioId { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string email { get; set; }
        public string clave { get; set; }
        public int rolId { get; set; }
        public string Foto { get; set; }
    }
}
