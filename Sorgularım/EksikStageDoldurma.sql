INSERT  INTO dbo.LeagueStage
        ( Id ,
          name ,
          TournamentName ,
          shownOrder ,
          tournamentFK ,
          gender ,
          countryFK ,
          countryName ,
          startDate ,
          endDate ,
          n ,
          IsActive ,
          CurrentWeek ,
          TotalWeek ,
          MatchCount ,
          ut ,
          Logo ,
          state
        )
        SELECT  DISTINCT
                lf.tournament_stageFK Id ,
                ISNULL(ls.name,
                       ( SUBSTRING(CAST(MIN(CAST(lf.StartDate AS DATE)) AS VARCHAR(4)),
                                   1, 4) ) + '/'
                       + ( SUBSTRING(CAST(MAX(CAST(lf.StartDate AS DATE)) AS VARCHAR(4)),
                                     1, 4) ) + ' Sezonu') name ,
                ISNULL(ls.TournamentName, lf.TournamentName) Tournamentname ,
                0 ShownOrder ,
                ISNULL(ls.tournamentFK, lf.TournamentID) TournamentFK ,
                'male' gender ,
                ls.countryFK ,
                countryName ,
                ISNULL(ls.startDate, ( MIN(CAST(lf.StartDate AS DATE)) )) startDate ,
                ISNULL(ls.endDate, ( MAX(CAST(lf.StartDate AS DATE)) )) EndDate ,
                1 n ,
                'False' IsActive ,
                1 CurrentWeek ,
                1 TotalWeek ,
                306 Matchcount ,
                GETDATE() ut ,
                NULL logo ,
                1 state 
        --lf.tournament_stageFK ,
        --lf.TournamentID , 
        FROM    dbo.LeagueFixture lf ( NOLOCK )
                LEFT JOIN dbo.League l ( NOLOCK ) ON l.Id = lf.TournamentID
                LEFT JOIN dbo.LeagueStage ls ( NOLOCK ) ON ls.Id = lf.tournament_stageFK
        WHERE   1 = 1
		--AND lf.tournament_stageFK=31081
        AND ls.name IS NULL
GROUP BY        l.Id ,
                l.name ,
                lf.tournament_stageFK ,
                lf.TournamentName ,
                ls.name ,
                ls.TournamentName ,
                lf.TournamentID ,
                ls.tournamentFK ,
                ls.startDate ,
                ls.endDate ,
                ls.countryFK ,
                countryName
        ORDER BY ISNULL(ls.startDate, ( MIN(CAST(lf.StartDate AS DATE)) ))
