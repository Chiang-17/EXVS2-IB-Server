namespace WebUIInfinite.Shared.Validation;

// Shared by the form and API. Player names are required; custom titles may be empty.
public static class PlayerTextPolicy
{
    private const string JapaneseSymbols =
        "　、。，．・：；？！゛゜´｀¨＾￣＿ヽヾゝゞ〃仝々〆〇ー―‐／＼〜～∥｜…‥‘’“”" +
        "（）〔〕［］｛｝〈〉《》「」『』【】＋−±×÷＝≠＜＞≦≧∞∴♂♀°′″℃￥＄￠￡％＃＆＊＠§" +
        "☆★○●◎◇◆□■△▲▽▼※〒→←↑↓〓♯♭♪†‡¶◯∀∂∇√∽∝∠⊥⌒∵∫∬Å‰♮ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ";

    public static bool IsValid(string? text, bool required) => text is not null
        && (!required || text.Length > 0) && HasAllowedCharacters(text);

    public static bool HasAllowedCharacters(string text) => text.All(IsAllowedCharacter);

    private static bool IsAllowedCharacter(char c) =>
        c is >= '\u0020' and <= '\u007E' // Printable ASCII: letters, digits, punctuation, symbols, space.
        or >= '\uFF01' and <= '\uFF5E' // Fullwidth ASCII forms.
        or >= '\uFFE0' and <= '\uFFE6' // Fullwidth currency and symbol forms.
        or >= '\u3041' and <= '\u3096' or >= '\u3099' and <= '\u309F' // Hiragana, marks.
        or >= '\u30A0' and <= '\u30FF' or >= '\u31F0' and <= '\u31FF' // Katakana.
        or >= '\uFF61' and <= '\uFF9F' // Halfwidth Japanese punctuation and katakana.
        || JapaneseSymbols.Contains(c) || JapaneseCharacters.Kanji.Contains(c)
        || JapaneseCharacters.AdditionalKanji.Contains(c);
}
