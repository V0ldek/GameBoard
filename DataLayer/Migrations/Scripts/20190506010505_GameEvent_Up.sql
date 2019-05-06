﻿DROP TRIGGER IF EXISTS TR_User_InsteadOfDelete;

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
    -- If I removed the following line, SQL Server would throw an excpetion:
    -- "Maximum stored procedure, function, trigger, or view nesting level exceeded (limit 32)"
    -- This is because TR_GameEvent_InsteadOfDelete triggers TR_GameEventParticipation_InsteadOfDelete 
    -- and that one triggers TR_GameEvent_InsteadOfDelete again and so on, indefinitely.
    IF TRIGGER_NESTLEVEL(OBJECT_ID('TR_GameEventParticipation_InsteadOfDelete')) <= 2
    BEGIN
      DELETE FROM GameEventParticipation
      FROM DELETED
      WHERE GameEventParticipation.ParticipantId = DELETED.ParticipantId
        AND GameEventParticipation.TakesPartInId = DELETED.TakesPartInId;

      DELETE FROM GameEvent
      FROM DELETED
      WHERE GameEvent.Id = DELETED.TakesPartInId
        AND DELETED.ParticipationStatus = 0; -- Creator
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
    WHERE GameEventParticipation.TakesPartInId = DELETED.Id;

    DELETE FROM Game -- This DELETE could be replaced with CascadeDelete on Games, but it seems more consistent to put everything in one place.
    FROM DELETED
    WHERE Game.GameEventId = DELETED.Id;

    DELETE FROM GameEvent
    FROM DELETED
    WHERE GameEvent.Id = DELETED.Id;
  END;

GO
