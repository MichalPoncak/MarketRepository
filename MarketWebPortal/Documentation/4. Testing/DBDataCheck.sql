use [Market]
go

-- truncate table dbo.Price

select MIN(pric_MarketPrice) as MinPrice from dbo.Price
select MAX(pric_MarketPrice) as MaxPrice from dbo.Price
select AVG(pric_MarketPrice) as AveragePrice from dbo.Price

-- UI output 285.2200012
select MAX(pric_MarketPrice) as HighestHourlyPriceForALimitedDateRange from dbo.Price
where pric_Date between CAST('2017-01-01 00:00:00.000' as date) and CAST('2017-01-15 00:00:00.000' as date);

-- declare hourly prices table variable
DECLARE @HourlyPriceTable TABLE (
    pric_Date DATE NOT NULL,
	pric_Hour int NOT NULL,
	pric_MarketPrice DEC(19,8) NOT NULL
);

DECLARE @MaxPriceDate DATE;
DECLARE @MaxPriceHour INT;
DECLARE @MaxHourlyPrice DECIMAL(19,8);

-- insert hourly prices into a table variable
INSERT INTO @HourlyPriceTable
SELECT CAST(pric_Date AS DATE), DATEPART(HOUR, pric_Date), SUM (pric_MarketPrice)
FROM dbo.Price
GROUP BY CAST(pric_Date AS DATE), DATEPART(HOUR, pric_Date);

-- max hourly price
SELECT @MaxHourlyPrice = MAX(pric_MarketPrice) FROM @HourlyPriceTable;

SELECT @MaxHourlyPrice as HighestHourlyPrice

SELECT @MaxPriceDate = pric_Date FROM @HourlyPriceTable where pric_MarketPrice = @MaxHourlyPrice;
SELECT @MaxPriceHour = pric_Hour FROM @HourlyPriceTable where pric_MarketPrice = @MaxHourlyPrice;

-- select granular records making up the highest hourly price
SELECT * FROM dbo.Price WHERE CAST(pric_Date AS DATE) = @MaxPriceDate and DATEPART(HOUR, pric_Date) = @MaxPriceHour;

--select * from dbo.Price order by pric_PriceID asc