USE [busses]
GO
/****** Object:  Table [dbo].[garageDrivers]    Script Date: 7.4.2021 г. 21:13:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[garageDrivers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[егн] [nvarchar](10) NOT NULL,
	[статус] [nvarchar](100) NULL,
	[качил се] [date] NULL,
	[пристигнал] [date] NULL,
	[курсове] [nvarchar](200) NULL,
	[забележка] [nvarchar](200) NULL,
	[team] [nvarchar](50) NULL,
	[fUser] [nvarchar](32) NULL,
	[fPC] [nvarchar](32) NULL,
	[sysDate] [datetime] NULL,
	[почивал] [int] NULL,
	[работил] [int] NULL,
	[име] [nvarchar](60) NULL,
 CONSTRAINT [PK_garageDrivers] PRIMARY KEY CLUSTERED 
(
	[егн] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[garageLocation]    Script Date: 7.4.2021 г. 21:13:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[garageLocation](
	[Id] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[garageTeam]    Script Date: 7.4.2021 г. 21:13:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[garageTeam](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[спедиция] [nvarchar](200) NULL,
	[влекач] [varchar](50) NOT NULL,
	[ремарке] [varchar](50) NULL,
	[шофьор 1] [nvarchar](60) NULL,
	[шофьор 2] [nvarchar](60) NULL,
	[диспонент] [nvarchar](200) NULL,
	[описание] [nvarchar](800) NULL,
	[локация] [tinyint] NOT NULL,
	[fUser] [nvarchar](32) NULL,
	[fPC] [nvarchar](32) NULL,
	[sysDate] [datetime] NULL,
	[startInside] [date] NULL,
	[readyFor] [date] NULL,
 CONSTRAINT [PK_garageTeam] PRIMARY KEY CLUSTERED 
(
	[влекач] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[garageTrailers]    Script Date: 7.4.2021 г. 21:13:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[garageTrailers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ремарке] [nvarchar](60) NOT NULL,
	[дестинация] [nvarchar](200) NULL,
	[описание] [nvarchar](800) NULL,
	[локация] [tinyint] NOT NULL,
	[fUser] [nvarchar](32) NULL,
	[fPC] [nvarchar](32) NULL,
	[sysDate] [datetime] NULL,
 CONSTRAINT [PK_garageTrailers] PRIMARY KEY CLUSTERED 
(
	[ремарке] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[garageDrivers] ON 

INSERT [dbo].[garageDrivers] ([id], [егн], [статус], [качил се], [пристигнал], [курсове], [забележка], [team], [fUser], [fPC], [sysDate], [почивал], [работил], [име]) VALUES (10, N'1000463572', N'', NULL, NULL, N'', N'', NULL, N'', N'OFFICE227', CAST(N'2021-04-05T16:36:58.223' AS DateTime), NULL, NULL, N'Иван Иванов')
INSERT [dbo].[garageDrivers] ([id], [егн], [статус], [качил се], [пристигнал], [курсове], [забележка], [team], [fUser], [fPC], [sysDate], [почивал], [работил], [име]) VALUES (1, N'1002281341', N'нов', CAST(N'2021-03-15' AS Date), CAST(N'2021-02-15' AS Date), N'вътрешно', N'Профил за тест тест', NULL, N'', N'OFFICE227', CAST(N'2021-03-22T14:38:33.163' AS DateTime), NULL, NULL, N'Георги Андреев')
INSERT [dbo].[garageDrivers] ([id], [егн], [статус], [качил се], [пристигнал], [курсове], [забележка], [team], [fUser], [fPC], [sysDate], [почивал], [работил], [име]) VALUES (8, N'1003963923', N'НОВ', CAST(N'2020-11-02' AS Date), CAST(N'2021-03-04' AS Date), N'вътрешно BG ремарке', N'в Украйна 30.03', NULL, N'admin', N'OFFICE227', CAST(N'2021-03-24T17:13:28.273' AS DateTime), NULL, NULL, N'Стоян Иванов Петров')
INSERT [dbo].[garageDrivers] ([id], [егн], [статус], [качил се], [пристигнал], [курсове], [забележка], [team], [fUser], [fPC], [sysDate], [почивал], [работил], [име]) VALUES (2, N'1621207400', N'ADR', CAST(N'2021-03-05' AS Date), CAST(N'2021-01-20' AS Date), N'RHENUS Gijon', N'', NULL, N'', N'OFFICE227', CAST(N'2021-03-23T11:23:52.687' AS DateTime), NULL, NULL, N'Емо Стоянов')
INSERT [dbo].[garageDrivers] ([id], [егн], [статус], [качил се], [пристигнал], [курсове], [забележка], [team], [fUser], [fPC], [sysDate], [почивал], [работил], [име]) VALUES (7, N'1640429170', N'RO', NULL, NULL, N'', N'', NULL, N'', N'OFFICE227', CAST(N'2021-03-24T14:36:19.657' AS DateTime), NULL, NULL, N'Евгени Дайнов')
INSERT [dbo].[garageDrivers] ([id], [егн], [статус], [качил се], [пристигнал], [курсове], [забележка], [team], [fUser], [fPC], [sysDate], [почивал], [работил], [име]) VALUES (9, N'5003084489', N'', NULL, NULL, N'', N'test', NULL, N'', N'OFFICE227', CAST(N'2021-03-26T11:17:21.453' AS DateTime), NULL, NULL, N'Симеон Генов')
INSERT [dbo].[garageDrivers] ([id], [егн], [статус], [качил се], [пристигнал], [курсове], [забележка], [team], [fUser], [fPC], [sysDate], [почивал], [работил], [име]) VALUES (4, N'6605273567', NULL, CAST(N'2021-02-27' AS Date), CAST(N'2021-03-12' AS Date), N'прави', N'болнични 18.04', NULL, NULL, NULL, CAST(N'2021-03-23T13:14:34.093' AS DateTime), NULL, NULL, N'Димитър Вълков')
INSERT [dbo].[garageDrivers] ([id], [егн], [статус], [качил се], [пристигнал], [курсове], [забележка], [team], [fUser], [fPC], [sysDate], [почивал], [работил], [име]) VALUES (3, N'7002104645', N'', CAST(N'2020-09-08' AS Date), CAST(N'2020-12-22' AS Date), N'вътрешно', N'болнични 06.03- прод', NULL, N'', N'OFFICE227', CAST(N'2021-03-23T13:11:08.677' AS DateTime), NULL, NULL, N'Явор Енчев')
SET IDENTITY_INSERT [dbo].[garageDrivers] OFF
GO
SET IDENTITY_INSERT [dbo].[garageLocation] ON 

INSERT [dbo].[garageLocation] ([Id], [Name]) VALUES (1, N'пътува')
INSERT [dbo].[garageLocation] ([Id], [Name]) VALUES (2, N'гараж Пловдив')
INSERT [dbo].[garageLocation] ([Id], [Name]) VALUES (3, N'база Барселона')
INSERT [dbo].[garageLocation] ([Id], [Name]) VALUES (4, N'паркинг')
SET IDENTITY_INSERT [dbo].[garageLocation] OFF
GO
SET IDENTITY_INSERT [dbo].[garageTeam] ON 

INSERT [dbo].[garageTeam] ([id], [спедиция], [влекач], [ремарке], [шофьор 1], [шофьор 2], [диспонент], [описание], [локация], [fUser], [fPC], [sysDate], [startInside], [readyFor]) VALUES (1, N'LKW', N'PB0001TA', N'PB7777EB', N'Иван Иванов', N'', NULL, N'', 2, NULL, NULL, CAST(N'2021-03-22T14:13:09.690' AS DateTime), NULL, NULL)
INSERT [dbo].[garageTeam] ([id], [спедиция], [влекач], [ремарке], [шофьор 1], [шофьор 2], [диспонент], [описание], [локация], [fUser], [fPC], [sysDate], [startInside], [readyFor]) VALUES (3, N'SESE', N'PB2000KH', N'PB1234', N'Георги Георгиев Петров', N'', N'Нежля', N'TEST', 1, NULL, NULL, CAST(N'2021-03-30T09:40:43.097' AS DateTime), NULL, NULL)
INSERT [dbo].[garageTeam] ([id], [спедиция], [влекач], [ремарке], [шофьор 1], [шофьор 2], [диспонент], [описание], [локация], [fUser], [fPC], [sysDate], [startInside], [readyFor]) VALUES (2, N'SESE', N'PB2857TA', N'PB0593EP', N'Емо Стоянов', N'', N'Зехра', N'', 1, NULL, NULL, CAST(N'2021-03-22T16:03:00.070' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[garageTeam] OFF
GO
SET IDENTITY_INSERT [dbo].[garageTrailers] ON 

INSERT [dbo].[garageTrailers] ([id], [ремарке], [дестинация], [описание], [локация], [fUser], [fPC], [sysDate]) VALUES (1, N'PB0593EP', N'TR', NULL, 2, NULL, NULL, CAST(N'2021-03-31T15:42:06.920' AS DateTime))
SET IDENTITY_INSERT [dbo].[garageTrailers] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [AK_влекач]    Script Date: 7.4.2021 г. 21:13:56 ******/
ALTER TABLE [dbo].[garageTeam] ADD  CONSTRAINT [AK_влекач] UNIQUE NONCLUSTERED 
(
	[влекач] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [AK_ремарке]    Script Date: 7.4.2021 г. 21:13:56 ******/
ALTER TABLE [dbo].[garageTrailers] ADD  CONSTRAINT [AK_ремарке] UNIQUE NONCLUSTERED 
(
	[ремарке] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[garageDrivers] ADD  CONSTRAINT [DF_garageDrivers_sysDate]  DEFAULT (getdate()) FOR [sysDate]
GO
ALTER TABLE [dbo].[garageTeam] ADD  CONSTRAINT [DF_garageTeam_локация]  DEFAULT ((2)) FOR [локация]
GO
ALTER TABLE [dbo].[garageTeam] ADD  CONSTRAINT [DF_garageTeam_sysDate]  DEFAULT (getdate()) FOR [sysDate]
GO
ALTER TABLE [dbo].[garageTrailers] ADD  CONSTRAINT [DF_garageTrailers_локация]  DEFAULT ((4)) FOR [локация]
GO
ALTER TABLE [dbo].[garageTrailers] ADD  CONSTRAINT [DF_garageTrailers_sysDate]  DEFAULT (getdate()) FOR [sysDate]
GO
ALTER TABLE [dbo].[garageTeam]  WITH CHECK ADD  CONSTRAINT [FK_Team_Location] FOREIGN KEY([локация])
REFERENCES [dbo].[garageLocation] ([Id])
GO
ALTER TABLE [dbo].[garageTeam] CHECK CONSTRAINT [FK_Team_Location]
GO
ALTER TABLE [dbo].[garageTrailers]  WITH CHECK ADD  CONSTRAINT [FK_Trailers_Location] FOREIGN KEY([локация])
REFERENCES [dbo].[garageLocation] ([Id])
GO
ALTER TABLE [dbo].[garageTrailers] CHECK CONSTRAINT [FK_Trailers_Location]
GO
