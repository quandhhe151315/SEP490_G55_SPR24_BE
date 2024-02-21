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
[restaurant_location] [nvarchar](MAX)
);
GO

CREATE TABLE [news]
(
[news_id] [int] IDENTITY(1,1) PRIMARY KEY,
[user_id] [int] NOT NULL FOREIGN KEY REFERENCES [users]([user_id]),
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
[recipe_title] [nvarchar](MAX),
[recipe_serve] [int],
[recipe_content] [nvarchar](MAX),
[recipe_rating] [int] NOT NULL,
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