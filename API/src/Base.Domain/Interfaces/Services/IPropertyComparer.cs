namespace TaskingSystem.Domain.Interfaces.Services
{
    /// <summary>
    /// Servicio genérico para comparar las propiedades de dos instancias de un tipo de clase específico.
    /// </summary>
    /// <typeparam name="T">El tipo de clase que se va a comparar. Debe ser una clase.</typeparam>
    public interface IPropertyComparer<T> where T : class
    {
        /// <summary>
        /// Compara dos instancias de la clase <typeparamref name="T"/> y devuelve una lista de las propiedades
        /// cuyos valores han cambiado, en forma de cadena de texto.
        /// </summary>
        /// <param name="original">La instancia original del objeto.</param>
        /// <param name="updated">La instancia actualizada del objeto.</param>
        /// <returns>Una cadena que describe los cambios entre las dos instancias, o un mensaje indicando que no hay cambios.</returns>
        /// <exception cref="ArgumentNullException">Se lanza si cualquiera de los objetos proporcionados es nulo.</exception>
        string Compare(T original, T updated);
    }
}
