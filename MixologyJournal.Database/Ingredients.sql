CREATE TABLE [dbo].[Ingredients]
(
	Id BIGINT NOT NULL PRIMARY KEY IDENTITY,
	RecipeId BIGINT NOT NULL,
	Name VARCHAR(100) NOT NULL, 
    [Brand] VARCHAR(200) NULL, 
    [AmountUnit] VARCHAR(50) NOT NULL, 
    [AmountQuantity] DECIMAL(5, 2) NOT NULL, 
    CONSTRAINT [FK_Ingredients_ToRecipe] FOREIGN KEY (RecipeId) REFERENCES Recipes(Id),

)
