CREATE TABLE dbo.ErrorLog
(
	ErrorLogID int IDENTITY(1,1) Primary Key,
	Message varchar(500) NULL,
	StackTrace varchar(max) NULL,
	InnerException varchar(max) NULL,
	LoggedInDetails varchar(max) NULL,
	QueryData varchar(max) NULL,
	FormData varchar(max) NULL,
	RouteData varchar(max) NULL,
	LoggedAt datetime NULL
)

CREATE TABLE dbo.Users(
	UserID int IDENTITY(1,1) Primary Key,
	Name nvarchar(100) NOT NULL,
	Email nvarchar(100) NOT NULL,
	LastFailedAttempt datetime NULL,
	FailedAttempts int NOT NULL,
	Status int NOT NULL,
	SourceType int NOT NULL,
	DateCreated datetime NOT NULL CONSTRAINT DF_Users_DateCreated  DEFAULT (getutcdate()),
	DateModified datetime NULL,
	DateConfirmed datetime NULL,
	DisplayName varchar(50) NULL,
	CountryID int NULL,
	StateID int NULL,
	OtherState varchar(50) NULL,
	CityID int NULL,
	OtherCity varchar(50) NULL,
	ZipCode varchar(6) NULL,
	Website varchar(200) NULL,
	RoleID int NOT NULL DEFAULT ((1)),
	ProfilePicture varchar(200) NULL,
	Password varbinary(250) NULL,
	RegisterVia int NOT NULL DEFAULT ((1)),
	RegistrationIP varchar(20) NULL,
	ResetPassword bit NOT NULL DEFAULT ((0)),
	PasswordResetCode varchar(50) NULL,
	FacbookUserID varchar(200) NULL,
	TotalEarning decimal(18, 2) NOT NULL DEFAULT ((0))
 )

CREATE TABLE dbo.UserLoginSessions(
	UserLoginSessionID uniqueidentifier Primary Key,
	UserID int Foreign Key References Users(UserID),
	LoggedInTime datetime NOT NULL,
	LoggedOutTime datetime NULL,
	SessionExpired bit NOT NULL DEFAULT ((0)),
	LoggedInDeviceToken varchar(500) NULL,
	DeviceType int NOT NULL
)

If not exists (select 1 from sysobjects where name='Document' and xtype='U') Begin Create Table Document(
	Id int Primary Key Identity(1,1),
	DisplayName Varchar(250) Not Null,
	SavedName Varchar(250) Not Null,
	Mime Varchar(250) Not Null,
	DownloadBinary Varbinary(max) Not Null,
) End