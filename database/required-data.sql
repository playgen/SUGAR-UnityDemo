-- --------------------------------------------------------
-- Host:                         files.playgen.com
-- Server version:               10.2.13-MariaDB-10.2.13+maria~jessie - mariadb.org binary distribution
-- Server OS:                    debian-linux-gnu
-- HeidiSQL Version:             9.5.0.5261
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping data for table SUGAR.Accounts: ~9 rows (approximately)
/*!40000 ALTER TABLE `Accounts` DISABLE KEYS */;
REPLACE INTO `Accounts` (`Id`, `AccountSourceId`, `Name`, `Password`, `UserId`) VALUES
	(20, 1, 'Joe', '$2a$13$oDUj9x8SsiuyM0USvI1zQOvyrSLxiQbla5p58yXNLficT1kLcgjzq', 3),
	(21, 1, 'Cathy', '$2a$13$KbgkP3qE9VJaRDa92cvJkOX/gqPQBN5tokej5Sk.rS8X5dduQHR1a', 4),
	(22, 1, 'Cindy', '$2a$13$qPFLgQeWENX0F5cK36sf5O8PlljrlqC7XbMEK/hAUx6TQVfKn913m', 5),
	(23, 1, 'Bob', '$2a$13$EPYRxfoy/E9yLqC.ALTyiuR78bo9UDFOlZdbHk5Se3wBLab3Z.7Me', 6),
	(24, 1, 'Frank', '$2a$13$mudiiC.8ZiMWivkxElOibOVgmOZsamBmLc0yI2oXTVYgpwJV9eMVS', 7),
	(25, 1, 'Jen', '$2a$13$JxmEp4DAEamGcOXW1GQCceIxMMgjAmStGg6S1BPMI/lqw5flLzPZa', 8),
	(26, 1, 'Dave', '$2a$13$OpxHlwwiqpQn.cG9yDDsaOP75SxmmpY27GZM1PEiMSFrzWastF1BO', 9),
	(27, 1, 'Kelly', '$2a$13$pQgtNxRKU/HMvZtb0Pew6.5pmF2wr7fGrtYsMT6JZoSAZQ3ShQ08C', 10);
/*!40000 ALTER TABLE `Accounts` ENABLE KEYS */;

-- Dumping data for table SUGAR.AccountSources: ~1 rows (approximately)
/*!40000 ALTER TABLE `AccountSources` DISABLE KEYS */;
REPLACE INTO `AccountSources` (`Id`, `ApiSecret`, `AutoRegister`, `Description`, `RequiresPassword`, `Token`, `UsernamePattern`) VALUES
	(1, NULL, b'0', 'SUGAR', b'1', 'SUGAR', NULL);
/*!40000 ALTER TABLE `AccountSources` ENABLE KEYS */;

-- Dumping data for table SUGAR.ActorClaims: ~0 rows (approximately)
/*!40000 ALTER TABLE `ActorClaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `ActorClaims` ENABLE KEYS */;

-- Dumping data for table SUGAR.ActorData: ~0 rows (approximately)
/*!40000 ALTER TABLE `ActorData` DISABLE KEYS */;
/*!40000 ALTER TABLE `ActorData` ENABLE KEYS */;

-- Dumping data for table SUGAR.ActorRoles: ~33 rows (approximately)
/*!40000 ALTER TABLE `ActorRoles` DISABLE KEYS */;
REPLACE INTO `ActorRoles` (`Id`, `ActorId`, `EntityId`, `RoleId`) VALUES
	(1, 1, -1, 1),
	(2, 1, -1, 2),
	(3, 1, -1, 3),
	(5, 1, -1, 4),
	(7, 1, -1, 5),
	(8, 1, -1, 6),
	(9, 1, 1, 2),
	(4, 1, 1, 4),
	(6, 1, 1, 5),
	(24, 1, 2, 2),
	(29, 1, 3, 2),
	(30, 1, 4, 2),
	(31, 1, 5, 2),
	(55, 1, 11, 3),
	(56, 1, 12, 3),
	(57, 1, 13, 3),
	(58, 1, 14, 3),
	(39, 3, 3, 4),
	(40, 3, 20, 5),
	(41, 4, 4, 4),
	(42, 4, 21, 5),
	(43, 5, 5, 4),
	(44, 5, 22, 5),
	(45, 6, 6, 4),
	(46, 6, 23, 5),
	(47, 7, 7, 4),
	(48, 7, 24, 5),
	(49, 8, 8, 4),
	(50, 8, 25, 5),
	(51, 9, 9, 4),
	(52, 9, 26, 5),
	(53, 10, 10, 4),
	(54, 10, 27, 5);
/*!40000 ALTER TABLE `ActorRoles` ENABLE KEYS */;

-- Dumping data for table SUGAR.Actors: ~13 rows (approximately)
/*!40000 ALTER TABLE `Actors` DISABLE KEYS */;
REPLACE INTO `Actors` (`Id`, `Description`, `Discriminator`, `Name`) VALUES
	(3, NULL, 'User', 'Joe'),
	(4, NULL, 'User', 'Cathy'),
	(5, NULL, 'User', 'Cindy'),
	(6, NULL, 'User', 'Bob'),
	(7, NULL, 'User', 'Frank'),
	(8, NULL, 'User', 'Jen'),
	(9, NULL, 'User', 'Dave'),
	(10, NULL, 'User', 'Kelly'),
	(11, 'Flying High', 'Group', 'High Flyers'),
	(12, 'Superdy Dupedity', 'Group', 'Super Duper Paratroopers'),
	(13, 'Better than all the rest', 'Group', 'Simply the Best'),
	(14, 'The groupiest', 'Group', 'Groupy Grouperson');
/*!40000 ALTER TABLE `Actors` ENABLE KEYS */;

-- Dumping data for table SUGAR.Claims: ~93 rows (approximately)
/*!40000 ALTER TABLE `Claims` DISABLE KEYS */;
REPLACE INTO `Claims` (`Id`, `ClaimScope`, `Description`, `Name`) VALUES
	(1, 4, NULL, 'Delete-Account'),
	(2, 2, NULL, 'Get-Resource'),
	(3, 0, NULL, 'Update-Match'),
	(4, 1, NULL, 'Create-Match'),
	(5, 3, NULL, 'Create-Match'),
	(6, 2, NULL, 'Create-Match'),
	(7, 1, NULL, 'Update-Match'),
	(8, 3, NULL, 'Update-Match'),
	(9, 2, NULL, 'Update-Match'),
	(10, 0, NULL, 'Create-Match'),
	(11, 1, NULL, 'Delete-Leaderboard'),
	(12, 1, NULL, 'Update-Leaderboard'),
	(13, 1, NULL, 'Create-Leaderboard'),
	(14, 3, NULL, 'Delete-GroupMember'),
	(15, 2, NULL, 'Delete-GroupMember'),
	(16, 3, NULL, 'Update-GroupMemberRequest'),
	(17, 2, NULL, 'Update-GroupMemberRequest'),
	(18, 3, NULL, 'Create-GroupMemberRequest'),
	(19, 2, NULL, 'Create-GroupMemberRequest'),
	(20, 3, NULL, 'Get-GroupMemberRequest'),
	(21, 3, NULL, 'Get-Resource'),
	(22, 1, NULL, 'Get-Resource'),
	(23, 2, NULL, 'Create-Resource'),
	(24, 3, NULL, 'Create-Resource'),
	(25, 3, NULL, 'Create-UserFriendRequest'),
	(26, 3, NULL, 'Get-UserFriendRequest'),
	(27, 0, NULL, 'Delete-User'),
	(28, 3, NULL, 'Update-User'),
	(29, 0, NULL, 'Create-User'),
	(30, 0, NULL, 'Get-User'),
	(31, 5, NULL, 'Delete-Role'),
	(32, 1, NULL, 'Create-Role'),
	(33, 2, NULL, 'Create-Role'),
	(34, 2, NULL, 'Get-GroupMemberRequest'),
	(35, 0, NULL, 'Create-Role'),
	(36, 2, NULL, 'Get-Role'),
	(37, 0, NULL, 'Get-Role'),
	(38, 5, NULL, 'Delete-RoleClaim'),
	(39, 5, NULL, 'Create-RoleClaim'),
	(40, 5, NULL, 'Get-RoleClaim'),
	(41, 1, NULL, 'Update-Resource'),
	(42, 3, NULL, 'Update-Resource'),
	(43, 2, NULL, 'Update-Resource'),
	(44, 1, NULL, 'Create-Resource'),
	(45, 1, NULL, 'Get-Role'),
	(46, 3, NULL, 'Update-UserFriendRequest'),
	(47, 2, NULL, 'Delete-Group'),
	(48, 1, NULL, 'Create-GameData'),
	(49, 3, NULL, 'Get-ActorData'),
	(50, 1, NULL, 'Delete-ActorClaim'),
	(51, 2, NULL, 'Delete-ActorClaim'),
	(52, 0, NULL, 'Delete-ActorClaim'),
	(53, 1, NULL, 'Create-ActorClaim'),
	(54, 2, NULL, 'Create-ActorClaim'),
	(55, 0, NULL, 'Create-ActorClaim'),
	(56, 3, NULL, 'Get-ActorClaim'),
	(57, 1, NULL, 'Get-ActorClaim'),
	(58, 2, NULL, 'Get-ActorClaim'),
	(59, 0, NULL, 'Get-ActorClaim'),
	(60, 1, NULL, 'Delete-Achievement'),
	(61, 1, NULL, 'Update-Achievement'),
	(62, 1, NULL, 'Create-Achievement'),
	(63, 1, NULL, 'Get-Achievement'),
	(64, 0, NULL, 'Delete-AccountSource'),
	(65, 0, NULL, 'Update-AccountSource'),
	(66, 0, NULL, 'Create-AccountSource'),
	(67, 0, NULL, 'Get-AccountSource'),
	(68, 2, NULL, 'Get-ActorData'),
	(69, 2, NULL, 'Create-ActorData'),
	(70, 3, NULL, 'Create-ActorData'),
	(71, 0, NULL, 'Get-ActorRole'),
	(72, 3, NULL, 'Create-GameData'),
	(73, 2, NULL, 'Create-GameData'),
	(74, 1, NULL, 'Get-GameData'),
	(75, 3, NULL, 'Get-GameData'),
	(76, 2, NULL, 'Get-GameData'),
	(77, 1, NULL, 'Delete-Game'),
	(78, 1, NULL, 'Update-Game'),
	(79, 0, NULL, 'Create-Game'),
	(80, 2, NULL, 'Delete-Alliance'),
	(81, 2, NULL, 'Update-Group'),
	(82, 2, NULL, 'Update-AllianceRequest'),
	(83, 2, NULL, 'Get-AllianceRequest'),
	(84, 1, NULL, 'Delete-ActorRole'),
	(85, 2, NULL, 'Delete-ActorRole'),
	(86, 0, NULL, 'Delete-ActorRole'),
	(87, 1, NULL, 'Create-ActorRole'),
	(88, 2, NULL, 'Create-ActorRole'),
	(89, 0, NULL, 'Create-ActorRole'),
	(90, 1, NULL, 'Get-ActorRole'),
	(91, 2, NULL, 'Get-ActorRole'),
	(92, 2, NULL, 'Create-AllianceRequest'),
	(93, 3, NULL, 'Delete-UserFriend');
/*!40000 ALTER TABLE `Claims` ENABLE KEYS */;

-- Dumping data for table SUGAR.EvaluationCriterias: ~10 rows (approximately)
/*!40000 ALTER TABLE `EvaluationCriterias` DISABLE KEYS */;
REPLACE INTO `EvaluationCriterias` (`Id`, `ComparisonType`, `CriteriaQueryType`, `EvaluationDataCategory`, `EvaluationDataKey`, `EvaluationDataType`, `EvaluationId`, `Scope`, `Value`) VALUES
	(1, 3, 0, 1, 'Chocolate', 1, 1, 0, '100'),
	(2, 3, 1, 0, 'FRIENDS_MADE', 1, 2, 0, '1'),
	(3, 3, 0, 1, 'Chocolate', 1, 3, 0, '100'),
	(4, 3, 1, 0, 'CHOCOLATE_SHARED', 1, 4, 0, '50'),
	(5, 3, 1, 0, 'CHOCOLATE_SHARED', 1, 5, 3, '100'),
	(6, 3, 0, 1, 'Chocolate', 1, 6, 0, '100'),
	(7, 3, 1, 0, 'FRIENDS_MADE', 1, 7, 0, '1'),
	(8, 3, 0, 1, 'Chocolate', 1, 8, 0, '100'),
	(9, 3, 1, 0, 'CHOCOLATE_SHARED', 1, 9, 0, '50'),
	(10, 3, 1, 0, 'CHOCOLATE_SHARED', 1, 10, 3, '100');
/*!40000 ALTER TABLE `EvaluationCriterias` ENABLE KEYS */;

-- Dumping data for table SUGAR.EvaluationData: ~39 rows (approximately)
/*!40000 ALTER TABLE `EvaluationData` DISABLE KEYS */;
REPLACE INTO `EvaluationData` (`Id`, `ActorId`, `Category`, `DateCreated`, `DateModified`, `EvaluationDataType`, `GameId`, `Key`, `MatchId`, `Value`) VALUES
	(1, 11, 1, '2018-05-16 10:30:14.674296', '2018-05-16 10:30:14.674188', 1, 5, 'Chocolate', NULL, '100'),
	(2, 11, 3, '2018-05-16 10:30:14.725644', '2018-05-16 10:30:14.725620', 0, 5, 'CHOCOLATE_COLLECTOR', NULL, NULL),
	(3, 11, 0, '2018-05-16 10:30:28.667768', '2018-05-16 10:30:28.667766', 1, 5, 'FRIENDS_MADE', NULL, '1'),
	(4, 11, 3, '2018-05-16 10:30:28.686062', '2018-05-16 10:30:28.686062', 0, 5, 'FRIENDLY_SORT', NULL, NULL),
	(5, 11, 0, '2018-05-16 10:30:30.372275', '2018-05-16 10:30:30.372275', 1, 5, 'FRIENDS_MADE', NULL, '1'),
	(6, 11, 0, '2018-05-16 10:30:32.146307', '2018-05-16 10:30:32.146306', 1, 5, 'FRIENDS_MADE', NULL, '1'),
	(8, 3, 1, '2018-05-16 10:30:14.674296', '2018-05-16 11:07:15.914677', 1, 5, 'Chocolate', NULL, '85'),
	(9, 15, 1, '2018-05-16 10:57:53.494196', '2018-05-16 10:57:53.494195', 1, 5, 'Chocolate', NULL, '100'),
	(10, 15, 3, '2018-05-16 10:57:53.525359', '2018-05-16 10:57:53.525358', 0, 5, 'CHOCOLATE_COLLECTOR', NULL, NULL),
	(11, 17, 1, '2018-05-16 11:03:12.926001', '2018-05-16 11:03:12.925999', 1, 5, 'Chocolate', NULL, '100'),
	(12, 17, 3, '2018-05-16 11:03:12.968890', '2018-05-16 11:03:12.968889', 0, 5, 'CHOCOLATE_COLLECTOR', NULL, NULL),
	(13, 4, 1, '2018-05-16 11:03:12.926001', '2018-05-16 11:03:12.925999', 1, 5, 'Chocolate', NULL, '89'),
	(14, 5, 1, '2018-05-16 11:03:12.926001', '2018-05-16 12:03:23.850653', 1, 5, 'Chocolate', NULL, '33'),
	(15, 6, 1, '2018-05-16 11:03:12.926001', '2018-05-16 11:03:12.925999', 1, 5, 'Chocolate', NULL, '122'),
	(16, 7, 1, '2018-05-16 11:03:12.926001', '2018-05-16 12:03:29.739528', 1, 5, 'Chocolate', NULL, '107'),
	(17, 8, 1, '2018-05-16 11:03:12.926001', '2018-05-16 11:07:20.958824', 1, 5, 'Chocolate', NULL, '79'),
	(18, 9, 1, '2018-05-16 11:03:12.926001', '2018-05-16 11:03:12.925999', 1, 5, 'Chocolate', NULL, '28'),
	(19, 10, 1, '2018-05-16 11:03:12.926001', '2018-05-16 11:03:12.925999', 1, 5, 'Chocolate', NULL, '8'),
	(20, 11, 1, '2018-05-16 11:03:12.926001', '2018-05-16 11:03:12.925999', 1, 5, 'Chocolate', NULL, '80'),
	(21, 12, 1, '2018-05-16 11:03:12.926001', '2018-05-16 11:03:12.925999', 1, 5, 'Chocolate', NULL, '85'),
	(22, 13, 1, '2018-05-16 11:03:12.926001', '2018-05-16 11:07:19.877238', 1, 5, 'Chocolate', NULL, '22'),
	(23, 14, 1, '2018-05-16 11:03:12.926001', '2018-05-16 11:03:12.925999', 1, 5, 'Chocolate', NULL, '57'),
	(24, 19, 1, '2018-05-16 11:06:55.756980', '2018-05-16 11:07:17.364974', 1, 5, 'Chocolate', NULL, '59'),
	(25, 19, 3, '2018-05-16 11:06:55.798130', '2018-05-16 11:06:55.798128', 0, 5, 'CHOCOLATE_COLLECTOR', NULL, NULL),
	(26, 19, 0, '2018-05-16 11:06:59.812254', '2018-05-16 11:06:59.812253', 1, 5, 'FRIENDS_MADE', NULL, '1'),
	(27, 19, 3, '2018-05-16 11:06:59.858844', '2018-05-16 11:06:59.858843', 0, 5, 'FRIENDLY_SORT', NULL, NULL),
	(28, 19, 0, '2018-05-16 11:07:01.758560', '2018-05-16 11:07:01.758559', 1, 5, 'FRIENDS_MADE', NULL, '1'),
	(29, 19, 0, '2018-05-16 11:07:15.216891', '2018-05-16 11:07:15.216890', 1, 5, 'CHOCOLATE_SHARED', NULL, '5'),
	(30, 19, 0, '2018-05-16 11:07:16.148863', '2018-05-16 11:07:16.148862', 1, 5, 'CHOCOLATE_SHARED', NULL, '25'),
	(31, 20, 1, '2018-05-16 11:07:16.685045', '2018-05-16 11:07:20.951778', 1, 5, 'Chocolate', NULL, '5'),
	(32, 19, 0, '2018-05-16 11:07:16.960919', '2018-05-16 11:07:16.960918', 1, 5, 'CHOCOLATE_SHARED', NULL, '10'),
	(33, 19, 0, '2018-05-16 11:07:17.601190', '2018-05-16 11:07:17.601189', 1, 5, 'CHOCOLATE_SHARED', NULL, '1'),
	(34, 21, 1, '2018-05-16 12:02:56.590896', '2018-05-16 12:03:40.036297', 1, 5, 'Chocolate', NULL, '96'),
	(35, 21, 3, '2018-05-16 12:02:56.627489', '2018-05-16 12:02:56.627488', 0, 5, 'CHOCOLATE_COLLECTOR', NULL, NULL),
	(36, 21, 0, '2018-05-16 12:03:06.197176', '2018-05-16 12:03:06.197175', 1, 5, 'FRIENDS_MADE', NULL, '1'),
	(37, 21, 3, '2018-05-16 12:03:06.243068', '2018-05-16 12:03:06.243067', 0, 5, 'FRIENDLY_SORT', NULL, NULL),
	(38, 21, 0, '2018-05-16 12:03:24.292197', '2018-05-16 12:03:24.292196', 1, 5, 'CHOCOLATE_SHARED', NULL, '1'),
	(39, 22, 1, '2018-05-16 12:03:25.211905', '2018-05-16 12:03:40.029298', 1, 5, 'Chocolate', NULL, '2'),
	(40, 21, 0, '2018-05-16 12:03:25.657281', '2018-05-16 12:03:25.657280', 1, 5, 'CHOCOLATE_SHARED', NULL, '5'),
	(41, 23, 1, '2018-05-16 12:10:38.558025', '2018-05-16 12:10:38.558024', 1, 5, 'Chocolate', NULL, '100'),
	(42, 23, 3, '2018-05-16 12:10:38.598441', '2018-05-16 12:10:38.598440', 0, 5, 'CHOCOLATE_COLLECTOR', NULL, NULL);
/*!40000 ALTER TABLE `EvaluationData` ENABLE KEYS */;

-- Dumping data for table SUGAR.Evaluations: ~10 rows (approximately)
/*!40000 ALTER TABLE `Evaluations` DISABLE KEYS */;
REPLACE INTO `Evaluations` (`Id`, `ActorType`, `Description`, `Discriminator`, `GameId`, `Name`, `Token`) VALUES
	(1, 1, 'This is an achievement.', 'Achievement', 2, 'Chocolate Collector', 'CHOCOLATE_COLLECTOR'),
	(2, 1, 'Add someone as a friend', 'Achievement', 2, 'Friendly Sort', 'FRIENDLY_SORT'),
	(3, 2, 'Group has at least 100 chocolate', 'Achievement', 2, 'Collective Craves Chocolate', 'COLLECTIVE_CRAVES'),
	(4, 1, 'Share 50 chocolate', 'Skill', 2, 'Sharing is Caring', 'SHARING_IS_CARING'),
	(5, 2, 'Group members have shared 100 chocolate', 'Skill', 2, 'One for All', 'ONE_FOR_ALL'),
	(6, 1, 'This is an achievement.', 'Achievement', 5, 'Chocolate Collector', 'CHOCOLATE_COLLECTOR'),
	(7, 1, 'Add someone as a friend', 'Achievement', 5, 'Friendly Sort', 'FRIENDLY_SORT'),
	(8, 2, 'Group has at least 100 chocolate', 'Achievement', 5, 'Collective Craves Chocolate', 'COLLECTIVE_CRAVES'),
	(9, 1, 'Share 50 chocolate', 'Skill', 5, 'Sharing is Caring', 'SHARING_IS_CARING'),
	(10, 2, 'Group members have shared 100 chocolate', 'Skill', 5, 'One for All', 'ONE_FOR_ALL');
/*!40000 ALTER TABLE `Evaluations` ENABLE KEYS */;

-- Dumping data for table SUGAR.Games: ~5 rows (approximately)
/*!40000 ALTER TABLE `Games` DISABLE KEYS */;
REPLACE INTO `Games` (`Id`, `Name`) VALUES
	(5, 'SUGAR-Demo');
/*!40000 ALTER TABLE `Games` ENABLE KEYS */;

-- Dumping data for table SUGAR.Leaderboards: ~4 rows (approximately)
/*!40000 ALTER TABLE `Leaderboards` DISABLE KEYS */;
REPLACE INTO `Leaderboards` (`Token`, `GameId`, `ActorType`, `CriteriaScope`, `EvaluationDataCategory`, `EvaluationDataKey`, `EvaluationDataType`, `LeaderboardType`, `Name`) VALUES
	('MOST_CHOCOLATE_GROUP', 2, 2, 0, 1, 'Chocolate', 1, 0, 'Most Chocolate (Group)'),
	('MOST_CHOCOLATE_GROUP', 5, 2, 0, 1, 'Chocolate', 1, 0, 'Most Chocolate (Group)'),
	('MOST_CHOCOLATE_USER', 2, 1, 0, 1, 'Chocolate', 1, 0, 'Most Chocolate (User)'),
	('MOST_CHOCOLATE_USER', 5, 1, 0, 1, 'Chocolate', 1, 0, 'Most Chocolate (User)');
/*!40000 ALTER TABLE `Leaderboards` ENABLE KEYS */;

-- Dumping data for table SUGAR.Matches: ~0 rows (approximately)
/*!40000 ALTER TABLE `Matches` DISABLE KEYS */;
/*!40000 ALTER TABLE `Matches` ENABLE KEYS */;

-- Dumping data for table SUGAR.RelationshipRequests: ~0 rows (approximately)
/*!40000 ALTER TABLE `RelationshipRequests` DISABLE KEYS */;
/*!40000 ALTER TABLE `RelationshipRequests` ENABLE KEYS */;

-- Dumping data for table SUGAR.Relationships: ~4 rows (approximately)
/*!40000 ALTER TABLE `Relationships` DISABLE KEYS */;
REPLACE INTO `Relationships` (`RequestorId`, `AcceptorId`) VALUES
	(1, 11),
	(1, 12),
	(1, 13),
	(1, 14);
/*!40000 ALTER TABLE `Relationships` ENABLE KEYS */;

-- Dumping data for table SUGAR.Rewards: ~0 rows (approximately)
/*!40000 ALTER TABLE `Rewards` DISABLE KEYS */;
/*!40000 ALTER TABLE `Rewards` ENABLE KEYS */;

-- Dumping data for table SUGAR.RoleClaims: ~93 rows (approximately)
/*!40000 ALTER TABLE `RoleClaims` DISABLE KEYS */;
REPLACE INTO `RoleClaims` (`RoleId`, `ClaimId`) VALUES
	(1, 3),
	(1, 10),
	(1, 27),
	(1, 29),
	(1, 30),
	(1, 35),
	(1, 37),
	(1, 52),
	(1, 55),
	(1, 59),
	(1, 64),
	(1, 65),
	(1, 66),
	(1, 67),
	(1, 71),
	(1, 79),
	(1, 86),
	(1, 89),
	(2, 4),
	(2, 7),
	(2, 11),
	(2, 12),
	(2, 13),
	(2, 22),
	(2, 32),
	(2, 41),
	(2, 44),
	(2, 45),
	(2, 48),
	(2, 50),
	(2, 53),
	(2, 57),
	(2, 60),
	(2, 61),
	(2, 62),
	(2, 63),
	(2, 74),
	(2, 77),
	(2, 78),
	(2, 84),
	(2, 87),
	(2, 90),
	(3, 2),
	(3, 6),
	(3, 9),
	(3, 15),
	(3, 17),
	(3, 19),
	(3, 23),
	(3, 33),
	(3, 34),
	(3, 36),
	(3, 43),
	(3, 47),
	(3, 51),
	(3, 54),
	(3, 58),
	(3, 68),
	(3, 69),
	(3, 73),
	(3, 76),
	(3, 80),
	(3, 81),
	(3, 82),
	(3, 83),
	(3, 85),
	(3, 88),
	(3, 91),
	(3, 92),
	(4, 5),
	(4, 8),
	(4, 14),
	(4, 16),
	(4, 18),
	(4, 20),
	(4, 21),
	(4, 24),
	(4, 25),
	(4, 26),
	(4, 28),
	(4, 42),
	(4, 46),
	(4, 49),
	(4, 56),
	(4, 70),
	(4, 72),
	(4, 75),
	(4, 93),
	(5, 1),
	(6, 31),
	(6, 38),
	(6, 39),
	(6, 40);
/*!40000 ALTER TABLE `RoleClaims` ENABLE KEYS */;

-- Dumping data for table SUGAR.Roles: ~6 rows (approximately)
/*!40000 ALTER TABLE `Roles` DISABLE KEYS */;
REPLACE INTO `Roles` (`Id`, `ClaimScope`, `Default`, `Name`) VALUES
	(1, 0, b'1', 'Global'),
	(2, 1, b'1', 'Game'),
	(3, 2, b'1', 'Group'),
	(4, 3, b'1', 'User'),
	(5, 4, b'1', 'Account'),
	(6, 5, b'1', 'Role');
/*!40000 ALTER TABLE `Roles` ENABLE KEYS */;