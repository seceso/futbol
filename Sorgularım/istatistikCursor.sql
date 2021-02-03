DECLARE @cursor1_holder NVARCHAR(MAX) ,
    @cursor1 CURSOR ,
    @cursor2 CURSOR ,
    @takim INT ,
    @macId NVARCHAR(10)
    --@cursor2_holder NVARCHAR(MAX)


SET 
@cursor1 = CURSOR FAST_FORWARD FOR
    SELECT  REPLACE(dbo.Turkceye(name),'''','.'),number,EventId
    FROM    dbo.MatchLineUps (NOLOCK)
    WHERE   EventId = 1261766
            AND number = 1
OPEN @cursor1
FETCH NEXT FROM @cursor1 INTO @cursor1_holder, @takim, @macId
DECLARE @sayi INT= 0
WHILE @@FETCH_STATUS = 0
    BEGIN	
	--PRINT @cursor1_holder+ ' : '
        DECLARE @cursor2_holder NVARCHAR(MAX)
        SET @cursor2 =CURSOR FAST_FORWARD FOR
            SELECT  incident_code,COUNT(incident_code)
            FROM    dbo.MatchLiveInfoEvent (NOLOCK)
            WHERE   event_desc LIKE '%' + @cursor1_holder + '%'
                    AND incident_code NOT IN  ('Goal','SecondHalfStarted','MatchStarted','Yellow','Substitution','Comment','SecondYellow','Red')
                    AND EventId = 1261766
					GROUP BY incident_code
            --HAVING  COUNT(incident_code) > 0
			--PRINT @cursor1_holder
        OPEN @cursor2
        FETCH NEXT FROM @cursor2 INTO @cursor2_holder, @sayi

        WHILE @@FETCH_STATUS = 0
            BEGIN	

                DECLARE @query NVARCHAR(MAX)
                SET @query = 'select ' + CHAR(39) + @macId + CHAR(39) + ','
                    + CHAR(39) + @cursor1_holder + CHAR(39) + ', ' + ''
                    + CHAR(39) + @cursor2_holder + CHAR(39) + ', '
                    + CAST(@sayi AS NVARCHAR(10)) 
                EXECUTE sp_executesql @query;
				--PRINT @query
                
                FETCH NEXT FROM @cursor2 INTO @cursor2_holder, @sayi
            END
        CLOSE @cursor2
        DEALLOCATE @cursor2
        FETCH NEXT FROM @cursor1 INTO @cursor1_holder, @takim, @macId
    END
CLOSE @cursor1
DEALLOCATE @cursor1


