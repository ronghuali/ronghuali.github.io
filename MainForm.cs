using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using HtmlPaperManager.Models;
using HtmlPaperManager.Services;

namespace HtmlPaperManager
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class MainForm : Form
    {
        private List<Paper> papers;
        private HtmlParser htmlParser;
        private HtmlGenerator htmlGenerator;
        private string currentHtmlPath;
        private AppConfig config;
        private bool isEnglishMode = false;
        private StringBuilder debugInfo; // 调试信息

        public MainForm()
        {
            InitializeComponent();
            InitializeData();
            LoadConfig();
            
            // 设置ListBox为自定义绘制模式以支持年份条目加粗
            listPapers.DrawMode = DrawMode.OwnerDrawFixed;
            listPapers.DrawItem += ListPapers_DrawItem;
        }

        /// <summary>
        /// 显示调试信息窗口
        /// </summary>
        private void ShowDebugInfo()
        {
            if (debugInfo == null || debugInfo.Length == 0)
            {
                MessageBox.Show("没有调试信息可显示", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 创建调试信息显示窗口
            Form debugForm = new Form()
            {
                Text = "HTML解析调试信息",
                Width = 800,
                Height = 600,
                StartPosition = FormStartPosition.CenterParent
            };

            TextBox debugTextBox = new TextBox()
            {
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10),
                Text = debugInfo.ToString(),
                ReadOnly = true
            };

            Button closeButton = new Button()
            {
                Text = "关闭",
                Dock = DockStyle.Bottom,
                Height = 30
            };

            // 关闭按钮点击事件
            closeButton.Click += (s, e) => debugForm.Close();

            debugForm.Controls.Add(debugTextBox);
            debugForm.Controls.Add(closeButton);
            
            // 使用非模态显示
            debugForm.Show();
        }

        /// <summary>
        /// 添加调试信息
        /// </summary>
        private void AppendDebugInfo(string message)
        {
            if (debugInfo == null)
                debugInfo = new StringBuilder();
            
            debugInfo.AppendLine(message);
        }

        /// <summary>
        /// 显示非对话框通知
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="color">颜色</param>
        private void ShowNotification(string message, Color color)
        {
            lblNotification.Text = message;
            lblNotification.ForeColor = color;
            lblNotification.Visible = true;
            
            // 3秒后自动隐藏
            var timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += (sender, e) =>
            {
                lblNotification.Visible = false;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        private void btnToggleEnglish_Click(object sender, EventArgs e)
        {
            if (papers.Count == 0)
            {
                ShowNotification("没有论文数据", Color.Orange);
                return;
            }

            try
            {
                // 获取当前HTML
                string htmlContent = htmlGenerator.GenerateHtml(papers);
                
                if (isEnglishMode)
                {
                    // 切换回中文版本
                    htmlContent = htmlContent.Replace("undergraduate", "本科生");
                    htmlContent = htmlContent.Replace("font-size:12.0pt", "font-size:16.0pt");
                    btnToggleEnglish.Text = "切换成英文版";
                    ShowNotification("已切换为中文版本", Color.Blue);
                    isEnglishMode = false;
                }
                else
                {
                    // 切换为英文版本
                    htmlContent = htmlContent.Replace("本科生", "undergraduate");
                    htmlContent = htmlContent.Replace("font-size:16.0pt", "font-size:12.0pt");
                    btnToggleEnglish.Text = "切换成中文版";
                    ShowNotification("已切换为英文版本", Color.Blue);
                    isEnglishMode = true;
                }
                
                // 更新预览
                txtPreview.Text = htmlContent;
            }
            catch (Exception ex)
            {
                ShowNotification($"切换失败：{ex.Message}", Color.Red);
            }
        }

        private void btnShowDebug_Click(object sender, EventArgs e)
        {
            ShowDebugInfo();
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
            {
                ShowNotification("请先选择一个有效的HTML文件", Color.Orange);
                return;
            }

            try
            {
                string folderPath = Path.GetDirectoryName(txtFilePath.Text);
                System.Diagnostics.Process.Start("explorer.exe", folderPath);
            }
            catch (Exception ex)
            {
                ShowNotification($"打开文件夹失败：{ex.Message}", Color.Red);
            }
        }

        private void UpdatePreview()
        {
            if (papers.Count == 0)
            {
                txtPreview.Text = "没有论文数据";
                return;
            }

            try
            {
                string htmlContent = htmlGenerator.GenerateHtml(papers);
                
                // 如果当前是英文模式，应用英文转换
                if (isEnglishMode)
                {
                    htmlContent = htmlContent.Replace("本科生", "undergraduate");
                    htmlContent = htmlContent.Replace("font-size:16.0pt", "font-size:12.0pt");
                }
                
                txtPreview.Text = htmlContent;
            }
            catch (Exception ex)
            {
                txtPreview.Text = $"生成预览失败：{ex.Message}";
            }
        }

        private void InitializeData()
        {
            papers = new List<Paper>();
            htmlParser = new HtmlParser();
            htmlGenerator = new HtmlGenerator();
            debugInfo = new StringBuilder();
        }

        private void LoadConfig()
        {
            try
            {
                config = AppConfig.Load();
                
                // 设置HTML文件夹路径
                if (!string.IsNullOrEmpty(config.HtmlFolderPath))
                {
                    txtHtmlFolderPath.Text = config.HtmlFolderPath;
                }
                
                // 设置英文版路径
                if (!string.IsNullOrEmpty(config.EnglishFilePath))
                {
                    txtEnglishFilePath.Text = config.EnglishFilePath;
                }
                
                // 加载上次打开的文件
                if (!string.IsNullOrEmpty(config.LastOpenedFile) && File.Exists(config.LastOpenedFile))
                {
                    LoadHtmlFile(config.LastOpenedFile, false);
                    // 根据当前文件路径更新其他路径
                    UpdatePathsFromCurrentFile();
                }
                else
                {
                    // 如果没有上次打开的文件，但有其他路径配置，尝试更新
                    if (!string.IsNullOrEmpty(txtHtmlFolderPath.Text))
                    {
                        UpdateDefaultPaths();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveConfig()
        {
            try
            {
                if (config == null)
                    config = new AppConfig();
                
                if (!string.IsNullOrEmpty(currentHtmlPath))
                {
                    config.LastOpenedFile = currentHtmlPath;
                }

                // 保存HTML文件夹和英文版路径
                config.HtmlFolderPath = txtHtmlFolderPath.Text;
                config.EnglishFilePath = txtEnglishFilePath.Text;
                
                config.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadHtmlFile(string filePath, bool showMessage = true)
        {
            try
            {
                // 重置调试信息
                debugInfo.Clear();
                debugInfo.AppendLine("=== HTML解析调试信息 ===");
                debugInfo.AppendLine($"文件路径: {filePath}");
                debugInfo.AppendLine($"解析时间: {DateTime.Now}");
                debugInfo.AppendLine();

                if (!File.Exists(filePath))
                {
                    debugInfo.AppendLine("错误: 文件不存在！");
                    if (showMessage)
                        MessageBox.Show("文件不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                debugInfo.AppendLine("开始解析HTML文件...");
                var parsedPapers = htmlParser.ParseHtmlFileWithDebug(filePath, debugInfo);
                
                papers.Clear();
                papers.AddRange(parsedPapers);
                
                currentHtmlPath = filePath;
                
                // 更新文件路径显示
                txtFilePath.Text = filePath;
                
                UpdatePaperNumbers();
                listPapers.DataSource = null;
                listPapers.DataSource = papers;
                listPapers.DisplayMember = "ToString";
                
                UpdatePreview();
                SaveConfig();
                
                // 添加最终统计到调试信息
                int paperCount = papers.Count(p => p.EntryType == PaperEntryType.Paper);
                int yearCount = papers.Count(p => p.EntryType == PaperEntryType.YearHeader);
                int commentCount = papers.Count(p => p.EntryType == PaperEntryType.Comment);
                
                debugInfo.AppendLine();
                debugInfo.AppendLine("=== 解析结果统计 ===");
                debugInfo.AppendLine($"论文条目: {paperCount}");
                debugInfo.AppendLine($"年份标记: {yearCount}");
                debugInfo.AppendLine($"注释条目: {commentCount}");
                debugInfo.AppendLine($"总条目数: {papers.Count}");
                
                if (showMessage)
                {
                    string message = $"成功加载 {paperCount} 篇论文！\n" +
                                   $"其他统计：{yearCount} 个年份标记，{commentCount} 个注释\n\n" +
                                   $"右键点击条目列表可查看详细调试信息";
                    
                    MessageBox.Show(message, "成功", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (debugInfo != null)
                {
                    debugInfo.AppendLine($"解析异常: {ex.Message}");
                    debugInfo.AppendLine($"堆栈跟踪: {ex.StackTrace}");
                }
                
                if (showMessage)
                    MessageBox.Show($"加载文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdatePaperNumbers()
        {
            int paperNumber = 1;
            foreach (var paper in papers)
            {
                if (paper.EntryType == PaperEntryType.Paper)
                {
                    paper.PaperNumber = paperNumber++;
                }
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "HTML files (*.html)|*.html|All files (*.*)|*.*";
                dialog.Title = "选择HTML文件";
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadHtmlFile(dialog.FileName);
                }
            }
        }

        private void btnAddPaper_Click(object sender, EventArgs e)
        {
            using (var dialog = new PaperEditDialog(new Paper(), txtHtmlFolderPath.Text))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    InsertAfterCurrent(dialog.Paper);
                }
            }
        }

        private void InsertAfterCurrent(Paper newPaper)
        {
            int insertIndex = papers.Count;
            if (listPapers.SelectedIndex >= 0)
            {
                insertIndex = listPapers.SelectedIndex + 1;
            }
            
            papers.Insert(insertIndex, newPaper);
            UpdatePaperNumbers();
            RefreshList();
            UpdatePreview();
            listPapers.SelectedIndex = insertIndex;
        }

        private void btnEditPaper_Click(object sender, EventArgs e)
        {
            if (listPapers.SelectedIndex < 0)
            {
                MessageBox.Show("请先选择一篇论文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int index = listPapers.SelectedIndex;
            Paper paper = papers[index];

            // 根据论文类型选择不同的编辑方式
            switch (paper.EntryType)
            {
                case PaperEntryType.Paper:
                    EditPaper(index, paper);
                    break;
                case PaperEntryType.YearHeader:
                    EditYearHeader(index, paper);
                    break;
                case PaperEntryType.Comment:
                    EditComment(index, paper);
                    break;
            }
        }

        private void EditPaper(int index, Paper paper)
        {
            using (var dialog = new PaperEditDialog(paper, txtHtmlFolderPath.Text))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    papers[index] = dialog.Paper;
                    UpdatePaperNumbers();
                    RefreshList();
                    UpdatePreview();
                    listPapers.SelectedIndex = index;
                }
            }
        }

        private void EditYearHeader(int index, Paper paper)
        {
            string newYear = ShowInputDialog("编辑年份", "请输入年份:", paper.Year);
            if (!string.IsNullOrEmpty(newYear))
            {
                papers[index] = Paper.CreateYearHeader(newYear);
                RefreshList();
                UpdatePreview();
                listPapers.SelectedIndex = index;
            }
        }

        private void EditComment(int index, Paper paper)
        {
            string newTitle = ShowInputDialog("编辑评论", "请输入评论内容:", paper.CommentText);
            if (!string.IsNullOrEmpty(newTitle))
            {
                papers[index] = Paper.CreateComment(newTitle);
                RefreshList();
                UpdatePreview();
                listPapers.SelectedIndex = index;
            }
        }

        private string ShowInputDialog(string title, string prompt, string defaultValue = "")
        {
            Form inputForm = new Form()
            {
                Width = 400,
                Height = 160,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblPrompt = new Label() { Left = 10, Top = 15, Text = prompt, Width = 350 };
            TextBox txtInput = new TextBox() { Left = 10, Top = 40, Width = 350, Text = defaultValue };
            Button btnOK = new Button() { Text = "确定", Left = 230, Width = 60, Top = 70, DialogResult = DialogResult.OK };
            Button btnCancel = new Button() { Text = "取消", Left = 300, Width = 60, Top = 70, DialogResult = DialogResult.Cancel };

            btnOK.Click += (sender, e) => { inputForm.Close(); };
            btnCancel.Click += (sender, e) => { inputForm.Close(); };

            inputForm.Controls.Add(lblPrompt);
            inputForm.Controls.Add(txtInput);
            inputForm.Controls.Add(btnOK);
            inputForm.Controls.Add(btnCancel);
            inputForm.AcceptButton = btnOK;
            inputForm.CancelButton = btnCancel;

            return inputForm.ShowDialog() == DialogResult.OK ? txtInput.Text : "";
        }

        private void btnDeletePaper_Click(object sender, EventArgs e)
        {
            if (listPapers.SelectedIndex < 0)
            {
                MessageBox.Show("请先选择一篇论文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("确定要删除选中的项目吗？", "确认删除", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                papers.RemoveAt(listPapers.SelectedIndex);
                UpdatePaperNumbers();
                RefreshList();
                UpdatePreview();
            }
        }

        private void btnAddYear_Click(object sender, EventArgs e)
        {
            string year = ShowInputDialog("添加年份", "请输入年份:");
            if (!string.IsNullOrEmpty(year))
            {
                InsertAfterCurrent(Paper.CreateYearHeader(year));
            }
        }

        private void btnAddComment_Click(object sender, EventArgs e)
        {
            string comment = ShowInputDialog("添加评论", "请输入评论内容:");
            if (!string.IsNullOrEmpty(comment))
            {
                InsertAfterCurrent(Paper.CreateComment(comment));
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (papers.Count == 0)
            {
                ShowNotification("没有论文数据", Color.Orange);
                return;
            }

            try
            {
                // 复制当前预览窗口的内容
                string htmlContent = txtPreview.Text;
                if (string.IsNullOrWhiteSpace(htmlContent))
                {
                    // 如果预览为空，则重新生成
                    htmlContent = htmlGenerator.GenerateHtml(papers);
                    if (isEnglishMode)
                    {
                        htmlContent = htmlContent.Replace("本科生", "undergraduate");
                        htmlContent = htmlContent.Replace("font-size:16.0pt", "font-size:12.0pt");
                    }
                }
                
                Clipboard.SetText(htmlContent);
                ShowNotification("HTML已复制到剪贴板", Color.Green);
            }
            catch (Exception ex)
            {
                ShowNotification($"复制失败：{ex.Message}", Color.Red);
            }
        }

        private void RefreshList()
        {
            int selectedIndex = listPapers.SelectedIndex;
            listPapers.DataSource = null;
            listPapers.DataSource = papers;
            listPapers.DisplayMember = "ToString";
            if (selectedIndex >= 0 && selectedIndex < papers.Count)
            {
                listPapers.SelectedIndex = selectedIndex;
            }
        }

        private void listPapers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 选择变化时的处理可以在这里添加
        }

        private void listPapers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listPapers.SelectedIndex < 0 || listPapers.SelectedIndex >= papers.Count)
                return;

            // 获取选中的Paper
            Paper selectedPaper = papers[listPapers.SelectedIndex];
            
            try
            {
                // 生成完整的HTML内容
                string fullHtml = htmlGenerator.GenerateHtml(papers);
                
                // 根据当前语言模式调整HTML
                if (isEnglishMode)
                {
                    fullHtml = fullHtml.Replace("本科生", "undergraduate");
                    fullHtml = fullHtml.Replace("font-size:16.0pt", "font-size:12.0pt");
                }
                
                // 更新预览窗口
                txtPreview.Text = fullHtml;
                
                // 查找选中条目在HTML中的位置
                string searchText = GetSearchText(selectedPaper);
                
                if (!string.IsNullOrEmpty(searchText))
                {
                    // 在预览窗口中查找并定位到该条目
                    int startIndex = FindTextInHtml(fullHtml, searchText);
                    if (startIndex >= 0)
                    {
                        // 将光标定位到找到的位置
                        txtPreview.SelectionStart = startIndex;
                        txtPreview.SelectionLength = Math.Min(searchText.Length, fullHtml.Length - startIndex);
                        txtPreview.ScrollToCaret();
                        txtPreview.Focus();
                        
                        ShowNotification($"已定位到选中条目在完整HTML中的位置", Color.Green);
                    }
                    else
                    {
                        ShowNotification("在HTML中未找到该条目的精确位置", Color.Orange);
                    }
                }
                else
                {
                    ShowNotification("无法生成该条目的搜索文本", Color.Orange);
                }
            }
            catch (Exception ex)
            {
                ShowNotification($"定位失败：{ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// 获取用于搜索的文本
        /// </summary>
        /// <param name="paper">论文对象</param>
        /// <returns>搜索文本</returns>
        private string GetSearchText(Paper paper)
        {
            switch (paper.EntryType)
            {
                case PaperEntryType.YearHeader:
                    // 年份标记，使用年份文本搜索
                    string yearSearchText = $"<strong>{paper.Year}</strong>";
                    if (isEnglishMode)
                    {
                        yearSearchText = yearSearchText.Replace("font-size:16.0pt", "font-size:12.0pt");
                    }
                    return yearSearchText;

                case PaperEntryType.Comment:
                    // 注释条目，搜索注释内容
                    return $"/* {paper.CommentText} */";

                case PaperEntryType.Paper:
                    // 论文条目，使用标题的部分文本搜索
                    if (!string.IsNullOrEmpty(paper.Title))
                    {
                        // 使用论文标题的前30个字符作为搜索文本
                        string titleText = paper.Title.Length > 30 ? 
                            paper.Title.Substring(0, 30) : paper.Title;
                        
                        if (isEnglishMode)
                        {
                            titleText = titleText.Replace("本科生", "undergraduate");
                        }
                        
                        return titleText;
                    }
                    break;
            }
            
            return string.Empty;
        }

        /// <summary>
        /// 在HTML中查找文本的位置
        /// </summary>
        /// <param name="html">HTML内容</param>
        /// <param name="searchText">搜索文本</param>
        /// <returns>找到的位置，-1表示未找到</returns>
        private int FindTextInHtml(string html, string searchText)
        {
            // 先尝试精确匹配
            int index = html.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
            
            if (index >= 0)
                return index;

            // 如果精确匹配失败，尝试更宽泛的匹配
            // 移除HTML标签后再搜索
            string cleanSearchText = System.Text.RegularExpressions.Regex.Replace(searchText, "<.*?>", "").Trim();
            if (!string.IsNullOrEmpty(cleanSearchText) && cleanSearchText.Length > 5)
            {
                return html.IndexOf(cleanSearchText, StringComparison.OrdinalIgnoreCase);
            }

            return -1;
        }

        // 这些方法需要实现，因为Designer中有对应的事件处理
        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (listPapers.SelectedIndex <= 0) return;
            
            int selectedIndex = listPapers.SelectedIndex;
            var selectedPaper = papers[selectedIndex];
            
            papers.RemoveAt(selectedIndex);
            papers.Insert(selectedIndex - 1, selectedPaper);
            
            UpdatePaperNumbers();
            RefreshList();
            UpdatePreview();
            listPapers.SelectedIndex = selectedIndex - 1;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (listPapers.SelectedIndex < 0 || listPapers.SelectedIndex >= papers.Count - 1) return;
            
            int selectedIndex = listPapers.SelectedIndex;
            var selectedPaper = papers[selectedIndex];
            
            papers.RemoveAt(selectedIndex);
            papers.Insert(selectedIndex + 1, selectedPaper);
            
            UpdatePaperNumbers();
            RefreshList();
            UpdatePreview();
            listPapers.SelectedIndex = selectedIndex + 1;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImport.Text))
            {
                ShowNotification("请输入要导入的内容", Color.Orange);
                return;
            }

            try
            {
                var importedPaper = htmlParser.ParseImportedPaper(txtImport.Text);
                if (importedPaper != null)
                {
                    InsertAfterCurrent(importedPaper);
                    txtImport.Clear();
                    ShowNotification("成功导入论文", Color.Green);
                }
                else
                {
                    ShowNotification("导入内容格式不正确", Color.Orange);
                }
            }
            catch (Exception ex)
            {
                ShowNotification($"导入失败：{ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// 自定义绘制ListBox项目，年份条目显示为加粗
        /// </summary>
        private void ListPapers_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0 || e.Index >= papers.Count)
                return;

            var paper = papers[e.Index];
            
            // 根据条目类型选择字体和颜色
            Font font;
            Color textColor;
            
            if (paper.EntryType == PaperEntryType.YearHeader)
            {
                // 年份条目使用加粗字体
                font = new Font(e.Font, FontStyle.Bold);
                textColor = Color.DarkBlue;
            }
            else if (paper.EntryType == PaperEntryType.Comment)
            {
                // 注释条目使用斜体
                font = new Font(e.Font, FontStyle.Italic);
                textColor = Color.DarkGreen;
            }
            else
            {
                // 论文条目使用普通字体
                font = e.Font;
                textColor = (e.State & DrawItemState.Selected) != 0 ? SystemColors.HighlightText : e.ForeColor;
            }

            // 计算文本绘制位置，确保垂直居中
            var textBounds = e.Bounds;
            textBounds.X += 2; // 左侧留一点边距
            textBounds.Width -= 4; // 左右都留边距
            
            // 使用 StringFormat 确保文本垂直居中和左对齐
            using (var brush = new SolidBrush(textColor))
            using (var stringFormat = new StringFormat())
            {
                stringFormat.LineAlignment = StringAlignment.Center; // 垂直居中
                stringFormat.Alignment = StringAlignment.Near;       // 左对齐
                stringFormat.Trimming = StringTrimming.EllipsisCharacter; // 超长文本显示省略号
                stringFormat.FormatFlags = StringFormatFlags.NoWrap;      // 不换行
                
                e.Graphics.DrawString(paper.ToString(), font, brush, textBounds, stringFormat);
            }

            // 如果需要释放字体资源
            if (font != e.Font)
            {
                font.Dispose();
            }

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// 预览窗口双击事件 - 定位到对应的论文条目
        /// </summary>
        private void txtPreview_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (papers == null || papers.Count == 0) return;

            try
            {
                // 获取双击位置的字符索引
                int charIndex = txtPreview.GetCharIndexFromPosition(e.Location);
                if (charIndex < 0) return;

                // 获取当前行和相邻行的内容（论文条目可能跨多行）
                string text = txtPreview.Text;
                string contextText = GetContextTextFromPosition(text, charIndex);
                
                if (string.IsNullOrWhiteSpace(contextText)) return;

                // 尝试从上下文中提取论文信息
                Paper targetPaper = FindPaperFromHtmlContext(contextText);
                if (targetPaper != null)
                {
                    // 在listPapers中找到对应的条目并选中
                    for (int i = 0; i < listPapers.Items.Count; i++)
                    {
                        if (listPapers.Items[i] == targetPaper)
                        {
                            listPapers.SelectedIndex = i;
                            listPapers.Focus();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"双击定位失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从指定位置获取上下文文本（包含当前行和相邻行）
        /// </summary>
        private string GetContextTextFromPosition(string text, int charIndex)
        {
            if (string.IsNullOrEmpty(text) || charIndex < 0 || charIndex >= text.Length)
                return string.Empty;

            // 找到当前行的起始和结束位置
            int currentLineStart = text.LastIndexOf('\n', charIndex) + 1;
            int currentLineEnd = text.IndexOf('\n', charIndex);
            if (currentLineEnd == -1) currentLineEnd = text.Length;

            // 获取当前行内容
            string currentLine = text.Substring(currentLineStart, currentLineEnd - currentLineStart).Trim();
            
            // 如果当前行看起来是论文条目的一部分，尝试获取完整的论文条目
            if (IsPartOfPaperEntry(currentLine))
            {
                return GetCompletePaperEntry(text, currentLineStart);
            }

            return currentLine;
        }

        /// <summary>
        /// 判断是否是论文条目的一部分
        /// </summary>
        private bool IsPartOfPaperEntry(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return false;
            
            // 检查是否包含论文条目的典型特征
            return line.Contains("<p>") || 
                   line.Contains("&nbsp;") || 
                   line.Contains("(pdf)") || 
                   line.Contains("(code)") ||
                   line.Contains("<strong>") ||
                   (line.Contains(".") && (line.Contains("20") || line.Contains("19"))) ||
                   line.Contains("</p>");
        }

        /// <summary>
        /// 获取完整的论文条目（可能跨多行）
        /// </summary>
        private string GetCompletePaperEntry(string text, int startPos)
        {
            // 向前查找<p>标签的开始
            int pStart = text.LastIndexOf("<p>", startPos);
            if (pStart == -1) pStart = startPos;

            // 向后查找</p>标签的结束
            int pEnd = text.IndexOf("</p>", startPos);
            if (pEnd == -1) 
            {
                // 如果没找到</p>，查找下一个<p>或文本结束
                int nextP = text.IndexOf("<p>", startPos + 1);
                pEnd = nextP == -1 ? text.Length : nextP;
            }
            else
            {
                pEnd += 4; // 包含</p>标签
            }

            return text.Substring(pStart, pEnd - pStart);
        }

        /// <summary>
        /// 从HTML上下文中找到对应的论文对象（改进版）
        /// </summary>
        private Paper FindPaperFromHtmlContext(string htmlContext)
        {
            if (string.IsNullOrWhiteSpace(htmlContext) || papers == null) return null;

            // 移除HTML标签，获取纯文本
            string cleanText = System.Text.RegularExpressions.Regex.Replace(htmlContext, "<[^>]+>", "").Trim();
            cleanText = cleanText.Replace("&nbsp;", " ").Replace("  ", " ");
            
            if (string.IsNullOrWhiteSpace(cleanText)) return null;

            var paperCandidates = papers.Where(p => p.EntryType == PaperEntryType.Paper).ToList();

            // 按匹配优先级进行匹配
            Paper bestMatch = null;
            int bestScore = 0;
            string bestReason = "";

            foreach (var paper in paperCandidates)
            {
                int score = 0;
                string reason = "";

                // 1. 标题完全匹配 (最高优先级)
                if (!string.IsNullOrEmpty(paper.Title) && cleanText.Contains(paper.Title))
                {
                    score += 100;
                    reason += $"标题匹配({paper.Title}); ";
                }

                // 2. 标题部分匹配 (去除标点符号)
                if (!string.IsNullOrEmpty(paper.Title))
                {
                    string normalizedTitle = paper.Title.Replace(":", "").Replace(",", "").Replace(".", "");
                    string normalizedCleanText = cleanText.Replace(":", "").Replace(",", "").Replace(".", "");
                    if (normalizedCleanText.Contains(normalizedTitle) || normalizedTitle.Contains(normalizedCleanText))
                    {
                        score += 80;
                        reason += $"标题部分匹配; ";
                    }
                }

                // 3. 作者匹配
                if (!string.IsNullOrEmpty(paper.Authors))
                {
                    string cleanAuthors = System.Text.RegularExpressions.Regex.Replace(paper.Authors, "<[^>]+>", "");
                    if (cleanText.Contains(cleanAuthors))
                    {
                        score += 60;
                        reason += $"作者完全匹配; ";
                    }
                    else if (cleanAuthors.Contains("Rong-Hua Li") && cleanText.Contains("Rong-Hua Li"))
                    {
                        score += 40;
                        reason += $"包含Rong-Hua Li; ";
                    }
                }

                // 4. 发表信息匹配
                if (!string.IsNullOrEmpty(paper.PublicationInfo) && cleanText.Contains(paper.PublicationInfo))
                {
                    score += 50;
                    reason += $"发表信息匹配; ";
                }

                // 5. PDF链接匹配
                if (!string.IsNullOrEmpty(paper.PdfLink) && htmlContext.Contains(paper.PdfLink))
                {
                    score += 30;
                    reason += $"PDF链接匹配; ";
                }

                // 更新最佳匹配
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMatch = paper;
                    bestReason = reason;
                }
            }

            return bestMatch;
        }

        private void btnSelectHtmlFolder_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择HTML文件夹";
                if (!string.IsNullOrEmpty(txtHtmlFolderPath.Text))
                {
                    dialog.SelectedPath = txtHtmlFolderPath.Text;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtHtmlFolderPath.Text = dialog.SelectedPath;
                    
                    // 自动设置默认路径
                    UpdateDefaultPaths();
                    SaveConfig();
                    
                    ShowNotification("HTML文件夹路径已更新", Color.Green);
                }
            }
        }

        private void btnSelectEnglishFile_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "HTML files (*.html)|*.html|All files (*.*)|*.*";
                dialog.Title = "选择英文版HTML文件";
                
                if (!string.IsNullOrEmpty(txtEnglishFilePath.Text) && File.Exists(txtEnglishFilePath.Text))
                {
                    dialog.InitialDirectory = Path.GetDirectoryName(txtEnglishFilePath.Text);
                    dialog.FileName = Path.GetFileName(txtEnglishFilePath.Text);
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtEnglishFilePath.Text = dialog.FileName;
                    
                    // 根据英文版文件推导HTML文件夹路径
                    UpdatePathsFromEnglishFile();
                    SaveConfig();
                    
                    ShowNotification("英文版文件路径已更新", Color.Green);
                }
            }
        }

        private void btnReplaceDivBlock_Click(object sender, EventArgs e)
        {
            try
            {
                if (papers.Count == 0)
                {
                    MessageBox.Show("没有论文数据，无法替换div块", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string targetFilePath = "";
                string fileType = "";

                if (isEnglishMode)
                {
                    // 英文版模式
                    if (string.IsNullOrEmpty(txtEnglishFilePath.Text))
                    {
                        MessageBox.Show("请先设置英文版文件路径", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    targetFilePath = txtEnglishFilePath.Text;
                    fileType = "英文版";
                }
                else
                {
                    // 中文版模式
                    if (string.IsNullOrEmpty(txtFilePath.Text))
                    {
                        MessageBox.Show("请先选择HTML文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    targetFilePath = txtFilePath.Text;
                    fileType = "中文版";
                }

                if (!File.Exists(targetFilePath))
                {
                    MessageBox.Show($"{fileType}文件不存在：{targetFilePath}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 读取目标文件内容
                string fileContent = File.ReadAllText(targetFilePath, Encoding.UTF8);
                
                // 获取预览内容
                string previewContent = txtPreview.Text;
                if (string.IsNullOrWhiteSpace(previewContent))
                {
                    MessageBox.Show("预览内容为空，无法替换", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 查找div块并替换
                string pattern = @"<div id=""papers"">.*?</div>";
                var regex = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.Singleline);
                
                if (regex.IsMatch(fileContent))
                {
                    // 从预览内容中提取div块
                    var previewMatch = regex.Match(previewContent);
                    if (previewMatch.Success)
                    {
                        string newDivContent = previewMatch.Value;
                        string newFileContent = regex.Replace(fileContent, newDivContent);
                        
                        // 写回文件
                        File.WriteAllText(targetFilePath, newFileContent, Encoding.UTF8);
                        
                        MessageBox.Show($"{fileType}文件的div块已成功替换！\n文件路径：{targetFilePath}", 
                            "替换成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("预览内容中没有找到div块", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"{fileType}文件中没有找到<div id=\"papers\">块", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"替换div块时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGit_Click(object sender, EventArgs e)
        {
            ShowGitDialog();
        }

        private void btnSearchInPreview_Click(object sender, EventArgs e)
        {
            ShowSearchDialog();
        }

        private void ShowGitDialog()
        {
            string workingDirectory = "";
            
            // 确定Git工作目录
            if (!string.IsNullOrEmpty(txtHtmlFolderPath.Text) && Directory.Exists(txtHtmlFolderPath.Text))
            {
                workingDirectory = txtHtmlFolderPath.Text;
            }
            else if (!string.IsNullOrEmpty(currentHtmlPath))
            {
                workingDirectory = Path.GetDirectoryName(currentHtmlPath);
            }
            else
            {
                MessageBox.Show("请先设置HTML文件夹路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 创建Git操作窗口
            Form gitForm = new Form()
            {
                Text = "Git操作",
                Size = new Size(600, 500),
                MinimumSize = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent,
                ShowIcon = false
            };

            // 工作目录标签
            Label lblWorkDir = new Label()
            {
                Text = $"工作目录：{workingDirectory}",
                Location = new Point(10, 10),
                Size = new Size(580, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // Git Add按钮
            Button btnGitAdd = new Button()
            {
                Text = "git add -A",
                Location = new Point(10, 40),
                Size = new Size(100, 30)
            };

            // Commit消息输入框
            Label lblCommitMsg = new Label()
            {
                Text = "提交消息:",
                Location = new Point(120, 45),
                Size = new Size(70, 20)
            };

            TextBox txtCommitMsg = new TextBox()
            {
                Location = new Point(200, 43),
                Size = new Size(250, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // Git Commit按钮
            Button btnGitCommit = new Button()
            {
                Text = "git commit",
                Location = new Point(460, 40),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Git Push按钮
            Button btnGitPush = new Button()
            {
                Text = "git push",
                Location = new Point(10, 80),
                Size = new Size(100, 30)
            };

            // 输出文本框
            TextBox txtOutput = new TextBox()
            {
                Location = new Point(10, 120),
                Size = new Size(560, 320),
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                ReadOnly = true,
                Font = new Font("Consolas", 9),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // 事件处理
            btnGitAdd.Click += (s, e) => ExecuteGitCommand("git add -A", workingDirectory, txtOutput);
            btnGitCommit.Click += (s, e) => 
            {
                if (string.IsNullOrWhiteSpace(txtCommitMsg.Text))
                {
                    MessageBox.Show("请输入提交消息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ExecuteGitCommand($"git commit -m \"{txtCommitMsg.Text}\"", workingDirectory, txtOutput);
            };
            btnGitPush.Click += (s, e) => ExecuteGitCommand("git push", workingDirectory, txtOutput);

            // 添加控件
            gitForm.Controls.AddRange(new Control[] 
            { 
                lblWorkDir, btnGitAdd, lblCommitMsg, txtCommitMsg, btnGitCommit, btnGitPush, txtOutput 
            });

            gitForm.ShowDialog();
        }

        private void ExecuteGitCommand(string command, string workingDirectory, TextBox outputTextBox)
        {
            try
            {
                outputTextBox.AppendText($"> {command}\r\n");
                
                var processInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = "git",
                    Arguments = command.Substring(4), // 去掉"git "前缀
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = System.Diagnostics.Process.Start(processInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    
                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(output))
                    {
                        outputTextBox.AppendText(output + "\r\n");
                    }
                    
                    if (!string.IsNullOrEmpty(error))
                    {
                        outputTextBox.AppendText("错误: " + error + "\r\n");
                    }

                    outputTextBox.AppendText($"命令执行完成，退出代码: {process.ExitCode}\r\n\r\n");
                    outputTextBox.SelectionStart = outputTextBox.Text.Length;
                    outputTextBox.ScrollToCaret();
                }
            }
            catch (Exception ex)
            {
                outputTextBox.AppendText($"执行命令时发生错误: {ex.Message}\r\n\r\n");
            }
        }

        private void ShowSearchDialog()
        {
            if (string.IsNullOrEmpty(txtPreview.Text))
            {
                MessageBox.Show("预览内容为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string searchText = ShowInputDialog("搜索", "请输入要搜索的内容:");
            if (!string.IsNullOrEmpty(searchText))
            {
                SearchInPreview(searchText);
            }
        }

        private void SearchInPreview(string searchText)
        {
            if (string.IsNullOrEmpty(searchText) || string.IsNullOrEmpty(txtPreview.Text))
                return;

            int startIndex = txtPreview.SelectionStart + txtPreview.SelectionLength;
            int foundIndex = txtPreview.Text.IndexOf(searchText, startIndex, StringComparison.OrdinalIgnoreCase);
            
            if (foundIndex == -1)
            {
                // 从头开始搜索
                foundIndex = txtPreview.Text.IndexOf(searchText, 0, StringComparison.OrdinalIgnoreCase);
            }

            if (foundIndex >= 0)
            {
                txtPreview.Select(foundIndex, searchText.Length);
                txtPreview.ScrollToCaret();
                txtPreview.Focus();
                ShowNotification($"找到匹配项 (位置: {foundIndex})", Color.Green);
            }
            else
            {
                ShowNotification("未找到匹配项", Color.Orange);
            }
        }

        private void UpdateDefaultPaths()
        {
            if (!string.IsNullOrEmpty(txtHtmlFolderPath.Text))
            {
                string folderPath = txtHtmlFolderPath.Text;
                
                // 如果中文版路径为空，设置默认值
                if (string.IsNullOrEmpty(txtFilePath.Text))
                {
                    string indexPath = Path.Combine(folderPath, "index.html");
                    if (File.Exists(indexPath))
                    {
                        txtFilePath.Text = indexPath;
                    }
                }
                
                // 如果英文版路径为空，设置默认值
                if (string.IsNullOrEmpty(txtEnglishFilePath.Text))
                {
                    string englishPath = Path.Combine(folderPath, "ronghuali.html");
                    if (File.Exists(englishPath))
                    {
                        txtEnglishFilePath.Text = englishPath;
                    }
                }
            }
        }

        private void UpdatePathsFromEnglishFile()
        {
            if (!string.IsNullOrEmpty(txtEnglishFilePath.Text) && File.Exists(txtEnglishFilePath.Text))
            {
                string folderPath = Path.GetDirectoryName(txtEnglishFilePath.Text);
                
                // 设置HTML文件夹路径
                if (string.IsNullOrEmpty(txtHtmlFolderPath.Text))
                {
                    txtHtmlFolderPath.Text = folderPath;
                }
                
                // 设置中文版路径
                if (string.IsNullOrEmpty(txtFilePath.Text))
                {
                    string indexPath = Path.Combine(folderPath, "index.html");
                    if (File.Exists(indexPath))
                    {
                        txtFilePath.Text = indexPath;
                    }
                }
            }
        }

        private void UpdatePathsFromCurrentFile()
        {
            if (!string.IsNullOrEmpty(currentHtmlPath) && File.Exists(currentHtmlPath))
            {
                string folderPath = Path.GetDirectoryName(currentHtmlPath);
                
                // 设置HTML文件夹路径
                if (string.IsNullOrEmpty(txtHtmlFolderPath.Text))
                {
                    txtHtmlFolderPath.Text = folderPath;
                }
                
                // 设置英文版路径
                if (string.IsNullOrEmpty(txtEnglishFilePath.Text))
                {
                    string englishPath = Path.Combine(folderPath, "ronghuali.html");
                    if (File.Exists(englishPath))
                    {
                        txtEnglishFilePath.Text = englishPath;
                    }
                }
            }
        }
    }
}
