USE VitalSignReadings;

BEGIN TRAN;

DECLARE @PersonCounter INT = 1,
		@PersonsToInsert INT = 100;

DECLARE @MinTemperature DECIMAL = 36.1,
		@MaxTemperature DECIMAL = 37.8;

WHILE @PersonCounter <= @PersonsToInsert
BEGIN
	DECLARE @YearsToSubtractFromToday INT = 
		(SELECT ABS(CHECKSUM(NEWID()) % (80 - 18 + 1)) + 18)

	INSERT INTO [People] (BirthDate, BaseTemperature)
	VALUES 
	(
		DATEADD(YYYY, -@YearsToSubtractFromToday, GETDATE()),
		(SELECT ROUND(RAND(CHECKSUM(NEWID())) * (@MaxTemperature - @MinTemperature) + @MinTemperature, 1))
	);

	SET @PersonCounter = @PersonCounter + 1;

END

COMMIT TRAN;