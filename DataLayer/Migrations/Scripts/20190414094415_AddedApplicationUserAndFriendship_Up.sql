DROP TRIGGER IF EXISTS uniquelastingorpendingfriendship;

GO

CREATE TRIGGER uniquelastingorpendingfriendship
  ON friendships
  INSTEAD OF INSERT
AS
  BEGIN
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
    BEGIN TRANSACTION
      DECLARE @friendshipstatus INT;
      SET @friendshipstatus =
      (SELECT friendships.friendshipstatus
       FROM friendships,
            inserted
       WHERE inserted.requestedtoid = friendships.requestedbyid
         AND inserted.requestedbyid = friendships.requestedtoid);
      -- Checking if RequestedTo have already sent an friend request to RequestedBy.
      IF @friendshipstatus = 0
        THROW 50000, 'PendingFromRequestedTo', 1;
      IF @friendshipstatus = 2
        THROW 50002, 'Lasts', 1;
      -- @friendshipStatus IS NULL or @friendshipStatus = 1, so we can proceed.
      SET @friendshipstatus =
      (SELECT friendships.friendshipstatus
       FROM friendships,
            inserted
       WHERE inserted.requestedbyid = friendships.requestedbyid
         AND inserted.requestedtoid = friendships.requestedtoid);

      IF @friendshipstatus IS NULL
        INSERT INTO friendships (requestedbyid, requestedtoid, friendshipstatus)
         (SELECT requestedbyid, requestedtoid, friendshipstatus
          FROM inserted); -- better way to do this? the trigger isn't called recursively.
      ELSE IF @friendshipstatus = 0
        THROW 50001, 'PendingFromRequestedBy', 1;
      ELSE IF @friendshipstatus = 2
        THROW 50002, 'Lasts', 1;
      ELSE IF @friendshipstatus = 1
        UPDATE friendships
        SET friendships.friendshipstatus = inserted.friendshipstatus
        FROM inserted
        WHERE inserted.requestedbyid = friendships.requestedbyid
          AND inserted.requestedtoid = friendships.requestedtoid; -- code repetition, better way TO do this?
      ELSE THROW 50004, 'NotSupportedValue', 1;
      SELECT id
      FROM friendships f
      WHERE @@ROWCOUNT > 0
        AND f.id = scope_identity();
	COMMIT;
END;

GO

DROP TRIGGER IF EXISTS cascadedeletefriendships;

GO

CREATE TRIGGER cascadedeletefriendships
  ON [User]
  INSTEAD OF DELETE
AS
  BEGIN
    DELETE FROM friendships
    FROM deleted
    WHERE requestedbyid = deleted.id
       OR requestedtoid = deleted.id;
    DELETE FROM [User]
    FROM deleted
    WHERE [User].id = deleted.id   -- I know that it is not in the best style, but I don't know how to do it better. AFTER DELETE does not work, cause the deletion violates the onDelete restrict constraint and I cannot set NO ACTION.
  END;

GO