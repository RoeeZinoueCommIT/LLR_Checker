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
        List<Int32> _startHeaderIdx, _endHeaderIdx;

        // Temp list
        List<string> _tempList;

        // Global verbs list
        List<string> _globalVerbInfo;
        public LLRC_CheckList()
        {
            appWord = new Microsoft.Office.Interop.Word.Application();
            _res = new List<string>();

            _startHeaderIdx = new List<int>();
            _endHeaderIdx = new List<int>();

            _tempList = new List<string>();
            _globalVerbInfo = new List<string>();
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

            for (int idx = 0; idx < _stream.Length; idx++)
            {
                MatchCollection matches = Regex.Matches(_stream[idx], "(\\s+)");
                int[] spaceArray = matches.OfType<Match>().Select(m => m.Length).ToArray();

                if (false == CheckSpaces(spaceArray))
                {
                    _res.Add("Line: " + idx + Environment.NewLine + " string: " + _stream[idx]);
                }

            }
            return _res;
        }

        private bool CheckSpaces(int[] spaceArray)
        {
            for (int idx = 1; idx < spaceArray.Length; idx++)
            {
                if (spaceArray[idx] > 1 && spaceArray[idx] < LLRC_Common.ALLOWED_BIG_SPACE_NUM)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Check Tabs

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
                            _res.Add("Pos: " + idx + Environment.NewLine + " line: " + _stream[idx]);
                        }
                    }
                }
            }
            return _res;
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
            return _res;
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
            return _res;
        }

        #endregion

        #region Check struct header

        internal List<string> CheckStructHeader(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();

            HeaderGetIdx(_stream, "$STRUCTURES$", "/*--");

            string moduleName = Path.GetFileName(_filePath).Split('_')[0];

            try
            {
                for (int idx = 0; idx < Math.Min(_startHeaderIdx.Count, _endHeaderIdx.Count); idx++)
                {
                    CheckInGroupKey(_stream, _startHeaderIdx[idx], _endHeaderIdx[idx], moduleName);

                    _globalVerbInfo.Clear();
                    GetGlobalVerbsInfo(_stream, _endHeaderIdx[idx] + 1);
                    CheckHeaderFields(_stream, "Structure", LLRC_Common.STRUCTURE_FIELDS,_startHeaderIdx[idx], _endHeaderIdx[idx]);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something wrong with 1 or more of function header");
            }
            return _res;
        }
        #endregion

        internal List<string> CheckDefineHeader(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();

            HeaderGetIdx(_stream, "$DEFINES$", "/*--");

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
            return _res;
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
            HeaderGetIdx(_stream, "$PROCEDURE$", "/*--");

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

            return _res;
        }

        private int CheckParamKey(string[] _stream, int startLine, int stopLine)
        {
            int issuePos = 0;
            for (int idx = startLine; idx < stopLine; idx++)
            {
                if (_stream[idx].Contains("range:"))
                {
                    _res.Add(string.Format("range issue, Line pos: {0}", idx + 1));
                }
            }
            return issuePos;
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

        #region Check global header
        internal List<string> CheckGlobalHeader(string _filePath)
        {
            _res.Clear();
            _stream = File.ReadAllLines(_filePath);
            _startHeaderIdx.Clear(); _endHeaderIdx.Clear();

            HeaderGetIdx(_stream, "$GLOBAL VARIABLES$", "/*--");

            string moduleName = Path.GetFileName(_filePath).Split('_')[0];

            try
            {
                for (int idx = 0; idx < Math.Min(_startHeaderIdx.Count, _endHeaderIdx.Count); idx++)
                {
                    CheckInGroupKey(_stream, _startHeaderIdx[idx], _endHeaderIdx[idx], moduleName);

                    _globalVerbInfo.Clear();
                    GetGlobalVerbsInfo(_stream, _endHeaderIdx[idx] + 1);

                    CheckHeaderFields(_stream, "Global", LLRC_Common.GLOBAL_FIELDS, _startHeaderIdx[idx], _endHeaderIdx[idx]);
                    CheckStaticGlobalVerbs(_endHeaderIdx[idx] + 1);
                    CheckVeriableNameLine(_stream, moduleName, _startHeaderIdx[idx], _endHeaderIdx[idx]);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something wrong with 1 or more of function header");
            }
            return _res;
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

        private void HeaderGetIdx(string[] _Stream, string firsHeaderLineKey, string lastHeaderLineKey)
        {
            for (int idx = 0; idx < _stream.Length; idx++)
            {
                if (_stream[idx].Contains(firsHeaderLineKey ))
                {
                    _startHeaderIdx.Add(idx);

                    for (int idxLast = idx; idxLast < Math.Min(idx + 20, _Stream.Length); idxLast++)
                    {
                        if (_stream[idxLast].Contains(lastHeaderLineKey))
                        {
                            _endHeaderIdx.Add(idxLast);
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
        #endregion



    }
}
