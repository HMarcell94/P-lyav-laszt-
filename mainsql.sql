-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: main
-- ------------------------------------------------------
-- Server version	8.0.30

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
-- Table structure for table `applications`
--

DROP TABLE IF EXISTS `applications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `applications` (
  `ApplicationID` int NOT NULL AUTO_INCREMENT,
  `JobID` int NOT NULL,
  `EmployeeID` int NOT NULL,
  `ApplicationDate` date NOT NULL,
  `ApplicationStatus` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  PRIMARY KEY (`ApplicationID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_hungarian_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `applications`
--

LOCK TABLES `applications` WRITE;
/*!40000 ALTER TABLE `applications` DISABLE KEYS */;
INSERT INTO `applications` VALUES (6,0,0,'2024-03-30','string');
/*!40000 ALTER TABLE `applications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employees`
--

DROP TABLE IF EXISTS `employees`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employees` (
  `EmployeeID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `ContactNumber` varchar(255) COLLATE utf8mb3_hungarian_ci DEFAULT NULL,
  `Portfolio` text CHARACTER SET utf8mb3 COLLATE utf8mb3_hungarian_ci,
  `Resume` text CHARACTER SET utf8mb3 COLLATE utf8mb3_hungarian_ci,
  PRIMARY KEY (`EmployeeID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_hungarian_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employees`
--

LOCK TABLES `employees` WRITE;
/*!40000 ALTER TABLE `employees` DISABLE KEYS */;
INSERT INTO `employees` VALUES (1,10,'string',NULL,NULL);
/*!40000 ALTER TABLE `employees` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employers`
--

DROP TABLE IF EXISTS `employers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employers` (
  `EmployerID` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `CompanyName` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `CompanyDescription` text CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `CompanyLogo` text CHARACTER SET utf8mb3 COLLATE utf8mb3_hungarian_ci NOT NULL,
  PRIMARY KEY (`EmployerID`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_hungarian_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employers`
--

LOCK TABLES `employers` WRITE;
/*!40000 ALTER TABLE `employers` DISABLE KEYS */;
INSERT INTO `employers` VALUES (1,10,'nando','string','string'),(2,10,'string','string','string'),(10,10,'string','string','string'),(30,10,'string','string','string'),(31,0,'string','string','string');
/*!40000 ALTER TABLE `employers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `jobs`
--

DROP TABLE IF EXISTS `jobs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `jobs` (
  `JobID` int NOT NULL AUTO_INCREMENT,
  `EmployerID` int DEFAULT NULL,
  `JobTitle` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_hungarian_ci DEFAULT NULL,
  `JobDescription` text CHARACTER SET utf8mb3 COLLATE utf8mb3_hungarian_ci,
  `JobRequirements` text CHARACTER SET utf8mb3 COLLATE utf8mb3_hungarian_ci,
  `JobLocation` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_hungarian_ci DEFAULT NULL,
  `JobSalary` int DEFAULT NULL,
  `Picture` text COLLATE utf8mb3_hungarian_ci,
  PRIMARY KEY (`JobID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_hungarian_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `jobs`
--

LOCK TABLES `jobs` WRITE;
/*!40000 ALTER TABLE `jobs` DISABLE KEYS */;
INSERT INTO `jobs` VALUES (2,0,'string','string','string','string',0,NULL),(3,0,'string','string','string','string',0,NULL),(4,NULL,'cxycxcy','cyxcycíyc',NULL,NULL,11000,NULL),(5,NULL,'cxycxcy','cyxcycíyc',NULL,NULL,11000,NULL),(6,NULL,'cxycxcy','cyxcycíyc',NULL,NULL,11000,NULL),(7,NULL,NULL,NULL,NULL,NULL,0,NULL);
/*!40000 ALTER TABLE `jobs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `RoleID` int NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_hungarian_ci DEFAULT NULL,
  PRIMARY KEY (`RoleID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_hungarian_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,'employer'),(2,'employee'),(3,'Admin');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserID` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) COLLATE utf8mb3_hungarian_ci NOT NULL,
  `Password` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Email` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `RoleID` int NOT NULL DEFAULT '1',
  `First_Name` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_hungarian_ci NOT NULL,
  `Last_Name` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_hungarian_ci NOT NULL,
  `Salt` varchar(255) COLLATE utf8mb3_hungarian_ci DEFAULT NULL,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `Last_Name_UNIQUE` (`Last_Name`),
  UNIQUE KEY `First_Name_UNIQUE` (`First_Name`),
  KEY `RoleID` (`RoleID`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_hungarian_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (34,'vyíc','FA7LKxcd7Wa7KfJYt1nXIaj+/ncTEp5eWM5qiNzTjgg7k5ZY','daví@gmail.com',2,'adavay','davay','FA7LKxcd7Wa7KfJYt1nXIa'),(35,'string','string','string',1,'string','string','string'),(36,'tafas','pvo85NCcVxLdVjOtwPTA5WcKJ5atTuLnv35GX/3PXJeLnIfE','email@gmail.com',2,'da','dadxc','pvo85NCcVxLdVjOtwPTA5Q=='),(37,'felhasz','Q7CLSwUgqNeOkzAjK5A5pNWW2gYg5khJi+JfskZ/qcIJlmru','emails@gmail.com',1,'Vez','kere','Q7CLSwUgqNeOkzAjK5A5pA=='),(39,'felhasznalonev','PYgS5Lk1gHn3vwIiidDAJ8SLUqBHXm6dKLAt0BqXvk8ZqDTV','adfaf@gmail.com',1,'vezeteknev','keresztnev','PYgS5Lk1gHn3vwIiidDAJw=='),(40,'cxyyxcy','cGWOrLy+0QCnC52IO/8JfWGsizaYeezmP6MsXiMmROvhTtha','kdakd@gmail.com',2,'adsdax','cxyc','cGWOrLy+0QCnC52IO/8JfQ=='),(41,'users','vJ5vrYcmmIJylbms/t0p1StYx6IH0qLDWfHtqIj6aOVvFSiP','emadc@gmail.com',1,'first','last','vJ5vrYcmmIJylbms/t0p1Q=='),(42,'cycx','r8NEicxfEtm2OS4qYcebLSrHtiSw5AwlA1mhqqj7+48R0Bsn','cyycc@gmail.com',2,'yxcyxcxc','cyy','r8NEicxfEtm2OS4qYcebLQ=='),(44,'Admin','fBQJaeDobrST/l41kcQAGkzADk5beJykEB0+E/7WwNNuVgl0','Admin@gmail.com',3,'Admin','Admin','fBQJaeDobrST/l41kcQAGg=='),(45,'dac íxíy','HW/1bdeCIl1znYHsUZolFJoHeakF/F8Hl1+wsYr6tvvcJ+Yn','avyc@gmail.com',1,'Employer','Employer','HW/1bdeCIl1znYHsUZolFA==');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-04-04 11:04:16
