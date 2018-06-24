using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LLLRC
{
    public partial class MainForm : Form
    {
        #region Class c`tor and verbs

        // File descriptor
        string _filePath = string.Empty;

        // Check list class
        LLRC_CheckList _checkList;

        // File type
        LLRC_Common.FILE_TYPE cFileType = LLRC_Common.FILE_TYPE.UNDEFINED;

        // Fix items type
        LLRC_FixItemsAndUtils _fixItems;
        LLRC_Common.ALLOWED_FIX_ITEMS cFixItem = LLRC_Common.ALLOWED_FIX_ITEMS.NOT_ALLOWED; 
        public MainForm()
        {
            InitializeComponent();
            _checkList = new LLRC_CheckList();
            _fixItems = new LLRC_FixItemsAndUtils();
        }
        #endregion

        #region Open file

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (ofdOpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _filePath = ofdOpenFile.FileName;
                if(_filePath.Contains(".c"))
                {
                    cFileType = LLRC_Common.FILE_TYPE.SOURCE;
                }
                else if(_filePath.Contains(".h"))
                {
                    cFileType = LLRC_Common.FILE_TYPE.HEADER;
                }
                else
                {
                    cFileType = LLRC_Common.FILE_TYPE.UNDEFINED;
                }
                ShowFileFields();
            }
        }

        private void ShowFileFields()
        {
            lblFilePath.Text = Path.GetFileName(_filePath);
            lblFileDate.Text = File.GetLastAccessTime(_filePath).ToString();
            InfraMessageOk(LLRC_Common.MSG_APP_OK_FILE_LOAD);
        }

        #endregion

        #region Application infra

        private void InfraMessageOk(string val)
        {
            lblAppStatus.ForeColor = Color.Black;
            lblAppStatus.Text = val;
        }

        private void InfraMessageFail(string val)
        {
            lblAppStatus.ForeColor = Color.Red;
            lblAppStatus.Text = val;
        }

        private void btnRtbResClear_Click(object sender, EventArgs e)
        {
            rtbResDisplay.Clear();
        }

        private void btnFixObject_Click(object sender, EventArgs e)
        {
            if (cFixItem == LLRC_Common.ALLOWED_FIX_ITEMS.NOT_ALLOWED)
            {
                InfraMessageFail(LLRC_Common.MSG_APP_FAIL_NOT_SELECT_VALID_SUBJECT);
            }
            else
            {
                switch (cFixItem)
                {
                    case LLRC_Common.ALLOWED_FIX_ITEMS.TABS:
                        _fixItems.FixTabs(rtbResDisplay.Lines, _filePath);
                        break;

                    case LLRC_Common.ALLOWED_FIX_ITEMS.SPACES:
                        _fixItems.FixSpaces(rtbResDisplay.Lines, _filePath);
                        break;
                }
            }
        }
        #endregion

        #region Check subjects

        private void btnCheckSubject_Click(object sender, EventArgs e)
        {
            if(cFileType == LLRC_Common.FILE_TYPE.UNDEFINED)
            {
                InfraMessageFail(LLRC_Common.MSG_APP_FAIL_NOT_VALID_FILE_FORMAT);
                return;
            }

            rtbResDisplay.Clear();
            InfraMessageOk(LLRC_Common.MSG_APP_OK_CHECKING_ITEM);
            lblNumRes.Text = string.Empty;
            cFixItem = LLRC_Common.ALLOWED_FIX_ITEMS.NOT_ALLOWED;

            #region Check general and common

            if (true == rdbCheckSpaces.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckSpaces(_filePath).ToArray();
                cFixItem = LLRC_Common.ALLOWED_FIX_ITEMS.SPACES;
            }
            else if (true == rdbCheckGrammerLines.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckGrammer(_filePath).ToArray();
            }
            else if (true == rdbCheckGrammerDifrenences.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckGrammer(_filePath).ToArray();
                rtbResDisplay.Lines = _checkList.GrammerRemoveDifrances(rtbResDisplay.Lines).ToArray();

            }
            else if(true == rdbCheckElbitTypes.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckTypes(_filePath).ToArray();
            }
            else if (true == rdbCheckStructHeader.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckStructHeader(_filePath).ToArray();
                cFixItem = LLRC_Common.ALLOWED_FIX_ITEMS.HEADER_STRUCTURE;
            }
            else if (true == rdbCheckDefineHeader.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckDefineHeader(_filePath).ToArray();
            }
            else if(true == rdbCheckTabs.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckTabs(_filePath).ToArray();
                cFixItem = LLRC_Common.ALLOWED_FIX_ITEMS.TABS;
            }
            #endregion

            #region Check source file

            else if (true == rdbCheckFunctionHeader.Checked)
            {
                if(cFileType == LLRC_Common.FILE_TYPE.SOURCE)
                {
                    rtbResDisplay.Lines = _checkList.CheckFunctionHeader(_filePath).ToArray();
                }
                else
                {
                    InfraMessageFail(LLRC_Common.MSG_APP_FAIL_WRONG_SUBJECT_FOR_FILE_FORMAT);
                }   
            }
            else if(true == rdbCheckSourceStructure.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckSourceStructure(_filePath).ToArray();
            }
            else if (true == rdbCheckGlobal.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckGlobalHeader(_filePath).ToArray();
            }
            else if(true == rdbCheckFunctionNames.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckSourceFunctionNames(_filePath).ToArray();
            }
            #endregion

            #region Check header file

            else if (true == rdbCheckHeaderStructure.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckHeaderStructure(_filePath).ToArray();
            }
            #endregion

            lblNumRes.Text = rtbResDisplay.Lines.Count().ToString();
            InfraMessageOk(LLRC_Common.MSG_APP_OK_FINISH_ANALYZE_SUBJECT);
        }
        #endregion

        private void btnViewFile_Click(object sender, EventArgs e)
        {
            rtbViewFile.Lines = _checkList.ViewFile(_filePath).ToArray();
        }

        private void btnViewFileClear_Click(object sender, EventArgs e)
        {
            rtbViewFile.Clear();
        }

        private void btnFileViewFind_Click(object sender, EventArgs e)
        {
            int len = rtbViewFile.TextLength;
            int index = 0;
            int lastIndex = rtbViewFile.Text.LastIndexOf(tbxFindKey.Text);

            while (index < lastIndex)
            {
                rtbViewFile.Find(tbxFindKey.Text, index, len, RichTextBoxFinds.None);
                rtbViewFile.SelectionBackColor = Color.Yellow;
                index = rtbViewFile.Text.IndexOf(this.tbxFindKey.Text, index) + 1;
            }
        }

        private void btnFindKeyClear_Click(object sender, EventArgs e)
        {
            int len = rtbViewFile.TextLength;
            int index = 0;
            int lastIndex = rtbViewFile.Text.LastIndexOf(tbxFindKey.Text);

            while (index < lastIndex)
            {
                rtbViewFile.Find(tbxFindKey.Text, index, len, RichTextBoxFinds.None);
                rtbViewFile.SelectionBackColor = Color.Gainsboro;
                index = rtbViewFile.Text.IndexOf(this.tbxFindKey.Text, index) + 1;
            }
        }
    }
 }

