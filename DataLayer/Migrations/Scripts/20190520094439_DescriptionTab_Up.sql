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

    DELETE FROM DescriptionTab
    FROM DELETED
    WHERE DescriptionTab.GameEventId = DELETED.Id;

END;

GO

INSERT INTO DescriptionTab(GameEventId)
    SELECT EventsWithoutDescription.Id From 
    (SELECT Id FROM GameEvent EXCEPT SELECT GameEventId as ID FROM DescriptionTab) as EventsWithoutDescription;

GO