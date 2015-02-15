using Mp3Editor.Handler;
using Mp3Editor.Entities;
using System;
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
using System.Windows.Shapes;

namespace Mp3Editor
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public EditWindow()
        {
            InitializeComponent();
        }

        private string filePath;

        public EditWindow(string filePath)
            : this()
        {
            this.filePath = filePath;

            Reader reader = new Reader(filePath);
            Tag tag = reader.ReadTag();

            if (tag != null)
            {
                txtTitolo.Text = tag.Title;
                txtArtista.Text = tag.Artist;
                txtAlbum.Text = tag.Album;
                txtAnno.Text = tag.Year;
                txtGenere.Text = tag.Genre;
                txtCommento.Text = tag.Comment;
            }            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Tag tag = new Tag()
            {
                Title = txtTitolo.Text,
                Artist = txtArtista.Text,
                Album = txtAlbum.Text,
                Year = txtAnno.Text,
                Genre = txtGenere.Text,
                Comment = txtCommento.Text
            };

            Writer writer = new Writer(filePath);
            writer.WriteTag(tag);

            MessageBox.Show("Salvato!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DropWindow window = new DropWindow();
            window.Show();
            this.Close();
        }
    }
}
