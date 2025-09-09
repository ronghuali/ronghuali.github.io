using System;
using System.Collections.Generic;

namespace HtmlPaperManager.Models
{
    /// <summary>
    /// 论文条目数据模型
    /// </summary>
    public class Paper
    {
        /// <summary>
        /// 作者列表（包含HTML格式）
        /// </summary>
        public string Authors { get; set; }

        /// <summary>
        /// 论文标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 发表信息（期刊/会议、卷号、页码、年份等）
        /// </summary>
        public string PublicationInfo { get; set; }

        /// <summary>
        /// PDF链接路径
        /// </summary>
        public string PdfLink { get; set; }

        /// <summary>
        /// 代码链接
        /// </summary>
        public string CodeLink { get; set; }

        /// <summary>
        /// 是否显示PDF链接
        /// </summary>
        public bool ShowPdfLink { get; set; }

        /// <summary>
        /// 是否显示Code链接
        /// </summary>
        public bool ShowCodeLink { get; set; }

        /// <summary>
        /// 年份（用于分组，可以是任意字符串如"*~2021"）
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 条目类型（论文、年份标记、注释）
        /// </summary>
        public PaperEntryType EntryType { get; set; }

        /// <summary>
        /// 注释内容（当EntryType为Comment时使用）
        /// </summary>
        public string CommentText { get; set; }

        /// <summary>
        /// 论文序号（仅用于论文类型）
        /// </summary>
        public int PaperNumber { get; set; }

        /// <summary>
        /// 原始HTML内容（用于预览显示）
        /// </summary>
        public string OriginalHtml { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Paper()
        {
            Authors = string.Empty;
            Title = string.Empty;
            PublicationInfo = string.Empty;
            PdfLink = string.Empty;
            CodeLink = "https://github.com/ronghuali";
            ShowPdfLink = true;
            ShowCodeLink = false;
            Year = DateTime.Now.Year.ToString();
            EntryType = PaperEntryType.Paper;
            CommentText = string.Empty;
            PaperNumber = 0; // 将在更新列表时重新分配
            OriginalHtml = string.Empty;
        }

        /// <summary>
        /// 生成默认PDF路径
        /// </summary>
        public void GenerateDefaultPdfPath()
        {
            if (!string.IsNullOrEmpty(Title))
            {
                // 去掉标题中的冒号，避免文件路径问题
                string cleanTitle = Title.Replace(":", "");
                PdfLink = $"./PaperFiles/{cleanTitle}.pdf";
            }
        }

        /// <summary>
        /// 获取显示用的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            switch (EntryType)
            {
                case PaperEntryType.Paper:
                    // 清理文本，移除多余的空白字符和换行
                    string cleanAuthors = CleanText(Authors);
                    string cleanTitle = CleanText(Title);
                    string cleanPublicationInfo = CleanText(PublicationInfo);
                    return $"[{PaperNumber}] {cleanAuthors}. {cleanTitle}. {cleanPublicationInfo}";
                case PaperEntryType.YearHeader:
                    return $"[年份] {Year}";
                case PaperEntryType.Comment:
                    return $"[注释] {CommentText}";
                default:
                    return base.ToString();
            }
        }

        /// <summary>
        /// 清理文本中的多余空白字符
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <returns>清理后的文本</returns>
        private string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
                
            // 先解码HTML实体，然后移除换行符并替换为空格，然后合并多个空格为单个空格
            text = System.Net.WebUtility.HtmlDecode(text);
            return System.Text.RegularExpressions.Regex.Replace(
                text.Replace("\n", " ").Replace("\r", " ").Trim(), 
                @"\s+", " ");
        }

        /// <summary>
        /// 创建年份标记条目
        /// </summary>
        /// <param name="yearText">年份文本</param>
        /// <returns>年份标记条目</returns>
        public static Paper CreateYearHeader(string yearText)
        {
            var yearHtml = $"<span style=\"font-size:16.0pt\"><strong>{yearText}</strong></span>";
            return new Paper
            {
                EntryType = PaperEntryType.YearHeader,
                Year = yearText,
                OriginalHtml = yearHtml
            };
        }

        /// <summary>
        /// 创建注释条目
        /// </summary>
        /// <param name="comment">注释内容</param>
        /// <returns>注释条目</returns>
        public static Paper CreateComment(string comment)
        {
            var commentHtml = $"<!-- /* {comment} */ -->";
            return new Paper
            {
                EntryType = PaperEntryType.Comment,
                CommentText = comment,
                OriginalHtml = commentHtml
            };
        }
    }

    /// <summary>
    /// 论文条目类型
    /// </summary>
    public enum PaperEntryType
    {
        /// <summary>
        /// 普通论文条目
        /// </summary>
        Paper,

        /// <summary>
        /// 年份标记
        /// </summary>
        YearHeader,

        /// <summary>
        /// 注释条目
        /// </summary>
        Comment
    }
}
