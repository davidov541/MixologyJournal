using System;
using System.Linq;
using MixologyJournal.ViewModel.Recipe;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MixologyJournal.View
{
    public sealed partial class IngredientView : UserControl
    {
        public Object Ingredient
        {
            get { return (IIngredientViewModel)GetValue(IngredientProperty); }
            set { SetValue(IngredientProperty, value); }
        }

        public static DependencyProperty IngredientProperty =
            DependencyProperty.Register("Ingredient", typeof(IIngredientViewModel), typeof(IngredientView), new PropertyMetadata(null));

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(Boolean), typeof(IngredientView), new PropertyMetadata(false, IsExpandedChanged));

        public static void IsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IngredientView).ApplyExpanded();
        }

        public IEditRecipeViewModel Recipe
        {
            get { return (IEditRecipeViewModel)GetValue(RecipeProperty); }
            set { SetValue(RecipeProperty, value); }
        }

        public static DependencyProperty RecipeProperty =
            DependencyProperty.Register("Recipe", typeof(IViewRecipeViewModel), typeof(IngredientView), new PropertyMetadata(null));

        public IngredientView()
        {
            this.InitializeComponent();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void Expand_Click(Object sender, RoutedEventArgs e)
        {
            IsExpanded = !IsExpanded;
        }

        private void ApplyExpanded()
        {
            if (IsExpanded)
            {
                PaneStoryboard.Children.OfType<DoubleAnimation>().FirstOrDefault().To = EditPane.ActualHeight;
                PaneStoryboard.Begin();
            }
            else
            {
                CloseStoryboard.Begin();
            }
        }

        private void Remove_Click(Object sender, RoutedEventArgs e)
        {
            Recipe.RemoveIngredient(Ingredient as IEditIngredientViewModel);
        }
    }
}
