using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLLRC
{
    class LLRC_FixItems
    {
        #region Class verbs and c`tor

        string[] fixedLinesArray;
        string[] _stream;
        #endregion

        internal void FixTabs(string[] linesToFix, string _filePath)
        {

            Int32 linePos = 0, tabValue = 0;
            _stream = File.ReadAllLines(_filePath);

            string newLine = string.Empty, begingSpaceStr = string.Empty; ;
            foreach (var line in linesToFix)
            {
                if(line == string.Empty)
                {
                    continue;
                }
                linePos = Int32.Parse(line.Split(':')[1].Split(',')[0]);
                tabValue = Int32.Parse(line.Split(':')[2].Split(',')[0]);
                newLine = line.Split(':')[3];

                // Create space begin
                for (int i = 0; i < tabValue; i++)
                {
                    begingSpaceStr += " ";
                }

                // Fix tabs:
                newLine = newLine.TrimStart();
                newLine = begingSpaceStr + newLine;

                // Store new line at strings array
                _stream[linePos - 1] = newLine;

                // Write strings array to file
                WriteNewArrayToFile(_filePath, _stream);
            }
        }

        private void WriteNewArrayToFile(string _filePath, string[] _stream)
        {
            File.WriteAllLines(_filePath, _stream, Encoding.UTF8);
        }
    }
}
