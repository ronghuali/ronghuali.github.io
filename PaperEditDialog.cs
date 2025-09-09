using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HtmlPaperManager.Models;
using HtmlPaperManager.Services;

namespace HtmlPaperManager
{
    /// <summary>
    /// 论文编辑对话框
    /// </summary>
    public partial class PaperEditDialog : Form
    {
        private HtmlGenerator htmlGenerator;
        private string htmlFolderPath; // 添加HTML文件夹路径字段

        public Paper Paper { get; private set; }

        public PaperEditDialog() : this(new Paper(), "")
        {
        }

        public PaperEditDialog(Paper paper, string htmlFolder = "")
        {
            InitializeComponent();
            htmlGenerator = new HtmlGenerator();
            htmlFolderPath = htmlFolder;
            Paper = new Paper
            {
                Authors = paper.Authors,
                Title = paper.Title,
                PublicationInfo = paper.PublicationInfo,
                PdfLink = paper.PdfLink,
                CodeLink = paper.CodeLink,
                ShowPdfLink = paper.ShowPdfLink,
                ShowCodeLink = paper.ShowCodeLink,
                Year = paper.Year,
                EntryType = paper.EntryType,
                CommentText = paper.CommentText
            };

            LoadPaperData();
            UpdatePreview();
            UpdatePdfButtonState(); // 添加PDF按钮状态更新
        }

        private void LoadPaperData()
        {
            txtAuthors.Text = Paper.Authors;
            txtTitle.Text = Paper.Title;
            txtPublicationInfo.Text = Paper.PublicationInfo;
            txtPdfLink.Text = Paper.PdfLink;
            txtCodeLink.Text = Paper.CodeLink;
            chkShowPdf.Checked = Paper.ShowPdfLink;
            chkShowCode.Checked = Paper.ShowCodeLink;
            // 年份字段已移除，不再设置年份值
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SavePaperData();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("请输入论文标题", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAuthors.Text))
            {
                MessageBox.Show("请输入作者信息", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAuthors.Focus();
                return false;
            }

            return true;
        }

        private void SavePaperData()
        {
            Paper.Authors = ProcessAuthors(txtAuthors.Text.Trim());
            Paper.Title = txtTitle.Text.Trim();
            Paper.PublicationInfo = txtPublicationInfo.Text.Trim();
            Paper.PdfLink = txtPdfLink.Text.Trim();
            Paper.CodeLink = txtCodeLink.Text.Trim();
            Paper.ShowPdfLink = chkShowPdf.Checked;
            Paper.ShowCodeLink = chkShowCode.Checked;
            // 年份字段已移除，保持原有年份值
        }

        private string ProcessAuthors(string authors)
        {
            // 自动处理Rong-Hua Li的加粗标记
            if (authors.Contains("Rong-Hua Li") && !authors.Contains("<strong>Rong-Hua Li"))
            {
                authors = authors.Replace("Rong-Hua Li*", "<strong>Rong-Hua Li*</strong>");
                authors = authors.Replace("Rong-Hua Li", "<strong>Rong-Hua Li</strong>");
            }
            return authors;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSelectPdf_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                openFileDialog.Title = "选择PDF文件";
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string relativePath = GetRelativePath(openFileDialog.FileName);
                    txtPdfLink.Text = relativePath;
                    chkShowPdf.Checked = true;
                    UpdatePreview();
                }
            }
        }

        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                // 不对文件名进行URL编码，保持原始空格
                string fileName = txtTitle.Text.Trim();
                txtPdfLink.Text = $"./PaperFiles/{fileName}.pdf";
                chkShowPdf.Checked = true;
                UpdatePreview();
            }
        }

        private void btnDefaultCode_Click(object sender, EventArgs e)
        {
            txtCodeLink.Text = "https://github.com/ronghuali";
            chkShowCode.Checked = true;
            UpdatePreview();
        }

        /// <summary>
        /// 打开PDF文件按钮点击事件
        /// </summary>
        private void btnOpenPdf_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPdfLink.Text))
            {
                MessageBox.Show("PDF链接为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string pdfPath = txtPdfLink.Text.Trim();
                
                // 如果是相对路径，结合HTML文件夹路径
                if (!Path.IsPathRooted(pdfPath) && !string.IsNullOrEmpty(htmlFolderPath))
                {
                    // 处理相对路径，如 "./PaperFiles/xxx.pdf"
                    if (pdfPath.StartsWith("./"))
                    {
                        pdfPath = pdfPath.Substring(2); // 移除 "./"
                    }
                    
                    pdfPath = Path.Combine(htmlFolderPath, pdfPath);
                }

                if (File.Exists(pdfPath))
                {
                    // 使用默认程序打开PDF
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = pdfPath,
                        UseShellExecute = true, // 使用系统默认程序打开
                        WorkingDirectory = htmlFolderPath // 设置工作目录为HTML文件夹
                    });
                }
                else
                {
                    MessageBox.Show($"PDF文件不存在:\n{pdfPath}", "文件不存在", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开PDF文件:\n{ex.Message}", "打开失败", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 更新PDF按钮状态
        /// </summary>
        private void UpdatePdfButtonState()
        {
            btnOpenPdf.Enabled = !string.IsNullOrWhiteSpace(txtPdfLink.Text);
        }

        private void txtAuthors_TextChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void txtPublicationInfo_TextChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void txtPdfLink_TextChanged(object sender, EventArgs e)
        {
            UpdatePreview();
            UpdatePdfButtonState(); // 更新PDF按钮状态
        }

        private void txtCodeLink_TextChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void chkShowPdf_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void chkShowCode_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            try
            {
                var tempPaper = new Paper
                {
                    Authors = ProcessAuthors(txtAuthors.Text.Trim()),
                    Title = txtTitle.Text.Trim(),
                    PublicationInfo = txtPublicationInfo.Text.Trim(),
                    PdfLink = txtPdfLink.Text.Trim(),
                    CodeLink = txtCodeLink.Text.Trim(),
                    ShowPdfLink = chkShowPdf.Checked,
                    ShowCodeLink = chkShowCode.Checked,
                    Year = Paper?.Year ?? DateTime.Now.Year.ToString() // 使用原有年份或当前年份
                };

                string previewHtml = htmlGenerator.GeneratePreviewHtml(tempPaper);
                webPreview.DocumentText = $@"
                    <html>
                    <head>
                        <meta charset='utf-8'>
                        <style>
                            body {{ font-family: Arial, sans-serif; margin: 10px; }}
                            p {{ margin: 5px 0; }}
                            a {{ color: blue; text-decoration: underline; }}
                        </style>
                    </head>
                    <body>
                        {previewHtml}
                    </body>
                    </html>";
            }
            catch (Exception)
            {
                // 预览更新失败时忽略错误
            }
        }

        private string GetRelativePath(string fullPath)
        {
            try
            {
                // 尝试生成相对于应用程序目录的相对路径
                string appDir = Application.StartupPath;
                Uri appUri = new Uri(appDir + Path.DirectorySeparatorChar);
                Uri fileUri = new Uri(fullPath);
                string relativePath = appUri.MakeRelativeUri(fileUri).ToString().Replace('/', Path.DirectorySeparatorChar);
                
                // 解码URL编码，避免空格变成%20
                relativePath = Uri.UnescapeDataString(relativePath);
                
                return relativePath;
            }
            catch
            {
                // 如果生成相对路径失败，返回完整路径
                return fullPath;
            }
        }
    }
}
