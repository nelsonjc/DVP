namespace TaskingSystem.Application.Constants
{
    public static class MessageConstant
    {
        public const string DEFAULT_ERROR_MESSAGE = "Ha ocurrido un error, por favor intentelo de nuevo o contacte con el administrador";
        public const string LOG_ERROR_MESSAGE = "Ha ocurrido la siguiente exception: {message}";
        public const string LOG_ERROR_DETAIL_MESSAGE = "Ha ocurrido la siguiente excepción: {message}, con el  detalle: {detail}, en la hora: {time}";
        public const string PAGED_PROPERTY = "La expreasión 'property' deria ser una propiedad de la entidad";
        public const string AUTH_OK = "¡Usuario autenticado en el sistema!";
        public const string AUTH_ERROR = "Usuario o contraseña no valido.";
        public const string HASH_ERROR = "Formato hash inesperado.";
        public const string AUTHENTICATION_ERROR = "Error de autenticación.";

        public const string TASK_CREATION_OBSERVATION = "Se ha registrado una nueva tarea";
    }
}
