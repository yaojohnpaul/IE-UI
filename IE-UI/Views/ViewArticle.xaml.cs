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
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class ViewArticle : Window
    {
        /// <summary>
        /// The text boxes
        /// </summary>
        private List<TextBox> TextBoxes;
        /// <summary>
        /// The article
        /// </summary>
        private DisplayArticle Article;
        /// <summary>
        /// The file path
        /// </summary>
        private string FilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewArticle"/> class.
        /// </summary>
        /// <param name="article">The article.</param>
        /// <param name="filePath">The file path.</param>
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

        /// <summary>
        /// Sets the text boxes is read only.
        /// </summary>
        /// <param name="v">if set to <c>true</c> [v].</param>
        private void SetTextBoxesIsReadOnly(bool v)
        {
            foreach (TextBox tb in TextBoxes)
            {
                tb.IsReadOnly = v;
            }
        }

        /// <summary>
        /// Sets the body text block.
        /// </summary>
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
                //body = body.Insert(segment.EndIndex, "</bold>");
                //body = body.Insert(segment.StartIndex, "<bold>");
            }

            InlineExpression.SetInlineExpression(BodyTextBlock, body);

            //Console.WriteLine("Who: {0} | When: {1} | Where: {2} | What: {3} | Why: {4}",
            //    whoAnnotations.Count > 0 ? whoMatched.Count(x => x == true) / whoAnnotations.Count : -1,
            //    whenAnnotations.Count > 0 ? whenMatched.Count(x => x == true) / whenAnnotations.Count : -1,
            //    whereAnnotations.Count > 0 ? whereMatched.Count(x => x == true) / whereAnnotations.Count : -1,
            //    whatMatched ? 1 : -1,
            //    whyMatched ? 1 : -1);
        }

        /// <summary>
        /// Handles the Click event of the EditButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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
                XmlNode root;
                string formattedDateFilePath = FilePath.Insert(FilePath.Length - 4, "_format_date");
                string invertedIndexFilePath = FilePath.Insert(FilePath.Length - 4, "_inverted_index");
                bool whoChanged = Article.Annotation.Who.Trim() != WhoTextBox.Text.Trim();
                bool whenChanged = Article.Annotation.FormattedWhen.Trim() != WhenTextBox.Text.Trim();
                bool whereChanged = Article.Annotation.Where.Trim() != WhereTextBox.Text.Trim();
                bool whatChanged = Article.Annotation.What.Trim() != WhatTextBox.Text.Trim();
                bool whyChanged = Article.Annotation.Why.Trim() != WhyTextBox.Text.Trim();

                // Edit base file

                doc.Load(FilePath);

                root = doc.DocumentElement;

                if(whoChanged) root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["who"].InnerText = WhoTextBox.Text;
                if(whenChanged) root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["when"].InnerText = WhenTextBox.Text;
                if(whereChanged) root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["where"].InnerText = WhereTextBox.Text;
                if(whatChanged) root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["what"].InnerText = WhatTextBox.Text;
                if(whyChanged) root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["why"].InnerText = WhyTextBox.Text;

                doc.Save(FilePath);

                // Edit formatted date file
                
                doc.Load(formattedDateFilePath);

                root = doc.DocumentElement;

                if(whoChanged) root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["who"].InnerText = WhoTextBox.Text;
                if(whenChanged) root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["when"].InnerText = WhenTextBox.Text;
                if(whereChanged) root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["where"].InnerText = WhereTextBox.Text;
                if(whatChanged) root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["what"].InnerText = WhatTextBox.Text;
                if(whyChanged) root.SelectSingleNode("/data/article[" + (Article.Annotation.Index + 1) + "]")["why"].InnerText = WhyTextBox.Text;

                doc.Save(formattedDateFilePath);

                // Edit inverted index file
                
                doc.Load(invertedIndexFilePath);

                XmlNode whoNode = doc.DocumentElement.SelectSingleNode("/data/who");
                XmlNode whenNode = doc.DocumentElement.SelectSingleNode("/data/when");
                XmlNode whereNode = doc.DocumentElement.SelectSingleNode("/data/where");
                XmlNode whatNode = doc.DocumentElement.SelectSingleNode("/data/what");
                XmlNode whyNode = doc.DocumentElement.SelectSingleNode("/data/why");

                if(whoChanged) UpdateInvertedIndexFile(doc, whoNode, Article.Annotation.Who, WhoTextBox.Text);
                if(whenChanged) UpdateInvertedIndexFile(doc, whenNode, Article.Annotation.When, WhenTextBox.Text);
                if(whereChanged) UpdateInvertedIndexFile(doc, whereNode, Article.Annotation.Where, WhereTextBox.Text);
                if(whatChanged) UpdateInvertedIndexFile(doc, whatNode, Article.Annotation.What, WhatTextBox.Text);
                if(whyChanged) UpdateInvertedIndexFile(doc, whyNode, Article.Annotation.Why, WhyTextBox.Text);

                doc.Save(invertedIndexFilePath);

                EditButtonLabel.Text = "edit";
            }
        }

        /// <summary>
        /// Updates the inverted index file.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="baseNode">The base node.</param>
        /// <param name="oldFeature">The old feature.</param>
        /// <param name="newFeature">The new feature.</param>
        private void UpdateInvertedIndexFile(XmlDocument doc, XmlNode baseNode, string oldFeature, string newFeature)
        {
            XmlNode newNode = null;
            XmlNodeList nodeList = baseNode.SelectNodes("entry");

            foreach (XmlNode entry in nodeList)
            {
                Console.WriteLine("''{0}'' vs ''{1}''", entry["text"].InnerText, oldFeature);

                if (entry["text"].InnerText == oldFeature)
                {
                    foreach (XmlNode index in entry.SelectNodes("articleIndex"))
                    {
                        if (Convert.ToInt32(index.InnerText) == Article.Annotation.Index)
                        {
                            index.ParentNode.RemoveChild(index);
                        }
                    }
                }
                else if (entry["text"].InnerText == newFeature)
                {
                    newNode = entry;
                }
            }

            if (newNode == null)
            {
                XmlNode newText = doc.CreateElement("text");
                newText.InnerText = newFeature;

                XmlNode newEntry = doc.CreateElement("entry");
                newEntry.AppendChild(newText);

                baseNode.AppendChild(newEntry);

                newNode = newEntry;
            }

            XmlNode newIndex = doc.CreateElement("articleIndex");
            newIndex.InnerText = Convert.ToString(Article.Annotation.Index);

            newNode.AppendChild(newIndex);
        }

        /// <summary>
        /// Handles the RequestNavigate event of the Hyperlink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RequestNavigateEventArgs"/> instance containing the event data.</param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }

    /// <summary>
    /// Class for containing the indices of a feature in the body of the article.
    /// </summary>
    public class BodySegment
    {
        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        public int StartIndex { get; set; }

        /// <summary>
        /// Gets or sets the end index.
        /// </summary>
        /// <value>
        /// The end index.
        /// </value>
        public int EndIndex { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string Label { get; set; }

        /// <summary>
        /// Checks if this body segment intersects the specified body segment.
        /// </summary>
        /// <param name="bs">The body segment.</param>
        /// <returns><c>true</c> if they intersect; otherwise, <c>false</c>.</returns>
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

