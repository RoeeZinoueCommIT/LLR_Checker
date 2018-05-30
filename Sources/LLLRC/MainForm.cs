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
        public MainForm()
        {
            InitializeComponent();
            _checkList = new LLRC_CheckList();
        }
        #endregion

        #region Open file

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (ofdOpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _filePath = ofdOpenFile.FileName;
                ShowFileFields();
            }
        }

        private void ShowFileFields()
        {
            lblFilePath.Text = Path.GetFileName(_filePath);
            lblFileDate.Text = File.GetLastAccessTime(_filePath).ToString();
            InfraMessageOk("File Loaded OK");
        }

        #endregion

        #region Application infra

        private void InfraMessageOk(string val)
        {
            lblAppStatus.ForeColor = Color.Black;
            lblAppStatus.Text = val;
        }
        #endregion

        #region Check subjects

        private void btnCheckSubject_Click(object sender, EventArgs e)
        {
            rtbResDisplay.Clear();
            lblNumRes.Text = string.Empty;

            #region Check general and common

            if (true == rdbCheckSpaces.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckSpaces(_filePath).ToArray();
            }
            else if (true == rdbCheckGrammer.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckGrammer(_filePath).ToArray();
            }
            else if(true == rdbCheckElbitTypes.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckTypes(_filePath).ToArray();
            }
            else if (true == rdbCheckStructHeader.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckStructHeader(_filePath).ToArray();
            }
            else if (true == rdbCheckDefineHeader.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckDefineHeader(_filePath).ToArray();
            }
            #endregion

            #region Check source file

            else if (true == rdbCheckFunctionHeader.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckFunctionHeader(_filePath).ToArray();
            }
            else if(true == rdbCheckSourceStructure.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckSourceStructure(_filePath).ToArray();
            }
            else if (true == rdbCheckGlobal.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckGlobalHeader(_filePath).ToArray();
            }
            #endregion

            #region Check header file

            else if (true == rdbCheckHeaderStructure.Checked)
            {
                rtbResDisplay.Lines = _checkList.CheckHeaderStructure(_filePath).ToArray();
            }
            #endregion

            lblNumRes.Text = rtbResDisplay.Lines.Count().ToString();
        }
        #endregion

        private void btnRtbResClear_Click(object sender, EventArgs e)
        {
            rtbResDisplay.Clear();
        }
    }
}
