# 🎰 LoteríaVirtual

> 🚧 **Proyecto en desarrollo**

LoteríaVirtual es una aplicación de gestión y participación en sorteos que nació como proyecto integrador de la carrera de Desarrollo de Software y que actualmente continúa evolucionando como **proyecto personal**.

El objetivo es transformar progresivamente el prototipo académico original en una aplicación más completa, segura y mantenible, con una arquitectura preparada para seguir creciendo y evaluar a futuro un posible uso comercial.

## 👨‍💻 Mi participación

En la etapa académica asumí el rol de **Tech Lead** y participé principalmente en decisiones de arquitectura, backend e integración entre los distintos componentes del sistema.

Luego de finalizar esa instancia decidí continuar el proyecto por cuenta propia, trabajando en mejoras de seguridad, estructura, experiencia de uso y preparación para futuras etapas.

> El proyecto original fue desarrollado en equipo. Este repositorio conserva trabajo realizado durante esa etapa y continúa siendo evolucionado como proyecto personal.

## ✨ Funcionalidades actuales

### Usuarios y seguridad
- Registro e inicio de sesión de jugadores
- Autenticación mediante JWT
- Contraseñas protegidas con BCrypt
- Roles diferenciados para **Administrador** y **Jugador**
- Endpoints protegidos según rol

### Sorteos
- Creación y gestión de sorteos
- Apertura de sorteos y generación de cartones
- Ejecución del sorteo desde el rol administrador
- Consulta de resultados
- Cancelación de sorteos

### Cartones y jugadas
- Generación de cartones
- Reserva y compra de cartones
- Aprobación de pagos
- Cancelación de reservas
- Consulta de jugadas por jugador

### Interfaces
- Aplicación web para jugadores
- Registro y login
- Visualización de sorteos
- Seguimiento de jugadas
- Vista de sorteo en vivo
- Aplicación de escritorio WinForms orientada a la administración

## 🏗️ Arquitectura

La solución está separada en proyectos con responsabilidades distintas:

```text
LoteriaVirtual
├── Loteria.ApiWeb       → API REST + interfaz web
├── Loteria.Negocio      → lógica de negocio y servicios
├── Loteria.Datos        → acceso a datos y repositorios
├── Loteria.Entidades    → entidades y DTOs
├── Loteria.Escritorio   → aplicación administrativa WinForms
├── Scripts              → scripts de base de datos
└── Docker / Compose     → entorno local
```

La capa de datos utiliza **Dapper** y el patrón Repository para trabajar con MySQL.

## 🛠️ Tecnologías

- **Lenguaje:** C#
- **Backend:** .NET / ASP.NET Core
- **Base de datos:** MySQL
- **Acceso a datos:** Dapper
- **Seguridad:** JWT · BCrypt
- **Documentación de API:** Swagger / OpenAPI
- **Escritorio:** Windows Forms
- **Contenedores:** Docker · Docker Compose
- **Control de versiones:** Git · GitHub

## 🔐 Configuración y seguridad

Las credenciales reales no deben almacenarse en el repositorio.

El proyecto utiliza variables de entorno para la configuración sensible. Como referencia se incluye:

```text
.env.example
```

Para desarrollo local:

```bash
cp .env.example .env
```

Luego reemplazá los valores de ejemplo por valores locales seguros.

Variables principales:

```env
MYSQL_DATABASE=loteriavirtual
MYSQL_ROOT_PASSWORD=...

JWT_KEY=...
JWT_ISSUER=LoteriaVirtualAPI
JWT_AUDIENCE=UsuariosDeLoteria

BOOTSTRAP_ADMIN_USERNAME=...
BOOTSTRAP_ADMIN_PASSWORD=...
```

`BOOTSTRAP_ADMIN_USERNAME` y `BOOTSTRAP_ADMIN_PASSWORD` son opcionales y se utilizan únicamente para crear el primer administrador en un entorno nuevo.

> ⚠️ Nunca reutilices claves o contraseñas que hayan sido publicadas previamente en el historial del repositorio.

## 🚀 Ejecución con Docker

Con Docker y Docker Compose instalados:

```bash
docker compose up --build
```

La API queda disponible localmente en:

```text
http://localhost:8080
```

La base de datos MySQL se expone localmente por el puerto `3307`.

## 📚 API

Durante el entorno de desarrollo, Swagger/OpenAPI permite explorar y probar los endpoints de la API.

La aplicación incluye operaciones relacionadas con:

- usuarios
- jugadores
- sorteos
- cartones
- jugadas
- autenticación y autorización

## 🗺️ Roadmap

El proyecto continúa en evolución. Algunas mejoras previstas son:

- [x] Separación en capas
- [x] Autenticación JWT y roles
- [x] Persistencia con MySQL y Dapper
- [x] Interfaz web para jugadores
- [x] Aplicación administrativa WinForms
- [x] Dockerización del entorno
- [x] Eliminación de credenciales sensibles del código actual
- [ ] Refactorizar `Program.cs` y separar endpoints por módulos
- [ ] Ampliar cobertura de tests automatizados
- [ ] Mejorar manejo global de errores y respuestas HTTP
- [ ] Mejorar diseño y experiencia de usuario
- [ ] Preparar despliegue productivo
- [ ] Revisar seguridad y reglas de negocio antes de un uso real
- [ ] Definir infraestructura y estrategia de observabilidad

## 📌 Estado del proyecto

El sistema tiene funcionalidades operativas y permite demostrar el flujo principal de usuarios, sorteos y cartones, pero **todavía no debe considerarse un producto listo para producción**.

Actualmente se encuentra en una etapa de refactorización y evolución desde un proyecto académico hacia una aplicación personal más profesional.

## 👤 Autor / continuidad del proyecto

**Emanuel Moya**  
Full Stack Developer · Backend con Node.js, NestJS, TypeScript y experiencia también con C#/.NET

- GitHub: https://github.com/osomoya94
- LinkedIn: https://www.linkedin.com/in/emanuel-moya-desarrolladorfullstack/
- Portfolio: https://emanuel-moya.vercel.app/
