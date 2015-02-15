using Mp3Editor.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Editor.Handler
{
    public class Writer
    {
        private string fileName;

        public Writer(string fileName)
        {
            this.fileName = fileName;
            
        }

        public void WriteTag(Tag tag)
        {
            // Open the file
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Write);

            if (stream.Length >= 128)
            {
                // http://en.wikipedia.org/wiki/ID3#Layout

                byte[] headerField = ConvertToByte("TAG", 3);
                byte[] titleField = ConvertToByte(tag.Title, 30);
                byte[] artistField = ConvertToByte(tag.Artist, 30);
                byte[] albumField = ConvertToByte(tag.Album, 30);
                byte[] yearField = ConvertToByte(tag.Year, 4);
                byte[] commentField = ConvertToByte(tag.Comment, 30);
                byte[] genreField = ConvertToByte(tag.Genre, 1);

                // ID3 layout is in the last 128 bytes
                // Seek there
                stream.Seek(-128, SeekOrigin.End);
                stream.Write(headerField, 0, headerField.Length);
                stream.Write(titleField, 0, titleField.Length);
                stream.Write(artistField, 0, artistField.Length);
                stream.Write(albumField, 0, albumField.Length);
                stream.Write(yearField, 0, yearField.Length);
                stream.Write(commentField, 0, commentField.Length);
                stream.Write(genreField, 0, genreField.Length);
            }

            stream.Close();
        }

        private byte[] ConvertToByte(string str, int length)
        {
            byte[] data;
            // Pad the string to the right
            // Add null 0x0 if necessary
            string paddedString = str.PadRight(length, '\0');
            data = Encoding.Default.GetBytes(paddedString);

            return data;
        }
    }
}
