USE [master]
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name='KitchenDelights')
BEGIN
ALTER DATABASE [KitchenDelights] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE [KitchenDelights]
END
GO
CREATE DATABASE [KitchenDelights] COLLATE Latin1_General_100_CI_AI_SC_UTF8
GO

USE [KitchenDelights];
GO

CREATE TABLE [advertisement]
(
[advertisement_id] [int] IDENTITY(1,1) PRIMARY KEY,
[advertisement_image] [varchar](MAX),
[advertisement_link] [varchar](MAX),
[advertisement_status] [bit] NOT NULL
);
GO

CREATE TABLE [role]
(
[role_id] [int] IDENTITY(1,1) PRIMARY KEY,
[role_name] [nvarchar](25)
);
GO

CREATE TABLE [status]
(
[status_id] [int] IDENTITY(1,1) PRIMARY KEY,
[status_name] [nvarchar](25)
);
GO

CREATE TABLE [users]
(
[user_id] [int] IDENTITY(1,1) PRIMARY KEY,
[username] [nvarchar](50),
[first_name] [nvarchar](50),
[middle_name] [nvarchar](50),
[last_name] [nvarchar](50),
[email] [nvarchar](50),
[phone] [nvarchar](12),
[avatar] [varchar](MAX),
[password_hash] [varchar](MAX),
[reset_token] [varchar](10),
[reset_expire] [datetime],
[status_id] [int] NOT NULL FOREIGN KEY REFERENCES [status]([status_id]),
[role_id] [int] NOT NULL FOREIGN KEY REFERENCES [role]([role_id]),
[interaction] [int] NOT NULL,
);
GO

CREATE TABLE [address]
(
[address_id] [int] IDENTITY(1,1) PRIMARY KEY,
[user_id] [int] NOT NULL FOREIGN KEY REFERENCES [users]([user_id]),
[address_details] [nvarchar](MAX)
);
GO

CREATE TABLE [restaurant_recommendation]
(
[restaurant_id] [int] IDENTITY(1,1) PRIMARY KEY,
[user_id] [int] NOT NULL FOREIGN KEY REFERENCES [users]([user_id]),
[restaurant_name] [nvarchar](MAX),
[restaurant_details] [nvarchar](MAX),
[restaurant_location] [nvarchar](MAX),
[featured_image] [nvarchar](MAX)
);
GO

CREATE TABLE [news]
(
[news_id] [int] IDENTITY(1,1) PRIMARY KEY,
[user_id] [int] NOT NULL FOREIGN KEY REFERENCES [users]([user_id]),
[featured_image] [nvarchar](MAX),
[news_title] [nvarchar](MAX),
[news_content] [nvarchar](MAX),
[news_status] [bit] NOT NULL,
[create_date] [datetime]
);
GO

CREATE TABLE [category]
(
[category_id] [int] IDENTITY(1,1) PRIMARY KEY,
[parent_id] [int] NULL FOREIGN KEY REFERENCES [category]([category_id]),
[category_name] [nvarchar](MAX),
[category_type] [bit] NOT NULL
);
GO

CREATE TABLE [countries]
(
[country_id] [int] IDENTITY(1,1) PRIMARY KEY,
[country_name] [nvarchar](MAX)
);
GO

CREATE TABLE [recipe]
(
[recipe_id] [int] IDENTITY(1,1) PRIMARY KEY,
[user_id] [int] NOT NULL FOREIGN KEY REFERENCES [users]([user_id]),
[featured_image] [nvarchar](MAX),
[video_link] [nvarchar](MAX),
[recipe_title] [nvarchar](MAX),
[recipe_serve] [int],
[recipe_content] [nvarchar](MAX),
[recipe_rating] [decimal](2,1) NOT NULL,
[recipe_status] [bit] NOT NULL,
[is_free] [bit] NOT NULL,
[recipe_price] [money],
[create_date] [datetime] NOT NULL
);
GO

CREATE TABLE [recipe_category]
(
[recipe_id] [int] NOT NULL FOREIGN KEY REFERENCES [recipe]([recipe_id]),
[category_id] [int] NOT NULL FOREIGN KEY REFERENCES [category]([category_id]),
PRIMARY KEY ([recipe_id], [category_id])
);
GO

CREATE TABLE [recipe_country]
(
[recipe_id] [int] NOT NULL FOREIGN KEY REFERENCES [recipe]([recipe_id]),
[country_id] [int] NOT NULL FOREIGN KEY REFERENCES [countries]([country_id]),
PRIMARY KEY ([recipe_id], [country_id])
);
GO

CREATE TABLE [ingredient]
(
[ingredient_id] [int] IDENTITY(1,1) PRIMARY KEY,
[ingredient_name] [nvarchar](MAX),
[ingredient_unit] [nvarchar](50), 
);
GO

CREATE TABLE [marketplace]
(
[marketplace_id] [int] IDENTITY(1,1) PRIMARY KEY,
[marketplace_name] [nvarchar](MAX),
[marketplace_logo] [nvarchar](MAX)
);
GO

CREATE TABLE [ingredient_marketplace]
(
[ingredient_id] [int] FOREIGN KEY REFERENCES [ingredient]([ingredient_id]),
[marketplace_id] [int] FOREIGN KEY REFERENCES [marketplace]([marketplace_id]),
[marketplace_link] [nvarchar](MAX),
PRIMARY KEY ([ingredient_id], [marketplace_id])
);
GO

CREATE TABLE [recipe_ingredient]
(
[recipe_id] [int] NOT NULL FOREIGN KEY REFERENCES [recipe]([recipe_id]),
[ingredient_id] [int] NOT NULL FOREIGN KEY REFERENCES [ingredient]([ingredient_id]),
[unit_value] [decimal](9,2),
PRIMARY KEY ([recipe_id], [ingredient_id])
);
GO

CREATE TABLE [recipe_rating]
(
[rating_id] [int] IDENTITY(1,1) PRIMARY KEY,
[recipe_id] [int] NOT NULL FOREIGN KEY REFERENCES [recipe]([recipe_id]),
[user_id] [int] NOT NULL FOREIGN KEY REFERENCES [users]([user_id]),
[rating_value] [int] NOT NULL,
[rating_content] [nvarchar](MAX),
[create_date] [datetime] NOT NULL
);
GO

CREATE TABLE [blog]
(
[blog_id] [int] IDENTITY(1,1) PRIMARY KEY,
[user_id] [int] NOT NULL FOREIGN KEY REFERENCES [users]([user_id]),
[category_id] [int] NOT NULL FOREIGN KEY REFERENCES [category]([category_id]),
[blog_title] [nvarchar](MAX),
[blog_content] [nvarchar](MAX),
[blog_image] [nvarchar](MAX),
[blog_status] [bit],
[create_date] [datetime] NOT NULL
);
GO

CREATE TABLE [blog_comment]
(
[comment_id] [int] IDENTITY(1,1) PRIMARY KEY,
[blog_id] [int] NOT NULL FOREIGN KEY REFERENCES [blog](blog_id),
[parent_id] [int] NULL FOREIGN KEY REFERENCES [blog_comment](comment_id),
[user_id] [int] NOT NULL FOREIGN KEY REFERENCES [users]([user_id]),
[comment_content] [nvarchar](MAX),
[create_date] [datetime] NOT NULL
);
GO

CREATE TABLE [bookmark]
(
[user_id] [int] FOREIGN KEY REFERENCES [users]([user_id]),
[recipe_id] [int] FOREIGN KEY REFERENCES [recipe]([recipe_id]),
PRIMARY KEY ([user_id], [recipe_id])
);
GO

CREATE TABLE [menu]
(
[menu_id] [int] IDENTITY(1,1) PRIMARY KEY,
[featured_image] [nvarchar](MAX),
[menu_name] [nvarchar](MAX),
[menu_description] [nvarchar](MAX),
[user_id] [int] NOT NULL FOREIGN KEY REFERENCES [users]([user_id]),
);
GO

CREATE TABLE [menu_recipe]
(
[menu_id] [int] NOT NULL FOREIGN KEY REFERENCES [menu]([menu_id]),
[recipe_id] [int] NOT NULL FOREIGN KEY REFERENCES [recipe]([recipe_id]),
PRIMARY KEY ([menu_id], [recipe_id])
);
GO

CREATE TABLE [voucher]
(
[voucher_code] [varchar](10) PRIMARY KEY,
[user_id] [int] NOT NULL FOREIGN KEY REFERENCES [users]([user_id]),
[discount_percentage] [tinyint] NOT NULL
);
GO

CREATE TABLE [cart_item]
(
[user_id] [int] FOREIGN KEY REFERENCES [users]([user_id]),
[recipe_id] [int] FOREIGN KEY REFERENCES [recipe]([recipe_id]),
[voucher_code] [varchar](10) NULL FOREIGN KEY REFERENCES [voucher]([voucher_code]),
PRIMARY KEY ([user_id], [recipe_id])
);
GO

CREATE TABLE [payment_history]
(
[user_id] [int] FOREIGN KEY REFERENCES [users]([user_id]),
[recipe_id] [int] FOREIGN KEY REFERENCES [recipe]([recipe_id]),
[actual_price] [money] NOT NULL,
[purchase_date] [datetime] NOT NULL,
PRIMARY KEY ([user_id], [recipe_id])
);
GO

CREATE TABLE [verification]
(
[verification_id] [int] IDENTITY(1,1) PRIMARY KEY,
[user_id] [int] FOREIGN KEY REFERENCES [users]([user_id]),
[verification_path] [nvarchar](MAX),
[verification_status] [int] NOT NULL,
[verification_date] [datetime] NOT NULL
);

INSERT INTO [role]([role_name]) VALUES ('Administrator'), ('Moderator'), ('Writer'), ('Chef'), ('users');
INSERT INTO [status]([status_name]) VALUES ('Active'), ('Banned'), ('Deleted');
GO

INSERT [dbo].[users] ([username], [first_name], [middle_name], [last_name], [email], [phone], [avatar], [password_hash], [reset_token], [reset_expire], [status_id], [role_id], [interaction]) VALUES (N'admin1', N'Nguyễn', N'Văn', N'A', N'admin1@mail.com', NULL, NULL, N'3A7A4EE1E7AE7D0C9F95193571200B157A6BE0FFA59EDC8F84D39E4AD2D4D9FE:C433EF5F9226A6E7DE52883236AC00EF:301200:SHA256', NULL, NULL, 1, 1, 0)
INSERT [dbo].[users] ([username], [first_name], [middle_name], [last_name], [email], [phone], [avatar], [password_hash], [reset_token], [reset_expire], [status_id], [role_id], [interaction]) VALUES (N'admin2', N'Nguyễn', N'Văn', N'B', N'admin2@mail.com', NULL, NULL, N'9652CA7A298AEA405FF93D8714AA04B36A2AA1A3B2617B4758C4768A8917936F:CA4912154E13CA51BC1ED4867E206AAC:301200:SHA256', NULL, NULL, 1, 1, 0)
INSERT [dbo].[users] ([username], [first_name], [middle_name], [last_name], [email], [phone], [avatar], [password_hash], [reset_token], [reset_expire], [status_id], [role_id], [interaction]) VALUES (N'moderator1', N'Nguyễn', N'Văn', N'C', N'moderator1@mail.com', NULL, NULL, N'0097E91CDC8D129E69217DF5B37D57985AF014FE7A0CF27C55BCDE5B12587B19:A00581EFE381F1DAED9CFB12F0069A1C:301200:SHA256', NULL, NULL, 1, 2, 0)
INSERT [dbo].[users] ([username], [first_name], [middle_name], [last_name], [email], [phone], [avatar], [password_hash], [reset_token], [reset_expire], [status_id], [role_id], [interaction]) VALUES (N'moderator2', N'Nguyễn', N'Văn', N'D', N'moderator2@mail.com', NULL, NULL, N'5D70822DF27569E7F94668EE6BDDFA5E3111EB91179DFBE1B75AA545692A55ED:ECB7EC3E8B602C4F55BBC150CE3DAACC:301200:SHA256', NULL, NULL, 1, 2, 0)
INSERT [dbo].[users] ([username], [first_name], [middle_name], [last_name], [email], [phone], [avatar], [password_hash], [reset_token], [reset_expire], [status_id], [role_id], [interaction]) VALUES (N'writer1', N'Nguyễn', N'Văn', N'E', N'writer1@mail.com', NULL, NULL, N'8758CF72B19639DBD0828E1AC82A05D6F7A3B87DC5B05A5FF1B7D50220F0DCC2:B078E14863646BDF6DE6882CD1F2505C:301200:SHA256', NULL, NULL, 1, 3, 0)
INSERT [dbo].[users] ([username], [first_name], [middle_name], [last_name], [email], [phone], [avatar], [password_hash], [reset_token], [reset_expire], [status_id], [role_id], [interaction]) VALUES (N'writer2', N'Nguyễn', N'Văn', N'F', N'writer2@mail.com', NULL, NULL, N'4750631D5FCD0AFBEBF66F81FA5AC296C9BF005DFA614DE4E1ECA838383DF92A:14A201737568ACE50D5A80C357248537:301200:SHA256', NULL, NULL, 1, 3, 0)
INSERT [dbo].[users] ([username], [first_name], [middle_name], [last_name], [email], [phone], [avatar], [password_hash], [reset_token], [reset_expire], [status_id], [role_id], [interaction]) VALUES (N'chief1', N'Nguyễn', N'Văn', N'G', N'chief1@mail.com', NULL, NULL, N'3AC2CBDBF4BB7C2AA8184170538F2D4E89F782C40CBD0B774B10B8AFC9FC8CF1:906448FF2F84A81E577A31811C93F233:301200:SHA256', NULL, NULL, 1, 4, 0)
INSERT [dbo].[users] ([username], [first_name], [middle_name], [last_name], [email], [phone], [avatar], [password_hash], [reset_token], [reset_expire], [status_id], [role_id], [interaction]) VALUES (N'chief2', N'Nguyễn', N'Văn', N'H', N'chief2@mail.com', NULL, NULL, N'44A041BD1767D53C58179DE1F720A12312FD5002B5CCE23C7966AC4FA45ED9D2:87D5516F35BCE64974332E491EAC8F0E:301200:SHA256', NULL, NULL, 1, 4, 0)
INSERT [dbo].[users] ([username], [first_name], [middle_name], [last_name], [email], [phone], [avatar], [password_hash], [reset_token], [reset_expire], [status_id], [role_id], [interaction]) VALUES (N'user1', N'Nguyễn', N'Văn', N'I', N'user1@mail.com', NULL, NULL, N'112E13249A04BE6A4A8FDB80D585A622B86ECCD1BD403CE290FC40A200F6B008:1A816DBC8FE9554FA657BE009D57D639:301200:SHA256', NULL, NULL, 1, 5, 0)
INSERT [dbo].[users] ([username], [first_name], [middle_name], [last_name], [email], [phone], [avatar], [password_hash], [reset_token], [reset_expire], [status_id], [role_id], [interaction]) VALUES (N'user2', N'Nguyễn', N'Văn', N'J', N'user2@mail.com', NULL, NULL, N'FCE1709E74FA43BA7FE95E8FF95C8A3834D8A04D452818871AE7AEA58C3A93C5:6C91B8AE636CFF439A6B987D836719AB:301200:SHA256', NULL, NULL, 1, 5, 0)
GO