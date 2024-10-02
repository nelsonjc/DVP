// notification.service.ts
import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor() { }

  // Método para mostrar una notificación de éxito
  showSuccess(message: string) {
    Swal.fire({
      icon: 'success',
      title: 'Éxito',
      text: message,
      timer: 3000,
      timerProgressBar: true,
      showCloseButton: true
    });
  }

  // Método para mostrar una notificación de error
  showError(message: string) {
    Swal.fire({
      icon: 'error',
      title: 'Error',
      text: message,
      timer: 3000,
      timerProgressBar: true,
      showCloseButton: true
    });
  }

  // Método para mostrar una notificación de advertencia
  showWarning(message: string) {
    Swal.fire({
      icon: 'warning',
      title: 'Advertencia',
      text: message,
      timer: 3000,
      timerProgressBar: true,
      showCloseButton: true
    });
  }

  // Método para mostrar una notificación de información
  showInfo(message: string) {
    Swal.fire({
      icon: 'info',
      title: 'Información',
      text: message,
      timer: 3000,
      timerProgressBar: true,
      showCloseButton: true
    });
  }

  // Método para mostrar una notificación de confirmación
  showConfirm(message: string): Promise<boolean> {
    return Swal.fire({
      icon: 'warning',
      title: 'Confirmación',
      text: message,
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      return result.isConfirmed; // Devuelve true si el usuario confirma
    });
  }
}
