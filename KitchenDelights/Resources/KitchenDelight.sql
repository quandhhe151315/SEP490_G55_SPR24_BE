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

CREATE TABLE [account]
(
[account_id] [int] IDENTITY(1,1) PRIMARY KEY,
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
[role_id] [int] NOT NULL FOREIGN KEY REFERENCES [role]([role_id])
);
GO

CREATE TABLE [address]
(
[address_id] [int] IDENTITY(1,1) PRIMARY KEY,
[account_id] [int] NOT NULL FOREIGN KEY REFERENCES [account]([account_id]),
[address_details] [nvarchar](MAX)
);
GO

CREATE TABLE [restaurant_recommendation]
(
[restaurant_id] [int] IDENTITY(1,1) PRIMARY KEY,
[account_id] [int] NOT NULL FOREIGN KEY REFERENCES [account]([account_id]),
[restaurant_name] [nvarchar](MAX),
[restaurant_details] [nvarchar](MAX),
[restaurant_location] [nvarchar](MAX)
);
GO

CREATE TABLE [news]
(
[news_id] [int] IDENTITY(1,1) PRIMARY KEY,
[account_id] [int] NOT NULL FOREIGN KEY REFERENCES [account]([account_id]),
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

CREATE TABLE [recipe]
(
[recipe_id] [int] IDENTITY(1,1) PRIMARY KEY,
[account_id] [int] NOT NULL FOREIGN KEY REFERENCES [account]([account_id]),
[category_id] [int] NOT NULL FOREIGN KEY REFERENCES [category]([category_id]),
[recipe_title] [nvarchar](MAX),
[recipe_content] [nvarchar](MAX),
[recipe_rating] [int] NOT NULL,
[recipe_status] [bit] NOT NULL,
[is_free] [bit] NOT NULL,
[recipe_price] [money],
[create_date] [datetime] NOT NULL
);
GO

CREATE TABLE [recipe_rating]
(
[rating_id] [int] IDENTITY(1,1) PRIMARY KEY,
[recipe_id] [int] NOT NULL FOREIGN KEY REFERENCES [recipe]([recipe_id]),
[account_id] [int] NOT NULL FOREIGN KEY REFERENCES [account]([account_id]),
[rating_value] [int] NOT NULL,
[rating_content] [nvarchar](MAX),
[create_date] [datetime] NOT NULL
);
GO

CREATE TABLE [blog]
(
[blog_id] [int] IDENTITY(1,1) PRIMARY KEY,
[account_id] [int] NOT NULL FOREIGN KEY REFERENCES [account]([account_id]),
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
[account_id] [int] NOT NULL FOREIGN KEY REFERENCES [account]([account_id]),
[comment_content] [nvarchar](MAX),
[create_date] [datetime] NOT NULL
);
GO

CREATE TABLE [bookmark]
(
[account_id] [int] FOREIGN KEY REFERENCES [account]([account_id]),
[recipe_id] [int] FOREIGN KEY REFERENCES [recipe]([recipe_id]),
PRIMARY KEY ([account_id], [recipe_id])
);
GO

CREATE TABLE [menu]
(
[menu_id] [int] IDENTITY(1,1) PRIMARY KEY,
[menu_name] [nvarchar](MAX),
[menu_description] [nvarchar](MAX),
[menu_access] [bit] NOT NULL,
[account_id] [int] NOT NULL FOREIGN KEY REFERENCES [account]([account_id]),
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
[account_id] [int] NOT NULL FOREIGN KEY REFERENCES [account]([account_id]),
[discount_percentage] [tinyint] NOT NULL
);
GO

CREATE TABLE [cart_item]
(
[account_id] [int] FOREIGN KEY REFERENCES [account]([account_id]),
[recipe_id] [int] FOREIGN KEY REFERENCES [recipe]([recipe_id]),
[voucher_code] [varchar](10) NULL FOREIGN KEY REFERENCES [voucher]([voucher_code]),
PRIMARY KEY ([account_id], [recipe_id])
);
GO

CREATE TABLE [payment_history]
(
[account_id] [int] FOREIGN KEY REFERENCES [account]([account_id]),
[recipe_id] [int] FOREIGN KEY REFERENCES [recipe]([recipe_id]),
[actual_price] [money] NOT NULL,
[purchase_date] [datetime] NOT NULL,
PRIMARY KEY ([account_id], [recipe_id])
);
GO

INSERT INTO [role]([role_name]) VALUES ('Administrator'), ('Moderator'), ('Writer'), ('Chef'), ('User');
INSERT INTO [status]([status_name]) VALUES ('Active'), ('Banned'), ('Deleted');
GO