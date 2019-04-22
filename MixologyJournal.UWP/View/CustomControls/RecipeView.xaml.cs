using System;
using System.Collections.Specialized;
using System.Linq;
using MixologyJournal.ViewModel.Recipe;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MixologyJournal.View
{
    public sealed partial class RecipeView : UserControl
    {
        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        public static DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register("ReadOnly", typeof(Boolean), typeof(RecipeView), new PropertyMetadata(false));

        public IViewRecipeViewModel Recipe
        {
            get { return (IViewRecipeViewModel)GetValue(RecipeProperty); }
            set { SetValue(RecipeProperty, value); }
        }

        public static DependencyProperty RecipeProperty =
            DependencyProperty.Register("Recipe", typeof(IViewRecipeViewModel), typeof(RecipeView), new PropertyMetadata(null, RecipeChanged));

        public static void RecipeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RecipeView view = d as RecipeView;
            if (e.OldValue != null)
            {
                // (e.OldValue as IViewRecipeViewModel).Ingredients.CollectionChanged -= view.RecipeView_PropertyChanged;
            }
            // (e.NewValue as IViewRecipeViewModel).Ingredients.CollectionChanged += view.RecipeView_PropertyChanged;
        }

        private async void RecipeView_PropertyChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IIngredientViewModel ingred in e.NewItems.OfType<IIngredientViewModel>())
                {
                    await Dispatcher.RunIdleAsync(a =>
                    {
                        ContentControl newListItem = IngredientsList.ContainerFromItem(ingred) as ContentControl;
                        if (newListItem != null)
                        {
                            IngredientView view = newListItem.ContentTemplateRoot as IngredientView;
                            view.IsExpanded = true;
                        }
                    });
                }
            }
        }

        public bool WritingEnabled
        {
            get
            {
                return !ReadOnly;
            }
        }

        public bool IsTitleVisible
        {
            get
            {
                return ShowTitleBox && WritingEnabled;
            }
        }

        public bool ShowTitleBox
        {
            get { return (bool)GetValue(ShowTitleBoxProperty); }
            set { SetValue(ShowTitleBoxProperty, value); }
        }

        public static DependencyProperty ShowTitleBoxProperty =
            DependencyProperty.Register("ShowTitleBox", typeof(Boolean), typeof(RecipeView), new PropertyMetadata(true));

        public RecipeView()
        {
            this.InitializeComponent();
        }
    }
}
