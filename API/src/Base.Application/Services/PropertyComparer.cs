using TaskingSystem.Domain.Interfaces.Services;

namespace TaskingSystem.Application.Services
{
    public class PropertyComparer<T> : IPropertyComparer<T> where T : class
    {
        ///<inheritdoc/>
        public string Compare(T original, T updated)
        {
            if (updated == null)
            {
                throw new ArgumentNullException(nameof(updated), "El objeto actualizado no puede ser nulo.");
            }

            var changes = new List<string>();
            var properties = typeof(T).GetProperties();

            // Itera sobre todas las propiedades de la clase.
            foreach (var prop in properties)
            {
                // Verifica si la propiedad es una lista o un IEnumerable y la ignora.
                if (typeof(IEnumerable<object>).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
                {
                    continue; // Ignora las listas, pero permite que se comparen strings
                }

                // Obtiene los valores de la propiedad para los objetos original y actualizado.
                var originalValue = original == null ? null : prop.GetValue(original);
                var updatedValue = prop.GetValue(updated);

                // Compara los valores y registra los cambios si son diferentes.
                if (!Equals(originalValue, updatedValue))
                {
                    if (original == null)
                    {
                        changes.Add($"<strong>{prop.Name}</strong>: '{updatedValue}'<br/>"); // Nuevo objeto, solo tiene valor actualizado
                    }
                    else
                    {
                        changes.Add($"<strong>{prop.Name}</strong>: Ha pasado de '{originalValue}' <strong>a</strong> '{updatedValue}'<br/>"); // Cambio entre original y actualizado
                    }
                }
            }

            // Devuelve los cambios encontrados o un mensaje indicando que no se han detectado cambios.
            return changes.Count > 0 ? string.Join("\n", changes) : "No se han detectado cambios.";
        }
    }

}
