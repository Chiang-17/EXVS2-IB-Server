using WebUIInfinite.Shared.Validation;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;
using WebUIInfinite.Client.Constants;
using WebUIInfinite.Shared.Resources;

namespace WebUIInfinite.Client.Validator;

public class NameValidator : INameValidator
{
    private IStringLocalizer<Resource> _localizer;

    private const string JpKanji = JapaneseCharacters.Kanji;
    
    public NameValidator(IStringLocalizer<Resource> localizer)
    {
        _localizer = localizer;
    }
    
    public string? ValidatePlayerName(string playerName)
    {
        return ValidatePlayerText(playerName, _localizer["validateplayername"], required: true);
    }
    
    public string? ValidateTeamName(string teamName)
    {
        return ValidateName(teamName, _localizer["validatetriadteamname"]);
    }
    
    public string? ValidatePvPTeamName(string teamName)
    {
        return ValidateName(teamName, _localizer["validatepvpteamname"]);
    }
    
    public string? ValidateCustomizeMessage(string message)
    {
        return ValidateMessage(message, _localizer["validatemessage"]);
    }
    
    public string? ValidateCustomizeTitle(string message)
    {
        return ValidatePlayerText(message, _localizer["validate_custom_title"], required: false);
    }
    
    private String? ValidateName(string name, string errorMessagePart)
    {
        // Allow both halfwidth and fullwidth forms of every previously permitted symbol.
        const string prePattern = @"^[ 　";
        const string afterPattern = @"ぁ-んァ-ンｧ-ﾝﾞﾟa-zA-Z0-9ａ-ｚＡ-Ｚ０-９ー－<>＜＞+＋*＊÷=＝;；:：←/／\\＼_＿|｜・･@＠!！?？&＆★()（）^＾◇∀Ξν×†ω♪♭#＃∞〆→↓↑%％※ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ☆◆\[\]［］「」『』【】-]{1,12}$";
        const string pattern = prePattern + JpKanji + afterPattern;
        
        return name.Length switch
        {
            0 => errorMessagePart + $" {_localizer["validation_required"]}",
            > (int) NameLength.TeamNameMaxLength => errorMessagePart + $" {_localizer["validation_length_1"]} 12 {_localizer["validation_length_2"]}",
            _ => !Regex.IsMatch(name, pattern) ? errorMessagePart + $" {_localizer["validation_invalidchar"]}" : null
        };
    }
    
    private String? ValidateMessage(string message, string errorMessagePart)
    {
        const string prePattern = @"^[ ";
        const string afterPattern = @"\/ぁ-んァ-ンｧ-ﾝﾞﾟa-zA-Z0-9ａ-ｚＡ-Ｚ０-９-_ー＜＞＋－＊÷＝；：←／＼＿｜・＠！？＆★（）＾◇∀Ξν×†ω♪♭#∞〆→↓↑％※ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ☆◆\[\]「」『』【】]{1,13}$";
        const string pattern = prePattern + JpKanji + afterPattern;
        
        return message.Length switch
        {
            0 => null,
            > (int) NameLength.MessageMaxLength => errorMessagePart + $" {_localizer["validation_length_1"]} 13 {_localizer["validation_length_2"]}",
            _ => !Regex.IsMatch(message, pattern) ? errorMessagePart + $" {_localizer["validation_invalidchar"]}" : null
        };
    }

    public IEnumerable<string> ValidateMessageEnumerable(string message)
    {
        const string prePattern = @"^[ ";
        const string afterPattern = @"\/ぁ-んァ-ンｧ-ﾝﾞﾟa-zA-Z0-9ａ-ｚＡ-Ｚ０-９-_ー＜＞＋－＊÷＝；：←／＼＿｜・＠！？＆★（）＾◇∀Ξν×†ω♪♭#∞〆→↓↑％※ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ☆◆\[\]「」『』【】]{1,13}$";
        const string pattern = prePattern + JpKanji + afterPattern;

        if (string.IsNullOrWhiteSpace(message))
        {
            yield break;
        }
        
        if (message.Length > (int) NameLength.MessageMaxLength)
        {
            yield return _localizer["validatemessage"] + $" {_localizer["validation_length_1"]} 13 {_localizer["validation_length_2"]}";
        }

        if (!Regex.IsMatch(message, pattern))
        {
            yield return _localizer["validatemessage"] + $" {_localizer["validation_invalidchar"]}";
        }
    }
    
    private string? ValidatePlayerText(string text, string errorMessagePart, bool required)
    {
        return text.Length switch
        {
            0 => required ? errorMessagePart + $" {_localizer["validation_required"]}" : null,
            _ => !PlayerTextPolicy.HasAllowedCharacters(text) ? errorMessagePart + $" {_localizer["validation_invalidchar"]}" : null
        };
    }
}
