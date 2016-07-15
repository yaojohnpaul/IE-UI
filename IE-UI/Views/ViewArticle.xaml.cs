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
            SetBodyTextBlock();

            WhoTextBox.Text = article.Annotation.Who;
            WhenTextBox.Text = article.Annotation.FormattedWhen;
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

        private void SetBodyTextBlock()
        {
            string body = Article.Article.Body;

            List<bool> whoMatched = new List<bool>();
            List<bool> whenMatched = new List<bool>();
            List<bool> whereMatched = new List<bool>();
            bool whatMatched = false;
            bool whyMatched = false;

            List<string> whoAnnotations = new List<string>();
            List<string> whenAnnotations = new List<string>();
            List<string> whereAnnotations = new List<string>();

            List<BodySegment> listBodySegments = new List<BodySegment>();

            // Match annotations to body

            foreach (string who in Article.Annotation.Who.Split(';'))
            {
                if (!string.IsNullOrEmpty(who))
                {
                    bool isFound = body.Contains(who);

                    if (isFound)
                    {
                        whoMatched.Add(true);
                        whoAnnotations.Add(who);

                        int index = body.IndexOf(who);

                        listBodySegments.Add(new BodySegment()
                        {
                            StartIndex = index,
                            EndIndex = index + who.Length,
                            Label = "WHO"
                        });
                    }
                    else
                    {
                        whoMatched.Add(false);

                        Console.WriteLine("\"{0}\" was not found in the article.", who);
                    }
                }
                else
                {
                    break;
                }
            }

            foreach (string when in Article.Annotation.When.Split(';'))
            {
                if (!string.IsNullOrEmpty(when))
                {
                    bool isFound = body.Contains(when);

                    if (isFound)
                    {
                        whenMatched.Add(true);
                        whenAnnotations.Add(when);

                        int index = body.IndexOf(when);

                        listBodySegments.Add(new BodySegment()
                        {
                            StartIndex = index,
                            EndIndex = index + when.Length,
                            Label = "WHEN"
                        });
                    }
                    else
                    {
                        whenMatched.Add(false);

                        Console.WriteLine("\"{0}\" was not found in the article.", when);
                    }
                }
                else
                {
                    break;
                }
            }

            foreach (string where in Article.Annotation.Where.Split(';'))
            {
                if (!string.IsNullOrEmpty(where))
                {
                    bool isFound = body.Contains(where);

                    if (isFound)
                    {
                        whereMatched.Add(true);
                        whereAnnotations.Add(where);

                        int index = body.IndexOf(where);

                        listBodySegments.Add(new BodySegment()
                        {
                            StartIndex = index,
                            EndIndex = index + where.Length,
                            Label = "WHERE"
                        });
                    }
                    else
                    {
                        whereMatched.Add(false);

                        Console.WriteLine("\"{0}\" was not found in the article.", where);
                    }
                }
                else
                {
                    break;
                }
            }

            if (!string.IsNullOrEmpty(Article.Annotation.What))
            {
                string what = Article.Annotation.What;

                what = what.Replace("`` ", "\"");
                what = what.Replace(" ''", "\"");

                whatMatched = body.Contains(what);

                if (whatMatched)
                {
                    int index = body.IndexOf(what);

                    listBodySegments.Add(new BodySegment()
                    {
                        StartIndex = index,
                        EndIndex = index + what.Length,
                        Label = "WHAT"
                    });
                }
                else
                {
                    Console.WriteLine("\"{0}\" was not found in the article.", what);
                }
            }

            if (!string.IsNullOrEmpty(Article.Annotation.Why))
            {
                whyMatched = body.Contains(Article.Annotation.Why);

                if (whyMatched)
                {
                    int index = body.IndexOf(Article.Annotation.Why);

                    listBodySegments.Add(new BodySegment()
                    {
                        StartIndex = index,
                        EndIndex = index + Article.Annotation.Why.Length,
                        Label = "WHY"
                    });
                }
                else
                {
                    Console.WriteLine("\"{0}\" was not found in the article.", Article.Annotation.Why);
                }
            }

            // Check for overlapping ranges

            for (int i = 0; i < listBodySegments.Count; i++)
            {
                for (int j = i + 1; j < listBodySegments.Count; j++)
                {
                    if (listBodySegments[i].Intersects(listBodySegments[j]))
                    {
                        Console.WriteLine("\"{0}\":{1} intersects with \"{2}\":{3}",
                            body.Substring(listBodySegments[i].StartIndex, listBodySegments[i].EndIndex),
                            listBodySegments[i].Label,
                            body.Substring(listBodySegments[j].StartIndex, listBodySegments[j].EndIndex),
                            listBodySegments[j].Label);
                    }
                }
            }

            listBodySegments = listBodySegments.OrderByDescending(s => s.EndIndex).ToList();

            // Set the body text block

            foreach (BodySegment segment in listBodySegments)
            {
                body = body.Insert(segment.EndIndex, "</bold>");
                body = body.Insert(segment.StartIndex, "<bold>");
            }

            Console.WriteLine(body);

            InlineExpression.SetInlineExpression(BodyTextBlock, body);

            Console.WriteLine("Who: {0} | When: {1} | Where: {2} | What: {3} | Why: {4}",
                whoAnnotations.Count > 0 ? whoMatched.Count(x => x == true) / whoAnnotations.Count : -1,
                whenAnnotations.Count > 0 ? whenMatched.Count(x => x == true) / whenAnnotations.Count : -1,
                whereAnnotations.Count > 0 ? whereMatched.Count(x => x == true) / whereAnnotations.Count : -1,
                whatMatched ? 1 : -1,
                whyMatched ? 1 : -1);
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

                // Edit base file

                doc.Load(FilePath);

                XmlNode root = doc.DocumentElement;

                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["who"].InnerText = WhoTextBox.Text;
                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["when"].InnerText = WhenTextBox.Text;
                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["where"].InnerText = WhereTextBox.Text;
                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["what"].InnerText = WhatTextBox.Text;
                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["why"].InnerText = WhyTextBox.Text;

                doc.Save(FilePath);

                // Edit formatted date file
                
                string formattedDateFilePath = FilePath.Insert(FilePath.Length - 4, "_format_date");

                doc.Load(formattedDateFilePath);

                root = doc.DocumentElement;

                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["who"].InnerText = WhoTextBox.Text;
                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["when"].InnerText = WhenTextBox.Text;
                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["where"].InnerText = WhereTextBox.Text;
                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["what"].InnerText = WhatTextBox.Text;
                root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["why"].InnerText = WhyTextBox.Text;

                doc.Save(FilePath);

                // Edit inverted index file
                
                string invertedIndexFilePath = FilePath.Insert(FilePath.Length - 4, "_inverted_index");

                EditButtonLabel.Text = "edit";
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }

    public class BodySegment
    {
        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public string Label { get; set; }

        public bool Intersects(BodySegment bs)
        {
            if (this.StartIndex <= bs.EndIndex)
            {
                return (this.EndIndex >= bs.StartIndex);
            }
            else if (this.EndIndex >= bs.StartIndex)
            {
                return (this.StartIndex <= bs.EndIndex);
            }

            return false;
        }
    }
}

