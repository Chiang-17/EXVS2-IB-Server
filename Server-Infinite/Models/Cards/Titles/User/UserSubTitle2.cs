using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerInfinite.Models.Cards.Titles.User;

[Table("exvs2ob_user_sub_title_2")]
[Index(nameof(Id))]
[Index(nameof(UserTitleSettingId))]
public class UserSubTitle2 : UserTitle
{
}
