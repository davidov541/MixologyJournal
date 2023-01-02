-- Script Date: 1/2/2023 2:48 PM  - ErikEJ.SqlCeScripting version 3.5.2.94
INSERT INTO [Recipe]
           ([Id]
           ,[Name]
           ,[StepsString])
     VALUES
           ("BuiltIn-1",
           "Old-Fashioned",
           "Combine all ingredients||Stir with ice||Serve");
INSERT INTO [IngredientUsage]
           ([Id]
           ,[IngredientId]
           ,[UnitId]
           ,[Amount]
           ,[Brand]
           ,[OwnerId])
     VALUES
           ("BuiltIn-OldFashioned-1",
           "BuiltIn-1",
           "BuiltIn-1",
           "2",
           NULL,
           "BuiltIn-1");
INSERT INTO [IngredientUsage]
           ([Id]
           ,[IngredientId]
           ,[UnitId]
           ,[Amount]
           ,[Brand]
           ,[OwnerId])
     VALUES
           ("BuiltIn-OldFashioned-2",
           "BuiltIn-2",
           "BuiltIn-1",
           "0.5",
           NULL,
           "BuiltIn-1");
INSERT INTO [IngredientUsage]
           ([Id]
           ,[IngredientId]
           ,[UnitId]
           ,[Amount]
           ,[Brand]
           ,[OwnerId])
     VALUES
           ("BuiltIn-OldFashioned-3",
           "BuiltIn-3",
           "BuiltIn-2",
           "2",
           NULL,
           "BuiltIn-1");
