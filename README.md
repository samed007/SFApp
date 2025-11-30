# SurferPets – Mini ERP  
Gestión de Productos · Inventario · Ventas · Entradas · Devoluciones

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-blue" alt=".NET 8" />
  <img src="https://img.shields.io/badge/SQL%20Server-2019%2F2022-green" alt="SQL Server" />
  <img src="https://img.shields.io/badge/STATUS-Producción-Compatible-green" alt="Estado" />
  <img src="https://img.shields.io/badge/Licencia-Uso%20Interno-lightgrey" alt="Licencia" />
</p>

---

## Índice

- [Descripción](#descripción)
- [Estado del proyecto](#estado-del-proyecto)
- [Funcionalidades principales](#funcionalidades-principales)
- [Tecnologías utilizadas](#tecnologías-utilizadas)
- [Instalación y configuración](#instalación-y-configuración)
- [Importar base de datos (.bak)](#importar-base-de-datos-bak)
- [Credenciales](#credenciales)
- [Uso básico](#uso-básico)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Endpoints - Rutas principales](#endpoints---rutas-principales)
- [Base de datos y SPs](#base-de-datos-y-sps)
- [Autor](#autor)
- [Licencia](#licencia)
- [Futuras mejoras](#futuras-mejoras)
- [FAQ](#faq)

---

## Descripción

SurferPets es un mini ERP desarrollado en ASP.NET Core 8 MVC con SQL Server, pensado para la gestión interna de un punto de venta.

Incluye módulos de:

- Productos  
- Clientes  
- Inventario  
- Ventas  
- Entradas de mercancía  
- Devoluciones  
- Informes  

---

## Estado del proyecto

Versión funcional (backend, frontend, base de datos y transacciones).  
En desarrollo mejoras como tickets, código de barras, dashboard, etc.

---

## Funcionalidades principales

- Gestión de productos (crear, editar, activar/inactivar)
- Gestión de clientes
- Registro de ventas (cobros)
- Registro de entradas
- Registro de devoluciones
- Control de stock automático
- Informes y consultas
- Autenticación de usuarios
- Arquitectura MVC con Servicios + DAO + SPs

---

## Tecnologías utilizadas

| Área        | Tecnologías |
|------------|-------------|
| Backend    | ASP.NET Core 8 MVC, C# |
| Base de datos | SQL Server 2019 / 2022 |
| Vistas     | Razor (.cshtml), Bootstrap |
| Herramientas | Visual Studio 2022, SSMS |
| Control de versiones | Git / GitHub |
| Documentación | Markdown |

---

## Instalación y configuración

### 1. Clonar repositorio

```bash
git clone https://github.com/samed007/SFApp.git
```

---

## Importar base de datos (.bak)

Si ya tienes el archivo `SurferPets.bak`, no necesitas crear la base de datos manualmente.

### 1. Abrir SQL Server Management Studio (SSMS)
### 2. Clic derecho en **Databases**
### 3. Seleccionar **Restore Database**
### 4. Elegir **Device** → agregar el archivo `.bak`
### 5. Seleccionar el archivo `SurferPets.bak`
### 6. Aceptar para restaurar

La base de datos queda lista con tablas, vistas, tipos y SPs preinstalados.

---

## Credenciales

### Administrador
Usuario: admin  
Clave: 123456  

### Usuario estándar  
Usuario: user  
Clave: 123456  

---

## Uso básico

### Administrador
- Crear y editar productos  
- Registrar entradas  

### Usuario
- Realizar ventas desde Cobro  
- Confirmar transacciones  
- Consultar informes  

---

## Estructura del proyecto

```
SFApp/
│── Controllers/
│── Services/
│── DAO/
│── Models/
│── DTOs/
│── Views/
│── wwwroot/
└── appsettings.json
```

---

## Endpoints - Rutas principales

### Usuario estándar

| Operación        | Método | Ruta                           | Descripción                 |
|------------------|--------|--------------------------------|-----------------------------|
| Login            | POST   | /Auth/Login                    | Iniciar sesión              |
| Home             | GET    | /Home/Index                    | Pantalla principal          |
| Vista de cobro   | GET    | /Cobro/Index                   | Selección de productos      |
| Confirmar venta  | POST   | /Cobro/ConfirmarTransaccion    | Registrar venta y actualizar stock |

### Administrador

| Operación         | Método | Ruta                         | Descripción                  |
|-------------------|--------|------------------------------|------------------------------|
| Listar productos  | GET    | /Producto/Index              | Ver listado de productos     |
| Crear producto    | POST   | /Producto/Crear              | Registrar nuevo producto     |
| Editar producto   | POST   | /Producto/Editar/{id}        | Editar producto existente    |
| Entrada de stock  | POST   | /Entrada/Registrar           | Registrar ingreso de stock   |
| Informes stock    | GET    | /Informe/Stock               | Consultar informes de stock  |

---

## Base de datos y SPs

Tablas principales:

- Productos  
- Clientes  
- Usuarios  
- Inventario  
- Transacciones  

Procedimientos almacenados:

- sp_RegistrarTransaccionVenta  
- sp_ActualizarTransaccion  
- sp_ObtenerStock  
- sp_VentasPorDia  

---

## Autor

Christian Edgardo Perez Vera  
Desarrollador del sistema SurferPets.

---

## Licencia

Uso interno y educativo.  
No se permite distribución comercial.

---

## Futuras mejoras

- Lectura de código de barras  
- Impresión de tickets  
- Sistema de fidelización  
- Dashboard gráfico  
- Gestión multi-tienda  

---

## FAQ

¿Se puede usar sin Visual Studio?  
Sí, solo necesitas .NET 8 y SQL Server.

¿Qué pasa si cancelo una venta?  
El sistema ajusta el stock automáticamente mediante SPs.

¿Está terminado?  
La versión base sí, pero se seguirán implementando mejoras.
