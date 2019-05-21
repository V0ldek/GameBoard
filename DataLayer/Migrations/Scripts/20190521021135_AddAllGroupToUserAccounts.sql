-- In this script name of main user group is hardcoded (this name is 'All')

-- Add 'All' group to user's accounts

INSERT INTO [Group] (OwnerId, Name)
SELECT [User].Id, 'All'
FROM [User];

DECLARE @UsersWithAllGroupId TABLE
(
	UserId NVARCHAR(450),
	AllGroupId INT
)

INSERT INTO @UsersWithAllGroupId (UserId, AllGroupId)
SELECT [User].Id, [Group].Id
FROM [User] 
JOIN [Group] ON [User].Id = [Group].OwnerId AND [Group].Name = 'All';

--Add all friends that friendship were requested by friends of users

INSERT INTO [GroupUser] (GroupId, UserId)
SELECT UsersWithAllGroupId.AllGroupId, [Friendship].RequestedById
FROM @UsersWithAllGroupId AS UsersWithAllGroupId 
JOIN [Friendship] ON UsersWithAllGroupId.UserId = [Friendship].RequestedToId;

--Add all friends that friendship were requested by users

INSERT INTO [GroupUser] (GroupId, UserId)
SELECT UsersWithAllGroupId.AllGroupId, [Friendship].RequestedToId
FROM @UsersWithAllGroupId AS UsersWithAllGroupId 
JOIN [Friendship] ON UsersWithAllGroupId.UserId = [Friendship].RequestedById;