using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LLLRC
{
    class LLRC_CheckList
    {
        #region C`tor and verbs

        // File descriptor
        string[] _stream;

        // Return list results
        List<string> _res;
        
        //Grammer correction verbs
        Microsoft.Office.Interop.Word.Application appWord;
        Object nullArgmnt = null;

        // Function header checker
        List<Int32> _startHeaderIdx, _endHeaderIdx, _tempStartHeaderIdx, _tempEndHeaderIdx;

        // Temp list
        List<string> _tempList;

        // Global verbs list
        List<string> _objectVerbInfo;

        // List of header fileds
        List<string> _headerReadFields;

        // FunctionsNamesList
        List<string> _funcNames;

        public LLRC_CheckList()
        {
            appWord = new Microsoft.Office.Interop.Word.Application();
            _res = new List<string>();

            _startHeaderIdx = new List<int>();
            _endHeaderIdx = new List<int>();

            _tempList = new List<string>();
            _objectVerbInfo = new List<string>();
            _headerReadFields = new List<string>();

            _funcNames = new List<string>();
            _tempStartHeaderIdx = new List<int>();
            _tempEndHeaderIdx = new List<int>();
        }
        #endregion

        #region General and common check methods

        #region English grammer checker

        internal List<string> CheckGrammer(string _filePath)
        {
            _res.Clear();
            appWord.Documents.Add();
            _stream = File.ReadAllLines(_filePath);
            bool foundWordInDic = false;

            for (int idx = 0; idx < _stream.Length; idx++)
            {
                if ((false == _stream[idx].Contains("*")) && (_stream[idx] != string.Empty) && (false == _stream[idx].Contains("include")))
                {
                    var lineObject = _stream[idx].Split();
                    foreach (var word in lineObject)
                    {
                        if (word == string.Empty)
                        {
                            continue;
                        }
                        else
                        {
                            if (false == CheckWordForGrammer(word))
                            {
                                foundWordInDic = false;

                                // Check if the word apear in the allowed words list
                                foreach (var item in LLRC_Common.ALLOWED_WORDS)
                                {
                                    if(word == item)
                                    {
                                        foundWordInDic = true;
                                        break;
                                    }
                                }
                                if(false == foundWordInDic)
                                {
                                    _res.Add("Line: " + idx + " Word: " + word);
                                }
                            }
                        }
                    }
                }
            }
            return _res;
        }

        private bool CheckWordForGrammer(string word)
        {
            // Clear word from prefix or endix:
            word = word.Replace(":", string.Empty);

            // Check if word contain numbers:
            if (word.Any(c => char.IsDigit(c)))
            {
                return true;
            }
            else if (false == Regex.IsMatch(word, @"^[a-zA-Z]+$"))
            {
                return true;
            }

            return appWord.CheckSpelling(word, ref nullArgmnt, ref nullArgmnt, ref nullArgmnt,
                                                            ref nullArgmnt, ref nullArgmnt, ref nullArgmnt,
                                                            ref nullArgmnt, ref nullArgmnt, ref nullArgmnt,
                                                            ref nullArgmnt, ref nullArgmnt, ref nullArgmnt);
        }



        #endregion

        #region Check spaces

        internal List<string> CheckSpaces(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            int spacePos = 0;

            for (int idx = 0; idx < _stream.Length; idx++)
            {
                MatchCollection matches = Regex.Matches(_stream[idx], "(\\s+)");
                int[] spaceArray = matches.OfType<Match>().Select(m => m.Length).ToArray();

                spacePos = CheckSpaces(spaceArray);
                if (spacePos >= 0 )
                {
                    _res.Add(string.Format("Line: {0}" + Environment.NewLine + "Pos: {1}" + Environment.NewLine + LLRC_Common.SPACE_STR_KEY_WORD + "{2}" + Environment.NewLine, idx + 1, spacePos, _stream[idx]));
                }
            }
            return _res;
        }

        private int CheckSpaces(int[] spaceArray)
        {
            for (int idx = 1; idx < spaceArray.Length; idx++)
            {
                if (spaceArray[idx] > 1)
                {
                    return idx;
                }
            }
            return (-1);
        }

        internal List<string> GrammerRemoveDifrances(string[] lines)
        {
            _res.Clear();

            for (int idx = 0; idx < lines.Length; idx++)
            {
                lines[idx] = lines[idx].Split(':')[2];
            }
            _res = lines.ToList();
            _res = _res.Distinct().ToList();

            return (_res);
        }
        #endregion

        #region Check Tabs

        internal List<string> CheckTabs(string _filePath)
        {
            // First get range of code that don`t contain any header type
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();
            GetLineIdx(_stream, "{", "}", LLRC_Common.SPECIAL_END_FINDS.END_OF_FUNCTION);

            // Remove duplicate from list
            _startHeaderIdx = _startHeaderIdx.Distinct().ToList();
            _endHeaderIdx = _endHeaderIdx.Distinct().ToList();

            // Now check Tabs for all possible ranges
            try
            {
                for (int idx = 0; idx < Math.Min(_startHeaderIdx.Count, _endHeaderIdx.Count); idx++)
                {
                    CheckTabsSpace(_stream, _startHeaderIdx[idx], _endHeaderIdx[idx], 1);
                }
            }
            catch (Exception ex)
            {
                _res.Add(string.Format(LLRC_Common.EXEPTION_FIELD + " {0}", ex.Message));
            }

            return (_res);
        }

        private void CheckTabsSpace(string[] _stream, int startPos, int stopPos, int tabIndex)
        {
            int tabPosIndx = 1;
            bool inSwitchLoop = false;
            int decTabPos = 0, IncTabPos = 0;
            string lastLine = string.Empty;
            for (int rowIdx = startPos + 1; rowIdx < stopPos; rowIdx++)
            {
                decTabPos = 0;
                IncTabPos = 0;
                
                MatchCollection matches = Regex.Matches(_stream[rowIdx], "(\\s+)");
                int[] spaceArray = matches.OfType<Match>().Select(m => m.Length).ToArray();

                if ((spaceArray.Length == 0) || (true == _stream[rowIdx].Contains("#"))) // If there is nothing writing in row skip row.
                {
                    continue;
                }
                else
                {
                    if (rowIdx == 809)
                    {
                        int a = 0x7;
                    }

                    if (true == _stream[rowIdx].Contains("switch"))
                    {
                        inSwitchLoop = true;
                    }

                    #region Increment tab pos right

                    if (true == _stream[rowIdx].Contains("{"))
                    {
                        IncTabPos++;
                    }
                    if ((true == _stream[rowIdx].Contains("case")) && (false == _stream[rowIdx + 1].Contains("{")))
                    {
                        IncTabPos++;
                    }
                    if (true == _stream[rowIdx].Contains("default"))
                    {
                        if(true == lastLine.Contains("case"))
                        {
                            tabPosIndx--;
                        }
                        IncTabPos++;
                    }
                    #endregion

                    #region Decrement tab pos left

                    if(true == inSwitchLoop)
                    {
                        if (true == _stream[rowIdx].Contains("break;"))
                        {
                            if (true == _stream[rowIdx + 1].Contains("}"))
                            {
                                decTabPos = 2;
                                rowIdx++;   /* Don`t check next line, we already knew what is it } */ 
                                inSwitchLoop = false;
                            }
                            else
                            {
                                decTabPos++;
                            }
                            
                        }
                    }
                    else if (false == inSwitchLoop)
                    {
                        if ((true == _stream[rowIdx + 1].Contains("}")) && (false == _stream[rowIdx + 1].Contains("case:")))
                        {
                            decTabPos++;
                        }
                    }
                    
                    
                    #endregion
                    
                    #region Check tab position

                    if (spaceArray[0] != (4 * tabPosIndx))
                    {
                        _res.Add(string.Format("Tab error. Pos: {0}, Tab: {1}, Row: {2}" + Environment.NewLine, rowIdx + 1, 4 * tabPosIndx, _stream[rowIdx].Trim()));
                    }

                    // Decrement / increment tab position
                    if (decTabPos > 0)
                    {
                        for (int idx = 0; idx < decTabPos; idx++)
                        {
                            tabPosIndx--;
                        }
                    }
                    else if (IncTabPos > 0)
                    {
                        for (int idx = 0; idx < IncTabPos; idx++)
                        {
                            tabPosIndx++;
                        }

                    }

                    lastLine = _stream[rowIdx];
                    #endregion

                }
            }
        }

        #endregion

        #region Check not use in Elbit types

        internal List<string> CheckTypes(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            string newWord = string.Empty;
            char[] splitchar = { ' ', '(', ')' };
            foreach (var item in LLRC_Common.UNSUPPORT_ELBIT_TYPES)
            {
                for (int idx = 0; idx < _stream.Length; idx++)
                {
                    var lineArray = _stream[idx].Split(splitchar);
                    foreach (var word in lineArray)
                    {
                        if (true == word.Equals(item))
                        {
                            _res.Add(string.Format("Pos: {0} \n Line: {1}",idx + 1, _stream[idx]));
                        }
                    }
                }
            }
            return (_res);
        }
        #endregion

        #region Check source strucutre

        internal List<string> CheckSourceStructure(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);

            string foundKey = string.Empty;
            _res.Add("Found in file:" + Environment.NewLine);

            foreach (var field in LLRC_Common.SOURCE_STRUCT_TITLES)
            {
                var results = Array.FindAll(_stream, s => s.Equals(field));
                if(results.Length > 0)
                {
                    foundKey = new String(field.Where(Char.IsLetter).ToArray());
                    _res.Add(string.Format("Found fields: {0}", foundKey));
                }
                else
                {
                    _res.Add(string.Format("Don`t found fields: {0}", field));
                }
            }
            return (_res);
        }
        #endregion

        #region Check header file

        internal List<string> CheckHeaderStructure(string _filePath)
        {

            _res.Clear();
            _stream = File.ReadAllLines(_filePath);

            string foundKey = string.Empty;
            _res.Add("Found in file:" + Environment.NewLine);

            foreach (var field in LLRC_Common.HEADER_STRUCT_TITLES)
            {
                var results = Array.FindAll(_stream, s => s.Equals(field));
                if (results.Length > 0)
                {
                    foundKey = new String(field.Where(Char.IsLetter).ToArray());
                    _res.Add(string.Format("Found fields: {0}", foundKey));
                }
                else
                {
                    _res.Add(string.Format("Don`t found fields: {0}", field));
                }
            }
            return (_res);
        }

        #endregion

        #region Check struct header

        internal List<string> CheckStructHeader(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();

            GetLineIdx(_stream, "$STRUCTURES$", "/*--", LLRC_Common.SPECIAL_END_FINDS.NONE);

            string moduleName = GetModuleName(_filePath);

            try
            {
                for (int idx = 0; idx < Math.Min(_startHeaderIdx.Count, _endHeaderIdx.Count); idx++)
                {
                    CheckStructObject(_stream, moduleName, _startHeaderIdx[idx], _endHeaderIdx[idx]);
                }
            }
            catch (Exception ex)
            {
                _res.Add(string.Format(LLRC_Common.EXEPTION_FIELD + " {0}", ex.Message));
            }
            return (_res);
        }

        private void CheckStructObject(string[] _stream, string moduleName, int startPos, int endPos)
        {
            _objectVerbInfo.Clear();
            GetObjectVerbsInfo(_stream, startPos + 1, endPos + 1);
            CheckHeaderFields(_stream, "Structure", LLRC_Common.STRUCTURE_FIELDS, startPos, endPos);
            AnlyzeStructMember(_stream, startPos, endPos, moduleName);

            _res.Add(Environment.NewLine);
        }

        private void AnlyzeStructMember(string[] _stream, int startPos, int stopPos, string moduleName)
        {
            // Check struct name:
            CheckAndPrintStructName(_objectVerbInfo[_objectVerbInfo.Count - 1], moduleName, stopPos + 2);
            CheckInGroupKey(_stream, startPos, stopPos, moduleName);
            CheckStructMatch(_stream, startPos, stopPos);

            _res.Add(Environment.NewLine);
        }

        private void CheckAndPrintStructName(string input, string moduleName, int linePos)
        {
            string structName = input.Replace("}", string.Empty).Replace(";", string.Empty).Replace(" ", string.Empty);

            // Print struct name
            _res.Add(string.Format("Struct name: {0}, at line pos: {1}", structName, linePos));

            // Check struct name
            if (false == structName.Contains(string.Format("DT_{0}", moduleName)))
            {
                _res.Add(string.Format("Struct type is not correct"));
            }
        }

        private void CheckStructMatch(string[] _stream, int startPos, int stopPos)
        {
            string headerLine = string.Empty;
            string declerationLine = string.Empty;
            int linePos = startPos;

            foreach (var item in _headerReadFields)
            {
                if (true == item.Contains("Members Types:"))
                {
                    headerLine = item.Split(':')[1].Replace("|",",").Replace(" ", string.Empty);
                    break;
                }
            }

            if(headerLine != string.Empty)
            {
                var verbsContent = _objectVerbInfo[1].Replace(" ", string.Empty).Split(',');

                for (int idx = 3; idx < _objectVerbInfo.Count - 2; idx+=2)
                {
                    declerationLine += _objectVerbInfo[idx].Trim() + ",";
                }
                declerationLine = declerationLine.Substring(0, declerationLine.Length - 1);

                if (headerLine != declerationLine)
                {
                    _res.Add(string.Format("Members Types don`t match. \nHeader: {0} \nDecleration {1}", headerLine, declerationLine));
                }
            }
            else
            {
                _res.Add(string.Format("Members Types field don`t found in header"));
            }

        }

        private void GetStrcutVerbsInfo(string[] _stream, int startPosLines)
        {
            int linePos = startPosLines;

            string tmpStr = string.Empty;
            string veriableTypes = string.Empty;
            string veriableNames = string.Empty;

            _objectVerbInfo.Add(_stream[linePos++]);

            while(false == _stream[linePos].Contains("}"))
            {
                if (false == _stream[linePos].Contains("{"))
                {
                    veriableTypes += _stream[linePos].Split()[2] + ", ";
                    veriableNames += _stream[linePos].Split()[3].Split(';')[0] + ", ";
                }

                linePos++;
            }
            _objectVerbInfo.Add(veriableTypes);
            _objectVerbInfo.Add(veriableNames);
            _objectVerbInfo.Add(_stream[linePos].Split('}')[1].Split(';')[0]);
        }
        #endregion

        #region Check define header

        internal List<string> CheckDefineHeader(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();

            GetLineIdx(_stream, "$DEFINES$", "/*--", LLRC_Common.SPECIAL_END_FINDS.NONE);

            string moduleName = GetModuleName(_filePath);

            try
            {
                for (int idx = 0; idx < Math.Min(_startHeaderIdx.Count, _endHeaderIdx.Count); idx++)
                {
                    CheckDefineHeader(_stream, moduleName, _startHeaderIdx[idx], _endHeaderIdx[idx]);
                }
            }
            catch (Exception ex)
            {
                _res.Add(string.Format(LLRC_Common.EXEPTION_FIELD + " {0}", ex.Message));
            }
            return (_res);
        }

        private void CheckDefineHeader(string[] _stream, string moduleName, int startPos, int endPos)
        {
            _objectVerbInfo.Clear();

            // Get define header fields
            GetObjectVerbsInfo(_stream, endPos + 1, endPos);

            _res.Add(string.Format("Define name: {0} \nLine pos: {1}", _objectVerbInfo[1], endPos + 2));

            // Check define fields
            CheckHeaderFields(_stream, "Define", LLRC_Common.DEFINE_FIELDS, startPos, endPos);

            // Print and check define name
            CheckDefineName(startPos, endPos);

            _res.Add(Environment.NewLine);
        }
        #endregion

        #region Check Enums

        internal List<string> CheckEnums(string _filePath)
        {
            _res.Clear();

            return (_res);
        }
        #endregion

        private void CheckDefineName(int startPos, int stopPos)
        {
            string fieldDefine = string.Empty;
            string fieldHeader = string.Empty;

            // Check name
            fieldDefine = _objectVerbInfo[1].Trim();
            fieldHeader = _headerReadFields[1].Split(':')[1].Trim();
            if (true == fieldDefine.Contains("C_"))
            {
                if(false == fieldDefine.Contains(fieldHeader))
                {
                    _res.Add(string.Format("Define name in decleration {0} don`t match name in header: {1}", fieldDefine, fieldHeader));
                }
            }

            // Check define value
            fieldDefine = _objectVerbInfo[2].Trim();
            fieldHeader = _headerReadFields[3].Split(':')[1].Trim();
            if (false == fieldDefine.Contains(fieldHeader))
            {
                _res.Add(string.Format("Define value in decleration {0} don`t match value in header: {1}", fieldDefine, fieldHeader));
            }
        }
        #endregion

        #region Check source file

        #region Check source file header

        internal List<string> CheckSourceFileHeader(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();

            string moduleName = GetModuleName(_filePath);
            GetLineIdx(_stream, @"/**", "Logic", LLRC_Common.SPECIAL_END_FINDS.NONE);

            if(_endHeaderIdx.Count == 0 || _startHeaderIdx.Count == 0)
            {
                _res.Add(string.Format("Don`t found start and end position of header"));  
            }
            else
            {
                _endHeaderIdx[0] = _endHeaderIdx[0] + 2;
                try
                {
                    for (int idx = 0; idx < Math.Min(_startHeaderIdx.Count, _endHeaderIdx.Count); idx++)
                    {
                        CheckSourceFileHeader(_stream, _startHeaderIdx[idx], _endHeaderIdx[idx], moduleName);
                    }
                }
                catch (Exception ex)
                {
                    _res.Add(string.Format(LLRC_Common.EXEPTION_FIELD + " {0}", ex.Message));
                }
            }
            return (_res);
        }

        private void CheckSourceFileHeader(string[] _stream, int startPos, int endPos, string moduleName)
        {
            // Print module name:
            _res.Add(string.Format("Module name {0}: ", moduleName));

            // Check for TBD words
            CheckForTBDWord(_stream, startPos, endPos);

            // Check header fields
            CheckHeaderFields(_stream, "Src header", LLRC_Common.SRC_FILE_HEADER_FILEDS, startPos, endPos);

            // Check in group fields
            CheckInGroupKey(_stream, startPos, endPos, moduleName);

            _res.Add(Environment.NewLine);
        }

        private void CheckForTBDWord(string[] _stream, int startPos, int endPos)
        {
            for (int idx = startPos; idx < endPos; idx++)
            {
                if(_stream[idx].Contains("TBD"))
                {
                    _res.Add(string.Format("TBD found in line: {0}", _stream[idx]));
                }
            }
        }
        #endregion

        #region Check Function header

        internal List<string> CheckFunctionHeader(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();

            string moduleName = GetModuleName(_filePath);
            GetLineIdx(_stream, "$PROCEDURE$", "/*--", LLRC_Common.SPECIAL_END_FINDS.NONE);

            try
            {
                for (int idx = 0; idx < Math.Min(_startHeaderIdx.Count, _endHeaderIdx.Count); idx++)
                {
                    CheckFunctionHeaderContent(_stream, _startHeaderIdx[idx], _endHeaderIdx[idx], moduleName);
                }
            }
            catch (Exception ex)
            {
                _res.Add(string.Format(LLRC_Common.EXEPTION_FIELD + " {0}", ex.Message));
            }
            return (_res);
        }

        private void CheckFunctionHeaderContent(string[] _stream, int startPos, int endPos, string moduleName)
        {
            _objectVerbInfo.Clear();

            // Print function name
            checkFunctionsName(_stream, moduleName, endPos + 1, true);

            GetObjectVerbsInfo(_stream, startPos + 1, endPos);
            CheckHeaderFields(_stream, "Function", LLRC_Common.FUNCTION_FIELDS, startPos, endPos);
            CheckInGroupKey(_stream, startPos, endPos, moduleName);
            CheckFrsLinkField(_stream, startPos, endPos);
            CheckBriefKey(_stream, startPos, endPos);
            CheckParamKey(_stream, startPos, endPos);
            CheckDeriveDesc(_stream, startPos, endPos);
            _res.Add(Environment.NewLine);
        }

        private void CheckFrsLinkField(string[] _stream, int startPos, int endPos)
        {
            string word_val = string.Empty;
            string[] word_container;
            foreach (var field in _headerReadFields)
            {
                if(field.Contains(@"\FRSLinks_Common"))
                {
                    word_container = field.Split();
                    for (int idx = 1; idx < word_container.Length; idx++)
                    {
                        if(word_container[idx] != string.Empty)
                        {
                            word_val = word_container[idx];
                            break;
                        }
                        else
                        {
                            _res.Add(string.Format("FRS line. There is unnecessary space. Line ({0})", startPos));
                        }
                    }

                    if(false == word_val.Contains("FRSLinks_Common"))
                    {
                        if ((true == word_val.Contains("AHRS_GEN_SDD")) || (true == word_val.Contains("AHRS_BSP")))
                        {
                            return;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            _res.Add(string.Format("FRS_common issue. word: ({0}) is not valid. If it general please write AHRS_GEN_SDD_1453", word_val));
        }

        private void CheckParamKey(string[] _stream, int startLine, int stopLine)
        {
            string paramWord = string.Empty;
            bool foundFlag = false;
            for (int idx = startLine; idx < stopLine; idx++)
            {
                if (_stream[idx].Contains("range:"))
                {
                    foundFlag = false;
                    paramWord = _stream[idx].Split(':')[1].Split('.')[0].Trim();

                    if (true == _stream[idx - 1].Contains("*"))  // Check if veriable is pointer
                    {
                        foreach (var avbRange in LLRC_Common.ALLOWED_RANGE_VALUES_POINTER)
                        {
                            if(avbRange == paramWord)
                            {
                                foundFlag = true;
                                break;
                            }
                        }   
                    }
                    else if (true == _stream[idx].Contains("["))
                    {
                        foundFlag = true;
                    }
                    else
                    {
                        foreach (var avbRange in LLRC_Common.ALLOWED_RANGE_VALUES_PRIMITIVE)
                        {
                            if (avbRange == paramWord)
                            {
                                foundFlag = true;
                                break;
                            }
                        }
                    }
                    if (false == foundFlag)
                    {
                        _res.Add(string.Format("range issue, Line pos: {0}", idx + 1));
                    }
                }
            }
        }

        private void CheckInGroupKey(string[] _stream, int startLine, int stopLine, string moduleName)
        {
            string inGroupField = _headerReadFields[0];
            for (int idx = startLine; idx < stopLine; idx++)
            {
                if (inGroupField.Contains("ingroup"))
                {
                    if (false == inGroupField.Contains(moduleName))
                    {
                        _res.Add(string.Format("ingroup issue, module name ({0}) don`t found at line ({1})", moduleName, idx + 1));
                    }
                    break;
                }
            }
        }

        private void CheckBriefKey(string[] _stream, int startLine, int stopLine)
        {
            for (int idx = startLine; idx < stopLine; idx++)
            {
                if (_stream[idx].Contains("brief"))
                {
                    if ((false == _stream[idx].Contains("Local function:")) && (false == _stream[idx].Contains("Interface function:")))
                    {
                        _res.Add(string.Format("brief issue, Line pos: {0}", idx + 1));
                    }
                    break;
                }
            }
        }

        private void CheckDeriveDesc(string[] _stream, int startLine, int stopLine)
        {
            for (int idx = startLine; idx < stopLine; idx++)
            {
                if (_stream[idx].Contains("DerivedDesc"))
                {
                    while (false == _stream[idx].Contains("Justification") && (false == _stream[idx].Contains("/*--")))
                    {
                        if (false == _stream[idx].Contains("The function shall call") && (false == _stream[idx].Contains("N/A")))
                        {
                            _res.Add(string.Format("Derive description issue, Line pos: {0}", idx + 1));
                            break;
                        }
                        idx++;
                    }
                }
            }
        }
        #endregion

        #region Check functions contents

        internal List<string> CheckSourceFunctionContents(string _filePath)
        {
            _res.Clear();
            _funcNames.Clear();
            _stream = File.ReadAllLines(_filePath);
            string moduleName = GetModuleName(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();
            GetLineIdx(_stream, "{", "}", LLRC_Common.SPECIAL_END_FINDS.END_OF_FUNCTION);

            // Remove duplicate from list
            _startHeaderIdx = _startHeaderIdx.Distinct().ToList();

            GetFunctionNamesAndPos(_stream, _startHeaderIdx);

            try
            {
                for (int idx = 0; idx < Math.Min(_startHeaderIdx.Count, _endHeaderIdx.Count); idx++)
                {
                    CheckFunctionContents(_stream, moduleName, _startHeaderIdx[idx], _endHeaderIdx[idx]);
                }
            }
            catch (Exception ex)
            {
                _res.Add(string.Format(LLRC_Common.EXEPTION_FIELD + " {0}", ex.Message));
            }
            return (_res);
        }

        private void CheckFunctionContents(string[] _stream, string moduleName, int startPos, int endPos)
        {
            // Check function validty name
            checkFunctionsName(_stream, moduleName, startPos - 1, false);

            // Check functions inputs and return statement
            FunctionCheckReturnAndInputs(_stream, startPos, endPos);

            // Space between block
            _res.Add(Environment.NewLine);
        }

        private void checkFunctionsName(string[] _stream, string moduleName, int startPos, bool functionNameOnly)
        {
            string[] funcNameContainer;
            bool isEmptyRow = false;
            int lineIdx = 0;

            // Skip empty rows in file.
            while (_stream[startPos + lineIdx] == string.Empty)
            {
                lineIdx++;
                isEmptyRow = true;
            }

            funcNameContainer = _stream[startPos + lineIdx++].Split();

            string funcName = string.Empty;

            // Print function name

            if (true == funcNameContainer[0].Contains("static"))
            {
                funcName = funcNameContainer[2].Split('(')[0];
            }
            else
            {
                funcName = funcNameContainer[1].Split('(')[0];
            }
            
            _res.Add(string.Format("Function name: {0} \nstart pos: {1}", funcName, startPos));

            if (true == isEmptyRow)
            {
                _res.Add(string.Format("There is empty row between header and function. line pos {0}", startPos + 1));
            }

            // Check function name
            if (false == functionNameOnly)
            {
                // Check function name - Contain module name
                if (false == funcName.Contains(moduleName))
                {
                    _res.Add(string.Format("Issue: Don`t find module name ({0}) in function name ({1})", moduleName, funcName));
                }

                // Check function name - Contain "p_"
                if (false == funcName.Contains("p_"))
                {
                    _res.Add(string.Format("Issue: Don`t find prefix (p_) in function name ({0}) \n", funcName));
                }
            }
        }

        private void FunctionCheckReturnAndInputs(string[] _stream, int startPos, int endPos)
        {
            string[] functions_inputs_verbs;
            bool foundReturnInFuncDef = false;
            string return_value = string.Empty;

            if (true == _stream[startPos - 1].Contains("(") && true == _stream[startPos - 1].Contains(")"))
            {
                functions_inputs_verbs = _stream[startPos - 1].Split('(')[1].Split(')')[0].Split(',');

                // Check input for validity (start with xi, xo, etc...)
                foreach (var item in functions_inputs_verbs)
                {
                    if (false == item.Contains("xi_") &&
                        false == item.Contains("xo_") &&
                        false == item.Contains("xio_") &&
                        false == item.Contains("void"))
                    {
                        _res.Add(string.Format("Variable {0} don`t fit prefix (xi,xo,xio)", item));
                    }
                }

                // Check return statement
                if (true == _stream[endPos - 1].Contains("return"))
                {
                    return_value = _stream[endPos - 1].Trim().Split()[1].Replace(";", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);
                    foreach (var item in functions_inputs_verbs)
                    {
                        if (item == return_value)
                        {
                            foundReturnInFuncDef = true;
                        }
                    }
                    if (false == foundReturnInFuncDef)
                    {
                        if (true == return_value.Contains("xi_") ||
                        true == return_value.Contains("xo_") ||
                        true == return_value.Contains("xio_") ||
                        true == return_value.Contains("void"))
                        {
                            _res.Add(string.Format("Return value ({0}) at line ({1}) contain [xi / xo / xio] and don`t apear in function argument list", return_value, endPos));
                        }
                    }
                }
            }
        }

        #endregion

        #region Check global header
        internal List<string> CheckGlobalHeader(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear(); _tempStartHeaderIdx.Clear(); _tempEndHeaderIdx.Clear();

            GetLineIdx(_stream, "$GLOBAL VARIABLES$", "/*--", LLRC_Common.SPECIAL_END_FINDS.NONE);

            _tempStartHeaderIdx.AddRange(_startHeaderIdx);
            _tempEndHeaderIdx.AddRange(_endHeaderIdx);

            string moduleName = GetModuleName(_filePath);

            try
            {
                for (int idx = 0; idx < Math.Min(_tempStartHeaderIdx.Count, _tempEndHeaderIdx.Count); idx++)
                {
                    _objectVerbInfo.Clear();

                    CheckHeaderFields(_stream, "Global", LLRC_Common.GLOBAL_FIELDS, _tempStartHeaderIdx[idx], _tempEndHeaderIdx[idx]);
                    GetObjectVerbsInfo(_stream, _tempEndHeaderIdx[idx] + 1, _endHeaderIdx[idx]);
                    CheckAndPrintGlobalFields(moduleName, _startHeaderIdx[idx], _endHeaderIdx[idx]);
                }
            }
            catch (Exception ex)
            {
                _res.Add(string.Format(LLRC_Common.EXEPTION_FIELD + " {0}", ex.Message));
            }
            return _res;
        }

        private void CheckAndPrintGlobalFields(string moduleName, int startPos, int endPos)
        {
            // Check static field:
            if(_objectVerbInfo[0] != "static")
            {
                _res.Add(string.Format("Global name: {0}", _objectVerbInfo[1].Split('[')[0].Replace(';', ' ').Trim()));
                _res.Add(string.Format("Static field is missing"));
            }
            else
            {
                _res.Add(string.Format("Global name: {0}", _objectVerbInfo[2].Split('[')[0].Replace(';', ' ').Trim()));
            }
            
            
            // Check global name:
            CheckVeriableNameLine(_stream, moduleName, endPos);
            
            // Check with functions use this global:
            CheckGlobalUsedByRow(_stream, _objectVerbInfo[2]);

            // Check math in assignment field:
            CheckGlobalAssignmentField(_stream, endPos + 1);

            // Place new line to seperate between globals
            _res.Add(Environment.NewLine);
        }

        private void CheckGlobalAssignmentField(string[] _stream, int globalPos)
        {
            string declerationData = string.Empty;
            string headerData = string.Empty;

            if(false == _stream[globalPos].Contains("="))
            {
                _res.Add(string.Format("Don`t find any assignment field"));
            }
            
            else
            {
                if(false == _stream[globalPos].Split('=')[1].Contains(")"))
                {
                    declerationData = _stream[globalPos].Split('=')[1];
                }
                else
                {
                    declerationData = _stream[globalPos].Split('=')[1].Split(')')[1];
                }

                declerationData = declerationData.Replace("{", string.Empty).Replace("}", string.Empty).Replace(";", string.Empty).Replace(" ", string.Empty);

                // Check default value in global header:
                foreach (var item in _headerReadFields)
                {
                    if(true == item.Contains("Default value"))
                    {
                        headerData = item;
                        break;
                    }
                }
                
                if(headerData == string.Empty)
                {
                    _res.Add(string.Format("Don`t find Default value field in header"));
                }
                else
                {
                    headerData = headerData.Replace("{", string.Empty).Replace("}", string.Empty).Split(':')[1].Replace(" ", string.Empty).Replace(".", string.Empty);
                    if (declerationData != headerData)
                    {
                        _res.Add(string.Format("Assigment field in definishion ({0}) don`t match in header ({1}).", declerationData, headerData));
                    }
                }
            }
        }

        private void CheckGlobalUsedByRow(string[] _stream, string globalName)
        {
            _funcNames.Clear();
            _startHeaderIdx.Clear();
            List<int> globalIndex = new List<int>();
            Int32 tempFuncPos = 0;
            string tempFuncName = string.Empty, lineRes = string.Empty, fixGlobalName = string.Empty;

            // First find all lines in file that global apear:
            fixGlobalName = globalName.Split('[')[0].Replace(';', ' ').Trim();

            for (int idx = 0; idx < _stream.Length; idx++)
            {
                if (true == _stream[idx].Contains(fixGlobalName))
                {
                    globalIndex.Add(idx);
                }
            }

            // Get function start index and functions names:
            GetLineIdx(_stream, "{", "}", LLRC_Common.SPECIAL_END_FINDS.END_OF_FUNCTION);
            _startHeaderIdx = _startHeaderIdx.Distinct().ToList();
            GetFunctionNamesAndPos(_stream, _startHeaderIdx);

            foreach (var globalLine in globalIndex)
            {
                tempFuncName = string.Empty;

                for (int idx = 0; idx < _funcNames.Count; idx++)
                {
                    tempFuncPos = Int32.Parse(_funcNames[idx].Split(',')[0].Split(':')[1]);
                    if(tempFuncPos < globalLine)
                    {
                        tempFuncName = _funcNames[idx].Split(',')[1].Split(':')[1];
                    }
                    else
                    {
                        break;
                    }
                }

                if(tempFuncName != string.Empty)
                {
                    if(false == lineRes.Contains(tempFuncName))
                    {
                        lineRes += string.Format("{0},", tempFuncName);
                    }
                }
            }

            if(lineRes == string.Empty)
            {
                _res.Add(string.Format("Don`t found in any functions"));
            }
            else
            {
                _res.Add(string.Format("Found in functions: {0}", lineRes));
            }
            
        }

        private bool GetObjectVerbsInfo(string[] _stream, int startPos, int endPos)
        {
            bool ret = true;
            int linePos = startPos;

            if (_stream[startPos] == string.Empty)
            {
                _res.Add(string.Format("Can`t find object line {0}", startPos));
                ret = false;
            }
            else
            {
                while((_stream[linePos] != string.Empty) /*&& (linePos < endPos)*/ )
                {
                    var collection = _stream[linePos].Split();
                    foreach (var item in collection)
                    {
                        if (item != string.Empty)
                        {
                            _objectVerbInfo.Add(item);
                        }
                    }
                    linePos++;
                }
            }
            return (ret);
        }

        private void CheckStaticGlobalVerbs(int linePos)
        {
            if(_objectVerbInfo[0] != "static")
            {
                _res.Add(string.Format("Global. static not fould. line pos: {0}", linePos));
            }
        }
       
        private void CheckVeriableNameLine(string[] _stream, string moduleName, int globalPos)
        {
            string defineGlobalName = string.Empty, headerGlobalName = string.Empty; 
            //int idx = startLine;

            // Check global veriable name math rules:
            string nameShape = string.Format("g_{0}", moduleName);
            var nameArray = _objectVerbInfo[2].Split('[')[0].Split(';')[0].Split('_');

            if ((false == _objectVerbInfo[2].Contains(nameShape)) && (false == _objectVerbInfo[3].Contains(nameShape)))
            {
                _res.Add(string.Format("Global veriable name ({0}) dont match module name ({1}). pos: {0}", _objectVerbInfo[2], moduleName));
            }

            for (int i = 2; i < nameArray.Length; i++)
            {
                if(nameArray[i].Any(c => char.IsDigit(c)))
                {
                    continue;
                }
                bool isOnlyLower = nameArray[i].Count(c => Char.IsLower(c)) == nameArray[i].Length;
                if (false == isOnlyLower)
                {
                    _res.Add(string.Format("Global. veriable name dont contain only lower latter. pos: {0}", globalPos));
                }
            }

            // Check if global name in header match global name veriable: 
            headerGlobalName = _headerReadFields[1].Split(':')[1].Trim();
            defineGlobalName = _objectVerbInfo[2].Split('[')[0].Replace(";",string.Empty).Trim();
            if (headerGlobalName != defineGlobalName)
            {
                defineGlobalName = _objectVerbInfo[1].Split('[')[0].Trim();
                if (headerGlobalName == defineGlobalName)
                {
                    _res.Add(string.Format("Global name match veriable name in header, but missing static field, pos: {0}", globalPos));
                }
                else
                {
                    _res.Add(string.Format("Global name ({0}) don`t match veriable name ({1}) in header", defineGlobalName, headerGlobalName));
                }
            }
        }
        #endregion

        #endregion

        #region Common Checker utils

        private void GetLineIdx(string[] _Stream, string firsHeaderLineKey, string lastHeaderLineKey, LLRC_Common.SPECIAL_END_FINDS cSpEnd)
        {
            int idx = 0;

            for (idx = 0; idx < _stream.Length; idx++)
            {
                if (_stream[idx].Contains(firsHeaderLineKey ))
                {
                    _startHeaderIdx.Add(idx);

                    for (; idx < Math.Min(idx + 20, _Stream.Length); idx++)
                    {
                        if(cSpEnd == LLRC_Common.SPECIAL_END_FINDS.END_OF_FUNCTION)
                        {
                            MatchCollection matches = Regex.Matches(_stream[idx], "(\\s+)");
                            int[] spaceArray = matches.OfType<Match>().Select(m => m.Length).ToArray();

                            if ((true == _stream[idx].Contains(lastHeaderLineKey)) && (spaceArray.Length == 0))
                            {
                                _endHeaderIdx.Add(idx);
                                break;
                            }
                        }
                        else if (_stream[idx].Contains(lastHeaderLineKey))
                        {
                            _endHeaderIdx.Add(idx);
                            break;
                        }
                    }
                }
            }
        }


        private void CheckHeaderFields(string[] _stream, string moduleType, List<string> listOfKeys, int startPos, int stopPos)
        {
            bool findkey = false;
            _headerReadFields.Clear();

            foreach (var key in listOfKeys)
            {
                findkey = false;
                for (int idx = startPos; idx < stopPos; idx++)
                {
                    if (_stream[idx].Contains(key))
                    {
                        findkey = true;
                        if(_stream[idx] != string.Empty)
                        {
                            _headerReadFields.Add(_stream[idx]);
                        }
                        break;
                    }
                }
                if (false == findkey)
                {
                    _headerReadFields.Add(string.Empty);
                    _res.Add(string.Format("{0} header field: ({1}) don`t found. line pos: {2}", moduleType, key, startPos + 1));
                }
            }
        }

        private void GetFunctionNamesAndPos(string[] stream, List<int> _startHeaderIdx)
        {
            string tmpFuncName = string.Empty;
            string[] linesPart;
            foreach (var item in _startHeaderIdx)
            {
                if(item == 759)
                {
                    int a = 0x7;
                }

                tmpFuncName = stream[item - 1];
                if (true == tmpFuncName.Contains("typedef"))
                {
                    continue;
                }

                linesPart = tmpFuncName.Split();

                if (linesPart.Length < 2)
                {
                    continue;
                }
                // Find function name
                if (linesPart[0] == "static")
                {
                    tmpFuncName = linesPart[2].Split('(')[0];
                }
                else
                {

                    tmpFuncName = linesPart[1].Split('(')[0];
                }
                _funcNames.Add(string.Format("pos: {0}, name: {1}", item, tmpFuncName));
            }
        }

        private string GetModuleName(string _filePath)
        {
            return Path.GetFileName(_filePath).Split('_')[0].Split('.')[0];
        }
        #endregion

        #region Read file

        internal List<string> ViewFile(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);

            for (int idx = 0; idx < _stream.Length; idx++)
            {
                _res.Add(string.Format("{0}:{1}", idx + 1, _stream[idx]));
            }
            return (_res);
        }
        #endregion
    }
}
