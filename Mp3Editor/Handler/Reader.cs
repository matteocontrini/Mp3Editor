using Mp3Editor.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Editor.Handler
{
    public class Reader
    {
        private string fileName;

        public Reader(string fileName)
        {
            this.fileName = fileName;
        }

        public Tag ReadTag()
        {
            // Open the file
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            if (stream.Length >= 128)
            {
                // http://en.wikipedia.org/wiki/ID3#Layout

                byte[] headerField = new byte[3];
                byte[] titleField = new byte[30];
                byte[] artistField = new byte[30];
                byte[] albumField = new byte[30];
                byte[] yearField = new byte[4];
                byte[] commentField = new byte[30];
                byte[] genreField = new byte[1];

                // ID3 layout is in the last 128 bytes
                // Read from there to the end
                stream.Seek(-128, SeekOrigin.End);
                stream.Read(headerField, 0, headerField.Length);
                stream.Read(titleField, 0, titleField.Length);
                stream.Read(artistField, 0, artistField.Length);
                stream.Read(albumField, 0, albumField.Length);
                stream.Read(yearField, 0, yearField.Length);
                stream.Read(commentField, 0, commentField.Length);
                stream.Read(genreField, 0, genreField.Length);

                string headerString = Encoding.Default.GetString(headerField);

                // ID3 v1 layout starts with "TAG" string
                if (headerString == "TAG")
                {
                    string title = ConvertToString(titleField);
                    string artist = ConvertToString(artistField);
                    string album = ConvertToString(albumField);
                    string year = ConvertToString(yearField);
                    string comment = ConvertToString(commentField);
                    string genre = ConvertToString(genreField);

                    Tag = new Tag()
                    {
                        Title = title,
                        Artist = artist,
                        Album = album,
                        Year = year,
                        Comment = comment,
                        Genre = genre
                    };
                }
                else
                {
                    Tag = null;
                }
            }

            stream.Close();

            return Tag;
        }

        private string ConvertToString(byte[] data)
        {
            // http://msdn.microsoft.com/en-us/library/6aw8xdf2.aspx
            string[] toRemove = {
                                    "\0",
                                    "\n", "\t",
                                    "\v", "\b",
                                    "\r", "\f",
                                    "\a"
                                };

            string result = Encoding.Default.GetString(data);
            foreach (var item in toRemove)
            {
                result = result.Replace(item, string.Empty);
            }
            result = result.Trim();

            return result;
        }

        public Tag Tag { get; set; }
    }
}
