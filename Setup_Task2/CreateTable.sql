USE KPKtest -- ������� � ��
GO

--�������� ������� "��������"
CREATE TABLE Tests (
TestId				 INT              IDENTITY (1, 1) NOT NULL,
TestDate			 SMALLDATETIME					  NOT NULL,
BlockName			 NVARCHAR (50)					  NOT NULL,
Note                 NVARCHAR (200)    
);


--��������� ������� TestId, ��� ���������
ALTER TABLE Tests
ADD CONSTRAINT Tests_TestId 
PRIMARY KEY CLUSTERED (TestId);


--�� ��������� � ������� TestDate ���������� ������� ����
ALTER TABLE Tests
ADD CONSTRAINT Tests_TestDate_Default 
DEFAULT (getdate()) FOR TestDate
GO

--�������� �������� "���������"
CREATE TABLE Parameters (
ParameterId			 INT              IDENTITY (1, 1) NOT NULL,
TestId				 INT							  NOT NULL,
ParameterName		 NVARCHAR (200)					  NOT NULL,
RequiredValue		 DECIMAL (18,3)					  NOT NULL,
MeasuredValue        DECIMAL (18,3)					  NOT NULL
);


--��������� ������� ParameterId, ��� ���������
ALTER TABLE Parameters
ADD CONSTRAINT Parameters_ParameterId 
PRIMARY KEY CLUSTERED (ParameterId);

--�������� ����� ����� ��������� TestId ������ Tests � Parameters
ALTER TABLE Parameters
WITH CHECK ADD CONSTRAINT Parameters_TestId 
FOREIGN KEY (TestId) 
REFERENCES Tests (TestId)
ON UPDATE CASCADE -- ���� ��������� ID �������� �� � ID � ������� ���������� ����������
ON DELETE CASCADE -- ���� ����� ������� �������� (�.�. ID ��������) �� � ��� ��������� ��������� � ���� ��������� ����� �������


Insert into Tests (BlockName, Note) Values ('���-3�', '����')
Insert into Tests (TestDate, BlockName, Note) Values ('20200102', '���-18', '���-1 5400-1222')
Insert into Tests (TestDate, BlockName, Note) Values ('20200101', '���-1', '�� 3 ����')
Insert into Tests (TestDate, BlockName, Note) Values ('20200302', '����-1', '������� ������')
Insert into Tests (TestDate, BlockName, Note) Values ('20190102', '���-1�', '������������ ����������')

Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (1, '���������� ������� 27 �, �', 27, 27.5)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (1, '���������� ������� 36 � ���� �, �', 36, 36.005)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (1, '���������� ������� 36 � ���� �, �', 36, 36.025)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (1, '���������� ������� 36 � ���� �, �', 36, 36.035)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (2, '��� ����������� 27 �, �', 27, 27.5)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (2, '��� ����������� 36 � ���� �, �', 36, 36.005)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (2, '��� ����������� 36 � ���� �, �', 36, 36.025)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (2, '��� ����������� 36 � ���� �, �', 36, 36.035)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (3, '���������� �� ������ ��� 1-1, �', 7, 7.15)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (3, '���������� �� ������ ��� 1-2, �', 7, 7.25)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (3, '���������� �� ������ ��� 1-3, �', 7, 7.35)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (4, '�������� ��������� (������������), ����/���', 5, 4.95)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (4, '�������� ��������� (�����������), ����/���', 5, 5.02)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (4, '�������� ��������� (������ ����), ����/���', 3, 3.02)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (4, '�������� ��������� (����� �����), ����/���', 3, 2.99)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (5, '����� ������ �� ������� �����, ���', 90, 61.5)
Insert into Parameters (TestId, ParameterName, RequiredValue, MeasuredValue) Values (5, '����� �� ���������� ���������, ���', 9, 8.04)
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