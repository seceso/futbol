SELECT  
--DISTINCT incident_code
elapsed,event_desc ,
        incident_code ,
        EventId
FROM    dbo.MatchLiveInfoEvent (NOLOCK)
WHERE   1 = 1
			--AND event_desc LIKE '%Emre Nefiz%'
        AND incident_code = 'ShotOnTarget'
   --                 AND incident_code NOT IN  ('SecondHalfStarted','MatchStarted','Yellow','Substitution','Comment','SecondYellow','Red')
        AND EventId = 1261766
		ORDER BY incident_code

					--Maximillian Philipp, gol pasýný veren isim.

					
--UPDATE  dbo.MatchLiveInfoEvent
--SET     event_desc = REPLACE(event_desc, 'Maximillian Philipp',
--                             'Maximilian Philipp')
--WHERE   event_desc LIKE '%Maximillian Philipp%'