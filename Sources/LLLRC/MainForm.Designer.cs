namespace LLLRC
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnRtbResClear = new System.Windows.Forms.Button();
            this.lblNumRes = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rtbResDisplay = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rdbCheckHeaderStructure = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.rdbCheckGlobal = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.rdbCheckSpaces = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.rdbCheckSourceStructure = new System.Windows.Forms.RadioButton();
            this.rdbCheckGrammer = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.rdbCheckFunctionHeader = new System.Windows.Forms.RadioButton();
            this.rdbCheckTabs = new System.Windows.Forms.RadioButton();
            this.rdbCheckElbitTypes = new System.Windows.Forms.RadioButton();
            this.rdbCheckStructHeader = new System.Windows.Forms.RadioButton();
            this.rdbCheckFunctionNames = new System.Windows.Forms.RadioButton();
            this.btnCheckSubject = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblFileDate = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ofdOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.lblAppStatus = new System.Windows.Forms.Label();
            this.rdbCheckDefineHeader = new System.Windows.Forms.RadioButton();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(696, 556);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(688, 530);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Check File";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(3, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(679, 430);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Check subjects";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnRtbResClear);
            this.groupBox4.Controls.Add(this.lblNumRes);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.rtbResDisplay);
            this.groupBox4.Location = new System.Drawing.Point(136, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(537, 405);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Results";
            // 
            // btnRtbResClear
            // 
            this.btnRtbResClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRtbResClear.Location = new System.Drawing.Point(456, 376);
            this.btnRtbResClear.Name = "btnRtbResClear";
            this.btnRtbResClear.Size = new System.Drawing.Size(75, 23);
            this.btnRtbResClear.TabIndex = 3;
            this.btnRtbResClear.Text = "Clear window";
            this.btnRtbResClear.UseVisualStyleBackColor = true;
            this.btnRtbResClear.Click += new System.EventHandler(this.btnRtbResClear_Click);
            // 
            // lblNumRes
            // 
            this.lblNumRes.AutoSize = true;
            this.lblNumRes.Location = new System.Drawing.Point(128, 24);
            this.lblNumRes.Name = "lblNumRes";
            this.lblNumRes.Size = new System.Drawing.Size(10, 13);
            this.lblNumRes.TabIndex = 2;
            this.lblNumRes.Text = ".";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Number of occurrence:";
            // 
            // rtbResDisplay
            // 
            this.rtbResDisplay.BackColor = System.Drawing.SystemColors.ControlLight;
            this.rtbResDisplay.Location = new System.Drawing.Point(6, 66);
            this.rtbResDisplay.Name = "rtbResDisplay";
            this.rtbResDisplay.Size = new System.Drawing.Size(525, 304);
            this.rtbResDisplay.TabIndex = 0;
            this.rtbResDisplay.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.btnCheckSubject);
            this.groupBox3.Location = new System.Drawing.Point(0, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(130, 405);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Subjects";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rdbCheckDefineHeader);
            this.groupBox5.Controls.Add(this.rdbCheckHeaderStructure);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.rdbCheckGlobal);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.rdbCheckSpaces);
            this.groupBox5.Controls.Add(this.radioButton3);
            this.groupBox5.Controls.Add(this.rdbCheckSourceStructure);
            this.groupBox5.Controls.Add(this.rdbCheckGrammer);
            this.groupBox5.Controls.Add(this.radioButton8);
            this.groupBox5.Controls.Add(this.rdbCheckFunctionHeader);
            this.groupBox5.Controls.Add(this.rdbCheckTabs);
            this.groupBox5.Controls.Add(this.rdbCheckElbitTypes);
            this.groupBox5.Controls.Add(this.rdbCheckStructHeader);
            this.groupBox5.Controls.Add(this.rdbCheckFunctionNames);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(3, 19);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(121, 342);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "General and common";
            // 
            // rdbCheckHeaderStructure
            // 
            this.rdbCheckHeaderStructure.AutoSize = true;
            this.rdbCheckHeaderStructure.Location = new System.Drawing.Point(7, 320);
            this.rdbCheckHeaderStructure.Name = "rdbCheckHeaderStructure";
            this.rdbCheckHeaderStructure.Size = new System.Drawing.Size(92, 16);
            this.rdbCheckHeaderStructure.TabIndex = 10;
            this.rdbCheckHeaderStructure.TabStop = true;
            this.rdbCheckHeaderStructure.Text = "Header structure";
            this.rdbCheckHeaderStructure.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label7.Location = new System.Drawing.Point(32, 305);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "Header file";
            // 
            // rdbCheckGlobal
            // 
            this.rdbCheckGlobal.AutoSize = true;
            this.rdbCheckGlobal.Location = new System.Drawing.Point(6, 276);
            this.rdbCheckGlobal.Name = "rdbCheckGlobal";
            this.rdbCheckGlobal.Size = new System.Drawing.Size(79, 16);
            this.rdbCheckGlobal.TabIndex = 14;
            this.rdbCheckGlobal.TabStop = true;
            this.rdbCheckGlobal.Text = "Global header";
            this.rdbCheckGlobal.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label6.Location = new System.Drawing.Point(31, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "Source file";
            // 
            // rdbCheckSpaces
            // 
            this.rdbCheckSpaces.AutoSize = true;
            this.rdbCheckSpaces.Location = new System.Drawing.Point(6, 17);
            this.rdbCheckSpaces.Name = "rdbCheckSpaces";
            this.rdbCheckSpaces.Size = new System.Drawing.Size(54, 16);
            this.rdbCheckSpaces.TabIndex = 0;
            this.rdbCheckSpaces.TabStop = true;
            this.rdbCheckSpaces.Text = "Spaces";
            this.rdbCheckSpaces.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 210);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(82, 16);
            this.radioButton3.TabIndex = 8;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Source header";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // rdbCheckSourceStructure
            // 
            this.rdbCheckSourceStructure.AutoSize = true;
            this.rdbCheckSourceStructure.Location = new System.Drawing.Point(6, 232);
            this.rdbCheckSourceStructure.Name = "rdbCheckSourceStructure";
            this.rdbCheckSourceStructure.Size = new System.Drawing.Size(91, 16);
            this.rdbCheckSourceStructure.TabIndex = 9;
            this.rdbCheckSourceStructure.TabStop = true;
            this.rdbCheckSourceStructure.Text = "Source structure";
            this.rdbCheckSourceStructure.UseVisualStyleBackColor = true;
            // 
            // rdbCheckGrammer
            // 
            this.rdbCheckGrammer.AutoSize = true;
            this.rdbCheckGrammer.Location = new System.Drawing.Point(6, 39);
            this.rdbCheckGrammer.Name = "rdbCheckGrammer";
            this.rdbCheckGrammer.Size = new System.Drawing.Size(92, 16);
            this.rdbCheckGrammer.TabIndex = 1;
            this.rdbCheckGrammer.TabStop = true;
            this.rdbCheckGrammer.Text = "English grammer";
            this.rdbCheckGrammer.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(7, 150);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(77, 16);
            this.radioButton8.TabIndex = 13;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "Enum header";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // rdbCheckFunctionHeader
            // 
            this.rdbCheckFunctionHeader.AutoSize = true;
            this.rdbCheckFunctionHeader.Location = new System.Drawing.Point(6, 254);
            this.rdbCheckFunctionHeader.Name = "rdbCheckFunctionHeader";
            this.rdbCheckFunctionHeader.Size = new System.Drawing.Size(89, 16);
            this.rdbCheckFunctionHeader.TabIndex = 11;
            this.rdbCheckFunctionHeader.TabStop = true;
            this.rdbCheckFunctionHeader.Text = "Function header";
            this.rdbCheckFunctionHeader.UseVisualStyleBackColor = true;
            // 
            // rdbCheckTabs
            // 
            this.rdbCheckTabs.AutoSize = true;
            this.rdbCheckTabs.Location = new System.Drawing.Point(6, 61);
            this.rdbCheckTabs.Name = "rdbCheckTabs";
            this.rdbCheckTabs.Size = new System.Drawing.Size(43, 16);
            this.rdbCheckTabs.TabIndex = 2;
            this.rdbCheckTabs.TabStop = true;
            this.rdbCheckTabs.Text = "Tabs";
            this.rdbCheckTabs.UseVisualStyleBackColor = true;
            // 
            // rdbCheckElbitTypes
            // 
            this.rdbCheckElbitTypes.AutoSize = true;
            this.rdbCheckElbitTypes.Location = new System.Drawing.Point(6, 83);
            this.rdbCheckElbitTypes.Name = "rdbCheckElbitTypes";
            this.rdbCheckElbitTypes.Size = new System.Drawing.Size(66, 16);
            this.rdbCheckElbitTypes.TabIndex = 3;
            this.rdbCheckElbitTypes.TabStop = true;
            this.rdbCheckElbitTypes.Text = "Elbit types";
            this.rdbCheckElbitTypes.UseVisualStyleBackColor = true;
            // 
            // rdbCheckStructHeader
            // 
            this.rdbCheckStructHeader.AutoSize = true;
            this.rdbCheckStructHeader.Location = new System.Drawing.Point(6, 127);
            this.rdbCheckStructHeader.Name = "rdbCheckStructHeader";
            this.rdbCheckStructHeader.Size = new System.Drawing.Size(78, 16);
            this.rdbCheckStructHeader.TabIndex = 12;
            this.rdbCheckStructHeader.TabStop = true;
            this.rdbCheckStructHeader.Text = "Strcut header";
            this.rdbCheckStructHeader.UseVisualStyleBackColor = true;
            // 
            // rdbCheckFunctionNames
            // 
            this.rdbCheckFunctionNames.AutoSize = true;
            this.rdbCheckFunctionNames.Location = new System.Drawing.Point(6, 105);
            this.rdbCheckFunctionNames.Name = "rdbCheckFunctionNames";
            this.rdbCheckFunctionNames.Size = new System.Drawing.Size(89, 16);
            this.rdbCheckFunctionNames.TabIndex = 7;
            this.rdbCheckFunctionNames.TabStop = true;
            this.rdbCheckFunctionNames.Text = "Function names";
            this.rdbCheckFunctionNames.UseVisualStyleBackColor = true;
            // 
            // btnCheckSubject
            // 
            this.btnCheckSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckSubject.Location = new System.Drawing.Point(23, 376);
            this.btnCheckSubject.Name = "btnCheckSubject";
            this.btnCheckSubject.Size = new System.Drawing.Size(75, 23);
            this.btnCheckSubject.TabIndex = 6;
            this.btnCheckSubject.Text = "Check";
            this.btnCheckSubject.UseVisualStyleBackColor = true;
            this.btnCheckSubject.Click += new System.EventHandler(this.btnCheckSubject_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblFileDate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblFilePath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnOpenFile);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(679, 82);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Open file";
            // 
            // lblFileDate
            // 
            this.lblFileDate.AutoSize = true;
            this.lblFileDate.Location = new System.Drawing.Point(208, 58);
            this.lblFileDate.Name = "lblFileDate";
            this.lblFileDate.Size = new System.Drawing.Size(10, 13);
            this.lblFileDate.TabIndex = 7;
            this.lblFileDate.Text = ".";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "File date";
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(56, 58);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(10, 13);
            this.lblFilePath.TabIndex = 3;
            this.lblFilePath.Text = ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "File name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose file";
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFile.Location = new System.Drawing.Point(84, 24);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "Open";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(493, 507);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "File viewer";
            // 
            // ofdOpenFile
            // 
            this.ofdOpenFile.FileName = "ofdOpenFile";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label4.Location = new System.Drawing.Point(2, 561);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Application status |";
            // 
            // lblAppStatus
            // 
            this.lblAppStatus.AutoSize = true;
            this.lblAppStatus.Location = new System.Drawing.Point(104, 561);
            this.lblAppStatus.Name = "lblAppStatus";
            this.lblAppStatus.Size = new System.Drawing.Size(10, 13);
            this.lblAppStatus.TabIndex = 2;
            this.lblAppStatus.Text = ".";
            // 
            // rdbCheckDefineHeader
            // 
            this.rdbCheckDefineHeader.AutoSize = true;
            this.rdbCheckDefineHeader.Location = new System.Drawing.Point(7, 172);
            this.rdbCheckDefineHeader.Name = "rdbCheckDefineHeader";
            this.rdbCheckDefineHeader.Size = new System.Drawing.Size(80, 16);
            this.rdbCheckDefineHeader.TabIndex = 16;
            this.rdbCheckDefineHeader.TabStop = true;
            this.rdbCheckDefineHeader.Text = "Define header";
            this.rdbCheckDefineHeader.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 583);
            this.Controls.Add(this.lblAppStatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "LLR Checker";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnCheckSubject;
        private System.Windows.Forms.RadioButton rdbCheckElbitTypes;
        private System.Windows.Forms.RadioButton rdbCheckTabs;
        private System.Windows.Forms.RadioButton rdbCheckGrammer;
        private System.Windows.Forms.RadioButton rdbCheckSpaces;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox rtbResDisplay;
        private System.Windows.Forms.RadioButton rdbCheckHeaderStructure;
        private System.Windows.Forms.RadioButton rdbCheckSourceStructure;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton rdbCheckFunctionNames;
        private System.Windows.Forms.OpenFileDialog ofdOpenFile;
        private System.Windows.Forms.Label lblFileDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblAppStatus;
        private System.Windows.Forms.Label lblNumRes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRtbResClear;
        private System.Windows.Forms.RadioButton rdbCheckGlobal;
        private System.Windows.Forms.RadioButton rdbCheckFunctionHeader;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton rdbCheckStructHeader;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rdbCheckDefineHeader;
    }
}

