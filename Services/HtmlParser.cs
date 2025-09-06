using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using HtmlPaperManager.Models;

namespace HtmlPaperManager.Services
{
    /// <summary>
    /// HTML解析服务
    /// </summary>
    public class HtmlParser
    {
        /// <summary>
        /// 从HTML文件解析论文列表
        /// </summary>
        /// <param name="htmlFilePath">HTML文件路径</param>
        /// <returns>论文列表</returns>
        public List<Paper> ParseHtmlFile(string htmlFilePath)
        {
            var debugInfo = new StringBuilder();
            return ParseHtmlFileWithDebug(htmlFilePath, debugInfo);
        }

        /// <summary>
        /// 从HTML文件解析论文列表（带调试信息）
        /// </summary>
        /// <param name="htmlFilePath">HTML文件路径</param>
        /// <param name="debugInfo">调试信息输出</param>
        /// <returns>论文列表</returns>
        public List<Paper> ParseHtmlFileWithDebug(string htmlFilePath, StringBuilder debugInfo)
        {
            var papers = new List<Paper>();
            
            try
            {
                debugInfo.AppendLine($"加载HTML文件: {htmlFilePath}");
                
                var doc = new HtmlDocument();
                doc.Load(htmlFilePath, System.Text.Encoding.UTF8);
                
                debugInfo.AppendLine("HTML文档加载成功");
                
                // 查找papers div
                var papersDiv = doc.DocumentNode.SelectSingleNode("//div[@id='papers']");
                if (papersDiv == null)
                {
                    debugInfo.AppendLine("错误: 未找到papers div节点");
                    throw new Exception("未找到papers div节点");
                }

                debugInfo.AppendLine("找到papers div节点");
                debugInfo.AppendLine($"papers div下有 {papersDiv.ChildNodes.Count} 个子节点");
                
                int nodeIndex = 0;
                int skipCount = 0;
                
                // 解析所有子节点
                foreach (var node in papersDiv.ChildNodes)
                {
                    nodeIndex++;
                    
                    // 跳过空白文本节点
                    if (node.NodeType == HtmlNodeType.Text && string.IsNullOrWhiteSpace(node.InnerText))
                    {
                        skipCount++;
                        continue;
                    }

                    debugInfo.AppendLine($"节点 {nodeIndex}: 类型={node.NodeType}, 名称={node.Name}");

                    if (node.NodeType == HtmlNodeType.Element)
                    {
                        if (node.Name.ToLower() == "span" && IsYearHeader(node))
                        {
                            // 年份标记
                            var yearText = ExtractYear(node.InnerText);
                            if (!string.IsNullOrEmpty(yearText))
                            {
                                var yearPaper = Paper.CreateYearHeader(yearText);
                                yearPaper.OriginalHtml = node.OuterHtml; // 保存原始HTML
                                papers.Add(yearPaper);
                                debugInfo.AppendLine($"  -> 添加年份标记: {yearText}");
                            }
                            else
                            {
                                debugInfo.AppendLine($"  -> 跳过空年份标记");
                            }
                        }
                        else if (node.Name.ToLower() == "p")
                        {
                            // 论文条目
                            var paper = ParsePaperEntry(node);
                            if (paper != null)
                            {
                                papers.Add(paper);
                                debugInfo.AppendLine($"  -> 添加论文条目: {(paper.Title?.Length > 50 ? paper.Title.Substring(0, 50) + "..." : paper.Title ?? "[无标题]")}");
                            }
                            else
                            {
                                debugInfo.AppendLine($"  -> 论文条目解析失败");
                            }
                        }
                        else
                        {
                            debugInfo.AppendLine($"  -> 跳过元素: {node.Name}");
                        }
                    }
                    else if (node.NodeType == HtmlNodeType.Comment)
                    {
                        // 注释条目 - 对于注释节点，使用InnerHtml而不是InnerText
                        string commentContent = node.InnerHtml ?? node.InnerText ?? "";
                        
                        // 如果commentContent包含完整的HTML注释标记，需要清理
                        if (commentContent.StartsWith("<!--") && commentContent.EndsWith("-->"))
                        {
                            commentContent = commentContent.Substring(4, commentContent.Length - 7).Trim();
                        }
                        
                        debugInfo.AppendLine($"  -> 发现注释: {(commentContent.Trim()?.Length > 100 ? commentContent.Trim().Substring(0, 100) + "..." : commentContent.Trim() ?? "[空注释]")}");
                        var commentText = ExtractCommentText(commentContent, debugInfo);
                        if (!string.IsNullOrEmpty(commentText))
                        {
                            var commentPaper = Paper.CreateComment(commentText);
                            commentPaper.OriginalHtml = $"<!--{commentContent}-->"; // 保存完整的注释HTML
                            papers.Add(commentPaper);
                            debugInfo.AppendLine($"    -> 注释解析成功: '{commentText}'");
                        }
                        else
                        {
                            debugInfo.AppendLine($"    -> 注释提取失败，可能不符合格式要求");
                        }
                    }
                    else
                    {
                        debugInfo.AppendLine($"  -> 跳过节点类型: {node.NodeType}");
                    }
                }

                debugInfo.AppendLine($"跳过了 {skipCount} 个空白文本节点");
                debugInfo.AppendLine($"解析完成，共处理 {nodeIndex} 个节点，生成 {papers.Count} 个条目");
            }
            catch (Exception ex)
            {
                debugInfo.AppendLine($"解析异常: {ex.Message}");
                throw new Exception($"解析HTML文件失败: {ex.Message}", ex);
            }

            return papers;
        }

        /// <summary>
        /// 判断是否为年份标记
        /// </summary>
        /// <param name="spanNode">span节点</param>
        /// <returns></returns>
        private bool IsYearHeader(HtmlNode spanNode)
        {
            var style = spanNode.GetAttributeValue("style", "");
            return style.Contains("font-size:16.0pt");
        }

        /// <summary>
        /// 从文本中提取年份字符串
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>年份字符串</returns>
        private string ExtractYear(string text)
        {
            // 年份现在可以是任意字符串，直接返回清理后的文本
            return text.Trim();
        }

        /// <summary>
        /// 提取注释文本 - 只处理形如 "<!-- /* AAAI */ -->" 的注释
        /// </summary>
        /// <param name="commentText">注释内容</param>
        /// <param name="debugInfo">调试信息</param>
        /// <returns>注释文本</returns>
        private string ExtractCommentText(string commentText, StringBuilder debugInfo = null)
        {
            // 先清理前后的空白字符
            var cleanText = commentText?.Trim();
            debugInfo?.AppendLine($"    -> 原始注释内容: '{cleanText}'");
            
            if (string.IsNullOrEmpty(cleanText))
            {
                debugInfo?.AppendLine("    -> 注释为空");
                return string.Empty;
            }

            // 支持多种注释格式的正则表达式模式
            var patterns = new[]
            {
                (@"/\*\s*(.*?)\s*\*/", "/* 内容 */"),  // /* 注释内容 */
                (@"\*\s*(.*?)\s*\*", "* 内容 *"),      // * 注释内容 *
                (@"^\s*([^*]+)\s*$", "直接内容")        // 直接文本内容（不含星号）
            };

            foreach (var (pattern, description) in patterns)
            {
                var match = Regex.Match(cleanText, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (match.Success && match.Groups.Count > 1)
                {
                    string extracted = match.Groups[1].Value.Trim();
                    debugInfo?.AppendLine($"    -> 使用模式 '{description}' 提取到: '{extracted}'");
                    
                    // 过滤掉过短或过长的注释，但允许年份和短标记
                    if (extracted.Length >= 2 && extracted.Length <= 200)
                    {
                        debugInfo?.AppendLine($"    -> 注释提取成功: '{extracted}'");
                        return extracted;
                    }
                    else
                    {
                        debugInfo?.AppendLine($"    -> 注释长度不符合要求 (长度: {extracted.Length})");
                    }
                }
                else
                {
                    debugInfo?.AppendLine($"    -> 模式 '{description}' 匹配失败");
                }
            }

            // 如果所有模式都失败，尝试直接返回注释内容（去掉多余的空白）
            if (cleanText.Length >= 2 && cleanText.Length <= 200)
            {
                debugInfo?.AppendLine($"    -> 使用备用方案提取注释: '{cleanText}'");
                return cleanText;
            }

            debugInfo?.AppendLine("    -> 注释提取失败，可能不符合格式要求");
            return string.Empty;
        }

        /// <summary>
        /// 解析论文条目
        /// </summary>
        /// <param name="pNode">p节点</param>
        /// <returns>论文对象</returns>
        private Paper ParsePaperEntry(HtmlNode pNode)
        {
            try
            {
                var paper = new Paper();
                
                // 保存原始HTML
                paper.OriginalHtml = pNode.OuterHtml;
                
                // 获取完整的HTML内容
                string fullHtml = pNode.InnerHtml;
                
                // 提取PDF链接
                var pdfLink = ExtractPdfLink(pNode);
                if (!string.IsNullOrEmpty(pdfLink))
                {
                    paper.PdfLink = pdfLink;
                    paper.ShowPdfLink = true;
                }

                // 提取Code链接
                var codeLink = ExtractCodeLink(pNode);
                if (!string.IsNullOrEmpty(codeLink))
                {
                    paper.CodeLink = codeLink;
                    paper.ShowCodeLink = true;
                }

                // 移除链接部分，但保留HTML格式（如<strong>标签）
                string cleanHtml = RemoveLinksFromHtml(pNode);
                
                // 解析HTML内容
                ParseHtmlContent(cleanHtml, paper);
                
                return paper;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 提取PDF链接
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>PDF链接</returns>
        private string ExtractPdfLink(HtmlNode node)
        {
            var pdfLinks = node.SelectNodes(".//a[contains(@href, '.pdf')]");
            return pdfLinks?.FirstOrDefault()?.GetAttributeValue("href", "");
        }

        /// <summary>
        /// 提取Code链接
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns>Code链接</returns>
        private string ExtractCodeLink(HtmlNode node)
        {
            var codeLinks = node.SelectNodes(".//a[contains(text(), 'code') or contains(text(), 'Code')]");
            return codeLinks?.FirstOrDefault()?.GetAttributeValue("href", "");
        }

        /// <summary>
        /// 从文本中移除链接部分但保留HTML格式
        /// </summary>
        /// <param name="node">HTML节点</param>
        /// <returns>清理后的HTML内容</returns>
        private string RemoveLinksFromHtml(HtmlNode node)
        {
            // 克隆节点以避免修改原始节点
            var clonedNode = node.CloneNode(true);
            
            // 移除PDF链接
            var pdfLinks = clonedNode.SelectNodes(".//a[contains(@href, '.pdf')]");
            if (pdfLinks != null)
            {
                foreach (var link in pdfLinks.ToList())
                {
                    link.Remove();
                }
            }
            
            // 移除Code链接
            var codeLinks = clonedNode.SelectNodes(".//a[contains(text(), 'code') or contains(text(), 'Code')]");
            if (codeLinks != null)
            {
                foreach (var link in codeLinks.ToList())
                {
                    link.Remove();
                }
            }
            
            // 移除包含 (pdf)、(code)、(slide) 的文本节点
            string htmlContent = clonedNode.InnerHtml;
            htmlContent = Regex.Replace(htmlContent, @"\s*\(pdf\)\s*", " ");
            htmlContent = Regex.Replace(htmlContent, @"\s*\(code\)\s*", " ");
            htmlContent = Regex.Replace(htmlContent, @"\s*\(slide\)\s*", " ");
            
            return htmlContent.Trim();
        }

        /// <summary>
        /// 解析文本内容
        /// </summary>
        /// <param name="textContent">文本内容</param>
        /// <param name="paper">论文对象</param>
        private void ParseTextContent(string textContent, Paper paper)
        {
            // 使用正则表达式解析作者、标题和发表信息
            // 格式通常是: 作者. 标题. 发表信息
            
            var parts = textContent.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length >= 3)
            {
                paper.Authors = parts[0].Trim();
                paper.Title = parts[1].Trim();
                paper.PublicationInfo = string.Join(". ", parts.Skip(2)).Trim();
                
                // 提取年份 - 从PublicationInfo中提取数字年份
                var yearMatch = Regex.Match(paper.PublicationInfo, @"[*~]*(19|20)\d{2}");
                if (yearMatch.Success)
                {
                    var yearString = Regex.Match(yearMatch.Value, @"(19|20)\d{2}").Value;
                    paper.Year = yearString;
                }
            }
            else if (parts.Length == 2)
            {
                paper.Authors = parts[0].Trim();
                paper.Title = parts[1].Trim();
                paper.PublicationInfo = "";
            }
            else
            {
                // 如果格式不标准，将整个文本作为标题
                paper.Title = textContent.Trim();
                paper.Authors = "";
                paper.PublicationInfo = "";
            }
        }

        /// <summary>
        /// 解析HTML内容，保留格式标签
        /// </summary>
        /// <param name="htmlContent">HTML内容</param>
        /// <param name="paper">论文对象</param>
        private void ParseHtmlContent(string htmlContent, Paper paper)
        {
            // 创建一个临时的HTML文档来解析内容
            var tempDoc = new HtmlDocument();
            tempDoc.LoadHtml($"<div>{htmlContent}</div>");
            var contentNode = tempDoc.DocumentNode.FirstChild;
            
            // 获取纯文本用于解析结构，去除多余空白字符
            string textContent = contentNode.InnerText.Trim().Replace("\n", " ").Replace("\r", " ");
            // 合并多个空格为单个空格
            textContent = Regex.Replace(textContent, @"\s+", " ");
            // 清理HTML实体
            textContent = System.Net.WebUtility.HtmlDecode(textContent);
            
            // 使用正则表达式解析作者、标题和发表信息
            // 格式通常是: 作者. 标题. 发表信息
            var parts = textContent.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length >= 3)
            {
                // 对于作者部分，我们需要保留HTML格式
                string authorsText = parts[0].Trim();
                
                // 在原始HTML中查找作者部分，保留格式
                // 查找第一个句号之前的内容，保留HTML格式
                string beforeFirstDot = Regex.Match(htmlContent, @"^(.*?)\.", RegexOptions.Singleline).Groups[1].Value;
                if (!string.IsNullOrEmpty(beforeFirstDot))
                {
                    paper.Authors = beforeFirstDot.Trim();
                }
                else
                {
                    paper.Authors = authorsText;
                }
                
                paper.Title = parts[1].Trim();
                paper.PublicationInfo = string.Join(". ", parts.Skip(2)).Trim();
                
                // 提取年份 - 从PublicationInfo中提取数字年份  
                var yearMatch = Regex.Match(paper.PublicationInfo, @"[*~]*(19|20)\d{2}");
                if (yearMatch.Success)
                {
                    var yearString = Regex.Match(yearMatch.Value, @"(19|20)\d{2}").Value;
                    paper.Year = yearString;
                }
            }
            else if (parts.Length == 2)
            {
                // 只有两部分的情况
                string authorsText = parts[0].Trim();
                string beforeFirstDot = Regex.Match(htmlContent, @"^(.*?)\.", RegexOptions.Singleline).Groups[1].Value;
                if (!string.IsNullOrEmpty(beforeFirstDot))
                {
                    paper.Authors = beforeFirstDot.Trim();
                }
                else
                {
                    paper.Authors = authorsText;
                }
                
                paper.Title = parts[1].Trim();
                paper.PublicationInfo = "";
            }
            else if (parts.Length == 1)
            {
                // 只有一部分，可能是标题或作者
                paper.Title = textContent.Trim();
                paper.Authors = "";
                paper.PublicationInfo = "";
            }
        }

        /// <summary>
        /// 智能解析导入的论文文本
        /// </summary>
        /// <param name="inputText">输入文本</param>
        /// <returns>论文对象</returns>
        public Paper ParseImportedPaper(string inputText)
        {
            var paper = new Paper();
            
            try
            {
                // 清理输入文本
                inputText = inputText.Trim();
                
                // 尝试解析标准格式：作者: 标题. 发表信息
                var colonIndex = inputText.IndexOf(':');
                if (colonIndex > 0)
                {
                    // 提取作者部分
                    paper.Authors = ProcessAuthors(inputText.Substring(0, colonIndex).Trim());
                    
                    // 处理标题和发表信息
                    var remaining = inputText.Substring(colonIndex + 1).Trim();
                    var lastDotIndex = remaining.LastIndexOf('.');
                    
                    if (lastDotIndex > 0)
                    {
                        paper.Title = remaining.Substring(0, lastDotIndex).Trim();
                        paper.PublicationInfo = remaining.Substring(lastDotIndex + 1).Trim();
                    }
                    else
                    {
                        paper.Title = remaining;
                    }
                }
                else
                {
                    // 如果没有冒号，尝试其他解析方式
                    paper.Title = inputText;
                }
                
                // 提取年份
                ExtractYearFromPublication(paper);
                
                // 生成默认PDF路径
                paper.GenerateDefaultPdfPath();
                
                return paper;
            }
            catch (Exception)
            {
                return new Paper { Title = inputText };
            }
        }

        /// <summary>
        /// 处理作者信息
        /// </summary>
        /// <param name="authors">作者字符串</param>
        /// <returns>处理后的作者字符串</returns>
        private string ProcessAuthors(string authors)
        {
            // 将"Rong-Hua Li"用<strong>标签包裹
            authors = Regex.Replace(authors, @"\bRong-Hua Li\b", "<strong>Rong-Hua Li</strong>");
            
            // 处理通讯作者标记
            authors = Regex.Replace(authors, @"<strong>Rong-Hua Li</strong>\*", "<strong>Rong-Hua Li*</strong>");
            
            return authors;
        }

        /// <summary>
        /// 从发表信息中提取年份
        /// </summary>
        /// <param name="paper">论文对象</param>
        private void ExtractYearFromPublication(Paper paper)
        {
            var yearMatch = Regex.Match(paper.PublicationInfo, @"\((\d{4})\)");
            if (yearMatch.Success)
            {
                paper.Year = yearMatch.Groups[1].Value;
            }
        }
    }
}
