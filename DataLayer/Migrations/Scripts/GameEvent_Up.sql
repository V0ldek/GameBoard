DROP TRIGGER IF EXISTS TR_User_InsteadOfDelete;

GO

CREATE TRIGGER TR_User_InsteadOfDelete
  ON [User]
  INSTEAD OF DELETE
AS
  BEGIN
    DELETE FROM Friendship
    FROM DELETED
    WHERE RequestedById = DELETED.Id
       OR RequestedToId = DELETED.Id;

	DELETE FROM GameEventParticipation
	FROM DELETED
	WHERE GameEventParticipation.ParticipantId = DELETED.Id;

    DELETE FROM [User]
    FROM DELETED
    WHERE [User].Id = DELETED.Id;
  END;

GO

DROP TRIGGER IF EXISTS TR_GameEventParticipation_InsteadOfDelete;

GO

CREATE TRIGGER TR_GameEventParticipation_InsteadOfDelete
  ON GameEventParticipation
  INSTEAD OF DELETE
AS
  BEGIN
	IF TRIGGER_NESTLEVEL(OBJECT_ID('TR_GameEventParticipation_InsteadOfDelete')) <= 2
	BEGIN
	  DELETE FROM GameEventParticipation
	  FROM DELETED
	  WHERE GameEventParticipation.ParticipantId = DELETED.ParticipantId
	    AND GameEventParticipation.TakesPartInId = DELETED.TakesPartInId;

      DELETE FROM GameEvent
      FROM DELETED
	  WHERE GameEvent.Id = DELETED.TakesPartInId
		AND DELETED.ParticipationStatus = 'Creator';
	END;
  END;

GO

DROP TRIGGER IF EXISTS TR_GameEvent_InsteadOfDelete;

GO

CREATE TRIGGER TR_GameEvent_InsteadOfDelete
  ON GameEvent
  INSTEAD OF DELETE
AS
  BEGIN
    DELETE FROM GameEventParticipation
	FROM DELETED
	WHERE GameEventParticipation.TakesPartInId = DELETED.Id
	  AND GameEventParticipation.ParticipationStatus <> 'Creator';

	DELETE FROM Game -- This DELETE could be replaced with CascadeDelete on Games, but it seems more consistent to put everything in one place.
	FROM DELETED
	WHERE Game.GameEventId = DELETED.Id;

	DELETE FROM GameEvent
	FROM DELETED
	WHERE GameEvent.Id = DELETED.Id;
  END;

GO
