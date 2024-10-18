// api-response-handler.service.ts
import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class ApiResponseHandlerService {

  // Método para manejar la respuesta de la API
  handleApiResponse(response: any) {
    if (response && response.status === 0) {
      Swal.fire({
        icon: 'error',
        title: '¡Error! Servicio No Disponible',
        text: 'Lo sentimos, pero el servicio no está disponible en este momento. Por favor, intenta nuevamente más tarde o contacta a tu administrador si el problema persiste.'
      });
    }
    else if (response.success || response.status === 200) {
      // Manejo de la respuesta exitosa
      Swal.fire({
        icon: 'success',
        title: 'Éxito',
        text: 'La operación se realizó con éxito.'
      });
    } else if (response.status === 404) {
      Swal.fire({
        icon: 'error',
        title: '¡Oops! Ruta No Encontrada',
        text: 'Lo sentimos, pero la Ruta que estás buscando no se pudo encontrar. Puede que haya sido eliminada, haya cambiado de nombre o nunca haya existido. Por favor, pongase ne contanto con su adminisitrador'
      });
    } else if (response.status === 401) {
      // Manejo de error 401 - No autorizado
      Swal.fire({
        icon: 'error',
        title: 'Acceso denegado',
        text: 'No tienes permiso para realizar esta acción.'
      });
    } else if (response.status === 400) {
      // Manejo de error 400 - Errores de validación
      if (response.error && response.error.errors) {

        const messages: string[] = [];

        // Itera sobre las claves del objeto 'errors'
        for (const key in response.error.errors) {
          if (response.error.errors.hasOwnProperty(key)) {
            const errorMessages = response.error.errors[key];

            if (Array.isArray(errorMessages)) {
              messages.push(...errorMessages);
            } else {
              messages.push(errorMessages);
            }
          }
        }

        const singleText: string = messages.join('<br>'); // Usando <br> para HTML

        Swal.fire({
          icon: 'error',
          title: 'Errores de validación',
          html: singleText
        });
      } else if (response.error && response.error.errorMessage) {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: response.error.errorMessage
        });
      }
      else {
        // Manejo de otros errores 400
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: 'Se ha producido un error en la solicitud.'
        });
      }
    } else if (response.success === false && response.errorMessage) {
      // Manejo de errores de tipo { success: false }
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: response.errorMessage
      });
    } else {
      // Manejo de errores desconocidos
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: response.message ? response.message : response
      });
    }
  }
}
