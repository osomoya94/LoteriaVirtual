-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Version del servidor:        10.4.32-MariaDB
-- SO del servidor:             Win64
-- HeidiSQL Version:            12.14.0.7165
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

CREATE DATABASE IF NOT EXISTS `loteriavirtual` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `loteriavirtual`;

-- Tabla auxiliar que se mantiene porque `usuarios` depende de ella.
CREATE TABLE IF NOT EXISTS `roles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(50) NOT NULL,
  `Descripcion` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Nombre` (`Nombre`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Se mantiene sin cambios por pedido del usuario.
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

-- Se mantiene sin cambios por pedido del usuario.
CREATE TABLE IF NOT EXISTS `jugadores` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UsuarioId` int(11) NOT NULL,
  `Dni` varchar(15) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Saldo` decimal(10,2) DEFAULT 0.00,
  `Estado` varchar(20) DEFAULT 'Activo',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UsuarioId` (`UsuarioId`),
  UNIQUE KEY `Dni` (`Dni`),
  UNIQUE KEY `Email` (`Email`),
  CONSTRAINT `jugadores_ibfk_1` FOREIGN KEY (`UsuarioId`) REFERENCES `usuarios` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `sorteos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(120) NOT NULL,
  `fecha_sorteo` datetime NOT NULL,
  `estado` varchar(20) NOT NULL DEFAULT 'BORRADOR',
  `cantidad_cartones` int(11) NOT NULL,
  `precio_carton` decimal(10,2) NOT NULL,
  `porcentaje_premio` decimal(5,2) NOT NULL,
  `intervalo_extraccion_segundos` int(11) NOT NULL,
  `modo_extraccion` varchar(20) NOT NULL DEFAULT 'AUTOMATICA',
  `permite_multiples_ganadores` bit(1) NOT NULL DEFAULT b'0',
  `configuracion_premio` longtext DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_sorteos_estado_fecha` (`estado`, `fecha_sorteo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `cartones` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_sorteo` int(11) NOT NULL,
  `codigo_unico` varchar(60) NOT NULL,
  `patron_contenido` longtext NOT NULL,
  `hash_contenido` varchar(64) NOT NULL,
  `estado` varchar(20) NOT NULL DEFAULT 'DISPONIBLE',
  `fecha_generacion` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_cartones_sorteo_codigo` (`id_sorteo`, `codigo_unico`),
  KEY `idx_cartones_sorteo_estado` (`id_sorteo`, `estado`),
  KEY `idx_cartones_hash` (`hash_contenido`),
  CONSTRAINT `cartones_ibfk_1` FOREIGN KEY (`id_sorteo`) REFERENCES `sorteos` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `asignaciones_carton` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_carton` int(11) NOT NULL,
  `id_jugador` int(11) NOT NULL,
  `fecha_asignacion` datetime NOT NULL DEFAULT current_timestamp(),
  `fecha_vencimiento_reserva` datetime DEFAULT NULL,
  `estado` varchar(20) NOT NULL DEFAULT 'RESERVADO',
  `creado_por` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_asignaciones_carton` (`id_carton`),
  KEY `idx_asignaciones_jugador` (`id_jugador`),
  KEY `idx_asignaciones_estado` (`estado`),
  KEY `idx_asignaciones_creado_por` (`creado_por`),
  CONSTRAINT `asignaciones_carton_ibfk_1` FOREIGN KEY (`id_carton`) REFERENCES `cartones` (`id`),
  CONSTRAINT `asignaciones_carton_ibfk_2` FOREIGN KEY (`id_jugador`) REFERENCES `jugadores` (`Id`),
  CONSTRAINT `asignaciones_carton_ibfk_3` FOREIGN KEY (`creado_por`) REFERENCES `usuarios` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `ventas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_asignacion` int(11) NOT NULL,
  `monto` decimal(10,2) NOT NULL,
  `estado` varchar(20) NOT NULL DEFAULT 'PENDIENTE',
  `metodo_pago` varchar(30) DEFAULT NULL,
  `fecha_venta` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `idx_ventas_asignacion` (`id_asignacion`),
  KEY `idx_ventas_estado` (`estado`),
  CONSTRAINT `ventas_ibfk_1` FOREIGN KEY (`id_asignacion`) REFERENCES `asignaciones_carton` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `pagos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_venta` int(11) NOT NULL,
  `estado_verificacion` varchar(20) NOT NULL DEFAULT 'PENDIENTE',
  `comprobante` varchar(255) DEFAULT NULL,
  `fecha_verificacion` datetime DEFAULT NULL,
  `verificado_por` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_pagos_venta` (`id_venta`),
  KEY `idx_pagos_estado` (`estado_verificacion`),
  KEY `idx_pagos_verificado_por` (`verificado_por`),
  CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`id_venta`) REFERENCES `ventas` (`id`),
  CONSTRAINT `pagos_ibfk_2` FOREIGN KEY (`verificado_por`) REFERENCES `usuarios` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `extracciones` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_sorteo` int(11) NOT NULL,
  `numero_extraido` int(11) NOT NULL,
  `orden` int(11) NOT NULL,
  `fecha_hora` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_extracciones_sorteo_orden` (`id_sorteo`, `orden`),
  UNIQUE KEY `uk_extracciones_sorteo_numero` (`id_sorteo`, `numero_extraido`),
  CONSTRAINT `extracciones_ibfk_1` FOREIGN KEY (`id_sorteo`) REFERENCES `sorteos` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `premios` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_sorteo` int(11) NOT NULL,
  `tipo` varchar(30) NOT NULL,
  `monto_o_descripcion` varchar(120) NOT NULL,
  `monto_calculado` decimal(10,2) NOT NULL,
  `estado` varchar(20) NOT NULL DEFAULT 'PENDIENTE',
  PRIMARY KEY (`id`),
  KEY `idx_premios_sorteo` (`id_sorteo`),
  KEY `idx_premios_estado` (`estado`),
  CONSTRAINT `premios_ibfk_1` FOREIGN KEY (`id_sorteo`) REFERENCES `sorteos` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `ganadores` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_sorteo` int(11) NOT NULL,
  `id_jugador` int(11) NOT NULL,
  `id_carton` int(11) NOT NULL,
  `id_asignacion` int(11) NOT NULL,
  `id_premio` int(11) NOT NULL,
  `criterio_ganador` varchar(80) NOT NULL,
  `confirmado` bit(1) NOT NULL DEFAULT b'0',
  `numero_extraccion_detectado` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_ganadores_sorteo` (`id_sorteo`),
  KEY `idx_ganadores_jugador` (`id_jugador`),
  KEY `idx_ganadores_carton` (`id_carton`),
  KEY `idx_ganadores_asignacion` (`id_asignacion`),
  KEY `idx_ganadores_premio` (`id_premio`),
  CONSTRAINT `ganadores_ibfk_1` FOREIGN KEY (`id_sorteo`) REFERENCES `sorteos` (`id`),
  CONSTRAINT `ganadores_ibfk_2` FOREIGN KEY (`id_jugador`) REFERENCES `jugadores` (`Id`),
  CONSTRAINT `ganadores_ibfk_3` FOREIGN KEY (`id_carton`) REFERENCES `cartones` (`id`),
  CONSTRAINT `ganadores_ibfk_4` FOREIGN KEY (`id_asignacion`) REFERENCES `asignaciones_carton` (`id`),
  CONSTRAINT `ganadores_ibfk_5` FOREIGN KEY (`id_premio`) REFERENCES `premios` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS `auditoria` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_usuario` int(11) DEFAULT NULL,
  `accion` varchar(60) NOT NULL,
  `entidad` varchar(40) NOT NULL,
  `id_entidad` int(11) DEFAULT NULL,
  `fecha` datetime NOT NULL DEFAULT current_timestamp(),
  `detalle` longtext DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_auditoria_usuario` (`id_usuario`),
  KEY `idx_auditoria_entidad` (`entidad`, `id_entidad`),
  KEY `idx_auditoria_fecha` (`fecha`),
  CONSTRAINT `auditoria_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
