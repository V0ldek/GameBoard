DROP TRIGGER IF EXISTS TR_Friendship_InsteadOfInsert;

GO

CREATE TRIGGER TR_Friendship_InsteadOfInsert
  ON Friendship
  INSTEAD OF INSERT
AS
  BEGIN
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
    BEGIN TRANSACTION
      DECLARE @friendshipStatus INT;
      SET @friendshipStatus =
      (SELECT Friendship.FriendshipStatus
       FROM Friendship,
            INSERTED
       WHERE INSERTED.RequestedToId = Friendship.RequestedById
         AND INSERTED.RequestedById = Friendship.RequestedToId
		 AND INSERTED.FriendshipStatus <> 1); -- <> rejected
      -- Checking if RequestedTo have already sent an friend request to RequestedBy.
      IF @friendshipStatus = 0
        THROW 50000, 'PendingFromRequestedTo', 1;
      IF @friendshipStatus = 2
        THROW 50002, 'Lasts', 1;
      -- @friendshipStatus IS NULL, so we can proceed.
      SET @friendshipStatus =
      (SELECT Friendship.FriendshipStatus
       FROM Friendship,
            INSERTED
       WHERE INSERTED.RequestedById = Friendship.RequestedById
         AND INSERTED.RequestedToId = Friendship.RequestedToId
		 AND INSERTED.FriendshipStatus <> 1);

      IF @friendshipStatus IS NULL
        INSERT INTO Friendship (RequestedById, RequestedToId, FriendshipStatus)
         (SELECT RequestedById, RequestedToId, FriendshipStatus
          FROM INSERTED); -- better way to do this? the trigger isn't called recursively.
      ELSE IF @friendshipStatus = 0
        THROW 50001, 'PendingFromRequestedBy', 1;
      ELSE IF @friendshipStatus = 2
        THROW 50002, 'Lasts', 1;
      ELSE THROW 50004, 'NotSupportedValue', 1;
      SELECT Id
      FROM Friendship f
      WHERE @@ROWCOUNT > 0
        AND f.Id = scope_Identity();
	COMMIT;
END;

GO

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
    DELETE FROM [User]
    FROM DELETED
    WHERE [User].Id = DELETED.Id   -- I know that it is not in the best style, but I don't know how to do it better. AFTER DELETE does not work, cause the deletion violates the onDelete restrict constraint and I cannot set NO ACTION.
  END;

GO