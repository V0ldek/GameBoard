-- In this script name of main user group is hardcoded (this name is 'All')

-- Add 'All' group to user's accounts

INSERT INTO [Group] (OwnerId, Name)
SELECT [User].Id, 'All'
FROM [User];

DECLARE @UsersWithAllGroupId TABLE
(
	UserId NVARCHAR(450),
	AllGroupId int
)

-- we don't need to join on [Group].Name = 'All', because all users own only one group

INSERT INTO @UsersWithAllGroupId (UserId, AllGroupId)
SELECT [User].Id, [Group].Id
FROM [User] JOIN [Group] ON [User].Id = [Group].OwnerId;

--Add all friends that friendship were requested by friends of users

INSERT INTO [GroupUser] (GroupId, UserId)
SELECT UsersWithAllGroupId.AllGroupId, [Friendship].RequestedById
FROM @UsersWithAllGroupId as UsersWithAllGroupId JOIN [Friendship] ON UsersWithAllGroupId.UserId = [Friendship].RequestedToId;

--Add all friends that friendship were requested by users

INSERT INTO [GroupUser] (GroupId, UserId)
SELECT UsersWithAllGroupId.AllGroupId, [Friendship].RequestedToId
FROM @UsersWithAllGroupId as UsersWithAllGroupId JOIN [Friendship] ON UsersWithAllGroupId.UserId = [Friendship].RequestedById;