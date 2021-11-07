DROP DATABASE IF EXISTS Worker_Order
CREATE DATABASE Worker_Order;
GO

USE Worker_Order;
GO

--Table that stores the file that was read.
CREATE TABLE Worker_Order_Read
(
	 Read_ID	INT				NOT NULL IDENTITY(10000000, 1)
	,Read_File	VARCHAR(500)	NOT NULL
	,Read_Date	DATETIME		NOT NULL
	,Active		BIT				NOT NULL

	CONSTRAINT PK_Read_ID PRIMARY KEY (Read_ID)
);

--Table that stores the file logs.
DROP TABLE IF EXISTS Worker_Order_Read_Log
CREATE TABLE Worker_Order_Read_Log
(
	 Read_Log_ID	INT				NOT NULL IDENTITY(10000000, 1)
	,Read_Message	VARCHAR(500)	NOT NULL
	,Read_ID		INT				NOT NULL
	,Read_Date		DATETIME		NOT NULL
	,Active			BIT				NOT NULL

	CONSTRAINT PK_Read_Log_ID PRIMARY KEY (Read_Log_ID),

	CONSTRAINT FK_Worker_Order_Read_Log_Read_ID FOREIGN KEY (Read_ID)
	REFERENCES Worker_Order_Read
);

--Table that stores the information from XML file, without any filter.
DROP TABLE IF EXISTS Worker_Order_Pre_Order_Read
CREATE TABLE Worker_Order_Pre_Order_Read
(
	 Pre_Order_Read_ID					INT				NOT NULL IDENTITY(10000000, 1)
	,Pre_Order_Order_Number				VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Date				VARCHAR(200)	NOT NULL
	,Pre_Order_Delivery_Notes			VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Shipping_Name		VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Shipping_Street	VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Shipping_City		VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Shipping_State		VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Shipping_Zip		VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Shipping_Country	VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Billing_Name		VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Billing_Street		VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Billing_City		VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Billing_State		VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Billing_Zip		VARCHAR(200)	NOT NULL
	,Pre_Order_Order_Billing_Country	VARCHAR(200)	NOT NULL
	,Read_ID							INT				NOT NULL
	,Active								BIT				NOT NULL

	CONSTRAINT PK_Pre_Order_Read_ID PRIMARY KEY(Pre_Order_Read_ID)

	CONSTRAINT FK_Worker_Order_Pre_Order_Read_Read_ID FOREIGN KEY(Read_ID)
	REFERENCES Worker_Order_Read(Read_ID)
);

--Table that stores the items' information from XML file, without any filter.
DROP TABLE IF EXISTS Worker_Order_Pre_Order_Items_Read
CREATE TABLE Worker_Order_Pre_Order_Items_Read
(
	 Pre_Order_Item_Read_ID			INT				NOT NULL IDENTITY(10000000, 1)
	,Pre_Order_Item_Part_Number		VARCHAR(200)	NOT NULL
	,Pre_Order_Item_Product_Name	VARCHAR(200)	NOT NULL
	,Pre_Order_Item_Quantity		VARCHAR(200)	NOT NULL
	,Pre_Order_Item_Price			VARCHAR(200)	NOT NULL
	,Pre_Order_Item_Comment			VARCHAR(200)	NOT NULL
	,Pre_Order_Read_ID				INT				NOT NULL
	,Active							BIT				NOT NULL

	CONSTRAINT PK_Pre_Order_Item_Read_ID PRIMARY KEY(Pre_Order_Item_Read_ID)

	CONSTRAINT FK_Pre_Order_Pre_Order_Items_Read FOREIGN KEY(Pre_Order_Read_ID)
	REFERENCES Worker_Order_Pre_Order_Read(Pre_Order_Read_ID)
);

DROP TABLE IF EXISTS Worker_Order_Shipping_Address
CREATE TABLE Worker_Order_Shipping_Address
(
	 Shipping_Address_ID		INT				NOT NULL IDENTITY(10000000, 1)
	,Shipping_Address_Name		VARCHAR(200)	NOT NULL
	,Shipping_Address_Street	VARCHAR(200)	NOT NULL
	,Shipping_Address_City		VARCHAR(200)	NOT NULL
	,Shipping_Address_State		VARCHAR(200)	NOT NULL
	,Shipping_Address_Zip		VARCHAR(50)		NOT NULL
	,Shipping_Address_Country	VARCHAR(200)	NOT NULL
	,Insert_Date				DATETIME		NOT NULL
	,Active						BIT				NOT NULL

	CONSTRAINT PK_Shipping_Address_ID PRIMARY KEY(Shipping_Address_ID)
);

DROP TABLE IF EXISTS Worker_Order_Billing_Address
CREATE TABLE Worker_Order_Billing_Address
(
	 Billing_Address_ID			INT				NOT NULL IDENTITY(10000000, 1)
	,Billing_Address_Name		VARCHAR(200)	NOT NULL
	,Billing_Address_Street		VARCHAR(200)	NOT NULL
	,Billing_Address_City		VARCHAR(200)	NOT NULL
	,Billing_Address_State		VARCHAR(200)	NOT NULL
	,Billing_Address_Zip		VARCHAR(50)		NOT NULL
	,Billing_Address_Country	VARCHAR(200)	NOT NULL
	,Insert_Date				DATETIME		NOT NULL
	,Active						BIT				NOT NULL

	CONSTRAINT PK_Billing_Address_ID PRIMARY KEY(Billing_Address_ID)
);

DROP TABLE IF EXISTS Worker_Order
CREATE TABLE Worker_Order
(
	 Order_ID					INT				NOT NULL IDENTITY(10000000, 1)
	,Order_Number				VARCHAR(200)	NOT NULL
	,Order_Date					DATETIME		NOT NULL
	,Order_Shipping_Address_ID	INT				NOT NULL
	,Order_Billing_Address_ID	INT				NOT NULL
	,Order_Delivery_Notes		VARCHAR(200)	NOT NULL
	,Insert_Date				DATETIME		NOT NULL
	,Active						BIT				NOT NULL

	CONSTRAINT PK_Order_ID PRIMARY KEY(Order_ID)

	CONSTRAINT FK_Order_Shipping_Address_ID FOREIGN KEY (Order_Shipping_Address_ID)
	REFERENCES Worker_Order_Shipping_Address(Shipping_Address_ID),

	CONSTRAINT FK_Order_Billing_Address_ID FOREIGN KEY (Order_Billing_Address_ID)
	REFERENCES Worker_Order_Billing_Address(Billing_Address_ID)
);

DROP TABLE IF EXISTS Worker_Order_Item
CREATE TABLE Worker_Order_Item
(
	 Order_Item_ID 				INT				NOT NULL IDENTITY(10000000, 1)
	,Order_Item_Part_Number		VARCHAR(200)
	,Order_Item_Product_Name	VARCHAR(200)	NOT NULL
	,Order_Item_Quantity		INT				NOT NULL
	,Order_Item_Price			MONEY			NOT NULL
	,Order_Item_Comment			VARCHAR(200)	NOT NULL
	,Order_ID					INT				NOT NULL
	,Insert_Date				DATETIME		NOT NULL
	,Active						BIT				NOT NULL

	CONSTRAINT PK_Order_Item_ID PRIMARY KEY(Order_Item_ID)

	CONSTRAINT FK_Order_ID FOREIGN KEY (Order_ID)
	REFERENCES Worker_Order(Order_ID)
);