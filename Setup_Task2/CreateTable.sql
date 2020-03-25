USE KPKtest -- переход в БД
GO

--Создание таблицы "Проверки"
CREATE TABLE Tests (
TestId				 INT              IDENTITY (1, 1) NOT NULL,
TestDate			 SMALLDATETIME					  NOT NULL,
BlockName			 NVARCHAR (50)					  NOT NULL,
Note                 NVARCHAR (200)    
);


--Установка столбца TestId, как ключевого
ALTER TABLE Tests
ADD CONSTRAINT Tests_TestId 
PRIMARY KEY CLUSTERED (TestId);


--По умолчанию в столбец TestDate записываем текущую дату
ALTER TABLE Tests
ADD CONSTRAINT Tests_TestDate_Default 
DEFAULT (getdate()) FOR TestDate
GO

--Создание таблцицы "Параметры"
CREATE TABLE Parameters (
ParameterId			 INT              IDENTITY (1, 1) NOT NULL,
TestId				 INT							  NOT NULL,
ParameterName		 NVARCHAR (200)					  NOT NULL,
RequiredValue		 DECIMAL (18,3)					  NOT NULL,
MeasuredValue        DECIMAL (18,3)					  NOT NULL
);


--Установка столбца ParameterId, как ключевого
ALTER TABLE Parameters
ADD CONSTRAINT Parameters_ParameterId 
PRIMARY KEY CLUSTERED (ParameterId);

--Создание связи между стоблцами TestId таблиц Tests и Parameters
ALTER TABLE Parameters
WITH CHECK ADD CONSTRAINT Parameters_TestId 
FOREIGN KEY (TestId) 
REFERENCES Tests (TestId)
ON UPDATE CASCADE -- Если изменится ID проверки то и ID в таблице параметров изменитсся
ON DELETE CASCADE -- Если будет удалена проверка (т.е. ID проверки) то и все параметра связанные с этой проверкой будут удалены


Insert into Tests (BlockName, Note) Values ('АГБ-3К', 'Тест')
Insert into Tests (TestDate, BlockName, Note) Values ('20200102', 'БКК-18', 'КПК-1 5400-1222')
Insert into Tests (TestDate, BlockName, Note) Values ('20200101', 'СНП-1', 'Из 3 цеха')
Insert into Tests (TestDate, BlockName, Note) Values ('20200302', 'БСПК-1', 'Заменен модуль')
Insert into Tests (TestDate, BlockName, Note) Values ('20190102', 'ГМК-1А', 'Перепроверка параметров')

Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (1, 'Напряжение питания 27 В, В', 27, 27.5)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (1, 'Напряжение питания 36 В Фаза А, В', 36, 36.005)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (1, 'Напряжение питания 36 В Фаза В, В', 36, 36.025)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (1, 'Напряжение питания 36 В Фаза С, В', 36, 36.035)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (2, 'Ток потребления 27 В, А', 27, 27.5)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (2, 'Ток потребления 36 В Фаза А, А', 36, 36.005)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (2, 'Ток потребления 36 В Фаза В, А', 36, 36.025)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (2, 'Ток потребления 36 В Фаза С, А', 36, 36.035)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (3, 'Напряжение на канале АГД 1-1, В', 7, 7.15)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (3, 'Напряжение на канале АГД 1-2, В', 7, 7.25)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (3, 'Напряжение на канале АГД 1-3, В', 7, 7.35)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (4, 'Скорость прецессии (кабрирование), град/мин', 5, 4.95)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (4, 'Скорость прецессии (пикирование), град/мин', 5, 5.02)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (4, 'Скорость прецессии (правый крен), град/мин', 3, 3.02)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (4, 'Скорость прецессии (левый креен), град/мин', 3, 2.99)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (5, 'Время выхода на рабочий режим, сек', 90, 61.5)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (5, 'Время не выключения коррекции, сек', 9, 8.04)
GO

CREATE PROCEDURE [dbo].[sp_Tests]
    @testdate smalldatetime,
    @blockname nvarchar(50),
    @note nvarchar(200),
    @testId int out
AS
    INSERT INTO Tests (TestDate, BlockName, Note)
    VALUES (@testdate, @blockname, @note)
   
    SET @testId=SCOPE_IDENTITY()
GO

CREATE PROCEDURE [dbo].[sp_Parameters]
    @testId int,
    @parameterName nvarchar(20),
    @requiredValue decimal(18, 3),
    @measuredValue decimal(18, 3),
    @parameterId int out
AS

    INSERT INTO Parameters (TestId, ParameterName, RequiredValue, MeasuredValue)
    VALUES (@testId, @parameterName, @requiredValue, @measuredValue)
   
    SET @parameterId=SCOPE_IDENTITY()
GO