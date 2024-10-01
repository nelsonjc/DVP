# DVP
Prueba Técnia DVP

# Introducción a la Prueba del Sistema de Asignación de Tareas

En el contexto actual, donde la gestión eficiente de las tareas y la colaboración son fundamentales para el éxito de cualquier organización, la empresa DVP ha decidido implementar un sistema robusto que permita asignar y gestionar tareas de manera efectiva. Este sistema, diseñado con una arquitectura limpia, combina la potencia de .NET en el back-end y la versatilidad de Angular en el front-end, creando una solución integral y moderna.

## Objetivo

El objetivo de este proyecto es desarrollar un sistema de asignación de tareas que no solo garantice la autenticación de usuarios, sino que también implemente un sistema de roles que delimite las funcionalidades disponibles para cada tipo de usuario, como Administrador, Supervisor y Empleado. Esto asegurará que cada rol tenga acceso a las herramientas y datos necesarios para desempeñar sus funciones de manera eficiente.

## Requerimientos Técnicos

1. **Back-end (.NET)**:
   - Creé una REST API utilizando ASP.NET Core.
   - Arquitectura limpia con CQRS
   - Automapeer
   - Inyección de dependencias
   - Serilog
   - Implementé Entity Framework Core para la interacción con la base de datos.
   - Incorporé autenticación con JWT.
   - Definí las entidades Usuario, Rol y Tarea, con las validaciones necesarias.
     

2. **Front-end (Angular)**:
   - Desarrollé una aplicación en Angular.
   - Implementé el inicio de sesión y el registro de usuarios.
   - Creé vistas para la gestión de usuarios y tareas, adaptadas según el rol del usuario.
   - Utilicé Angular Material para enriquecer la interfaz de usuario (opcional).
   - Consumí la API del back-end para las operaciones CRUD.

## Descripción del Proyecto

La arquitectura del sistema se basa en CQRS (Command Query Responsibility Segregation) utilizando Entity Framework para manejar la interacción con la base de datos. Esto, en combinación con el patrón Repository y la inyección de dependencia, permite una separación clara de responsabilidades, lo que facilita la mantenibilidad y escalabilidad del sistema. Además, he aplicado los principios SOLID para asegurar un código limpio y fácil de entender.

En el lado del back-end, he implementado una REST API en ASP.NET Core que utiliza JWT (JSON Web Token) para gestionar la autenticación de usuarios. Esta API proporciona funcionalidades completas para la gestión de tareas y usuarios, incluyendo operaciones CRUD (Crear, Leer, Actualizar, Eliminar) y paginación para mejorar la experiencia del usuario al trabajar con grandes volúmenes de datos. Las pruebas unitarias se han llevado a cabo utilizando NUnit, garantizando que cada componente del sistema funcione como se espera.

Por otro lado, en el front-end, he desarrollado una aplicación en Angular que utiliza Tailwind CSS para crear una interfaz de usuario atractiva y receptiva. La aplicación cuenta con un sistema de inicio de sesión y registro de usuarios, junto con vistas para gestionar tanto usuarios como tareas, adaptándose a las capacidades específicas de cada rol. He incorporado funcionalidades de registro de logs y un flujo claro de estados para cada tarea, lo que permite un seguimiento eficaz del progreso.

## Requerimientos Funcionales

1. **Autenticación de Usuarios**: Implementé el registro y el inicio de sesión de usuarios, utilizando JWT para la autenticación segura.
2. **Sistema de Roles**: Diseñé roles diferenciados:
   - **Administrador**: Capacidad de crear, editar y eliminar usuarios y tareas.
   - **Supervisor**: Posibilidad de asignar tareas a los empleados y cambiar su estado.
   - **Empleado**: Visualización de tareas asignadas y actualización de su estado.
3. **Gestión de Tareas**: Se implementó el CRUD de tareas, permitiendo su asignación a usuarios y la actualización de su estado (Pendiente, En Proceso, Completada).
4. **Gestión de Usuarios**: Incluye el CRUD de usuarios y la asignación de roles.

En conclusión, este proyecto no solo representa un avance significativo en la gestión de tareas dentro de DVP, sino que también sirve como un ejemplo de cómo integrar tecnologías modernas en un sistema coherente y funcional.
