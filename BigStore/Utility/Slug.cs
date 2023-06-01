using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BigStore.Utility
{
    public static class Slug
    {
        //public static string GenerateSlug(string title)
        //{
        //    string slug = RemoveDiacritics(title);
        //    slug = Regex.Replace(slug, @"\s+", " "); // Chuyển nhiều khoảng trắng thành một khoảng trắng duy nhất
        //    slug = Regex.Replace(slug, @"[^a-zA-Z0-9\s]", ""); // Xóa các ký tự không hợp lệ
        //    slug = slug.ToLower(); // Chuyển tất cả thành chữ thường
        //    slug = slug.Replace(" ", "-"); // Thay thế khoảng trắng bằng dấu gạch ngang
        //    return slug;
        //}

        //private static string RemoveDiacritics(string text)
        //{
        //    string normalizedString = text.Normalize(NormalizationForm.FormD);
        //    StringBuilder stringBuilder = new();

        //    foreach (char c in normalizedString)
        //    {
        //        if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
        //            stringBuilder.Append(c);
        //    }

        //    return stringBuilder.ToString();
        //}

        public static string GenerateSlug(string text)
        {
            for (int i = 32; i < 48; i++)
            {

                text = text.Replace(((char)i).ToString(), " ").ToLower().Trim();

            }
            text = text.Replace(".", "-");

            text = text.Replace(" ", "-");

            text = text.Replace(",", "-");

            text = text.Replace(";", "-");

            text = text.Replace(":", "-");

            Regex regex = new(@"\p{IsCombiningDiacriticalMarks}+");

            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);

            return regex.Replace(strFormD, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        //public static string GenerateSlug(this string phrase)
        //{
        //    string str = phrase.RemoveAccent().ToLower();
        //    // invalid chars           
        //    str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        //    // convert multiple spaces into one space   
        //    str = Regex.Replace(str, @"\s+", " ").Trim();
        //    // cut and trim 
        //    str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        //    str = Regex.Replace(str, @"\s", "-"); // hyphens   
        //    return str;
        //}

        //public static string RemoveAccent(this string txt)
        //{
        //    byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
        //    return System.Text.Encoding.ASCII.GetString(bytes);
        //}
    }
}
