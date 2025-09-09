namespace HtmlPaperManager
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.txtHtmlFolderPath = new System.Windows.Forms.TextBox();
            this.lblHtmlFolder = new System.Windows.Forms.Label();
            this.btnSelectHtmlFolder = new System.Windows.Forms.Button();
            this.txtEnglishFilePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSelectEnglishFile = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnGit = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnAddComment = new System.Windows.Forms.Button();
            this.btnAddYear = new System.Windows.Forms.Button();
            this.btnDeletePaper = new System.Windows.Forms.Button();
            this.btnEditPaper = new System.Windows.Forms.Button();
            this.btnAddPaper = new System.Windows.Forms.Button();
            this.listPapers = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.txtImport = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSearchInPreview = new System.Windows.Forms.Button();
            this.btnReplaceDivBlock = new System.Windows.Forms.Button();
            this.txtPreview = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblNotification = new System.Windows.Forms.Label();
            this.btnToggleEnglish = new System.Windows.Forms.Button();
            this.btnShowDebug = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnSelectEnglishFile);
            this.groupBox1.Controls.Add(this.txtEnglishFilePath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnSelectHtmlFolder);
            this.groupBox1.Controls.Add(this.txtHtmlFolderPath);
            this.groupBox1.Controls.Add(this.lblHtmlFolder);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtFilePath);
            this.groupBox1.Controls.Add(this.btnSelectFile);
            this.groupBox1.Controls.Add(this.btnOpenFolder);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1176, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件选择";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 138);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1176, 420);
            this.splitContainer1.SplitterDistance = 750;
            this.splitContainer1.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 420);
            this.panel1.TabIndex = 0;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFile.Location = new System.Drawing.Point(730, 20);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFile.TabIndex = 0;
            this.btnSelectFile.Text = "选择文件";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFolder.Location = new System.Drawing.Point(811, 20);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(90, 23);
            this.btnOpenFolder.TabIndex = 3;
            this.btnOpenFolder.Text = "在文件夹中打开";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilePath.Location = new System.Drawing.Point(80, 22);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(640, 21);
            this.txtFilePath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "HTML文件:";
            // 
            // lblHtmlFolder
            // 
            this.lblHtmlFolder.AutoSize = true;
            this.lblHtmlFolder.Location = new System.Drawing.Point(15, 50);
            this.lblHtmlFolder.Name = "lblHtmlFolder";
            this.lblHtmlFolder.Size = new System.Drawing.Size(65, 12);
            this.lblHtmlFolder.TabIndex = 4;
            this.lblHtmlFolder.Text = "HTML文件夹:";
            // 
            // txtHtmlFolderPath
            // 
            this.txtHtmlFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHtmlFolderPath.Location = new System.Drawing.Point(80, 47);
            this.txtHtmlFolderPath.Name = "txtHtmlFolderPath";
            this.txtHtmlFolderPath.Size = new System.Drawing.Size(640, 21);
            this.txtHtmlFolderPath.TabIndex = 5;
            // 
            // btnSelectHtmlFolder
            // 
            this.btnSelectHtmlFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectHtmlFolder.Location = new System.Drawing.Point(730, 45);
            this.btnSelectHtmlFolder.Name = "btnSelectHtmlFolder";
            this.btnSelectHtmlFolder.Size = new System.Drawing.Size(75, 23);
            this.btnSelectHtmlFolder.TabIndex = 6;
            this.btnSelectHtmlFolder.Text = "选择文件夹";
            this.btnSelectHtmlFolder.UseVisualStyleBackColor = true;
            this.btnSelectHtmlFolder.Click += new System.EventHandler(this.btnSelectHtmlFolder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "英文版:";
            // 
            // txtEnglishFilePath
            // 
            this.txtEnglishFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEnglishFilePath.Location = new System.Drawing.Point(80, 72);
            this.txtEnglishFilePath.Name = "txtEnglishFilePath";
            this.txtEnglishFilePath.Size = new System.Drawing.Size(640, 21);
            this.txtEnglishFilePath.TabIndex = 8;
            // 
            // btnSelectEnglishFile
            // 
            this.btnSelectEnglishFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectEnglishFile.Location = new System.Drawing.Point(730, 70);
            this.btnSelectEnglishFile.Name = "btnSelectEnglishFile";
            this.btnSelectEnglishFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectEnglishFile.TabIndex = 9;
            this.btnSelectEnglishFile.Text = "选择英文版";
            this.btnSelectEnglishFile.UseVisualStyleBackColor = true;
            this.btnSelectEnglishFile.Click += new System.EventHandler(this.btnSelectEnglishFile_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Controls.Add(this.btnGit);
            this.groupBox2.Controls.Add(this.btnMoveDown);
            this.groupBox2.Controls.Add(this.btnMoveUp);
            this.groupBox2.Controls.Add(this.btnAddComment);
            this.groupBox2.Controls.Add(this.btnAddYear);
            this.groupBox2.Controls.Add(this.btnDeletePaper);
            this.groupBox2.Controls.Add(this.btnEditPaper);
            this.groupBox2.Controls.Add(this.btnAddPaper);
            this.groupBox2.Controls.Add(this.listPapers);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(750, 420);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "论文管理";
            // 
            // btnGit
            // 
            this.btnGit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGit.Location = new System.Drawing.Point(669, 176);
            this.btnGit.Name = "btnGit";
            this.btnGit.Size = new System.Drawing.Size(75, 30);
            this.btnGit.TabIndex = 7;
            this.btnGit.Text = "Git";
            this.btnGit.UseVisualStyleBackColor = true;
            this.btnGit.Click += new System.EventHandler(this.btnGit_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveDown.Location = new System.Drawing.Point(669, 140);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(75, 30);
            this.btnMoveDown.TabIndex = 6;
            this.btnMoveDown.Text = "下移";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveUp.Location = new System.Drawing.Point(669, 104);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(75, 30);
            this.btnMoveUp.TabIndex = 5;
            this.btnMoveUp.Text = "上移";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnAddComment
            // 
            this.btnAddComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddComment.Location = new System.Drawing.Point(669, 320);
            this.btnAddComment.Name = "btnAddComment";
            this.btnAddComment.Size = new System.Drawing.Size(75, 30);
            this.btnAddComment.TabIndex = 4;
            this.btnAddComment.Text = "添加注释";
            this.btnAddComment.UseVisualStyleBackColor = true;
            this.btnAddComment.Click += new System.EventHandler(this.btnAddComment_Click);
            // 
            // btnAddYear
            // 
            this.btnAddYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddYear.Location = new System.Drawing.Point(669, 284);
            this.btnAddYear.Name = "btnAddYear";
            this.btnAddYear.Size = new System.Drawing.Size(75, 30);
            this.btnAddYear.TabIndex = 3;
            this.btnAddYear.Text = "添加年份";
            this.btnAddYear.UseVisualStyleBackColor = true;
            this.btnAddYear.Click += new System.EventHandler(this.btnAddYear_Click);
            // 
            // btnDeletePaper
            // 
            this.btnDeletePaper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletePaper.Location = new System.Drawing.Point(669, 68);
            this.btnDeletePaper.Name = "btnDeletePaper";
            this.btnDeletePaper.Size = new System.Drawing.Size(75, 30);
            this.btnDeletePaper.TabIndex = 2;
            this.btnDeletePaper.Text = "删除";
            this.btnDeletePaper.UseVisualStyleBackColor = true;
            this.btnDeletePaper.Click += new System.EventHandler(this.btnDeletePaper_Click);
            // 
            // btnEditPaper
            // 
            this.btnEditPaper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditPaper.Location = new System.Drawing.Point(669, 32);
            this.btnEditPaper.Name = "btnEditPaper";
            this.btnEditPaper.Size = new System.Drawing.Size(75, 30);
            this.btnEditPaper.TabIndex = 1;
            this.btnEditPaper.Text = "编辑";
            this.btnEditPaper.UseVisualStyleBackColor = true;
            this.btnEditPaper.Click += new System.EventHandler(this.btnEditPaper_Click);
            // 
            // btnAddPaper
            // 
            this.btnAddPaper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPaper.Location = new System.Drawing.Point(669, 248);
            this.btnAddPaper.Name = "btnAddPaper";
            this.btnAddPaper.Size = new System.Drawing.Size(75, 30);
            this.btnAddPaper.TabIndex = 0;
            this.btnAddPaper.Text = "添加论文";
            this.btnAddPaper.UseVisualStyleBackColor = true;
            this.btnAddPaper.Click += new System.EventHandler(this.btnAddPaper_Click);
            // 
            // listPapers
            // 
            this.listPapers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listPapers.FormattingEnabled = true;
            this.listPapers.HorizontalScrollbar = true;
            this.listPapers.ItemHeight = 24;
            this.listPapers.Location = new System.Drawing.Point(15, 20);
            this.listPapers.Name = "listPapers";
            this.listPapers.Size = new System.Drawing.Size(648, 388);
            this.listPapers.TabIndex = 0;
            this.listPapers.SelectedIndexChanged += new System.EventHandler(this.listPapers_SelectedIndexChanged);
            this.listPapers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listPapers_MouseDoubleClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnImport);
            this.groupBox3.Controls.Add(this.txtImport);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(416, 120);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "论文导入";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(331, 85);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "导入";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtImport
            // 
            this.txtImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImport.Location = new System.Drawing.Point(15, 35);
            this.txtImport.Multiline = true;
            this.txtImport.Name = "txtImport";
            this.txtImport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtImport.Size = new System.Drawing.Size(391, 44);
            this.txtImport.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "复制论文信息到此处导入:";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.btnSearchInPreview);
            this.groupBox4.Controls.Add(this.btnReplaceDivBlock);
            this.groupBox4.Controls.Add(this.txtPreview);
            this.groupBox4.Location = new System.Drawing.Point(3, 129);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(416, 288);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "HTML预览";
            // 
            // btnSearchInPreview
            // 
            this.btnSearchInPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchInPreview.Location = new System.Drawing.Point(250, 16);
            this.btnSearchInPreview.Name = "btnSearchInPreview";
            this.btnSearchInPreview.Size = new System.Drawing.Size(75, 23);
            this.btnSearchInPreview.TabIndex = 1;
            this.btnSearchInPreview.Text = "搜索";
            this.btnSearchInPreview.UseVisualStyleBackColor = true;
            this.btnSearchInPreview.Click += new System.EventHandler(this.btnSearchInPreview_Click);
            // 
            // btnReplaceDivBlock
            // 
            this.btnReplaceDivBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplaceDivBlock.Location = new System.Drawing.Point(331, 16);
            this.btnReplaceDivBlock.Name = "btnReplaceDivBlock";
            this.btnReplaceDivBlock.Size = new System.Drawing.Size(75, 23);
            this.btnReplaceDivBlock.TabIndex = 2;
            this.btnReplaceDivBlock.Text = "替换div块";
            this.btnReplaceDivBlock.UseVisualStyleBackColor = true;
            this.btnReplaceDivBlock.Click += new System.EventHandler(this.btnReplaceDivBlock_Click);
            // 
            // txtPreview
            // 
            this.txtPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPreview.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtPreview.Location = new System.Drawing.Point(15, 45);
            this.txtPreview.Multiline = true;
            this.txtPreview.Name = "txtPreview";
            this.txtPreview.ReadOnly = true;
            this.txtPreview.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPreview.Size = new System.Drawing.Size(391, 233);
            this.txtPreview.TabIndex = 0;
            this.txtPreview.WordWrap = false;
            this.txtPreview.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtPreview_MouseDoubleClick);
            // 
            // lblNotification
            // 
            this.lblNotification.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNotification.AutoSize = true;
            this.lblNotification.BackColor = System.Drawing.Color.Transparent;
            this.lblNotification.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.lblNotification.Location = new System.Drawing.Point(15, 545);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(0, 12);
            this.lblNotification.TabIndex = 5;
            this.lblNotification.Visible = false;
            // 
            // btnToggleEnglish
            // 
            this.btnToggleEnglish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggleEnglish.Location = new System.Drawing.Point(990, 540);
            this.btnToggleEnglish.Name = "btnToggleEnglish";
            this.btnToggleEnglish.Size = new System.Drawing.Size(85, 25);
            this.btnToggleEnglish.TabIndex = 6;
            this.btnToggleEnglish.Text = "切换成英文版";
            this.btnToggleEnglish.UseVisualStyleBackColor = true;
            this.btnToggleEnglish.Click += new System.EventHandler(this.btnToggleEnglish_Click);
            // 
            // btnShowDebug
            // 
            this.btnShowDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowDebug.Location = new System.Drawing.Point(900, 540);
            this.btnShowDebug.Name = "btnShowDebug";
            this.btnShowDebug.Size = new System.Drawing.Size(85, 25);
            this.btnShowDebug.TabIndex = 7;
            this.btnShowDebug.Text = "查看调试信息";
            this.btnShowDebug.UseVisualStyleBackColor = true;
            this.btnShowDebug.Click += new System.EventHandler(this.btnShowDebug_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(1080, 540);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(85, 25);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "复制HTML";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 580);
            this.Controls.Add(this.btnToggleEnglish);
            this.Controls.Add(this.btnShowDebug);
            this.Controls.Add(this.lblNotification);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(1000, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HTML论文管理工具";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnAddComment;
        private System.Windows.Forms.Button btnAddYear;
        private System.Windows.Forms.Button btnDeletePaper;
        private System.Windows.Forms.Button btnEditPaper;
        private System.Windows.Forms.Button btnAddPaper;
        private System.Windows.Forms.ListBox listPapers;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TextBox txtImport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtPreview;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblNotification;
        private System.Windows.Forms.Button btnToggleEnglish;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Button btnShowDebug;
        private System.Windows.Forms.TextBox txtHtmlFolderPath;
        private System.Windows.Forms.Label lblHtmlFolder;
        private System.Windows.Forms.Button btnSelectHtmlFolder;
        private System.Windows.Forms.TextBox txtEnglishFilePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelectEnglishFile;
        private System.Windows.Forms.Button btnReplaceDivBlock;
        private System.Windows.Forms.Button btnGit;
        private System.Windows.Forms.Button btnSearchInPreview;
    }
}
