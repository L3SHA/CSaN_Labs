namespace Client
{
    partial class FileAttachment
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
            this.tbFileID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddMsg = new System.Windows.Forms.Button();
            this.btnDeleteMsg = new System.Windows.Forms.Button();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.btnDeleteStorage = new System.Windows.Forms.Button();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGetInfo = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ofdOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // tbFileID
            // 
            this.tbFileID.Location = new System.Drawing.Point(350, 30);
            this.tbFileID.Name = "tbFileID";
            this.tbFileID.Size = new System.Drawing.Size(100, 20);
            this.tbFileID.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(279, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File ID";
            // 
            // btnAddMsg
            // 
            this.btnAddMsg.Location = new System.Drawing.Point(350, 90);
            this.btnAddMsg.Name = "btnAddMsg";
            this.btnAddMsg.Size = new System.Drawing.Size(100, 37);
            this.btnAddMsg.TabIndex = 2;
            this.btnAddMsg.Text = "Add to message";
            this.btnAddMsg.UseVisualStyleBackColor = true;
            this.btnAddMsg.Click += new System.EventHandler(this.btnAddMsg_Click);
            // 
            // btnDeleteMsg
            // 
            this.btnDeleteMsg.Location = new System.Drawing.Point(350, 159);
            this.btnDeleteMsg.Name = "btnDeleteMsg";
            this.btnDeleteMsg.Size = new System.Drawing.Size(100, 37);
            this.btnDeleteMsg.TabIndex = 3;
            this.btnDeleteMsg.Text = "Delete from message";
            this.btnDeleteMsg.UseVisualStyleBackColor = true;
            this.btnDeleteMsg.Click += new System.EventHandler(this.btnDeleteMsg_Click);
            // 
            // lbFiles
            // 
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.Location = new System.Drawing.Point(563, 77);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(131, 173);
            this.lbFiles.TabIndex = 4;
            // 
            // btnDeleteStorage
            // 
            this.btnDeleteStorage.Location = new System.Drawing.Point(350, 231);
            this.btnDeleteStorage.Name = "btnDeleteStorage";
            this.btnDeleteStorage.Size = new System.Drawing.Size(100, 37);
            this.btnDeleteStorage.TabIndex = 5;
            this.btnDeleteStorage.Text = "Delete from storage";
            this.btnDeleteStorage.UseVisualStyleBackColor = true;
            this.btnDeleteStorage.Click += new System.EventHandler(this.btnDeleteStorage_Click);
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(119, 90);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(100, 37);
            this.btnLoadFile.TabIndex = 6;
            this.btnLoadFile.Text = "Load file";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(592, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Attached files";
            // 
            // btnGetInfo
            // 
            this.btnGetInfo.Location = new System.Drawing.Point(350, 299);
            this.btnGetInfo.Name = "btnGetInfo";
            this.btnGetInfo.Size = new System.Drawing.Size(100, 37);
            this.btnGetInfo.TabIndex = 8;
            this.btnGetInfo.Text = "Get file info";
            this.btnGetInfo.UseVisualStyleBackColor = true;
            this.btnGetInfo.Click += new System.EventHandler(this.btnGetInfo_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(350, 381);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 37);
            this.button1.TabIndex = 9;
            this.button1.Text = "Attach";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ofdOpenFile
            // 
            this.ofdOpenFile.FileName = "ofdOpenFile";
            // 
            // FileAttachment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGetInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.btnDeleteStorage);
            this.Controls.Add(this.lbFiles);
            this.Controls.Add(this.btnDeleteMsg);
            this.Controls.Add(this.btnAddMsg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFileID);
            this.Name = "FileAttachment";
            this.Text = "FileAttachment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbFileID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddMsg;
        private System.Windows.Forms.Button btnDeleteMsg;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Button btnDeleteStorage;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGetInfo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog ofdOpenFile;
    }
}