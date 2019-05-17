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

	DELETE FROM GroupUser
	FROM DELETED
	WHERE GroupUser.UserId = DELETED.Id;

	DELETE FROM [Group]
	FROM DELETED
	WHERE [Group].OwnerId = DELETED.Id;

    DELETE FROM [User]
    FROM DELETED
    WHERE [User].Id = DELETED.Id;
  END;

GO

DROP TRIGGER IF EXISTS TR_Group_InsteadOfDelete;

GO

CREATE TRIGGER TR_Group_InsteadOfDelete
  ON [Group]
  INSTEAD OF DELETE
AS
  BEGIN
    DELETE FROM GroupUser
	FROM DELETED
	WHERE GroupUser.GroupId = DELETED.Id;

	DELETE FROM [Group]
	FROM DELETED
	WHERE [Group].Id = DELETED.Id;
  END;

GO