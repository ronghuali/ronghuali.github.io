using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlPaperManager.Models;

namespace HtmlPaperManager.Services
{
    /// <summary>
    /// HTML生成服务
    /// </summary>
    public class HtmlGenerator
    {
        private const int IndentSize = 4;
        private const string Tab = "    "; // 4个空格作为缩进

        /// <summary>
        /// 生成论文列表的HTML代码
        /// </summary>
        /// <param name="papers">论文列表</param>
        /// <returns>HTML代码</returns>
        public string GenerateHtml(List<Paper> papers)
        {
            var html = new StringBuilder();
            
            // 添加开始标签
            html.AppendLine($"{Tab}{Tab}{Tab}<div id=\"papers\">");
            html.AppendLine();

            int paperCounter = 1; // 论文条目序号计数器

            foreach (var paper in papers)
            {
                switch (paper.EntryType)
                {
                    case PaperEntryType.YearHeader:
                        html.AppendLine(GenerateYearHeader(paper));
                        break;
                    case PaperEntryType.Comment:
                        html.AppendLine(GenerateComment(paper));
                        break;
                    case PaperEntryType.Paper:
                        html.AppendLine(GeneratePaperEntry(paper, paperCounter));
                        paperCounter++; // 只有论文条目才增加序号
                        break;
                }
            }

            // 添加结束标签
            html.AppendLine($"{Tab}{Tab}{Tab}</div>");

            return html.ToString();
        }

        /// <summary>
        /// 生成年份标记HTML
        /// </summary>
        /// <param name="paper">年份标记对象</param>
        /// <returns>年份标记HTML</returns>
        private string GenerateYearHeader(Paper paper)
        {
            var html = new StringBuilder();
            
            html.AppendLine($"{Tab}{Tab}{Tab}{Tab}<span style='font-size:16.0pt'><strong>{paper.Year}</strong></span></br>");
            html.AppendLine();

            return html.ToString();
        }

        /// <summary>
        /// 生成注释HTML
        /// </summary>
        /// <param name="paper">注释对象</param>
        /// <returns>注释HTML</returns>
        private string GenerateComment(Paper paper)
        {
            var html = new StringBuilder();
            html.AppendLine($"{Tab}{Tab}{Tab}{Tab}<!-- /* {paper.CommentText} */ -->");
            html.AppendLine(); // 注释后添加空行
            return html.ToString();
        }

        /// <summary>
        /// 生成论文条目HTML
        /// </summary>
        /// <param name="paper">论文对象</param>
        /// <param name="paperIndex">论文序号</param>
        /// <returns>论文条目HTML</returns>
        private string GeneratePaperEntry(Paper paper, int paperIndex)
        {
            var html = new StringBuilder();
            
            html.AppendLine($"{Tab}{Tab}{Tab}{Tab}<p id=\"paper-{paperIndex}\">");
            
            // 构建论文内容，不包含序号
            var content = new StringBuilder();
            content.Append($"{Tab}{Tab}{Tab}{Tab}{Tab}{paper.Authors}. {paper.Title}. {paper.PublicationInfo}");
            
            // 添加PDF链接
            if (paper.ShowPdfLink && !string.IsNullOrEmpty(paper.PdfLink))
            {
                content.AppendLine();
                content.Append($"{Tab}{Tab}{Tab}{Tab}{Tab}&nbsp;&nbsp;&nbsp;<a href=\"{paper.PdfLink}\">(pdf)</a>");
            }
            else if (!string.IsNullOrEmpty(paper.PdfLink))
            {
                // PDF链接存在但不显示，用注释包裹
                content.AppendLine();
                content.Append($"{Tab}{Tab}{Tab}{Tab}{Tab}<!-- &nbsp;&nbsp;&nbsp;<a href=\"{paper.PdfLink}\">(pdf)</a> -->");
            }

            // 添加Code链接
            if (paper.ShowCodeLink && !string.IsNullOrEmpty(paper.CodeLink))
            {
                content.AppendLine();
                content.Append($"{Tab}{Tab}{Tab}{Tab}{Tab}&nbsp;&nbsp;&nbsp;<a href=\"{paper.CodeLink}\">(code)</a>");
            }
            else if (!string.IsNullOrEmpty(paper.CodeLink) && paper.CodeLink != "https://github.com/ronghuali")
            {
                // Code链接存在但不显示，用注释包裹
                content.AppendLine();
                content.Append($"{Tab}{Tab}{Tab}{Tab}{Tab}<!-- &nbsp;&nbsp;&nbsp;<a href=\"{paper.CodeLink}\">(code)</a> -->");
            }

            html.Append(content.ToString());
            html.AppendLine();
            html.AppendLine($"{Tab}{Tab}{Tab}{Tab}</p>");
            html.AppendLine();

            return html.ToString();
        }

        /// <summary>
        /// 更新HTML文件中的papers部分
        /// </summary>
        /// <param name="originalHtmlPath">原始HTML文件路径</param>
        /// <param name="papers">论文列表</param>
        /// <returns>更新后的HTML内容</returns>
        public string UpdateHtmlFile(string originalHtmlPath, List<Paper> papers)
        {
            try
            {
                string originalContent = System.IO.File.ReadAllText(originalHtmlPath, Encoding.UTF8);
                
                // 查找papers div的开始和结束位置
                int startIndex = originalContent.IndexOf("<div id=\"papers\">", StringComparison.OrdinalIgnoreCase);
                if (startIndex == -1)
                {
                    throw new Exception("未找到papers div");
                }

                int endIndex = originalContent.IndexOf("</div>", startIndex, StringComparison.OrdinalIgnoreCase);
                if (endIndex == -1)
                {
                    throw new Exception("未找到papers div的结束标签");
                }
                endIndex += 6; // 包含</div>

                // 生成新的papers HTML
                string newPapersHtml = GenerateHtml(papers);
                
                // 替换原有内容
                string beforePapers = originalContent.Substring(0, startIndex);
                string afterPapers = originalContent.Substring(endIndex);
                
                // 移除新生成HTML中的多余缩进标签（因为我们会保持原有的缩进结构）
                newPapersHtml = newPapersHtml.Replace($"{Tab}{Tab}{Tab}<div id=\"papers\">", "<div id=\"papers\">")
                                           .Replace($"{Tab}{Tab}{Tab}</div>", "</div>");

                return beforePapers + newPapersHtml + afterPapers;
            }
            catch (Exception ex)
            {
                throw new Exception($"更新HTML文件失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 生成论文条目的预览HTML（用于编辑对话框预览）
        /// </summary>
        /// <param name="paper">论文对象</param>
        /// <returns>预览HTML</returns>
        public string GeneratePreviewHtml(Paper paper)
        {
            if (paper.EntryType == PaperEntryType.YearHeader)
            {
                return $"<span style='font-size:16.0pt'><strong>{paper.Year}</strong></span>";
            }
            else if (paper.EntryType == PaperEntryType.Comment)
            {
                return $"<!-- /* {paper.CommentText} */ -->";
            }
            else
            {
                var html = new StringBuilder();
                html.Append($"<p id=\"paper-preview\">{paper.Authors}. {paper.Title}. {paper.PublicationInfo}");
                
                if (paper.ShowPdfLink && !string.IsNullOrEmpty(paper.PdfLink))
                {
                    html.Append($" <a href=\"{paper.PdfLink}\">(pdf)</a>");
                }
                
                if (paper.ShowCodeLink && !string.IsNullOrEmpty(paper.CodeLink))
                {
                    html.Append($" <a href=\"{paper.CodeLink}\">(code)</a>");
                }
                
                html.Append("</p>");
                return html.ToString();
            }
        }

        /// <summary>
        /// 按年份对论文进行排序和分组
        /// </summary>
        /// <param name="papers">论文列表</param>
        /// <returns>排序后的论文列表</returns>
        public List<Paper> SortAndGroupPapers(List<Paper> papers)
        {
            var result = new List<Paper>();
            
            // 获取所有论文的年份，按降序排列
            var years = papers.Where(p => p.EntryType == PaperEntryType.Paper)
                              .Select(p => p.Year)
                              .Distinct()
                              .OrderByDescending(y => y)
                              .ToList();

            foreach (var year in years)
            {
                // 添加年份标记
                result.Add(Paper.CreateYearHeader(year));
                
                // 添加该年份的所有论文
                var yearPapers = papers.Where(p => p.EntryType == PaperEntryType.Paper && p.Year == year)
                                      .OrderBy(p => p.Title)
                                      .ToList();
                result.AddRange(yearPapers);
                
                // 添加该年份的注释
                var yearComments = papers.Where(p => p.EntryType == PaperEntryType.Comment && p.Year == year)
                                         .ToList();
                result.AddRange(yearComments);
            }

            return result;
        }
    }
}
