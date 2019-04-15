DROP TRIGGER IF EXISTS TR_Friendship_InsteadOfInsert;

GO

CREATE TRIGGER TR_Friendship_InsteadOfInsert
  ON friendships
  INSTEAD OF INSERT
AS
  BEGIN
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
    BEGIN TRANSACTION
      DECLARE @friendshipStatus INT;
      SET @friendshipStatus =
      (SELECT friendships.friendshipStatus
       FROM friendships,
            inserted
       WHERE inserted.requestedToId = friendships.requestedById
         AND inserted.requestedById = friendships.requestedToId);
      -- Checking if RequestedTo have already sent an friend request to RequestedBy.
      IF @friendshipStatus = 0
        THROW 50000, 'PendingFromRequestedTo', 1;
      IF @friendshipStatus = 2
        THROW 50002, 'Lasts', 1;
      -- @friendshipStatus IS NULL or @friendshipStatus = 1, so we can proceed.
      SET @friendshipStatus =
      (SELECT friendships.friendshipStatus
       FROM friendships,
            inserted
       WHERE inserted.requestedById = friendships.requestedById
         AND inserted.requestedToId = friendships.requestedToId);

      IF @friendshipStatus IS NULL
        INSERT INTO friendships (requestedById, requestedToId, friendshipStatus)
         (SELECT requestedById, requestedToId, friendshipStatus
          FROM inserted); -- better way to do this? the trigger isn't called recursively.
      ELSE IF @friendshipStatus = 0
        THROW 50001, 'PendingFromRequestedBy', 1;
      ELSE IF @friendshipStatus = 2
        THROW 50002, 'Lasts', 1;
      ELSE IF @friendshipStatus = 1
        UPDATE friendships
        SET friendships.friendshipStatus = inserted.friendshipStatus
        FROM inserted
        WHERE inserted.requestedById = friendships.requestedById
          AND inserted.requestedToId = friendships.requestedToId; -- code repetition, better way TO do this?
      ELSE THROW 50004, 'NotSupportedValue', 1;
      SELECT id
      FROM friendships f
      WHERE @@ROWCOUNT > 0
        AND f.id = scope_identity();
	COMMIT;
END;

GO

DROP TRIGGER IF EXISTS TR_Friendship_InsteadOfDelete;

GO

CREATE TRIGGER TR_Friendship_InsteadOfDelete
  ON [User]
  INSTEAD OF DELETE
AS
  BEGIN
    DELETE FROM friendships
    FROM deleted
    WHERE requestedById = deleted.id
       OR requestedToId = deleted.id;
    DELETE FROM [User]
    FROM deleted
    WHERE [User].id = deleted.id   -- I know that it is not in the best style, but I don't know how to do it better. AFTER DELETE does not work, cause the deletion violates the onDelete restrict constraint and I cannot set NO ACTION.
  END;

GO