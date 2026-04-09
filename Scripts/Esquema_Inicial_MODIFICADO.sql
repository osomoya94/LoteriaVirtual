-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         10.4.32-MariaDB - mariadb.org binary distribution
-- SO del servidor:              Win64
-- HeidiSQL Versión:             12.14.0.7165
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Volcando estructura de base de datos para loteriavirtual
CREATE DATABASE IF NOT EXISTS `loteriavirtual` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `loteriavirtual`;

-- Volcando estructura para tabla loteriavirtual.auditoria
CREATE TABLE IF NOT EXISTS `auditoria` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Fecha` datetime DEFAULT current_timestamp(),
  `Accion` varchar(100) NOT NULL,
  `Detalles` text DEFAULT NULL,
  `UsuarioId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `UsuarioId` (`UsuarioId`),
  CONSTRAINT `auditoria_ibfk_1` FOREIGN KEY (`UsuarioId`) REFERENCES `usuarios` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla loteriavirtual.cartones
CREATE TABLE IF NOT EXISTS `cartones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SorteoId` int(11) NOT NULL,
  `JugadorId` int(11) DEFAULT NULL,
  `NumerosCarton` text NOT NULL,
  `Estado` varchar(20) DEFAULT 'Disponible',
  `Precio` decimal(10,2) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `SorteoId` (`SorteoId`),
  KEY `JugadorId` (`JugadorId`),
  CONSTRAINT `cartones_ibfk_1` FOREIGN KEY (`SorteoId`) REFERENCES `sorteos` (`Id`),
  CONSTRAINT `cartones_ibfk_2` FOREIGN KEY (`JugadorId`) REFERENCES `jugadores` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla loteriavirtual.configuraciones
CREATE TABLE IF NOT EXISTS `configuraciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Clave` varchar(50) NOT NULL,
  `Valor` varchar(100) NOT NULL,
  `Descripcion` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Clave` (`Clave`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla loteriavirtual.jugadores
CREATE TABLE IF NOT EXISTS `jugadores` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UsuarioId` int(11) NOT NULL,
  `Dni` varchar(15) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Saldo` decimal(10,2) DEFAULT 0.00,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UsuarioId` (`UsuarioId`),
  UNIQUE KEY `Dni` (`Dni`),
  UNIQUE KEY `Email` (`Email`),
  CONSTRAINT `jugadores_ibfk_1` FOREIGN KEY (`UsuarioId`) REFERENCES `usuarios` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla loteriavirtual.premios
CREATE TABLE IF NOT EXISTS `premios` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SorteoId` int(11) NOT NULL,
  `CartonGanadorId` int(11) NOT NULL,
  `JugadorId` int(11) NOT NULL,
  `MontoGanado` decimal(10,2) NOT NULL,
  `FechaPago` datetime DEFAULT NULL,
  `Estado` varchar(20) DEFAULT 'PendientePago',
  PRIMARY KEY (`Id`),
  KEY `SorteoId` (`SorteoId`),
  KEY `CartonGanadorId` (`CartonGanadorId`),
  KEY `JugadorId` (`JugadorId`),
  CONSTRAINT `premios_ibfk_1` FOREIGN KEY (`SorteoId`) REFERENCES `sorteos` (`Id`),
  CONSTRAINT `premios_ibfk_2` FOREIGN KEY (`CartonGanadorId`) REFERENCES `cartones` (`Id`),
  CONSTRAINT `premios_ibfk_3` FOREIGN KEY (`JugadorId`) REFERENCES `jugadores` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla loteriavirtual.roles
CREATE TABLE IF NOT EXISTS `roles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(50) NOT NULL,
  `Descripcion` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Nombre` (`Nombre`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla loteriavirtual.sorteos
CREATE TABLE IF NOT EXISTS `sorteos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FechaProgramada` datetime NOT NULL,
  `Estado` varchar(20) DEFAULT 'Pendiente',
  `NumerosSorteados` text DEFAULT NULL,
  `PozoAcumulado` decimal(10,2) DEFAULT 0.00,
  `UsuarioCreadorId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `UsuarioCreadorId` (`UsuarioCreadorId`),
  CONSTRAINT `sorteos_ibfk_1` FOREIGN KEY (`UsuarioCreadorId`) REFERENCES `usuarios` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla loteriavirtual.transacciones
CREATE TABLE IF NOT EXISTS `transacciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `JugadorId` int(11) NOT NULL,
  `TipoTransaccion` varchar(20) NOT NULL,
  `Monto` decimal(10,2) NOT NULL,
  `Fecha` datetime DEFAULT current_timestamp(),
  `ReferenciaId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `JugadorId` (`JugadorId`),
  CONSTRAINT `transacciones_ibfk_1` FOREIGN KEY (`JugadorId`) REFERENCES `jugadores` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla loteriavirtual.usuarios
CREATE TABLE IF NOT EXISTS `usuarios` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RolId` int(11) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Activo` tinyint(1) DEFAULT 1,
  `FechaCreacion` datetime DEFAULT current_timestamp(),
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Username` (`Username`),
  KEY `RolId` (`RolId`),
  CONSTRAINT `usuarios_ibfk_1` FOREIGN KEY (`RolId`) REFERENCES `roles` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- La exportación de datos fue deseleccionada.

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
