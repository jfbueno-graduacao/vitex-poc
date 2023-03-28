USE VitalSignReadings;

BEGIN TRAN

DECLARE @PersonCounter INT = 1,
		@PersonsToInsert INT = 100;

WHILE @PersonCounter <= @PersonsToInsert 
BEGIN
	DECLARE @ReadingCounter INT = 1,
			@PersonId UNIQUEIDENTIFIER = NEWID(),
			@ReadingDate DATETIME2 = '2023-03-16 00:00:00';

	WHILE @ReadingCounter <= ((24 * 12) - 1)
	BEGIN
		SET @ReadingCounter = @ReadingCounter + 1;
		SET @ReadingDate = DATEADD(mi, 5, @ReadingDate);

		INSERT INTO [Temperature] (PersonId, [Value], [Timestamp])
		VALUES 
		(
			@PersonId, 
			(SELECT ABS(CHECKSUM(NEWID()) % (43 - 35 + 1)) + 35), 
			@ReadingDate
		);	
		
	END
	   
	SET @PersonCounter = @PersonCounter + 1;
END

COMMIT TRAN;