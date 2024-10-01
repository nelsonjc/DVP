import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ModalNameEnum } from '../../../core/enums/modal-name.enum';

@Injectable({
  providedIn: 'root'
})

export class ModalService {
  private modalStates: { [key: string]: BehaviorSubject<boolean> } = {};
  private id = new BehaviorSubject<string | null>(null);

  constructor() {
    // Inicializa el estado de cada modal
    this.modalStates[ModalNameEnum.TaskForm] = new BehaviorSubject<boolean>(false);
    this.modalStates[ModalNameEnum.TaskDetail] = new BehaviorSubject<boolean>(false);
    this.modalStates[ModalNameEnum.TaskChangeStatus] = new BehaviorSubject<boolean>(false);
  }

  openModal(modalName: string, id?: string) {
    if (id) {
      this.id.next(id); // Almacena el ID de la actividad      
    }
    else{
      this.id.next(null); // Reinicia el ID de la actividad si no se proporciona
    }

    this.modalStates[modalName]?.next(true); // Abre el modal
  }

  closeModal(modalName: string) {
    this.modalStates[modalName]?.next(false); // Cierra el modal
  }

  isModalOpen(modalName: string) {
    return this.modalStates[modalName]?.asObservable();
  }

  getIdFromModal() {
    return this.id.asObservable();
  }
}