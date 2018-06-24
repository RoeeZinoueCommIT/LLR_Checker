using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLLRC
{
    class LLRC_FixItemsAndUtils
    {
        #region Class verbs and c`tor

        string[] fixedLinesArray;
        string[] _stream;
        #endregion

        #region Fix general items

        #region Fix Tabs

        internal void FixTabs(string[] linesToFix, string _filePath)
        {
            Int32 linePos = 0, tabValue = 0;
            _stream = File.ReadAllLines(_filePath);

            string newLine = string.Empty, begingSpaceStr = string.Empty; ;
            foreach (var line in linesToFix)
            {
                if (line == string.Empty)
                {
                    continue;
                }
                linePos = Int32.Parse(line.Split(':')[1].Split(',')[0]);
                tabValue = Int32.Parse(line.Split(':')[2].Split(',')[0]);
                newLine = line.Split(':')[3];

                if (linePos == 696)
                {
                    int a = 0x7;
                }
                // Create space begin
                begingSpaceStr = string.Empty;
                for (int i = 0; i < tabValue; i++)
                {
                    begingSpaceStr += " ";
                }

                // Fix tabs:
                newLine = newLine.TrimStart();
                newLine = begingSpaceStr + newLine;

                // Store new line at strings array
                _stream[linePos - 1] = newLine;
            }

            // Write strings array to file
            WriteNewArrayToFile(_filePath, _stream);
        }

        #endregion

        #region Fix spaces

        internal void FixSpaces(string[] linesToFix, string _filePath)
        {
            _stream = File.ReadAllLines(_filePath);
            Int32 linePos = 0, spacePos = 0;
            string[] newLineArray;
            string newLine = string.Empty;

            for (int idx = 0; idx < linesToFix.Length - 4; idx += 4)
            {
                newLine = string.Empty;
                linePos = Int32.Parse(linesToFix[idx].Split(':')[1]);
                spacePos = Int32.Parse(linesToFix[idx + 1].Split(':')[1]);
                newLineArray = linesToFix[idx + 2].Substring(LLRC_Common.SPACE_STR_KEY_WORD.Length).Split();
                foreach (var word in newLineArray)
                {
                    if (word != string.Empty)
                    {
                        newLine += string.Format(" {0}", word);
                    }
                }
                _stream[linePos - 1] = newLine;
            }
            WriteNewArrayToFile(_filePath, _stream);
        }
        #endregion

        #endregion

        #region Common class methods
        private void WriteNewArrayToFile(string _filePath, string[] _stream)
        {
            File.WriteAllLines(_filePath, _stream, Encoding.UTF8);
        }
        #endregion
    }
}
