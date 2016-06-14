using IE_lib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Xml;

namespace IE_UI.Views
{
    /// <summary>
    /// Interaction logic for ViewArticle.xaml
    /// </summary>
    public partial class ViewArticle : Window
    {
        private List<TextBox> TextBoxes;
        private DisplayArticle Article;
        private string FilePath;

        public ViewArticle(DisplayArticle article, string filePath)
        {
            InitializeComponent();

            TextBoxes = new List<TextBox>();

            TextBoxes.Add(WhoTextBox);
            TextBoxes.Add(WhenTextBox);
            TextBoxes.Add(WhereTextBox);
            TextBoxes.Add(WhatTextBox);
            TextBoxes.Add(WhyTextBox);

            SetTextBoxesIsReadOnly(true);

            Article = article;
            FilePath = filePath;

            TitleTextBlock.Text = article.Article.Title;
            this.Title = TitleTextBlock.Text;
            AuthorTextBlock.Text = article.Article.Author;
            DateTextBlock.Text = article.Article.Date.ToString();
            Hyperlink.NavigateUri = new Uri(article.Article.Link);
            BodyTextBlock.Text = article.Article.Body;

            WhoTextBox.Text = article.Annotation.Who;
            WhenTextBox.Text = article.Annotation.When;
            WhereTextBox.Text = article.Annotation.Where;
            WhatTextBox.Text = article.Annotation.What;
            WhyTextBox.Text = article.Annotation.Why;
        }

        private void SetTextBoxesIsReadOnly(bool v)
        {
            foreach (TextBox tb in TextBoxes)
            {
                tb.IsReadOnly = v;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditButtonLabel.Text == "edit")
            {
                SetTextBoxesIsReadOnly(false);

                EditButtonLabel.Text = "save";
            }
            else if (EditButtonLabel.Text == "save")
            {
                SetTextBoxesIsReadOnly(true);

                XmlDocument doc = new XmlDocument();

                doc.Load(FilePath);

                XmlNode root = doc.DocumentElement;

                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["who"].InnerText = WhoTextBox.Text;
                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["when"].InnerText = WhenTextBox.Text;
                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["where"].InnerText = WhereTextBox.Text;
                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["what"].InnerText = WhatTextBox.Text;
                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["why"].InnerText = WhyTextBox.Text;

                doc.Save(FilePath);

                EditButtonLabel.Text = "edit";
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
