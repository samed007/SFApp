# 🐾 SurferPets – Mini ERP  
**Gestión de Productos · Inventario · Ventas · Entradas · Devoluciones**


<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-blue" alt=".NET 8" />
  <img src="https://img.shields.io/badge/SQL%20Server-2019%2F2022-green" alt="SQL Server" />
  <img src="https://img.shields.io/badge/STATUS-Producción-Compatible-green" alt="Estado" />
  <img src="https://img.shields.io/badge/Licencia-Uso%20Interno-lightgrey" alt="Licencia" />
</p>

---

## 📌 Índice

- [Descripción](#-descripción)
- [Estado del proyecto](#-estado-del-proyecto)
- [Funcionalidades](#-funcionalidades-principales)
- [Tecnologías](#-tecnologías-utilizadas)
- [Instalación y configuración](#-instalación-y-configuración)
- [Uso básico](#-uso-básico)
- [Estructura del proyecto](#-estructura-del-proyecto)
- [Endpoints](#-endpoints--rutas-principales)
- [Base de datos y SPs](#-base-de-datos-y-sps)
- [Autor](#-autor)
- [Licencia](#-licencia)
- [Futuras mejoras](#-futuras-mejoras)
- [FAQ](#-faq)

---

## 📝 Descripción

**SurferPets** es un mini ERP desarrollado en **ASP.NET Core 8 MVC** con **SQL Server**, pensado para la gestión interna de un punto de venta.  

Permite administrar:

- Productos  
- Clientes  
- Inventario  
- Ventas  
- Entradas de mercancía  
- Devoluciones  
- Informes  

Ideal para negocios pequeños que necesiten control total del stock sin sistemas complejos.

---

## 🚧 Estado del proyecto

✔ **Versión funcional** (backend, frontend, base de datos y transacciones).  
⚠ En desarrollo: mejoras futuras como tickets, código de barras, dashboard, etc.

---

## 🔧 Funcionalidades principales

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

## 🛠 Tecnologías utilizadas

| Área        | Tecnologías |
|------------|-------------|
| Backend    | ASP.NET Core 8 MVC, C# |
| Base de datos | SQL Server 2019 / 2022 |
| Vistas     | Razor (.cshtml), Bootstrap |
| Herramientas | Visual Studio 2022, SSMS |
| Control de versiones | Git / GitHub |
| Documentación | Markdown |

---

## 📦 Instalación y configuración

### 1. Clonar repositorio

```bash
git clone https://github.com/samed007/SFApp.git
2. Crear base de datos
En SQL Server Management Studio (SSMS):

sql
Copiar código
CREATE DATABASE SurferPets;
GO
Luego ejecuta los scripts del proyecto:

Tablas

Vistas (vst_Inventario, vst_modInventario)

Tipos (TipoProductos)

Procedimientos almacenados

3. Configurar la conexión SQL
Editar appsettings.json:

json
Copiar código
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=SurferPets;Trusted_Connection=True;TrustServerCertificate=True;"
}
Ejemplos de servidores:

(localdb)\\MSSQLLocalDB

localhost\\SQLEXPRESS

192.168.1.100

4. Restaurar paquetes y ejecutar
En Visual Studio:

Tools → NuGet Package Manager → Restore Packages

Luego:

bash
Copiar código
dotnet run
O simplemente F5.

🎯 Uso básico
👨‍💼 Administrador
Crear/editar productos

Registrar entradas

👤 Usuario
Realizar ventas desde Cobro

Confirmar transacciones

Consultar informes

Autenticación usando usuarios creados en SQL Server.

🗂 Estructura del proyecto
SFApp/
│── Controllers/
│── Services/
│── DAO/
│── Models/
│── DTOs/
│── Views/
│── wwwroot/
└── appsettings.json
🔁 Endpoints / Rutas principales
👤 Usuario estándar
## Endpoints (Usuario estándar)

| Operación        | Método | Ruta                           | Descripción                 |
|------------------|--------|--------------------------------|-----------------------------|
| Login            | POST   | /Auth/Login                    | Iniciar sesión              |
| Home             | GET    | /Home/Index                    | Pantalla principal          |
| Vista de cobro   | GET    | /Cobro/Index                   | Selección de productos      |
| Confirmar venta  | POST   | /Cobro/ConfirmarTransaccion    | Registrar venta y actualizar stock |


🛠 Administrador
## Endpoints (Administrador)

| Operación         | Método | Ruta                         | Descripción                  |
|-------------------|--------|------------------------------|------------------------------|
| Listar productos  | GET    | /Producto/Index              | Ver listado de productos     |
| Crear producto    | POST   | /Producto/Crear              | Registrar nuevo producto     |
| Editar producto   | POST   | /Producto/Editar/{id}        | Editar producto existente    |
| Entrada de stock  | POST   | /Entrada/Registrar           | Registrar ingreso de stock   |
| Informes stock    | GET    | /Informe/Stock               | Consultas de stock e informes |


🗃 Base de datos y SPs
Tablas principales:

Productos

Clientes

Usuarios

Inventario

Transacciones

Procedimientos almacenados:

sp_RegistrarTransaccionVenta

sp_ActualizarTransaccion

sp_ObtenerStock

sp_VentasPorDia

👥 Autor
Tu Nombre
Desarrollador del sistema SurferPets.

📄 Licencia
Uso interno y educativo.
No se permite su distribución comercial.

✅ Futuras mejoras
Lectura de código de barras

Impresión de tickets

Módulo de fidelización

Dashboard gráfico

Gestión multi-tienda

🙋 FAQ
¿Se puede usar sin Visual Studio?
✔ Sí. Solo necesitas .NET 8 y SQL Server.

¿Qué pasa si cancelo una venta?
✔ El sistema ajusta el stock correctamente gracias a los SPs.

¿Está terminado?
✔ La versión base sí, pero se seguirá ampliando.
