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