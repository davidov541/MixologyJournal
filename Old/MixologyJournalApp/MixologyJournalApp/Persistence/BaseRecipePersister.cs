using System;
using System.Xml.Linq;
using MixologyJournal.SourceModel.Recipe;

namespace MixologyJournal.Persistence
{
    internal class BaseRecipePersister
    {
        private const String PersistedTitle = "Title";
        private const String PersistedInstructions = "Instructions";
        private const String PersistedIngredient = "Ingredient";
        private const String PersistedIngredientName = "Name";
        private const String PersistedIngredientBrand = "Brand";
        private const String PersistedAmountQuantity = "AmountQuantity";
        private const String PersistedAmountUnit = "AmountUnit";
        private const String PersistedDerived = "DerivedRecipe";
        private const String PersistedId = "ID";

        protected void PopulateXmlElement(XElement entryElem, Recipe elementToWrite, XContainer parentNode)
        {
            entryElem.SetAttributeValue(PersistedTitle, elementToWrite.Name);
            entryElem.SetAttributeValue(PersistedInstructions, elementToWrite.Instructions);
            entryElem.SetAttributeValue(PersistedId, elementToWrite.ID.ToString());
            foreach (Ingredient ingredient in elementToWrite.Ingredients)
            {
                XElement ingredientElem = new XElement(PersistedIngredient);
                ingredientElem.SetAttributeValue(PersistedIngredientName, ingredient.Name);
                ingredientElem.SetAttributeValue(PersistedIngredientBrand, ingredient.Details);
                ingredientElem.SetAttributeValue(PersistedAmountUnit, ingredient.Amount.Unit.ToString());
                ingredientElem.SetAttributeValue(PersistedAmountQuantity, ingredient.Amount.Quantity.ToString());
                entryElem.Add(ingredientElem);
            }
            parentNode.Add(entryElem);
        }

        protected bool TryPopulateInstance(Recipe entry, XElement element)
        {
            if (element.Attribute(PersistedTitle) != null &&
                element.Attribute(PersistedInstructions) != null)
            {
                entry.Name = element.Attribute(PersistedTitle).Value;
                entry.Instructions = element.Attribute(PersistedInstructions).Value;
                // In case there were any ingredients added before hand, we should remove these and add whatever is persisted.
                entry.ClearIngredients();
                foreach (XElement elem in element.Elements())
                {
                    double amountQuantity = 0.0;
                    AmountUnit amountUnit = 0;
                    if (elem.Name.LocalName.Equals(PersistedIngredient) &&
                        elem.Attribute(PersistedIngredientName) != null &&
                        elem.Attribute(PersistedAmountUnit) != null &&
                        Enum.TryParse(elem.Attribute(PersistedAmountUnit).Value, out amountUnit) &&
                        elem.Attribute(PersistedAmountQuantity) != null &&
                        double.TryParse(elem.Attribute(PersistedAmountQuantity).Value, out amountQuantity))
                    {
                        String name = elem.Attribute(PersistedIngredientName).Value;
                        String brand = elem.Attribute(PersistedIngredientBrand) != null ? elem.Attribute(PersistedIngredientBrand).Value : null;
                        entry.AddIngredient(new Ingredient(name, brand, new Amount(amountQuantity, amountUnit)));
                    }
                }
                return true;
            }
            return false;
        }
    }
}
