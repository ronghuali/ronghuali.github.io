namespace HtmlPaperManager
{
    partial class PaperEditDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtAuthors = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPublicationInfo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPdfLink = new System.Windows.Forms.TextBox();
            this.btnSelectPdf = new System.Windows.Forms.Button();
            this.chkShowPdf = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCodeLink = new System.Windows.Forms.TextBox();
            this.chkShowCode = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.webPreview = new System.Windows.Forms.WebBrowser();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGeneratePdf = new System.Windows.Forms.Button();
            this.btnDefaultCode = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "作者：";
            // 
            // txtAuthors
            // 
            this.txtAuthors.Location = new System.Drawing.Point(60, 12);
            this.txtAuthors.Multiline = true;
            this.txtAuthors.Name = "txtAuthors";
            this.txtAuthors.Size = new System.Drawing.Size(500, 40);
            this.txtAuthors.TabIndex = 1;
            this.txtAuthors.TextChanged += new System.EventHandler(this.txtAuthors_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "标题：";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(60, 62);
            this.txtTitle.Multiline = true;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(500, 40);
            this.txtTitle.TabIndex = 3;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "发表信息：";
            // 
            // txtPublicationInfo
            // 
            this.txtPublicationInfo.Location = new System.Drawing.Point(83, 112);
            this.txtPublicationInfo.Multiline = true;
            this.txtPublicationInfo.Name = "txtPublicationInfo";
            this.txtPublicationInfo.Size = new System.Drawing.Size(477, 40);
            this.txtPublicationInfo.TabIndex = 5;
            this.txtPublicationInfo.TextChanged += new System.EventHandler(this.txtPublicationInfo_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "PDF链接：";
            // 
            // txtPdfLink
            // 
            this.txtPdfLink.Location = new System.Drawing.Point(77, 162);
            this.txtPdfLink.Name = "txtPdfLink";
            this.txtPdfLink.Size = new System.Drawing.Size(350, 21);
            this.txtPdfLink.TabIndex = 7;
            this.txtPdfLink.TextChanged += new System.EventHandler(this.txtPdfLink_TextChanged);
            // 
            // btnSelectPdf
            // 
            this.btnSelectPdf.Location = new System.Drawing.Point(433, 160);
            this.btnSelectPdf.Name = "btnSelectPdf";
            this.btnSelectPdf.Size = new System.Drawing.Size(60, 23);
            this.btnSelectPdf.TabIndex = 8;
            this.btnSelectPdf.Text = "选择文件";
            this.btnSelectPdf.UseVisualStyleBackColor = true;
            this.btnSelectPdf.Click += new System.EventHandler(this.btnSelectPdf_Click);
            // 
            // chkShowPdf
            // 
            this.chkShowPdf.AutoSize = true;
            this.chkShowPdf.Checked = true;
            this.chkShowPdf.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowPdf.Location = new System.Drawing.Point(77, 189);
            this.chkShowPdf.Name = "chkShowPdf";
            this.chkShowPdf.Size = new System.Drawing.Size(84, 16);
            this.chkShowPdf.TabIndex = 9;
            this.chkShowPdf.Text = "显示PDF链接";
            this.chkShowPdf.UseVisualStyleBackColor = true;
            this.chkShowPdf.CheckedChanged += new System.EventHandler(this.chkShowPdf_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "Code链接：";
            // 
            // txtCodeLink
            // 
            this.txtCodeLink.Location = new System.Drawing.Point(89, 215);
            this.txtCodeLink.Name = "txtCodeLink";
            this.txtCodeLink.Size = new System.Drawing.Size(338, 21);
            this.txtCodeLink.TabIndex = 11;
            this.txtCodeLink.TextChanged += new System.EventHandler(this.txtCodeLink_TextChanged);
            // 
            // chkShowCode
            // 
            this.chkShowCode.AutoSize = true;
            this.chkShowCode.Location = new System.Drawing.Point(89, 242);
            this.chkShowCode.Name = "chkShowCode";
            this.chkShowCode.Size = new System.Drawing.Size(96, 16);
            this.chkShowCode.TabIndex = 12;
            this.chkShowCode.Text = "显示Code链接";
            this.chkShowCode.UseVisualStyleBackColor = true;
            this.chkShowCode.CheckedChanged += new System.EventHandler(this.chkShowCode_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.webPreview);
            this.groupBox1.Location = new System.Drawing.Point(14, 275);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(546, 150);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "预览";
            // 
            // webPreview
            // 
            this.webPreview.Location = new System.Drawing.Point(6, 20);
            this.webPreview.MinimumSize = new System.Drawing.Size(20, 20);
            this.webPreview.Name = "webPreview";
            this.webPreview.Size = new System.Drawing.Size(534, 124);
            this.webPreview.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(404, 440);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(485, 440);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.Location = new System.Drawing.Point(499, 160);
            this.btnGeneratePdf.Name = "btnGeneratePdf";
            this.btnGeneratePdf.Size = new System.Drawing.Size(60, 23);
            this.btnGeneratePdf.TabIndex = 18;
            this.btnGeneratePdf.Text = "生成路径";
            this.btnGeneratePdf.UseVisualStyleBackColor = true;
            this.btnGeneratePdf.Click += new System.EventHandler(this.btnGeneratePdf_Click);
            // 
            // btnDefaultCode
            // 
            this.btnDefaultCode.Location = new System.Drawing.Point(433, 213);
            this.btnDefaultCode.Name = "btnDefaultCode";
            this.btnDefaultCode.Size = new System.Drawing.Size(75, 23);
            this.btnDefaultCode.TabIndex = 19;
            this.btnDefaultCode.Text = "默认链接";
            this.btnDefaultCode.UseVisualStyleBackColor = true;
            this.btnDefaultCode.Click += new System.EventHandler(this.btnDefaultCode_Click);
            // 
            // PaperEditDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(574, 482);
            this.Controls.Add(this.btnDefaultCode);
            this.Controls.Add(this.btnGeneratePdf);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkShowCode);
            this.Controls.Add(this.txtCodeLink);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkShowPdf);
            this.Controls.Add(this.btnSelectPdf);
            this.Controls.Add(this.txtPdfLink);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPublicationInfo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAuthors);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaperEditDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑论文";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAuthors;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPublicationInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPdfLink;
        private System.Windows.Forms.Button btnSelectPdf;
        private System.Windows.Forms.CheckBox chkShowPdf;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCodeLink;
        private System.Windows.Forms.CheckBox chkShowCode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.WebBrowser webPreview;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGeneratePdf;
        private System.Windows.Forms.Button btnDefaultCode;
    }
}
