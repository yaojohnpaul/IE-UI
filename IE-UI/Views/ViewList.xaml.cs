using IE_lib.Models;
using IE_UI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IE_UI.Views
{
    /// <summary>
    /// Interaction logic for ViewList.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Page" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class ViewList : Page
    {
        /// <summary>
        /// The results
        /// </summary>
        ParsedResults Results;
        /// <summary>
        /// The list raw list articles
        /// </summary>
        List<ListArticle> ListRawListArticles;

        /// <summary>
        /// The list query text boxes
        /// </summary>
        List<TextBox> ListQueryTextBoxes;
        /// <summary>
        /// The list criteria combo boxes
        /// </summary>
        List<ComboBox> ListCriteriaComboBoxes;
        /// <summary>
        /// The list type combo boxes
        /// </summary>
        List<ComboBox> ListTypeComboBoxes;

        /// <summary>
        /// The criterias
        /// </summary>
        private String[] Criterias = new String[] { "who", "when", "where", "what", "why" };
        /// <summary>
        /// The types
        /// </summary>
        private String[] Types = new String[] { "AND", "OR" };

        /// <summary>
        /// The is advanced
        /// </summary>
        private bool IsAdvanced = false;
        /// <summary>
        /// The path
        /// </summary>
        private string Path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewList"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public ViewList(string path)
        {
            InitializeComponent();

            App.Current.MainWindow.Title = "Articles list";

            Path = path;

            LoadArticles();
        }

        /// <summary>
        /// Loads the articles.
        /// </summary>
        private void LoadArticles()
        {
            Results = IE_lib.Main.View(Path);

            ListRawListArticles = new List<ListArticle>();

            if (Results == null)
            {
                MessageBox.Show(Application.Current.MainWindow,
                    "Either there are no articles found or other input files are missing.",
                    "Invalid input file");
            }
            else
            {
                foreach (DisplayArticle d in Results.ListDisplayArticles)
                {
                    ListRawListArticles.Add(new ListArticle()
                    {
                        DisplayArticle = d,
                        MatchedString = String.Format("{0} · {1}", d.Article.Date, d.Article.Author)
                    });
                }
            }

            if (!IsAdvanced) DisplayArticles(ListRawListArticles);
        }

        /// <summary>
        /// Handles the Click event of the AdvancedSearchToggle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AdvancedSearchToggle_Click(object sender, RoutedEventArgs e)
        {
            if (AdvancedSearchPanel.Visibility == Visibility.Collapsed)
            {
                LabelTextBlock.Text = "ADVANCED SEARCH";

                AdvancedSearchPanel.Visibility = Visibility.Visible;
                AdvancedSearchToggle.Content = Char.ConvertFromUtf32(0xE711);
                BasicSearchTextBox.Visibility = Visibility.Hidden;
                BasicSearchTextBox.Text = "";

                ListQueryTextBoxes = new List<TextBox>();
                ListCriteriaComboBoxes = new List<ComboBox>();
                ListTypeComboBoxes = new List<ComboBox>();

                StackPanel newPanel = new StackPanel();

                newPanel.Orientation = Orientation.Horizontal;
                newPanel.HorizontalAlignment = HorizontalAlignment.Left;

                TextBox newQuery = new TextBox();

                newQuery.Name = "searchQuery" + ListQueryTextBoxes.Count;
                newQuery.FontSize = 16;
                newQuery.Padding = new Thickness(8);
                newQuery.Width = 632;

                ListQueryTextBoxes.Add(newQuery);
                newPanel.Children.Add(newQuery);

                ComboBox newCriteria = new ComboBox();

                newCriteria.Name = "criteriaBox" + ListCriteriaComboBoxes.Count;
                newCriteria.FontSize = 16;
                newCriteria.Padding = new Thickness(8);
                newCriteria.Width = 100;

                foreach (String s in Criterias)
                {
                    newCriteria.Items.Add(s);
                }

                newCriteria.SelectedIndex = 0;

                ListCriteriaComboBoxes.Add(newCriteria);
                newPanel.Children.Add(newCriteria);

                QueryPanel.Children.Add(newPanel);
            }
            else if (AdvancedSearchPanel.Visibility == Visibility.Visible)
            {
                LabelTextBlock.Text = "ARTICLES";

                AdvancedSearchPanel.Visibility = Visibility.Collapsed;
                AdvancedSearchToggle.Content = Char.ConvertFromUtf32(0xE713);
                BasicSearchTextBox.Visibility = Visibility.Visible;

                QueryPanel.Children.RemoveRange(0, QueryPanel.Children.Count);
            }
        }

        /// <summary>
        /// Handles the Click event of the BackButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAdvanced)
            {
                this.NavigationService.Navigate(new Home());
            }
            else
            {
                DisplayArticles(ListRawListArticles);
                LabelTextBlock.Text = "ARTICLES";
                IsAdvanced = false;
            }
        }

        /// <summary>
        /// Displays the articles.
        /// </summary>
        /// <param name="articles">The articles.</param>
        private void DisplayArticles(List<ListArticle> articles)
        {
            if (AdvancedSearchPanel.Visibility == Visibility.Visible)
            {
                AdvancedSearchPanel.Visibility = Visibility.Collapsed;
                AdvancedSearchToggle.Content = Char.ConvertFromUtf32(0xE713);
                BasicSearchTextBox.Visibility = Visibility.Visible;

                QueryPanel.Children.RemoveRange(0, QueryPanel.Children.Count);
            }

            if (articles.Count > 0)
            {
                StatusText.Text = String.Format(articles.Count > 1 ? "displaying {0} articles" : "displaying {0} article", articles.Count);
                ArticleListView.ItemsSource = articles;
            }
            else
            {
                StatusText.Text = String.Format("no articles found");
                ArticleListView.ItemsSource = null;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the BasicSearchTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void BasicSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<ListArticle> filtered = new List<ListArticle>();

            foreach (ListArticle l in ListRawListArticles)
            {
                if (l.DisplayArticle.Article.Title.ToLower().Contains(BasicSearchTextBox.Text.ToLower()))
                {
                    filtered.Add(l);
                }
            }

            DisplayArticles(filtered);
        }

        /// <summary>
        /// Handles the Click event of the AdvancedSearchButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AdvancedSearchButton_Click(object sender, RoutedEventArgs e)
        {
            List<int> whoIndex = new List<int>();
            List<int> whenIndex = new List<int>();
            List<int> whereIndex = new List<int>();
            List<int> whatIndex = new List<int>();
            List<int> whyIndex = new List<int>();
            List<int> finalResults = new List<int>();
            List<int>[] queryResults = new List<int>[ListQueryTextBoxes.Count];

            for (int i = 0; i < ListQueryTextBoxes.Count; i++)
            {
                queryResults[i] = new List<int>();
            }

            List<List<int>> mergedAndResults = new List<List<int>>();
            List<ListArticle> filtered = new List<ListArticle>();

            //Find the index of the queries for each w
            for (int i = 0; i < ListCriteriaComboBoxes.Count; i++)
            {
                switch (ListCriteriaComboBoxes[i].Text)
                {
                    case "who":
                        whoIndex.Add(i);
                        break;
                    case "when":
                        whenIndex.Add(i);
                        break;
                    case "where":
                        whereIndex.Add(i);
                        break;
                    case "what":
                        whatIndex.Add(i);
                        break;
                    case "why":
                        whyIndex.Add(i);
                        break;
                    default:
                        break;
                }
            }

            //Find matches for who
            if (whoIndex.Count > 0)
            {
                foreach (KeyValuePair<String, List<int>> entry in Results.WhoReverseIndex)
                {
                    foreach (int queryIndex in whoIndex)
                    {
                        if (entry.Key.IndexOf(ListQueryTextBoxes[queryIndex].Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            queryResults[queryIndex].AddRange(entry.Value);
                        }
                    }
                }
            }

            //Find matches for when
            if (whenIndex.Count > 0)
            {
                foreach (KeyValuePair<String, List<int>> entry in Results.WhenReverseIndex)
                {
                    foreach (int queryIndex in whenIndex)
                    {
                        if (entry.Key.IndexOf(ListQueryTextBoxes[queryIndex].Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            queryResults[queryIndex].AddRange(entry.Value);
                        }
                    }
                }
            }

            //Find matches for where
            if (whereIndex.Count > 0)
            {
                foreach (KeyValuePair<String, List<int>> entry in Results.WhereReverseIndex)
                {
                    foreach (int queryIndex in whereIndex)
                    {
                        if (entry.Key.IndexOf(ListQueryTextBoxes[queryIndex].Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            queryResults[queryIndex].AddRange(entry.Value);
                        }
                    }
                }
            }

            //Find matches for what
            if (whatIndex.Count > 0)
            {
                foreach (KeyValuePair<String, List<int>> entry in Results.WhatReverseIndex)
                {
                    foreach (int queryIndex in whatIndex)
                    {
                        if (entry.Key.IndexOf(ListQueryTextBoxes[queryIndex].Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            queryResults[queryIndex].AddRange(entry.Value);
                        }
                    }
                }
            }

            //Find matches for why
            if (whyIndex.Count > 0)
            {
                foreach (KeyValuePair<String, List<int>> entry in Results.WhyReverseIndex)
                {
                    foreach (int queryIndex in whyIndex)
                    {
                        if (entry.Key.IndexOf(ListQueryTextBoxes[queryIndex].Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            queryResults[queryIndex].AddRange(entry.Value);
                        }
                    }
                }
            }

            int itr = 0;

            foreach (List<int> list in queryResults)
            {
                Console.WriteLine("Iteration " + itr);

                foreach (int num in list)
                {
                    Console.Write(num + ";");
                }

                itr++;
            }

            //Merge results from queries
            for (int i = 0; i < ListTypeComboBoxes.Count; i++)
            {
                if (ListTypeComboBoxes[i].Text.Equals("AND"))
                {
                    queryResults[i + 1] = queryResults[i].Intersect<int>(queryResults[i + 1]).ToList<int>();
                }
                else if (ListTypeComboBoxes[i].Text.Equals("OR"))
                {
                    mergedAndResults.Add(queryResults[i]);

                    if (i + 1 == ListTypeComboBoxes.Count)
                    {
                        finalResults.AddRange(queryResults[i + 1]);
                    }
                }
            }

            if (mergedAndResults.Count > 0)
            {
                foreach (List<int> result in mergedAndResults)
                {
                    finalResults.AddRange(result);
                }
                finalResults = finalResults.Distinct().ToList();
            }
            else
            {
                finalResults = queryResults[queryResults.Length - 1].Distinct().ToList();
            }

            foreach (int index in finalResults)
            {
                filtered.Add(ListRawListArticles.First(x => x.DisplayArticle.Annotation.Index == index));
            }

            DisplayArticles(filtered);

            LabelTextBlock.Text = "SEARCH RESULTS";

            IsAdvanced = true;
        }

        /// <summary>
        /// Handles the Click event of the CriteriaAddButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CriteriaAddButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel newPanel = new StackPanel();

            newPanel.Orientation = Orientation.Horizontal;
            newPanel.HorizontalAlignment = HorizontalAlignment.Left;

            TextBox newQuery = new TextBox();

            newQuery.Name = "searchQuery" + ListQueryTextBoxes.Count;
            newQuery.FontSize = 16;
            newQuery.Padding = new Thickness(8);
            newQuery.Width = 632;

            ListQueryTextBoxes.Add(newQuery);
            newPanel.Children.Add(newQuery);

            ComboBox newCriteria = new ComboBox();

            newCriteria.Name = "criteriaBox" + ListCriteriaComboBoxes.Count;
            newCriteria.FontSize = 16;
            newCriteria.Padding = new Thickness(8);
            newCriteria.Width = 100;

            foreach (String s in Criterias)
            {
                newCriteria.Items.Add(s);
            }

            newCriteria.SelectedIndex = 0;

            ListCriteriaComboBoxes.Add(newCriteria);
            newPanel.Children.Add(newCriteria);

            //ComboBox newType = new ComboBox();

            //newType.Name = "criteriaType" + ListTypeComboBoxes.Count;
            //newType.FontSize = 16;
            //newType.Padding = new Thickness(8);
            //newType.Width = 80;

            //foreach (String s in Types)
            //{
            //    newType.Items.Add(s);
            //}

            //newType.SelectedIndex = 0;

            //ListTypeComboBoxes.Add(newType);
            //newPanel.Children.Add(newType);

            newPanel.Margin = new Thickness(0, 4, 0, 0);

            QueryPanel.Children.Add(newPanel);
        }

        /// <summary>
        /// Handles the PreviewMouseWheel event of the ScrollViewer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseWheelEventArgs"/> instance containing the event data.</param>
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        /// <summary>
        /// Handles the PreviewMouseLeftButtonDown event of the ArticleListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void ArticleListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as ListArticle;

            if (item != null)
            {
                ViewArticle v = new ViewArticle(item.DisplayArticle, Results.FilePath);
                v.Closed += (s, args) =>
                {
                    LoadArticles();
                };
                v.ShowDialog();

            }
        }

        //private void ArticleListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    var item = ((FrameworkElement)e.OriginalSource).DataContext as ListArticle;

        //    if (item != null)
        //    {
        //        ViewArticle v = new ViewArticle(item.DisplayArticle, Results.FilePath);
        //        v.Show();
        //    }
        //}
    }
}
