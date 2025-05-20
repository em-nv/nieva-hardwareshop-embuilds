CREATE DATABASE  IF NOT EXISTS `hardwareshopdb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `hardwareshopdb`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: hardwareshopdb
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (21,'Plumbing','','2025-05-09 09:43:03','2025-05-09 13:12:30'),(22,'Paint','','2025-05-09 09:43:11','2025-05-09 13:08:14'),(23,'Adhesive',NULL,'2025-05-09 09:43:24','2025-05-09 09:43:24'),(24,'Gardening',NULL,'2025-05-09 09:43:36','2025-05-09 09:43:36'),(25,'Power & Hand Tools',NULL,'2025-05-09 09:44:00','2025-05-09 09:44:00'),(26,'Locks & Safety',NULL,'2025-05-09 09:44:21','2025-05-09 09:44:21'),(27,'Hammer','','2025-05-09 09:44:29','2025-05-09 13:08:33');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `category_products_count`
--

DROP TABLE IF EXISTS `category_products_count`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `category_products_count` (
  `category_id` int NOT NULL DEFAULT '0',
  `category` varchar(50) NOT NULL,
  `products_count` bigint NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `category_products_count`
--

LOCK TABLES `category_products_count` WRITE;
/*!40000 ALTER TABLE `category_products_count` DISABLE KEYS */;
INSERT INTO `category_products_count` VALUES (1,'Power Tools',3),(2,'Hand Tools',2),(3,'Fasteners',4),(4,'Plumbing',2),(5,'Electrical',1),(6,'Paint & Adhesives',1),(7,'Safety Equipment',1),(8,'Gardening Tools',5),(9,'Woodworking',1),(10,'Measuring Tools',1),(16,'Category A',2),(17,'Category B',0);
/*!40000 ALTER TABLE `category_products_count` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `customers`
--

DROP TABLE IF EXISTS `customers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customers` (
  `id` int NOT NULL AUTO_INCREMENT,
  `first_name` varchar(50) NOT NULL,
  `middle_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) NOT NULL,
  `phone_number` varchar(50) DEFAULT NULL,
  `address` varchar(255) NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `phone_number` (`phone_number`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customers`
--

LOCK TABLES `customers` WRITE;
/*!40000 ALTER TABLE `customers` DISABLE KEYS */;
INSERT INTO `customers` VALUES (15,'Joshua',NULL,'Non','4772','fdf','2025-05-03 16:18:25','2025-05-09 13:41:38'),(16,'Kyla',NULL,'a','09123456789','a','2025-05-09 13:28:18','2025-05-09 15:35:37'),(17,'h','','h','09123457789','l','2025-05-19 06:15:50','2025-05-19 06:16:16');
/*!40000 ALTER TABLE `customers` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `before_delete_customer` BEFORE DELETE ON `customers` FOR EACH ROW BEGIN  
    DECLARE transaction_count INT;  
    
    -- Count transactions related to the customer  
    SELECT COUNT(*) INTO transaction_count FROM sales WHERE customer_id = OLD.id;  
    
    -- Prevent deletion if transactions exist  
    IF transaction_count > 0 THEN  
        SIGNAL SQLSTATE '45000'  
        SET MESSAGE_TEXT = 'Cannot delete customer with existing transactions';  
    END IF;  
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `after_delete_customer` AFTER DELETE ON `customers` FOR EACH ROW BEGIN
    INSERT INTO deleted_customers (customer_id, first_name, middle_name, last_name, phone_number, address)
    VALUES (OLD.id, OLD.first_name, OLD.middle_name, OLD.last_name, OLD.phone_number, OLD.address);
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `deleted_customers`
--

DROP TABLE IF EXISTS `deleted_customers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `deleted_customers` (
  `id` int NOT NULL AUTO_INCREMENT,
  `customer_id` int DEFAULT NULL,
  `first_name` varchar(50) DEFAULT NULL,
  `middle_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) DEFAULT NULL,
  `phone_number` varchar(50) DEFAULT NULL,
  `address` varchar(255) DEFAULT NULL,
  `deleted_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `deleted_customers`
--

LOCK TABLES `deleted_customers` WRITE;
/*!40000 ALTER TABLE `deleted_customers` DISABLE KEYS */;
/*!40000 ALTER TABLE `deleted_customers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventories`
--

DROP TABLE IF EXISTS `inventories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventories` (
  `id` int NOT NULL AUTO_INCREMENT,
  `product_id` int NOT NULL,
  `stock_available` int unsigned DEFAULT NULL,
  `stock_status` enum('Out of Stock','Low Stock','Medium Stock','High Stock') DEFAULT 'Low Stock',
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `product_id` (`product_id`),
  CONSTRAINT `inventories_ibfk_1` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=80 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventories`
--

LOCK TABLES `inventories` WRITE;
/*!40000 ALTER TABLE `inventories` DISABLE KEYS */;
INSERT INTO `inventories` VALUES (37,63,50,'Medium Stock','2025-05-09 17:51:09','2025-05-19 13:59:18'),(38,64,200,'High Stock','2025-05-09 19:05:41','2025-05-19 14:00:48'),(39,65,500,'High Stock','2025-05-16 17:36:57','2025-05-19 14:01:18'),(40,66,15,'Medium Stock','2025-05-16 17:38:06','2025-05-19 13:57:26'),(41,67,24,'Medium Stock','2025-05-16 17:39:47','2025-05-16 17:39:48'),(42,68,11,'Medium Stock','2025-05-16 17:40:40','2025-05-19 13:55:32'),(43,69,15,'Medium Stock','2025-05-16 17:41:40','2025-05-16 17:41:48'),(44,70,20,'Medium Stock','2025-05-16 17:42:26','2025-05-16 17:42:28'),(45,71,13,'Medium Stock','2025-05-16 17:45:11','2025-05-16 17:45:18'),(46,72,20,'Medium Stock','2025-05-16 17:46:07','2025-05-16 17:46:27'),(47,73,20,'Medium Stock','2025-05-16 17:47:05','2025-05-16 17:47:08'),(48,74,12,'Medium Stock','2025-05-16 17:48:14','2025-05-16 17:48:18'),(49,75,10,'Low Stock','2025-05-16 17:49:07','2025-05-16 17:49:07'),(50,76,12,'Medium Stock','2025-05-16 17:49:54','2025-05-16 17:49:58'),(51,77,20,'Medium Stock','2025-05-16 17:50:39','2025-05-16 17:50:48'),(52,78,10,'Low Stock','2025-05-16 17:51:28','2025-05-16 17:51:28'),(53,79,20,'Medium Stock','2025-05-16 17:52:51','2025-05-16 17:52:58'),(54,80,20,'Medium Stock','2025-05-16 17:53:55','2025-05-16 17:53:58'),(55,81,20,'Medium Stock','2025-05-16 17:54:46','2025-05-16 17:54:48'),(56,82,12,'Medium Stock','2025-05-16 17:55:20','2025-05-16 17:55:28'),(57,83,22,'Medium Stock','2025-05-16 17:55:58','2025-05-16 17:55:58'),(58,84,20,'Medium Stock','2025-05-16 17:56:42','2025-05-16 17:56:48'),(59,85,20,'Medium Stock','2025-05-16 17:57:40','2025-05-16 17:57:48'),(60,86,20,'Medium Stock','2025-05-16 17:58:43','2025-05-16 17:58:48'),(61,87,23,'Medium Stock','2025-05-16 18:00:42','2025-05-16 18:00:48'),(62,88,25,'Medium Stock','2025-05-16 18:01:08','2025-05-16 18:01:18'),(63,89,15,'Medium Stock','2025-05-16 18:01:33','2025-05-16 18:01:38'),(64,90,15,'Medium Stock','2025-05-16 18:02:05','2025-05-16 18:02:08'),(65,91,20,'Medium Stock','2025-05-16 18:02:28','2025-05-16 18:02:28'),(66,92,5,'Low Stock','2025-05-16 18:02:55','2025-05-16 18:02:55'),(67,93,20,'Medium Stock','2025-05-16 18:03:13','2025-05-16 18:03:18'),(68,94,20,'Medium Stock','2025-05-16 18:03:35','2025-05-16 18:03:38'),(69,95,20,'Medium Stock','2025-05-16 18:03:59','2025-05-16 18:04:08'),(70,96,20,'Medium Stock','2025-05-16 18:04:29','2025-05-16 18:04:38'),(71,97,25,'Medium Stock','2025-05-16 18:04:52','2025-05-16 18:04:58'),(72,98,21,'Medium Stock','2025-05-16 18:05:10','2025-05-16 18:05:18'),(73,99,26,'Medium Stock','2025-05-16 18:05:40','2025-05-16 18:05:48'),(74,100,25,'Medium Stock','2025-05-16 18:05:59','2025-05-16 18:06:08'),(75,101,30,'Medium Stock','2025-05-16 18:06:27','2025-05-16 18:06:28'),(76,102,20,'Medium Stock','2025-05-16 18:06:42','2025-05-16 18:06:48'),(77,103,20,'Medium Stock','2025-05-16 18:06:59','2025-05-16 18:07:08'),(78,104,20,'Medium Stock','2025-05-16 18:07:14','2025-05-16 18:07:18'),(79,105,30,'Medium Stock','2025-05-16 18:07:29','2025-05-16 18:07:38');
/*!40000 ALTER TABLE `inventories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product_sales_and_count`
--

DROP TABLE IF EXISTS `product_sales_and_count`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product_sales_and_count` (
  `product_id` int NOT NULL DEFAULT '0',
  `product_name` varchar(50) NOT NULL,
  `total_quantity_sold` int DEFAULT NULL,
  `total_sales` double DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product_sales_and_count`
--

LOCK TABLES `product_sales_and_count` WRITE;
/*!40000 ALTER TABLE `product_sales_and_count` DISABLE KEYS */;
INSERT INTO `product_sales_and_count` VALUES (1,'Cordless Drill',6,9000),(2,'Hammer',4,720),(3,'Screws (Pack of 100)',5,1250),(4,'Pipe Wrench',2,400),(5,'Insulated Wire Cutter',3,105),(6,'Paint Brush Set',5,1250),(7,'Safety Gloves',4,485),(8,'Garden Shovel',2,240),(9,'Wood Saw',1,400),(10,'Laser Measure',6,900),(11,'Hand Pruner',NULL,NULL),(12,'Hand Rake',NULL,NULL),(13,'Hedge Shears',NULL,NULL),(14,'Loppers',NULL,NULL),(15,'Pipe Wrench',NULL,NULL),(27,'Product A',NULL,NULL),(28,'Product B',NULL,NULL);
/*!40000 ALTER TABLE `product_sales_and_count` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  `category_id` int NOT NULL,
  `supplier_id` int NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `category_id` (`category_id`),
  KEY `supplier_id` (`supplier_id`),
  CONSTRAINT `products_ibfk_1` FOREIGN KEY (`category_id`) REFERENCES `categories` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `products_ibfk_2` FOREIGN KEY (`supplier_id`) REFERENCES `suppliers` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=106 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (63,'Italy Standard Shower','',21,1,226.00,'2025-05-09 09:51:09','2025-05-19 05:59:11'),(64,'Elbow Pipe Connector','',21,1,81.00,'2025-05-09 11:05:41','2025-05-19 06:00:41'),(65,'Paint Brush 1.5\" 628','',22,6,34.99,'2025-05-16 09:36:57','2025-05-19 06:01:17'),(66,'Garden Shovel 86106','',24,2,540.70,'2025-05-16 09:38:06','2025-05-16 09:38:06'),(67,'WORTH Garden Tools 8 1/4 Forged Pruner','',24,8,201.91,'2025-05-16 09:39:47','2025-05-16 09:39:47'),(68,'Water Orange Hose (30m)','',24,4,712.77,'2025-05-16 09:40:40','2025-05-16 09:40:40'),(69,'VASE HG-8008PY 165*14CM','',24,7,28.64,'2025-05-16 09:41:40','2025-05-16 09:41:40'),(70,'TRIO Growmate Easy Mix 3 in 1 (7L)','',24,8,51.84,'2025-05-16 09:42:26','2025-05-16 09:42:26'),(71,'TACTIX Self Retracting Utility Knife','',25,10,129.59,'2025-05-16 09:45:11','2025-05-16 09:45:11'),(72,'Screwdriver Set (4pcs)','',25,3,210.80,'2025-05-16 09:46:07','2025-05-16 09:46:27'),(73,'Multi-Use Foldable PVC Hand Truck Trolley (150kg)','',25,2,1360.74,'2025-05-16 09:47:05','2025-05-16 09:47:05'),(74,'TACTIX Quick Change Driver Bit Set (56 pieces)','',25,2,1905.04,'2025-05-16 09:48:14','2025-05-16 09:48:14'),(75,'Stainless-Steel Padlock (40mm)','',26,3,77.63,'2025-05-16 09:49:07','2025-05-16 09:49:07'),(76,'TIESHEN Deadbolt Lock','',26,3,325.67,'2025-05-16 09:49:54','2025-05-16 09:49:54'),(77,'Stainless Steel Hasp ( inch)','',26,2,72.70,'2025-05-16 09:50:39','2025-05-16 09:50:39'),(78,'Stainless Steel Door Hinges 2 inch','',26,3,90.50,'2025-05-16 09:51:28','2025-05-16 09:51:28'),(79,'STELAR Push Button Combination Padlock (L40)','',26,3,80.87,'2025-05-16 09:52:51','2025-05-16 09:52:51'),(80,'TOPDA Spray Paint Sparkling Black #50 (400ml)','',22,6,94.34,'2025-05-16 09:53:55','2025-05-16 09:53:55'),(81,'Paint Roller With Handle 9\"','',22,6,64.50,'2025-05-16 09:54:46','2025-05-16 09:54:46'),(82,'Rottweiler Paint Tool Set 3 Pcs','',22,6,250.00,'2025-05-16 09:55:20','2025-05-16 09:55:20'),(83,'ROTTWEILER Paint Brush Set 10464 (5pcs)','',22,2,78.90,'2025-05-16 09:55:58','2025-05-16 09:55:58'),(84,'PYE Puttyfilla Cellulose Filler (500g)','',22,6,254.30,'2025-05-16 09:56:42','2025-05-16 09:56:42'),(85,'PUTTY TROWEL 60MM HPUT08060','',22,6,100.00,'2025-05-16 09:57:40','2025-05-16 09:57:40'),(86,'Paint Roller without handle 4\" (5pcs)','',22,6,68.50,'2025-05-16 09:58:43','2025-05-16 09:58:43'),(87,'Spray Paint Lacquer No.1 (400ml)','',22,6,150.00,'2025-05-16 10:00:42','2025-05-16 10:00:42'),(88,'Spray Paint Brown No.60 (400ml)','',22,6,160.00,'2025-05-16 10:01:08','2025-05-16 10:01:08'),(89,'Multipurpose Scratch Remover (100ml)','',22,6,120.00,'2025-05-16 10:01:33','2025-05-16 10:01:33'),(90,'Ball Pein Hammer','',27,2,180.00,'2025-05-16 10:02:05','2025-05-16 10:02:05'),(91,'Claw Hammer (23mm)','',27,10,200.50,'2025-05-16 10:02:28','2025-05-16 10:02:28'),(92,'Rubber Mallet (16oz)','',27,2,220.00,'2025-05-16 10:02:55','2025-05-16 10:02:55'),(93,'Sledge Hammer (10lb)','',27,7,500.50,'2025-05-16 10:03:13','2025-05-16 10:03:13'),(94,'Cross Peen Hammer (2lb)','',27,2,280.00,'2025-05-16 10:03:34','2025-05-16 10:03:34'),(95,'Fiberglass Handle Hammer','',27,1,210.30,'2025-05-16 10:03:59','2025-05-16 10:03:59'),(96,'3RING Super Glue (3pcs)','',23,5,100.25,'2025-05-16 10:04:29','2025-05-16 10:04:29'),(97,'Super Glue 502 (13g)','',23,3,91.00,'2025-05-16 10:04:52','2025-05-16 10:04:52'),(98,'3RING Shoe Adhesive (40ml)','',23,7,110.00,'2025-05-16 10:05:10','2025-05-16 10:05:10'),(99,'X\'TRASEAL Epoxy Steel Adhesive Set (3pcs)','',23,2,150.00,'2025-05-16 10:05:40','2025-05-16 10:05:40'),(100,'CHEMI-BOND Carpenter Glue','',23,1,200.00,'2025-05-16 10:05:59','2025-05-16 10:05:59'),(101,'UHU All Purpose Adhesive Glue (7ml)','',23,3,50.00,'2025-05-16 10:06:27','2025-05-16 10:06:27'),(102,'LOVPICK Arts and Crafts Glue (125ml)','',23,2,80.00,'2025-05-16 10:06:42','2025-05-16 10:06:42'),(103,'Glue Pen (40ml)','',23,2,60.00,'2025-05-16 10:06:59','2025-05-16 10:06:59'),(104,'CHEMI-BOND All Purpose Magic Glue (100g)','',23,1,125.00,'2025-05-16 10:07:14','2025-05-16 10:07:14'),(105,'X\'TRASEAL 3 Ton Epoxy','',23,2,160.00,'2025-05-16 10:07:29','2025-05-16 10:07:29');
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `after_product_insert` AFTER INSERT ON `products` FOR EACH ROW BEGIN
	INSERT INTO inventories (product_id)
    VALUES (NEW.id);

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `sales`
--

DROP TABLE IF EXISTS `sales`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sales` (
  `id` int NOT NULL AUTO_INCREMENT,
  `customer_id` int DEFAULT NULL,
  `product_id` int NOT NULL,
  `quantity` int unsigned NOT NULL,
  `total_price` decimal(10,2) NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `customer_id` (`customer_id`),
  KEY `product_id` (`product_id`),
  CONSTRAINT `sales_ibfk_1` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `sales_ibfk_2` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=79 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sales`
--

LOCK TABLES `sales` WRITE;
/*!40000 ALTER TABLE `sales` DISABLE KEYS */;
INSERT INTO `sales` VALUES (64,15,63,1,226.00,'2025-05-09 15:41:58','2025-05-09 15:41:58'),(65,15,63,5,1130.00,'2025-05-09 15:43:04','2025-05-09 15:43:04'),(66,15,63,4,904.00,'2025-05-09 15:53:37','2025-05-09 15:53:37'),(67,15,64,5,405.00,'2025-05-09 15:54:27','2025-05-09 15:54:27'),(68,15,63,6,1356.00,'2025-05-10 09:14:52','2025-05-10 09:14:52'),(69,15,64,5,405.00,'2025-05-10 09:14:52','2025-05-10 09:14:52'),(72,NULL,63,3,678.00,'2025-05-10 13:07:26','2025-05-10 13:07:26'),(73,15,63,1,226.00,'2025-05-10 13:09:07','2025-05-10 13:09:07'),(74,NULL,64,5,405.00,'2025-05-10 13:12:40','2025-05-10 13:12:40'),(75,15,63,5,1130.00,'2025-05-10 13:12:57','2025-05-10 13:12:57'),(76,16,63,7,1582.00,'2025-05-10 13:13:38','2025-05-10 13:13:38'),(77,15,68,3,2138.31,'2025-05-19 05:55:32','2025-05-19 05:55:32'),(78,16,66,5,2703.50,'2025-05-19 05:57:26','2025-05-19 05:57:26');
/*!40000 ALTER TABLE `sales` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `before_insert_sales` BEFORE INSERT ON `sales` FOR EACH ROW BEGIN
    DECLARE product_price DECIMAL(10,2);

    -- Get the price of the product
    SELECT price INTO product_price FROM products WHERE id = NEW.product_id;

    -- Calculate the total price
    SET NEW.total_price = NEW.quantity * product_price;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `after_insert_sales` AFTER INSERT ON `sales` FOR EACH ROW BEGIN
    -- Insert a new transaction record
    INSERT INTO transactions (sale_id, payment_method, amount_paid)
    VALUES (NEW.id, "Cash", NEW.total_price);

    -- Decrease stock_available in inventories for the sold product
    UPDATE inventories
    SET stock_available = stock_available - NEW.quantity,
        updated_at = NOW()
    WHERE product_id = NEW.product_id;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `before_update_sales` BEFORE UPDATE ON `sales` FOR EACH ROW BEGIN
    DECLARE product_price DECIMAL(10,2);

    -- Get the current price of the product
    SELECT price INTO product_price FROM products WHERE id = NEW.product_id;

    -- Recalculate the total price
    SET NEW.total_price = NEW.quantity * product_price;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `after_update_sales` AFTER UPDATE ON `sales` FOR EACH ROW BEGIN
    -- Update the amount_paid in transactions table
    UPDATE transactions
    SET amount_paid = NEW.total_price
    WHERE sale_id = NEW.id;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `sales_per_category`
--

DROP TABLE IF EXISTS `sales_per_category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sales_per_category` (
  `category_id` int NOT NULL,
  `category_name` varchar(255) DEFAULT NULL,
  `total_sales` decimal(10,2) DEFAULT '0.00',
  PRIMARY KEY (`category_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sales_per_category`
--

LOCK TABLES `sales_per_category` WRITE;
/*!40000 ALTER TABLE `sales_per_category` DISABLE KEYS */;
INSERT INTO `sales_per_category` VALUES (1,'Power Tools',9000.00),(2,'Hand Tools',720.00),(3,'Fasteners',0.00),(4,'Plumbing',0.00),(5,'Electrical',105.00),(6,'Paint & Adhesives',0.00),(7,'Safety Equipment',0.00),(8,'Gardening Tools',0.00),(9,'Woodworking',0.00),(10,'Measuring Tools',0.00),(16,'Category A',0.00),(17,'Category B',0.00);
/*!40000 ALTER TABLE `sales_per_category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `suppliers`
--

DROP TABLE IF EXISTS `suppliers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suppliers` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `email` varchar(255) DEFAULT NULL,
  `contact_person` varchar(255) DEFAULT NULL,
  `contact_person_number` varchar(20) NOT NULL,
  `address` varchar(255) NOT NULL,
  `status` varchar(20) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `contact_number` (`contact_person_number`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `suppliers`
--

LOCK TABLES `suppliers` WRITE;
/*!40000 ALTER TABLE `suppliers` DISABLE KEYS */;
INSERT INTO `suppliers` VALUES (1,'ToolMaster Ltd','toolmaster@gmail.com','Joshua Po','09128664993','Naga City, Camarines Sur','inactive','2025-04-23 11:45:54','2025-05-09 12:26:07'),(2,'HandyWorks','handyworks@gmail.com','David Kim','09123456789','Manila, Metro Manila','inactive','2025-04-23 11:45:54','2025-05-09 12:26:14'),(3,'FastenIt Co.','fastenit@gmail.com','Jarren Palma','09234567890','Quezon City, Metro Manila','active','2025-04-23 11:45:54','2025-04-23 11:45:54'),(4,'PipeLine Inc','pipelineinc@gmail.com','Allan Mapa','09345678901','Angeles City, Pampanga','active','2025-04-23 11:45:54','2025-04-23 11:45:54'),(5,'ElectricPro','electricpro@gmail.com','Jude Zara','09134567890','Taguig City, Metro Manila','active','2025-04-23 11:45:54','2025-04-23 11:45:54'),(6,'PaintWorld','paintworld@gmail.com','Elmo Dy','09212345678','Caloocan City, Metro Manila','active','2025-04-23 11:45:54','2025-04-23 11:45:54'),(7,'Gogo','gogo@gmail.com','Denver Camo','09456789012','Baguio City, Benguet','active','2025-04-23 11:45:54','2025-04-23 11:45:54'),(8,'GreenYard','greenyard@gmail.com','Gerald Lopez','09356789012','San Fernando, Pampanga','inactive','2025-04-23 11:45:54','2025-04-23 11:45:54'),(9,'WoodCrafters','woodcrafters@gmail.com','Teddy Lora','09567890123','Tarlac City, Tarlac','inactive','2025-04-23 11:45:54','2025-04-23 11:45:54'),(10,'PrecisionTools','precisiontools@gmail.com','Anthony Reyes','911369','Batangas City, Batangas','active','2025-04-23 11:45:54','2025-05-09 12:26:38');
/*!40000 ALTER TABLE `suppliers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transactions`
--

DROP TABLE IF EXISTS `transactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transactions` (
  `id` int NOT NULL AUTO_INCREMENT,
  `sale_id` int NOT NULL,
  `payment_method` varchar(50) NOT NULL,
  `amount_paid` decimal(10,2) NOT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `sale_id` (`sale_id`),
  CONSTRAINT `transactions_ibfk_1` FOREIGN KEY (`sale_id`) REFERENCES `sales` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transactions`
--

LOCK TABLES `transactions` WRITE;
/*!40000 ALTER TABLE `transactions` DISABLE KEYS */;
INSERT INTO `transactions` VALUES (38,64,'Cash',226.00,'2025-05-09 23:41:58','2025-05-09 23:41:58'),(39,65,'Cash',1130.00,'2025-05-09 23:43:04','2025-05-09 23:43:04'),(40,66,'Cash',904.00,'2025-05-09 23:53:37','2025-05-09 23:53:37'),(41,67,'Cash',405.00,'2025-05-09 23:54:27','2025-05-09 23:54:27'),(42,68,'Cash',1356.00,'2025-05-10 17:14:52','2025-05-10 17:14:52'),(43,69,'Cash',405.00,'2025-05-10 17:14:52','2025-05-10 17:14:52'),(46,72,'Cash',678.00,'2025-05-10 21:07:26','2025-05-10 21:07:26'),(47,73,'Cash',226.00,'2025-05-10 21:09:07','2025-05-10 21:09:07'),(48,74,'Cash',405.00,'2025-05-10 21:12:40','2025-05-10 21:12:40'),(49,75,'Cash',1130.00,'2025-05-10 21:12:57','2025-05-10 21:12:57'),(50,76,'Cash',1582.00,'2025-05-10 21:13:38','2025-05-10 21:13:38'),(51,77,'Cash',2138.31,'2025-05-19 13:55:32','2025-05-19 13:55:32'),(52,78,'Cash',2703.50,'2025-05-19 13:57:26','2025-05-19 13:57:26');
/*!40000 ALTER TABLE `transactions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `first_name` varchar(50) NOT NULL,
  `middle_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) NOT NULL,
  `username` varchar(50) NOT NULL,
  `email` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `reset_question` varchar(150) DEFAULT NULL,
  `reset_answer` varchar(150) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'Emman','Oriel','Nieva','admin','admin@gmail.com','123','Favorite Food','adobo','2025-04-23 11:37:34','2025-05-19 06:14:27'),(10,'Kelvin','Davin','Sy','kelvin','kelvin@gmail.com','1234','Favorite Food','adobo','2025-05-16 09:34:01','2025-05-16 11:02:51');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `view_category_products_count`
--

DROP TABLE IF EXISTS `view_category_products_count`;
/*!50001 DROP VIEW IF EXISTS `view_category_products_count`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `view_category_products_count` AS SELECT 
 1 AS `category_name`,
 1 AS `total_products`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_customer_addresses_count`
--

DROP TABLE IF EXISTS `view_customer_addresses_count`;
/*!50001 DROP VIEW IF EXISTS `view_customer_addresses_count`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `view_customer_addresses_count` AS SELECT 
 1 AS `address`,
 1 AS `address_count`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_product_listings`
--

DROP TABLE IF EXISTS `view_product_listings`;
/*!50001 DROP VIEW IF EXISTS `view_product_listings`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `view_product_listings` AS SELECT 
 1 AS `id`,
 1 AS `name`,
 1 AS `description`,
 1 AS `price`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_product_sales`
--

DROP TABLE IF EXISTS `view_product_sales`;
/*!50001 DROP VIEW IF EXISTS `view_product_sales`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `view_product_sales` AS SELECT 
 1 AS `product_id`,
 1 AS `product_name`,
 1 AS `total_sales`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_supplier_listings`
--

DROP TABLE IF EXISTS `view_supplier_listings`;
/*!50001 DROP VIEW IF EXISTS `view_supplier_listings`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `view_supplier_listings` AS SELECT 
 1 AS `name`,
 1 AS `email`,
 1 AS `contact_person`,
 1 AS `contact_person_number`,
 1 AS `address`*/;
SET character_set_client = @saved_cs_client;

--
-- Dumping events for database 'hardwareshopdb'
--
/*!50106 SET @save_time_zone= @@TIME_ZONE */ ;
/*!50106 DROP EVENT IF EXISTS `update_stock_status` */;
DELIMITER ;;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;;
/*!50003 SET character_set_client  = utf8mb4 */ ;;
/*!50003 SET character_set_results = utf8mb4 */ ;;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;;
/*!50003 SET @saved_time_zone      = @@time_zone */ ;;
/*!50003 SET time_zone             = 'SYSTEM' */ ;;
/*!50106 CREATE*/ /*!50117 DEFINER=`root`@`localhost`*/ /*!50106 EVENT `update_stock_status` ON SCHEDULE EVERY 10 SECOND STARTS '2025-04-03 22:20:18' ON COMPLETION NOT PRESERVE ENABLE DO BEGIN
    UPDATE inventories 
    SET stock_status = 
        CASE 
            WHEN stock_available = 0 OR stock_available IS NULL THEN 'Out of Stock'
            WHEN stock_available BETWEEN 1 AND 10 THEN 'Low Stock'
            WHEN stock_available BETWEEN 11 AND 50 THEN 'Medium Stock'
            ELSE 'High Stock'
        END;
END */ ;;
/*!50003 SET time_zone             = @saved_time_zone */ ;;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;;
/*!50003 SET character_set_client  = @saved_cs_client */ ;;
/*!50003 SET character_set_results = @saved_cs_results */ ;;
/*!50003 SET collation_connection  = @saved_col_connection */ ;;
DELIMITER ;
/*!50106 SET TIME_ZONE= @save_time_zone */ ;

--
-- Dumping routines for database 'hardwareshopdb'
--
/*!50003 DROP FUNCTION IF EXISTS `calculate_total_price` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `calculate_total_price`(p_product_id INT, p_quantity INT) RETURNS decimal(10,2)
    DETERMINISTIC
BEGIN
	DECLARE v_price DECIMAL(10,2);
    DECLARE v_total DECIMAL(10,2);
    
    -- Get the price of the product
    SELECT price INTO v_price FROM products WHERE id = p_product_id;
    
    -- Compute total price
    SET v_total = v_price * p_quantity;
    
    RETURN v_total;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `check_stock_availability` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `check_stock_availability`(p_product_id INT, p_quantity INT) RETURNS tinyint(1)
    DETERMINISTIC
BEGIN
	DECLARE v_stock INT;
    
    -- Get the available stock
    SELECT stock_available INTO v_stock FROM inventories WHERE product_id = p_product_id;
    
    -- Check if stock is sufficient
    IF v_stock >= p_quantity THEN
        RETURN TRUE;  -- Stock is available
    ELSE
        RETURN FALSE; -- Not enough stock
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `format_customer_name` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `format_customer_name`(first_name VARCHAR(50), middle_name VARCHAR(50), last_name VARCHAR(50)) RETURNS varchar(155) CHARSET utf8mb4
    DETERMINISTIC
BEGIN
RETURN CONCAT(
        first_name, 
        IF(middle_name IS NOT NULL AND middle_name != '', CONCAT(' ', LEFT(middle_name, 1), '.'), ''), 
        ' ', last_name
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `get_product_price_with_discount` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `get_product_price_with_discount`(product_id INT,
    discount_percent DECIMAL(5,2)) RETURNS varchar(255) CHARSET utf8mb4
    DETERMINISTIC
BEGIN
	DECLARE original_price DECIMAL(10,2);
    DECLARE discounted_price DECIMAL(10,2);
    DECLARE result_text VARCHAR(255);
    
    -- Get the product price from the view
    SELECT price INTO original_price 
    FROM view_product_listings 
    WHERE id = product_id;
    
    -- Calculate the discounted price
    SET discounted_price = original_price - (original_price * (discount_percent / 100));
    
    -- Format the output
    SET result_text = CONCAT('Original Price: ', original_price, ', Discounted Price: ', discounted_price);
    
    RETURN result_text;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `get_total_sales` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `get_total_sales`(start_date DATE, end_date DATE) RETURNS double
    DETERMINISTIC
BEGIN
	DECLARE total_sales DOUBLE;
    
    SELECT sum(total_price) INTO total_sales
    FROM sales
    WHERE sale_date BETWEEN start_date AND end_date;
RETURN ifnull(total_sales, 0);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `add_sale` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `add_sale`(IN p_product_id INT, IN p_quantity INT)
BEGIN
	DECLARE v_total_price DECIMAL(10,2);
    DECLARE v_stock_available BOOLEAN;

    -- Check if stock is available
    SET v_stock_available = check_stock_availability(p_product_id, p_quantity);

    IF v_stock_available THEN
        -- Calculate total price
        SET v_total_price = calculate_total_price(p_product_id, p_quantity);
        
        -- Insert the sale
        INSERT INTO sales (product_id, quantity, total_price)
        VALUES (p_product_id, p_quantity, v_total_price);
        
        -- Update inventory
        UPDATE inventories 
        SET stock_available = stock_available - p_quantity 
        WHERE product_id = p_product_id;
    ELSE
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Not enough stock available!';
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `update_category_products_count` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_category_products_count`()
BEGIN
    DECLARE done INT DEFAULT 0;
    DECLARE cat_id INT;
    DECLARE cat_name VARCHAR(255);
    DECLARE prod_count INT;
    DECLARE product_cursor CURSOR FOR 
        SELECT c.id, c.name 
        FROM categories c;
    
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;
    
    -- Delete all existing data from category_products_count before recalculating
    DELETE FROM category_products_count;
    
    OPEN product_cursor;
    
    read_loop: LOOP
        FETCH product_cursor INTO cat_id, cat_name;
        IF done THEN
            LEAVE read_loop;
        END IF;

        -- Count the products in the category
        SELECT COUNT(*) INTO prod_count
        FROM products p
        WHERE p.category_id = cat_id;

        -- Insert/update the category_products_count table
        INSERT INTO category_products_count (category_id, category, products_count)
        VALUES (cat_id, cat_name, prod_count)
        ON DUPLICATE KEY UPDATE
            products_count = prod_count;

    END LOOP;
    
    CLOSE product_cursor;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `update_product_sales_and_count` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_product_sales_and_count`()
BEGIN
    DECLARE done INT DEFAULT 0;
    DECLARE prod_id INT;
    DECLARE total_quantity INT;
    DECLARE total_sales DECIMAL(10,2);
    
    -- Declare cursor for iterating through the 'products' table
    DECLARE cur CURSOR FOR 
        SELECT id 
        FROM products;
        
    -- Declare handler for end of cursor
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;
    
    OPEN cur;
    
    -- Loop through all products
    read_loop: LOOP
        FETCH cur INTO prod_id;
        
        IF done THEN
            LEAVE read_loop;
        END IF;
        
        -- Get the updated total quantity and total sales for the current product
        SELECT 
            SUM(s.quantity),
            SUM(s.total_price)
        INTO 
            total_quantity, 
            total_sales
        FROM 
            sales s
        WHERE 
            s.product_id = prod_id;
        
        -- Check if the product exists in the product_sales_and_count table
        IF EXISTS (SELECT 1 FROM product_sales_and_count WHERE product_id = prod_id) THEN
            -- Update the existing product's data
            UPDATE product_sales_and_count
            SET 
                total_quantity_sold = total_quantity,
                total_sales = total_sales
            WHERE 
                product_id = prod_id;
        ELSE
            -- Insert the new product if it does not exist
            INSERT INTO product_sales_and_count (product_id, product_name, total_quantity_sold, total_sales)
            SELECT 
                p.id,
                p.name,
                IFNULL(SUM(s.quantity), 0),
                IFNULL(SUM(s.total_price), 0)
            FROM 
                products p
            LEFT JOIN 
                sales s ON p.id = s.product_id
            WHERE 
                p.id = prod_id
            GROUP BY 
                p.id, p.name;
        END IF;
        
    END LOOP;
    
    CLOSE cur;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `update_product_supplier_price` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_product_supplier_price`(
    IN supplier_id INT,  -- Supplier ID to filter products
    IN price_increase DECIMAL(5, 2)  -- Price increase percentage (e.g., 0.10 for 10%)
)
BEGIN
    -- Update the prices for products belonging to the specified supplier
    UPDATE products
    SET price = price * (1 + price_increase)
    WHERE supplier_id = supplier_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `update_sales_per_category` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_sales_per_category`(IN categoryId INT)
BEGIN
    -- Update total sales for the specified category
    UPDATE sales_per_category spc
    SET spc.total_sales = (
        SELECT COALESCE(SUM(s.total_price), 0)
        FROM sales s
        JOIN products p ON s.product_id = p.id
        WHERE p.category_id = categoryId
    )
    WHERE spc.category_id = categoryId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `view_category_products_count`
--

/*!50001 DROP VIEW IF EXISTS `view_category_products_count`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_category_products_count` AS select `c`.`name` AS `category_name`,count(`p`.`id`) AS `total_products` from (`categories` `c` left join `products` `p` on((`c`.`id` = `p`.`category_id`))) group by `c`.`id`,`c`.`name` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_customer_addresses_count`
--

/*!50001 DROP VIEW IF EXISTS `view_customer_addresses_count`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_customer_addresses_count` AS select `customers`.`address` AS `address`,count(0) AS `address_count` from `customers` group by `customers`.`address` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_product_listings`
--

/*!50001 DROP VIEW IF EXISTS `view_product_listings`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_product_listings` AS select `products`.`id` AS `id`,`products`.`name` AS `name`,`products`.`description` AS `description`,`products`.`price` AS `price` from `products` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_product_sales`
--

/*!50001 DROP VIEW IF EXISTS `view_product_sales`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_product_sales` AS select `products`.`id` AS `product_id`,`products`.`name` AS `product_name`,sum(`sales`.`total_price`) AS `total_sales` from (`products` join `sales` on((`products`.`id` = `sales`.`product_id`))) group by `products`.`id`,`products`.`name` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_supplier_listings`
--

/*!50001 DROP VIEW IF EXISTS `view_supplier_listings`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_supplier_listings` AS select `suppliers`.`name` AS `name`,`suppliers`.`email` AS `email`,`suppliers`.`contact_person` AS `contact_person`,`suppliers`.`contact_person_number` AS `contact_person_number`,`suppliers`.`address` AS `address` from `suppliers` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-20 19:32:35
