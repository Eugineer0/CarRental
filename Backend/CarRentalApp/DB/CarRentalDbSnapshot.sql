USE [CarRentalDB]
GO
/****** Object:  Table [dbo].[Cars]    Script Date: 3/25/2022 6:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cars](
	[Id] [uniqueidentifier] NOT NULL,
	[RegistrationNumber] [nchar](7) NOT NULL,
	[TypeId] [int] NOT NULL,
	[RentalCenterId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Cars] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CarTypes]    Script Date: 3/25/2022 6:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Brand] [nvarchar](32) NOT NULL,
	[Model] [nvarchar](64) NOT NULL,
	[SeatPlaces] [int] NOT NULL,
	[AverageConsumption] [float] NOT NULL,
	[GearboxType] [tinyint] NOT NULL,
	[Weight] [int] NOT NULL,
	[Length] [int] NOT NULL,
	[Power] [int] NOT NULL,
	[PricePerMinute] [decimal](18, 2) NOT NULL,
	[PricePerHour] [decimal](18, 2) NOT NULL,
	[PricePerDay] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_CarTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 3/25/2022 6:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StartRent] [datetime2](7) NOT NULL,
	[FinishRent] [datetime2](7) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[CarId] [uniqueidentifier] NOT NULL,
	[RentalCenterId] [uniqueidentifier] NOT NULL,
	[OverallPrice] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderServices]    Script Date: 3/25/2022 6:52:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderServices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
 CONSTRAINT [PK_OrderServices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshTokens]    Script Date: 3/25/2022 6:52:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshTokens](
	[Id] [uniqueidentifier] NOT NULL,
	[Token] [nvarchar](max) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RentalCenters]    Script Date: 3/25/2022 6:52:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentalCenters](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](32) NOT NULL,
	[Country] [nvarchar](64) NOT NULL,
	[City] [nvarchar](64) NOT NULL,
	[Address] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_RentalCenters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServicePrices]    Script Date: 3/25/2022 6:52:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServicePrices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [int] NOT NULL,
	[CarTypeId] [int] NOT NULL,
 CONSTRAINT [PK_ServicePrices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Services]    Script Date: 3/25/2022 6:52:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 3/25/2022 6:52:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Role] [tinyint] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/25/2022 6:52:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](254) NOT NULL,
	[Username] [nvarchar](25) NOT NULL,
	[HashedPassword] [binary](32) NOT NULL,
	[Salt] [binary](32) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Surname] [nvarchar](64) NOT NULL,
	[PassportNumber] [nchar](9) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[DriverLicenseSerialNumber] [nchar](9) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Cars] ([Id], [RegistrationNumber], [TypeId], [RentalCenterId]) VALUES (N'b26e669f-c2ed-42b0-bc72-0fba9e8f0d0f', N'5IR8512', 1, N'02e0d475-a516-4ffb-a68a-faa999faaad7')
INSERT [dbo].[Cars] ([Id], [RegistrationNumber], [TypeId], [RentalCenterId]) VALUES (N'23e07444-97f8-4f22-929f-16768fe24636', N'7HG2537', 4, N'02e0d475-a516-4ffb-a68a-faa999faaad7')
INSERT [dbo].[Cars] ([Id], [RegistrationNumber], [TypeId], [RentalCenterId]) VALUES (N'47e6a076-548a-4e91-87b8-2a77b70ace0f', N'7AV7823', 1, N'09abdf10-4f9e-4ad0-b4c3-690d2914a68a')
INSERT [dbo].[Cars] ([Id], [RegistrationNumber], [TypeId], [RentalCenterId]) VALUES (N'6fefc7a5-48cf-4acf-a714-462f77b6518d', N'5LK8965', 4, N'02e0d475-a516-4ffb-a68a-faa999faaad7')
INSERT [dbo].[Cars] ([Id], [RegistrationNumber], [TypeId], [RentalCenterId]) VALUES (N'1b0eeac3-57db-436b-9520-7c3b01bbd143', N'5PO1257', 2, N'02e0d475-a516-4ffb-a68a-faa999faaad7')
INSERT [dbo].[Cars] ([Id], [RegistrationNumber], [TypeId], [RentalCenterId]) VALUES (N'fa7534cb-aea2-474d-a4bf-e26397b703c3', N'5IU8562', 3, N'02e0d475-a516-4ffb-a68a-faa999faaad7')
INSERT [dbo].[Cars] ([Id], [RegistrationNumber], [TypeId], [RentalCenterId]) VALUES (N'fdc8148d-c6c1-4306-bc5e-f67c4d5508a2', N'7RT4356', 5, N'02e0d475-a516-4ffb-a68a-faa999faaad7')
GO
SET IDENTITY_INSERT [dbo].[CarTypes] ON 

INSERT [dbo].[CarTypes] ([Id], [Brand], [Model], [SeatPlaces], [AverageConsumption], [GearboxType], [Weight], [Length], [Power], [PricePerMinute], [PricePerHour], [PricePerDay]) VALUES (1, N'Volvo', N'XC90', 5, 8.5, 1, 1800, 5000, 180, CAST(1.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)))
INSERT [dbo].[CarTypes] ([Id], [Brand], [Model], [SeatPlaces], [AverageConsumption], [GearboxType], [Weight], [Length], [Power], [PricePerMinute], [PricePerHour], [PricePerDay]) VALUES (2, N'Volkswagen', N'Polo', 5, 5.5, 0, 1300, 4400, 100, CAST(0.40 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
INSERT [dbo].[CarTypes] ([Id], [Brand], [Model], [SeatPlaces], [AverageConsumption], [GearboxType], [Weight], [Length], [Power], [PricePerMinute], [PricePerHour], [PricePerDay]) VALUES (3, N'Land Rover', N'Range Rover Velar', 7, 10, 1, 2200, 5400, 470, CAST(2.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(650.00 AS Decimal(18, 2)))
INSERT [dbo].[CarTypes] ([Id], [Brand], [Model], [SeatPlaces], [AverageConsumption], [GearboxType], [Weight], [Length], [Power], [PricePerMinute], [PricePerHour], [PricePerDay]) VALUES (4, N'Skoda', N'Rapid', 5, 5.7, 0, 1350, 4500, 110, CAST(0.50 AS Decimal(18, 2)), CAST(12.00 AS Decimal(18, 2)), CAST(55.00 AS Decimal(18, 2)))
INSERT [dbo].[CarTypes] ([Id], [Brand], [Model], [SeatPlaces], [AverageConsumption], [GearboxType], [Weight], [Length], [Power], [PricePerMinute], [PricePerHour], [PricePerDay]) VALUES (5, N'Kia', N'Rio', 5, 5.3, 0, 1300, 4100, 120, CAST(0.50 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[CarTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([Id], [StartRent], [FinishRent], [UserId], [CarId], [RentalCenterId], [OverallPrice]) VALUES (1, CAST(N'1990-01-01T00:00:00.0000000' AS DateTime2), CAST(N'1990-01-02T00:00:00.0000000' AS DateTime2), N'4d68224f-e23f-4238-b287-08d9fa9849de', N'47e6a076-548a-4e91-87b8-2a77b70ace0f', N'09abdf10-4f9e-4ad0-b4c3-690d2914a68a', CAST(500.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'51b74827-c36f-4c31-a691-08d9fc57a5a0', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA3NjJ9.21BLTgvcy725Faxb7SvpY1yT8aIHk1U73uxXd0-bQNM', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'422587d2-862c-40bf-a692-08d9fc57a5a0', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA3NjR9.FFanx10d5Rd-5h2BCVzU5mo_55vJKt1epTWobqeIFtg', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'3a246802-2527-4fe9-a693-08d9fc57a5a0', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA3NjV9.gVTPUQ9EoTME4_GeKFq2pAszVEodnmMdFABebyN7cXg', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'a104b17c-b409-4ceb-a694-08d9fc57a5a0', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA3NjV9.gVTPUQ9EoTME4_GeKFq2pAszVEodnmMdFABebyN7cXg', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'd5efed3f-9bfd-4c0c-a695-08d9fc57a5a0', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA3NjZ9.VGdwGkHu_ETysvlZVoI1yLwPOsGy3Fy6Uh99VzAFl_k', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'e3cab1d9-430f-4257-a696-08d9fc57a5a0', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA3NjZ9.VGdwGkHu_ETysvlZVoI1yLwPOsGy3Fy6Uh99VzAFl_k', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'31e7eceb-de0f-44e9-a697-08d9fc57a5a0', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA3Njd9.bKMbyB2HNZuw72AhoC8yz_FkkNv7lR42vsXIzTb06N0', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'0eb8c035-1680-4b2d-a698-08d9fc57a5a0', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA3Njh9.C4UrrJu7kLdkxheK64GDX5Cpc5PdLs8pA7uM8V026Ic', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'ee73ca03-58ea-4384-7af6-08d9fc582a3f', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA5ODR9.sjmfpxlnAxP2pPpy-qkEQm06zS4FRkPL214qAlZ3S0Y', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'1cf45fc9-1b0f-47f4-7af7-08d9fc582a3f', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA5ODV9.j2FQnBMcQZqNgJrlCbzq6jNoL5ue1GBRlpvfQ9HYNTk', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'37835d13-1a11-4e4e-7af8-08d9fc582a3f', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA5ODZ9.v1H6Bsua65Y62x_z8qaXgSRsXq3_tCbc-7fStRAO-pA', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'6ffa860a-e0a2-40e4-7af9-08d9fc582a3f', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA5ODZ9.v1H6Bsua65Y62x_z8qaXgSRsXq3_tCbc-7fStRAO-pA', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'dce24b27-e70c-4ca0-7afa-08d9fc582a3f', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA5ODd9.c1L0XNDuOBo43yXqgOzL6I-PV0NX2TgBNB2be_SGbxo', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'76360462-cb9b-4bbe-7afb-08d9fc582a3f', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA5ODd9.c1L0XNDuOBo43yXqgOzL6I-PV0NX2TgBNB2be_SGbxo', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'cefa9927-c7b6-483a-7afc-08d9fc582a3f', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA5ODh9.Z7rzbDrlCST1u3wW5orrVV1UByTsYNhh8jTocLbOmLI', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'f8bae13a-1222-428f-7afd-08d9fc582a3f', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA5ODh9.Z7rzbDrlCST1u3wW5orrVV1UByTsYNhh8jTocLbOmLI', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'1dfc27e5-ebc3-4550-7afe-08d9fc582a3f', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1MzA5ODl9._0L3313t_sjDNSTQUCHaU1K_S5k8CaQB_pNTGrKvxsc', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'08d3f356-35ef-4648-5997-08d9fc683723', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY1Mzc4Nzh9.reVUIkl5kkMRVGGQ_VGVSxfFTHfPBhpc0ElLmbLKfgE', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'cd2dac57-503c-44a7-3e7c-08d9fcfcaa0b', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY2MDE2MzZ9.VicDYjF_fv_CBZMqQiHriuzJL8Wxq5bYLJOj7DbgY4s', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'4ef2d954-bc78-43a7-71fc-08d9fcfcbd0e', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY2MDE2Njh9.2j-D9RMvnb-F6S7pYjFrvGLqT1awQ3Q13G_om8ZxbXc', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'56590ebb-bb40-49d9-da46-08d9fcfd91ce', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY2MDIwMjV9.muqksKGSQTXSuhrWh2GBLSmVraX9pN3QQp56gJSjKmw', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'80587bb5-8d3e-4e31-1a80-08d9fcfdbf62', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY2MDIxMDF9._LV8O8INptsypeiTC5ZJgDVjXKD2RtGO9BxIKk_WIJk', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'db058db5-94a8-4119-9794-08d9fd08f9f8', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY2MDY5MjR9.eW3ZovKg4o9yR2B_50-PB0Fk_rui6OHYaDoY1Np34nw', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'3d604217-042e-4b6d-42c8-08d9fd09fec9', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY2MDczNjJ9.pAt-wALU1KGwVoCpnJpBQ1HnCzUl2_p0BcAXJZ-zKPg', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'12e13699-15a4-474b-73c4-08d9fd0a588b', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY2MDc1MTJ9.C-pd56B5q4KmdwHTvcMpEvlLApQmRjBWa30VRECt4PI', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'e28fae80-1a9d-49cf-73c5-08d9fd0a588b', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY2MDc3NDV9.ET3mK0NgfwiJEC4drxO4i4n9vto3OmQs_q4bWfkfi1U', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'59d16602-eea0-4cdb-0eac-08d9fdbfceb4', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY2ODU0NDl9.5FVKQxeuCdHleEYXrl92cfLk0yALSS6rCvbuEoewSHE', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'170091b0-aa0f-443e-deef-08d9fdc0d204', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY2ODU4ODR9.NmyPtqjGN8ccAMF53C36rkjhWNQH5SaiF8VuD3eH6L8', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'd31d3ab4-bebc-4976-2e67-08d9fdcbd3d8', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY2OTA2MTJ9.0ct3tXdmQnz4UBPENHX_w9py13l96bGrwdSv3sCMX-s', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'dc19fe0c-1332-4e81-6f2a-08d9fdcd15cf', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDY3MDI5NzB9.XUQ-fsX-4zpQy0EJPCqSJdK1wppFisoAjUzMk5WZExs', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'6965ab8e-21e8-41b5-d259-08da01ac987a', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDcxMTcwMDN9.Av4dT7SOpAVpQ14VmLrwvTw4ZXiBPdEctX7iKQuPsaw', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'c3ac7063-494a-465d-8575-08da01bb751f', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDcxMjMzODZ9.4_i5BB7wmt1og_5ogvCxPneV7fJ1tfwSuqqFnYf30JQ', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'92ec780d-d56f-4d48-3004-08da01d496ac', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDcxMzQxNzl9.8_EXkrljJU7anjBYcZ_cL8iBaYGFDcCvXHHQ6Uz0qk8', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'b643da5a-eaa3-4a40-75a5-08da05828384', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDc1Mzg3MzN9.RIk3y3Qs2UOI48Nac5yR68cnKusp6SYotVtb2Ndf4KE', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'a7bf1f80-a2fd-42b5-092e-08da08fd5933', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxZGU0NDM3Mi05NDk0LTQ1MTktNjIwMy0wOGRhMDM2NzU3NWYiLCJleHAiOjE2NDc5MjEzNDR9.bJCFtlbTSjSF9C2ZFx0AJ6lMpuhpPrm64L9nyLl_S3U', N'1de44372-9494-4519-6203-08da0367575f')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'6456318c-b0eb-4699-092f-08da08fd5933', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxZGU0NDM3Mi05NDk0LTQ1MTktNjIwMy0wOGRhMDM2NzU3NWYiLCJleHAiOjE2NDc5MjE1ODd9.XPbc_Q6wI3xn5EMxnYOrxab7-tIL7QLDQwjDI-Rs8QY', N'1de44372-9494-4519-6203-08da0367575f')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'4f9ad394-d77f-4913-0930-08da08fd5933', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxZGU0NDM3Mi05NDk0LTQ1MTktNjIwMy0wOGRhMDM2NzU3NWYiLCJleHAiOjE2NDc5MjE5ODV9.igeD960KZoqn8MMI_n3bZPmS8a2DLDebYe5JMoniaxY', N'1de44372-9494-4519-6203-08da0367575f')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'c8236783-bc2d-41da-0931-08da08fd5933', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxZGU0NDM3Mi05NDk0LTQ1MTktNjIwMy0wOGRhMDM2NzU3NWYiLCJleHAiOjE2NDc5MjIwODN9.Cx39d7FEK4vuPuHrAyJdsKgDNBp8l7hT5DiRQ6yME_s', N'1de44372-9494-4519-6203-08da0367575f')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'cccc11ef-1aae-412e-40fe-08da0b04f772', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI5MzllZGUzOS00ZGIxLTQ3ZTItYjI4OC0wOGQ5ZmE5ODQ5ZGUiLCJleHAiOjE2NDgxNDUwMjZ9.xOYBqfxtTo7d02D2Jnx3OUNdsMYr1-w-64BuRVXD02I', N'939ede39-4db1-47e2-b288-08d9fa9849de')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'e50ab52f-feed-4b5f-40ff-08da0b04f772', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI5MzllZGUzOS00ZGIxLTQ3ZTItYjI4OC0wOGQ5ZmE5ODQ5ZGUiLCJleHAiOjE2NDgxNDUzNzN9.PKVmKns13ZJnvW56ErIbURGC_7imkezmgF4QIITuyYI', N'939ede39-4db1-47e2-b288-08d9fa9849de')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'4f7be4c2-4b30-47ce-4100-08da0b04f772', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI5MzllZGUzOS00ZGIxLTQ3ZTItYjI4OC0wOGQ5ZmE5ODQ5ZGUiLCJleHAiOjE2NDgxNDU0MDF9.c7iLg9qkkz6x1Lahx-uj7ZVbEQ7C0VJSSWBAwSGMPzg', N'939ede39-4db1-47e2-b288-08d9fa9849de')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'7b9458bd-42aa-4b0e-fcbe-08da0b1580c9', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI5MzllZGUzOS00ZGIxLTQ3ZTItYjI4OC0wOGQ5ZmE5ODQ5ZGUiLCJleHAiOjE2NDgxNTE2MjB9.mNwVzkUj1VrrZy_Z876wDB6exhB6eroDsdvyO2-N_lM', N'939ede39-4db1-47e2-b288-08d9fa9849de')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'd9e8b62d-5682-466c-fcbf-08da0b1580c9', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI5MzllZGUzOS00ZGIxLTQ3ZTItYjI4OC0wOGQ5ZmE5ODQ5ZGUiLCJleHAiOjE2NDgxNTE2Mzl9.TTmcGcsLI9RSJPOe0nyzECqrL5xXklKd1MU4Q4z6vlQ', N'939ede39-4db1-47e2-b288-08d9fa9849de')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'b9ea2239-1607-41a5-2eb2-08da0beea1a8', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI5MzllZGUzOS00ZGIxLTQ3ZTItYjI4OC0wOGQ5ZmE5ODQ5ZGUiLCJleHAiOjE2NDgyNDQ4NzZ9.wWBccp6lnfiqd5JHA_AJEzF8QvXWuaoxYrFbVckp1pk', N'939ede39-4db1-47e2-b288-08d9fa9849de')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'2a9ce920-ef53-4c5b-2eb3-08da0beea1a8', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI5MzllZGUzOS00ZGIxLTQ3ZTItYjI4OC0wOGQ5ZmE5ODQ5ZGUiLCJleHAiOjE2NDgyNDU3Nzd9.3I4y9FpexNlFwrp8uGBFohnp3NwpQp9lCWCLUoHhdIQ', N'939ede39-4db1-47e2-b288-08d9fa9849de')
INSERT [dbo].[RefreshTokens] ([Id], [Token], [UserId]) VALUES (N'a341fc0f-1cb9-49bd-7e13-08da0e645b5d', N'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiNmMzNmExOS04YWNmLTRjM2EtZjNhYy0wOGQ5Zjg0MThkMzUiLCJleHAiOjE2NDg1MTUzNDF9.0drDdHFNE8cHH3B0IvjV6m2zdv6tqL9z2_xeyPdLIRU', N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
GO
INSERT [dbo].[RentalCenters] ([Id], [Name], [Country], [City], [Address]) VALUES (N'09abdf10-4f9e-4ad0-b4c3-690d2914a68a', N'AnyTime', N'Belarus', N'Grodno', N'Lenina 7')
INSERT [dbo].[RentalCenters] ([Id], [Name], [Country], [City], [Address]) VALUES (N'02e0d475-a516-4ffb-a68a-faa999faaad7', N'Hello', N'Belarus', N'Minsk', N'Nezavisimosti 4')
GO
SET IDENTITY_INSERT [dbo].[ServicePrices] ON 

INSERT [dbo].[ServicePrices] ([Id], [ServiceId], [CarTypeId]) VALUES (1, 1, 2)
INSERT [dbo].[ServicePrices] ([Id], [ServiceId], [CarTypeId]) VALUES (2, 2, 1)
INSERT [dbo].[ServicePrices] ([Id], [ServiceId], [CarTypeId]) VALUES (3, 3, 1)
SET IDENTITY_INSERT [dbo].[ServicePrices] OFF
GO
SET IDENTITY_INSERT [dbo].[Services] ON 

INSERT [dbo].[Services] ([Id], [Name]) VALUES (1, N'Full tank')
INSERT [dbo].[Services] ([Id], [Name]) VALUES (2, N'Clean interior')
INSERT [dbo].[Services] ([Id], [Name]) VALUES (3, N'Child seat')
SET IDENTITY_INSERT [dbo].[Services] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 

INSERT [dbo].[UserRoles] ([Id], [Role], [UserId]) VALUES (40, 2, N'4d68224f-e23f-4238-b287-08d9fa9849de')
INSERT [dbo].[UserRoles] ([Id], [Role], [UserId]) VALUES (41, 1, N'4d68224f-e23f-4238-b287-08d9fa9849de')
INSERT [dbo].[UserRoles] ([Id], [Role], [UserId]) VALUES (45, 1, N'939ede39-4db1-47e2-b288-08d9fa9849de')
INSERT [dbo].[UserRoles] ([Id], [Role], [UserId]) VALUES (46, 0, N'1de44372-9494-4519-6203-08da0367575f')
INSERT [dbo].[UserRoles] ([Id], [Role], [UserId]) VALUES (47, 3, N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
INSERT [dbo].[UserRoles] ([Id], [Role], [UserId]) VALUES (48, 2, N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35')
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
INSERT [dbo].[Users] ([Id], [Email], [Username], [HashedPassword], [Salt], [Name], [Surname], [PassportNumber], [DateOfBirth], [DriverLicenseSerialNumber]) VALUES (N'b6c36a19-8acf-4c3a-f3ac-08d9f8418d35', N'eugin.bazhenov@gmail.com', N'Qwer1', 0xB1019889107BE55CCF1BE794F54673F27CB41A99B7662E44F47EF96B8A1FECFB, 0x69ECA44126F0F0007500A88E2543453FDC79CF0BBFDFB55018013D6EEDB7C710, N'Evgen', N'Bazhenov', N'MC4536737', CAST(N'1990-01-01T00:00:00.0000000' AS DateTime2), NULL)
INSERT [dbo].[Users] ([Id], [Email], [Username], [HashedPassword], [Salt], [Name], [Surname], [PassportNumber], [DateOfBirth], [DriverLicenseSerialNumber]) VALUES (N'4d68224f-e23f-4238-b287-08d9fa9849de', N'Qwer@mail.com', N'Qwer', 0x305554B03FB8224134B2600A9C06495631483D672C5D275B6CF08BF67508440F, 0x68B79B60E429D7BA063E0E39B06DFB5197DEF116C8BC821F2526C9808F257117, N'Evgeniy', N'Bazhenov', N'MC1730569', CAST(N'1995-04-07T00:00:00.0000000' AS DateTime2), N'3AS789402')
INSERT [dbo].[Users] ([Id], [Email], [Username], [HashedPassword], [Salt], [Name], [Surname], [PassportNumber], [DateOfBirth], [DriverLicenseSerialNumber]) VALUES (N'939ede39-4db1-47e2-b288-08d9fa9849de', N'Qwert@mail.com', N'Qwert', 0x951548BFDC63824A49D2460756FDEF64E2266EFD0449A13AF3786D5FB1CC75CA, 0x0F6F7AAEA8C2168E6CF0F2759109932A39A43A9519E7AFD000E9CD9202695C58, N'Evgeniy', N'Bazhenov', N'MC1730569', CAST(N'1995-04-07T00:00:00.0000000' AS DateTime2), N'3AS789402')
INSERT [dbo].[Users] ([Id], [Email], [Username], [HashedPassword], [Salt], [Name], [Surname], [PassportNumber], [DateOfBirth], [DriverLicenseSerialNumber]) VALUES (N'1de44372-9494-4519-6203-08da0367575f', N'Qwert1@mail.com', N'Qwert1', 0xBD5964C9830E825375ABDA9B9EEE7224495CBC108995F1FE66F2C40B36A504EA, 0x067BC23F385A210D5C5202491F29145B3BAB1FB1C5C8832A0F572D77D62501F5, N'Evgeniy', N'Bazhenov', N'MC1730569', CAST(N'1995-04-07T00:00:00.0000000' AS DateTime2), NULL)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UC_Email]    Script Date: 3/25/2022 6:52:23 PM ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UC_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UC_Username]    Script Date: 3/25/2022 6:52:23 PM ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UC_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ((0.0)) FOR [OverallPrice]
GO
ALTER TABLE [dbo].[Cars]  WITH CHECK ADD  CONSTRAINT [FK_Cars_CarTypes_TypeId] FOREIGN KEY([TypeId])
REFERENCES [dbo].[CarTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cars] CHECK CONSTRAINT [FK_Cars_CarTypes_TypeId]
GO
ALTER TABLE [dbo].[Cars]  WITH CHECK ADD  CONSTRAINT [FK_Cars_RentalCenters_RentalCenterId] FOREIGN KEY([RentalCenterId])
REFERENCES [dbo].[RentalCenters] ([Id])
GO
ALTER TABLE [dbo].[Cars] CHECK CONSTRAINT [FK_Cars_RentalCenters_RentalCenterId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Cars_CarId] FOREIGN KEY([CarId])
REFERENCES [dbo].[Cars] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Cars_CarId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users_UserId]
GO
ALTER TABLE [dbo].[OrderServices]  WITH CHECK ADD  CONSTRAINT [FK_OrderServices_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderServices] CHECK CONSTRAINT [FK_OrderServices_Orders_OrderId]
GO
ALTER TABLE [dbo].[OrderServices]  WITH CHECK ADD  CONSTRAINT [FK_OrderServices_Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderServices] CHECK CONSTRAINT [FK_OrderServices_Services_ServiceId]
GO
ALTER TABLE [dbo].[RefreshTokens]  WITH CHECK ADD  CONSTRAINT [FK_RefreshTokens_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RefreshTokens] CHECK CONSTRAINT [FK_RefreshTokens_Users_UserId]
GO
ALTER TABLE [dbo].[ServicePrices]  WITH CHECK ADD  CONSTRAINT [FK_ServicePrices_CarTypes_CarTypeId] FOREIGN KEY([CarTypeId])
REFERENCES [dbo].[CarTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServicePrices] CHECK CONSTRAINT [FK_ServicePrices_CarTypes_CarTypeId]
GO
ALTER TABLE [dbo].[ServicePrices]  WITH CHECK ADD  CONSTRAINT [FK_ServicePrices_Services_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServicePrices] CHECK CONSTRAINT [FK_ServicePrices_Services_ServiceId]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users_UserId]
GO
