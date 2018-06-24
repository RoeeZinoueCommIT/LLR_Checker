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
        List<string> _globalVerbInfo;

        // Struct verbs list
        List<string> _structVerbInfo;

        // FunctionsNamesList
        List<string> _funcNames;

        public LLRC_CheckList()
        {
            appWord = new Microsoft.Office.Interop.Word.Application();
            _res = new List<string>();

            _startHeaderIdx = new List<int>();
            _endHeaderIdx = new List<int>();

            _tempList = new List<string>();
            _globalVerbInfo = new List<string>();
            _structVerbInfo = new List<string>();
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
                            if (true == CheckWordForGrammer(word))
                            {

                            }
                            else
                            {
                                _res.Add("Line: " + idx + " Word: " + word);
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
            catch (Exception)
            {
                Console.WriteLine("Something wrong with 1 or more of function header");
            }

            return (_res);
        }

        private void CheckTabsSpace(string[] _stream, int startPos, int stopPos, int tabIndex)
        {
            int tabPosIndx = 1;
            bool rowSignFlagPos = false, rowSignFlagNeg = false;
            string lastLine = string.Empty;
            for (int rowIdx = startPos + 1; rowIdx < stopPos; rowIdx++)
            {
                rowSignFlagPos = false;
                rowSignFlagNeg = false;
                MatchCollection matches = Regex.Matches(_stream[rowIdx], "(\\s+)");
                int[] spaceArray = matches.OfType<Match>().Select(m => m.Length).ToArray();

                if (spaceArray.Length == 0) // If there is nothing writing in row skip row.
                {
                    continue;
                }
                else
                {
                    
                    if (rowIdx == 774)
                    {
                        int a = 0x7;
                    }
                    if (true == _stream[rowIdx].Contains("{"))
                    {
                        tabPosIndx++;
                        rowSignFlagPos = true;
                    }
                    if ((true == _stream[rowIdx].Contains("case")))
                    {
                        tabPosIndx++;
                        rowSignFlagPos = true;
                    }
                    if (true == _stream[rowIdx].Contains("default"))
                    {
                        if((false == lastLine.Contains("break;")))
                        {
                            //tabPosIndx--;
                            rowSignFlagPos = true;
                        }
                        else
                        {
                            tabPosIndx++;
                        }
                        
                    }
                    if ((true == _stream[rowIdx].Contains("}")))
                    {
                        if((false == lastLine.Contains("break;")))
                        {
                            tabPosIndx--;
                        }
                        
                    }
                    if ((true == _stream[rowIdx].Contains("break;")))
                    {
                        tabPosIndx--;
                        rowSignFlagNeg = true;
                    }
                    if (true == _stream[rowIdx].Contains("#"))
                    {
                        continue;
                    }

                    // Check tabs positions
                    if ((false == rowSignFlagPos) && (false == rowSignFlagNeg))
                    {
                        if (spaceArray[0] != (4 * tabPosIndx))
                        {
                            _res.Add(string.Format("Tab error. Pos: {0}, Tab: {1}, Row: {2}" + Environment.NewLine, rowIdx + 1, 4 * tabPosIndx, _stream[rowIdx].Trim()));
                        }
                    }
                    else
                    {
                        if (rowSignFlagNeg == true)
                        {
                            if (spaceArray[0] != (4 * (tabPosIndx + 1)))
                            {
                                _res.Add(string.Format("Tab error. Pos: {0}, Tab: {1}, Row: {2}" + Environment.NewLine, rowIdx + 1, 4 * (tabPosIndx + 1), _stream[rowIdx].Trim()));
                            }
                        }
                        else
                        {
                            if (spaceArray[0] != (4 * (tabPosIndx - 1)))
                            {
                                _res.Add(string.Format("Tab error. Pos: {0}, Tab: {1}, Row: {2}" + Environment.NewLine, rowIdx + 1, 4 * (tabPosIndx - 1), _stream[rowIdx]));
                            }
                        }
                    }
                    lastLine = _stream[rowIdx];


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
            _structVerbInfo.Clear();
            _stream = File.ReadAllLines(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();

            GetLineIdx(_stream, "$STRUCTURES$", "/*--", LLRC_Common.SPECIAL_END_FINDS.NONE);

            string moduleName = Path.GetFileName(_filePath).Split('_')[0];

            try
            {
                for (int idx = 0; idx < Math.Min(_startHeaderIdx.Count, _endHeaderIdx.Count); idx++)
                {
                    CheckInGroupKey(_stream, _startHeaderIdx[idx], _endHeaderIdx[idx], moduleName);

                    _globalVerbInfo.Clear();
                    CheckHeaderFields(_stream, "Structure", LLRC_Common.STRUCTURE_FIELDS, _startHeaderIdx[idx], _endHeaderIdx[idx]);

                    GetStrcutVerbsInfo(_stream, _endHeaderIdx[idx] + 1);
                    CheckStructMatch(_stream, _startHeaderIdx[idx], _endHeaderIdx[idx]);


                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something wrong with 1 or more of function header");
            }
            return (_res);
        }

        private void CheckStructMatch(string[] _stream, int startPos, int stopPos)
        {
            string tempStr = string.Empty;
            int linePos = startPos;

            for (int idx = startPos; idx < stopPos; idx++)
            {
                if (true == _stream[idx].Contains("Members Types:"))
                {
                    while (false == _stream[idx].Contains("Members Names:"))
                    {
                        tempStr += _stream[idx];
                        idx++;
                    }
                    break;
                }
            }

            var headerContent = tempStr.Replace(" ", string.Empty).Split(':')[1].Split('|');
            var verbsContent = _structVerbInfo[1].Replace(" ", string.Empty).Split(',');

            if(headerContent.Length != verbsContent.Length)
            {
                _res.Add(string.Format("Strcut. Number of types in header and veriable don`t match. Type: "));
                return;
            }

            for (int idx = 0; idx < verbsContent.Length; idx++)
            {
                if(headerContent[idx] != verbsContent[idx])
                {
                    _res.Add(string.Format("Strcut. Type dont match. Type: {0}", headerContent[idx]));
                }
            }
        }

        private void GetStrcutVerbsInfo(string[] _stream, int startPosLines)
        {
            int linePos = startPosLines;

            string tmpStr = string.Empty;
            string veriableTypes = string.Empty;
            string veriableNames = string.Empty;

            _structVerbInfo.Add(_stream[linePos++]);

            while(false == _stream[linePos].Contains("}"))
            {
                if (false == _stream[linePos].Contains("{"))
                {
                    veriableTypes += _stream[linePos].Split()[2] + ", ";
                    veriableNames += _stream[linePos].Split()[3].Split(';')[0] + ", ";
                }

                linePos++;
            }
            _structVerbInfo.Add(veriableTypes);
            _structVerbInfo.Add(veriableNames);
            _structVerbInfo.Add(_stream[linePos].Split('}')[1].Split(';')[0]);
        }
        #endregion

        internal List<string> CheckDefineHeader(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();

            GetLineIdx(_stream, "$DEFINES$", "/*--", LLRC_Common.SPECIAL_END_FINDS.NONE);

            string moduleName = Path.GetFileName(_filePath).Split('_')[0];

            try
            {
                for (int idx = 0; idx < Math.Min(_startHeaderIdx.Count, _endHeaderIdx.Count); idx++)
                {
                    CheckHeaderFields(_stream, "Define", LLRC_Common.DEFINE_FIELDS, _startHeaderIdx[idx], _endHeaderIdx[idx]);

                    //CheckInGroupKey(_stream, _startHeaderIdx[idx], _endHeaderIdx[idx], moduleName);
                    _globalVerbInfo.Clear();
                    GetGlobalVerbsInfo(_stream, _endHeaderIdx[idx] + 1);
                    
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something wrong with 1 or more of function header");
            }
            return (_res);
        }
        #endregion

        #region Check source file

        #region Check Function header

        internal List<string> CheckFunctionHeader(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();


            string moduleName = Path.GetFileName(_filePath).Split('_')[0];
            GetLineIdx(_stream, "$PROCEDURE$", "/*--", LLRC_Common.SPECIAL_END_FINDS.NONE);

            try
            {
                for (int idx = 0; idx < Math.Min(_startHeaderIdx.Count, _endHeaderIdx.Count); idx++)
                {
                    CheckHeaderFields(_stream, "Function", LLRC_Common.FUNCTION_FIELDS, _startHeaderIdx[idx], _endHeaderIdx[idx]);
                    CheckInGroupKey(_stream, _startHeaderIdx[idx], _endHeaderIdx[idx], moduleName);
                    CheckBriefKey(_stream, _startHeaderIdx[idx], _endHeaderIdx[idx]);
                    CheckParamKey(_stream, _startHeaderIdx[idx], _endHeaderIdx[idx]);
                    CheckDeriveDesc(_stream, _startHeaderIdx[idx], _endHeaderIdx[idx]);

                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something wrong with 1 or more of function header");
            }

            return (_res);
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
            for (int idx = startLine; idx < stopLine; idx++)
            {
                if (_stream[idx].Contains("ingroup"))
                {
                    if (false == _stream[idx].Contains(moduleName))
                    {
                        _res.Add(string.Format("ingroup issue, Line pos: {0}", idx + 1));
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
                        }
                        idx++;
                    }
                }
            }
        }
        #endregion

        #region Check functions names

        internal List<string> CheckSourceFunctionNames(string _filePath)
        {
            _res.Clear();
            _funcNames.Clear();
            _stream = File.ReadAllLines(_filePath);
            string moduleName = Path.GetFileName(_filePath).Split('_')[0];
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();
            GetLineIdx(_stream, "{", "}", LLRC_Common.SPECIAL_END_FINDS.END_OF_FUNCTION);

            // Remove duplicate from list
            _startHeaderIdx = _startHeaderIdx.Distinct().ToList();

            GetFunctionNamesAndPos(_stream, _startHeaderIdx);
            checkFunctionsName(moduleName);
            return (_res);
        }


        private void checkFunctionsName(string moduleName)
        {
            string funcName = string.Empty, linePos = string.Empty;
            foreach (var line in _funcNames)
            {
                linePos = line.Split(',')[0].Split(':')[1];
                funcName = line.Split(',')[1].Split(':')[1];
                

                // Check function name - Contain module name
                if (false == funcName.Contains(moduleName))
                {
                    _res.Add(string.Format("Line pos = {0} \nFunction name: {1} \nIssue: Don`t find module name {2} \n", linePos + 1, funcName, moduleName));
                }

                // Check function name - Contain "p_"
                if (false == funcName.Contains("p_"))
                {
                    _res.Add(string.Format("Line pos = {0} \nFunction name: {1} \nIssue: Don`t find p_ prefix \n", linePos + 1, funcName));
                }
            }

            //linesPart = readLine.Split();

            //if (linesPart.Length < 3)
            //{
            //    return;
            //}
            //// Find function name
            //if (linesPart[0] == "static")
            //{
            //    functionNames = linesPart[2].Split('(')[0];
            //}
            //else
            //{

            //    functionNames = linesPart[1].Split('(')[0];
            //}


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

            string moduleName = Path.GetFileName(_filePath).Split('_')[0];

            try
            {
                for (int idx = 0; idx < Math.Min(_tempStartHeaderIdx.Count, _tempEndHeaderIdx.Count); idx++)
                {
                    CheckInGroupKey(_stream, _tempStartHeaderIdx[idx], _tempEndHeaderIdx[idx], moduleName);

                    _globalVerbInfo.Clear();
                    GetGlobalVerbsInfo(_stream, _tempEndHeaderIdx[idx] + 1);

                    _res.Add(string.Format("Global name: {0}", _globalVerbInfo[2].Split('[')[0].Replace(';', ' ').Trim()));

                    _res.Add(string.Format("Header issues:"));
                    CheckHeaderFields(_stream, "Global", LLRC_Common.GLOBAL_FIELDS, _tempStartHeaderIdx[idx], _tempEndHeaderIdx[idx]);
                    CheckStaticGlobalVerbs(_tempEndHeaderIdx[idx] + 1);
                    CheckVeriableNameLine(_stream, moduleName, _tempStartHeaderIdx[idx], _tempEndHeaderIdx[idx]);

                    CheckGlobalUsedByRow(_stream, _globalVerbInfo[2]);

                    _res.Add(string.Format(Environment.NewLine + "==========================================================" + Environment.NewLine));
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something wrong with 1 or more of function header");
            }
            return _res;
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

        private bool GetGlobalVerbsInfo(string[] _stream, int linePost)
        {
            var collection = _stream[linePost].Split();
            foreach (var item in collection)
            {
                _globalVerbInfo.Add(item);
            }
            return true;
        }

        private void CheckStaticGlobalVerbs(int linePos)
        {
            if(_globalVerbInfo[0] != "static")
            {
                _res.Add(string.Format("Global. static not fould. line pos: {0}", linePos));
            }
        }
       
        private void CheckVeriableNameLine(string[] _stream, string moduleName, int startLine, int stopLine)
        {
            string foundVariableName = string.Empty;
            int idx = startLine;

            // Check global veriable name math rules:
            string nameShape = string.Format("g_{0}", moduleName);
            var nameArray = _globalVerbInfo[2].Split('[')[0].Split(';')[0].Split('_');

            if ((false == _globalVerbInfo[2].Contains(nameShape)) && (false == _globalVerbInfo[3].Contains(nameShape)))
            {
                _res.Add(string.Format("Global. veriable name dont match module name. pos: {0}", stopLine + 1));
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
                    _res.Add(string.Format("Global. veriable name dont contain only lower latter. pos: {0}", stopLine + 1));
                }
            }

            // Check if global name in header match global name veriable: 
            for (; idx < stopLine; idx++)
            {
                if (_stream[idx].Contains("Variable Name:"))
                {
                    foundVariableName = _stream[idx].Split()[4];
                    break;
                }
            }
            if(foundVariableName == string.Empty)
            {
                _res.Add(string.Format("Variable name don`t match veriable name in header, pos: {0}", idx - 1));
                return;
            }

            if(false == _globalVerbInfo[2].Contains(foundVariableName))
            {
                _res.Add(string.Format("Global name issue, pos {0}", idx + 2));
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

            foreach (var key in listOfKeys)
            {
                findkey = false;
                for (int idx = startPos; idx < stopPos; idx++)
                {
                    if (_stream[idx].Contains(key))
                    {
                        findkey = true;
                        break;
                    }
                }
                if (false == findkey)
                {
                    _res.Add(string.Format("{0} header field: {1} don`t found. line pos: {2}", moduleType, key, startPos));
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
        #endregion
    }
}
