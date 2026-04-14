using System.Text.RegularExpressions;

namespace CVAnalyzer.Mappers
{
    public static class AnalysisParser
    {
        public static Dictionary<string, string> ParseSections(string text)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            
            var regex = new Regex(
                @"(?mi)^\s*.*?(Структура|Техническая\s+составляющая|Релевантность|Прочие\s+рекомендации|Совпадение\s+с\s+вакансией)",
                RegexOptions.Compiled);

            var matches = regex.Matches(text);

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
            
                var title = match.Groups[1].Value.Trim();
                int contentStart = match.Index + match.Length;
                
                int contentEnd = (i + 1 < matches.Count)
                    ? matches[i + 1].Index
                    : text.Length;

                if (contentEnd <= contentStart)
                    continue;

                var content = text
                    .Substring(contentStart, contentEnd - contentStart)
                    .Trim();

                result[title] = content;
            }

            return result;
        }
    }
}